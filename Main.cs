using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection; 
using System.Collections;
using Il2Cpp;

namespace ResearchBookTweaker
{
	public class Main : MelonMod
	{
		public override void OnInitializeMelon()
		{
            Debug.Log($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName.Contains("Menu"))
            {
                Main.ChangePrefabResearchItems("GEAR_BookFireStarting", Settings.settings.pointsFireStarting, Settings.settings.timeFireStarting);
                Main.ChangePrefabResearchItems("GEAR_BookRevolverFirearm", Settings.settings.pointsRevolverFirearm, Settings.settings.timeRevolverFirearm);
                Main.ChangePrefabResearchItems("GEAR_BookArchery", Settings.settings.pointsArchery, Settings.settings.timeArchery);
                Main.ChangePrefabResearchItems("GEAR_BookRifleFirearm", Settings.settings.pointsRifleFirearm, Settings.settings.timeRifleFirearm, Settings.settings.noBenefitRifleFirearm);
                Main.ChangePrefabResearchItems("GEAR_BookRifleFirearmAdvanced", Settings.settings.pointsRifleFirearmAdvanced, Settings.settings.timeRifleFirearmAdvanced);
                Main.ChangePrefabResearchItems("GEAR_BookGunsmithing", Settings.settings.pointsGunsmithing, Settings.settings.timeGunsmithing);
                Main.ChangePrefabResearchItems("GEAR_BookCooking", Settings.settings.pointsCooking, Settings.settings.timeCooking);
                Main.ChangePrefabResearchItems("GEAR_BookMending", Settings.settings.pointsMending, Settings.settings.timeMending);
                Main.ChangePrefabResearchItems("GEAR_BookIceFishing", Settings.settings.pointsIceFishing, Settings.settings.timeIceFishing);
                Main.ChangePrefabResearchItems("GEAR_BookCarcassHarvesting", Settings.settings.pointsCarcassHarvesting, Settings.settings.timeCarcassHarvesting);
            }
        }

        public static GearItem GetGearItemPrefab(string name) => GearItem.LoadGearItemPrefab(name).GetComponent<GearItem>();

        public static void ChangePrefabResearchItems(string name, int points, int time, int noBenefitLevel = 5)
        {
            GearItem gearItem = GetGearItemPrefab(name);
            if (gearItem == null)
            {
                MelonLogger.Msg(name + " GearItem Prefab is NULL!");
                return;
            }
            ResearchItem researchItem = gearItem.GetComponent<ResearchItem>();
            if (researchItem == null)
            {
                MelonLogger.Msg(name + " ResearchItem Prefab is NULL!");
                return;
            }
            researchItem.m_SkillPoints = points;
            researchItem.m_TimeRequirementHours = time;
            researchItem.m_NoBenefitAtSkillLevel = noBenefitLevel;
        }
    }
}