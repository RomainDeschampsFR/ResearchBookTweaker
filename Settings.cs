using Il2CppNewtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModSettings;
using System.Threading.Tasks;
using UnityEngine;

namespace ResearchBookTweaker
{
    internal class ResearchBookTweakerSettings : JsonModSettings
    {
        [Name("Reading interval length")]
        [Description("Sets the shortest amount of time that a book can be read for.")]
        [Choice("15 minutes", "30 minutes", "60 minutes")]
        public IntervalLength intervalLength = IntervalLength.MINS_30;

        [Name("Count interrupted progress")]
        [Description("Whether progress within a reading interval should still be counted when you're interrupted.")]
        public bool allowInterruptions = true;
    }

    internal static class Settings
    {

        private static ResearchBookTweakerSettings settings;

        public static void OnLoad()
        {
            settings = new ResearchBookTweakerSettings();
            settings.AddToModSettings("Research Book Tweaker");
        }

        internal static float GetReadingIntervalHours()
        {
            switch (settings.intervalLength)
            {
                case IntervalLength.MINS_60:
                    return 1f;
                case IntervalLength.MINS_30:
                    return 0.5f;
                case IntervalLength.MINS_15:
                    return 0.25f;
                default:
                    Debug.LogError("[ResearchBookTweaker] Unknown interval length: " + settings.intervalLength);
                    return 1f;
            }
        }

        internal static bool GetAllowInterruptions()
        {
            return settings.allowInterruptions;
        }
    }

    internal enum IntervalLength
    {
        MINS_15, MINS_30, MINS_60
    }
}

