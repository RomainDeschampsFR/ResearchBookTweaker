using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using System.Threading.Tasks;
using ResearchBookTweaker;
using UnityEngine;
using Il2CppTLD.Gear;
using MelonLoader;

namespace ResearchBookTweaker
{
    internal class Patches
    {
        public static float executionTime;

        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.RefreshHoursToRead), new Type[0])]
        private static class FixHoursToRead
        {
            internal static bool Prefix(Panel_Inventory_Examine __instance)
            {
                float hoursResearchRemaining = GetHoursResearchRemaining(__instance);
                float readingInterval = Settings.GetReadingIntervalHours();
                int maximumIntervals = Mathf.CeilToInt(hoursResearchRemaining / readingInterval);

                int intervalsToRead = Mathf.Clamp(__instance.m_HoursToRead, 1, maximumIntervals);
                __instance.m_HoursToRead = intervalsToRead;

                float hoursToRead = Math.Min(intervalsToRead * readingInterval, hoursResearchRemaining);
                __instance.m_TimeToReadLabel.text = hoursToRead.ToString("0.##");
                __instance.m_ReadHoursDecrease.gameObject.SetActive(intervalsToRead > 1);
                __instance.m_ReadHoursIncrease.gameObject.SetActive(intervalsToRead < maximumIntervals);

                if (Utils.IsGamepadActive())
                {
                    ButtonLegend buttonLegend = __instance.m_ButtonLegendContainer.m_ButtonLegend;
                    UISprite decrease = __instance.m_GamepadReadHoursSpriteDecrease;
                    UISprite increase = __instance.m_GamepadReadHoursSpriteIncrease;
                    // The second argument is declared as ref, but is never assigned to in ConfigureButtonIconSpriteName.
                    buttonLegend.ConfigureButtonIconSpriteName("Inventory_FilterLeft", ref decrease);
                    buttonLegend.ConfigureButtonIconSpriteName("Inventory_FilterRight", ref increase);
                }

                return false; // Never run the original
            }
        }

        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnReadHoursIncrease), new Type[0])]
        private static class FixOnReadHoursIncrease
        {
            internal static bool Prefix(Panel_Inventory_Examine __instance)
            {
                float hoursResearchRemaining = GetHoursResearchRemaining(__instance);
                int maximumIntervals = Mathf.CeilToInt(hoursResearchRemaining / Settings.GetReadingIntervalHours());
                int intervalsToRead = __instance.m_HoursToRead;

                if (intervalsToRead >= maximumIntervals)
                {
                    GameAudioManager.PlayGUIError();
                }
                else
                {
                    ++__instance.m_HoursToRead;
                    GameAudioManager.PlayGUIScroll();
                    __instance.RefreshHoursToRead();
                }
                return false; // Never run the original
            }
        }

        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.AccelerateTimeOfDay), new Type[] { typeof(int), typeof(bool) })]
        private static class ScaleStartRead
        {
            internal static void Prefix(Panel_Inventory_Examine __instance, ref int minutes)
            {
                if (!__instance.m_GearItem?.m_ResearchItem) return;

                float hoursResearchRemaining = GetHoursResearchRemaining(__instance);
                float minutesToRead = Math.Min(minutes * Settings.GetReadingIntervalHours(), hoursResearchRemaining * 60f);
                float hoursToRead = minutesToRead / 60f;

                __instance.m_ReadTimeSeconds = 1 + 3 * Mathf.Log(1 + hoursToRead);
                __instance.m_ProgressBarTimeSeconds = __instance.m_ReadTimeSeconds;
                minutes = Mathf.CeilToInt(minutesToRead);
            }
        }

        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.ReadComplete), new Type[] { typeof(float) })]
        private static class ScaleReadComplete
        {
            internal static void Prefix(Panel_Inventory_Examine __instance, ref float normalizedProgress)
            {
                if (!__instance.m_GearItem?.m_ResearchItem) return;

                int hoursToRead = __instance.m_HoursToRead;
                float intervalsRead = normalizedProgress * hoursToRead;
                if (!Settings.GetAllowInterruptions())
                {
                    intervalsRead = Mathf.Floor(intervalsRead);
                }

                float hoursRead = intervalsRead * Settings.GetReadingIntervalHours();
                __instance.m_GearItem.m_ResearchItem.Read(hoursRead);

                // Do the rest of the method as if we read for 0 minutes
                normalizedProgress = 0;
            }
        }

        [HarmonyPatch(typeof(ResearchItem), nameof(ResearchItem.OnResearchComplete), new Type[0])]
        private static class PreventDoubleSkillPointsBeingAwarded
        {
            internal static bool Prefix(ResearchItem __instance)
            {
                // The method is called twice in a row for some reasons. The code below from Zeo throw an error so... find another way to prevent it.
                if (Time.time - executionTime < 0.5f) return false;
                executionTime = Time.time;
                // Only run the original - and award skill points - if the item type is still Tool and not yet FireStarting
                return true; //__instance.GetComponent<GearItem>()?.m_Type ==  GearTypeEnum.Tool;  
            }
        }

        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.RefreshReadPanel), new Type[0])]
        private static class DisplayHoursReadProgressAsFraction
        {
            internal static void Postfix(Panel_Inventory_Examine __instance)
            {
                if (!__instance.m_GearItem?.m_ResearchItem) return;

                string text = Localization.Get("GAMEPLAY_HoursResearched");
                text = text.Replace("{val1}", __instance.m_GearItem.m_ResearchItem.GetElapsedHours().ToString("0.##"));
                text = text.Replace("{val2}", __instance.m_GearItem.m_ResearchItem.m_TimeRequirementHours.ToString());
                __instance.m_TimeToReadRemainingLabel.text = text;
            }
        }

        private static float GetHoursResearchRemaining(Panel_Inventory_Examine panel)
        {
            ResearchItem researchItem = panel.m_GearItem.m_ResearchItem;
            return researchItem.m_TimeRequirementHours - researchItem.GetElapsedHours();
        }
    }
}
