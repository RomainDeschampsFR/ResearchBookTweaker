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
        [Section("GLOBAL SETTINGS")]
        [Name("Reading interval length")]
        [Description("Sets the shortest amount of time that a book can be read for.")]
        [Choice("15 minutes", "30 minutes", "60 minutes")]
        public IntervalLength intervalLength = IntervalLength.MINS_30;

        [Name("Count interrupted progress")]
        [Description("Whether progress within a reading interval should still be counted when you're interrupted.")]
        public bool allowInterruptions = true;

        [Name("Can be read multiple times")]
        [Description("Research books can be read indefinitely \nWARNING : Not Compatible with UNIQUE KNOWLEDGE MOD")]
        public bool reredable = false;

        [Section("READING TIME AND GRANTED POINTS")]
        [Name("Archery : reading time")]
        [Description("Default Game Value: 5")]
        [Slider(1, 30)]
        public int timeArchery = 5;

        [Name("Archery : points")]
        [Description("Default Game Value: 10")]
        [Slider(2, 20)]
        public int pointsArchery = 10;

        [Name("Revolver : reading time")]
        [Description("Default Game Value: 5")]
        [Slider(1, 30)]
        public int timeRevolverFirearm = 5;

        [Name("Revolver : points")]
        [Description("Default Game Value: 10")]
        [Slider(2, 20)]
        public int pointsRevolverFirearm = 10;

        [Name("Rifle : reading time")]
        [Description("Default Game Value: 4")]
        [Slider(1, 30)]
        public int timeRifleFirearm = 4;

        [Name("Rifle : points")]
        [Description("Default Game Value: 5")]
        [Slider(2, 20)]
        public int pointsRifleFirearm = 5;

        [Name("Rifle : No benefit from level")]
        [Description("You will not earn any points for reading this book from the level you have chosen.\nDefault Game Value: 4")]
        [Slider(2, 5)]
        public int noBenefitRifleFirearm = 4;

        [Name("Advanced rifle : [reading time")]
        [Description("Default Game Value: 25")]
        [Slider(1, 30)]
        public int timeRifleFirearmAdvanced = 25;

        [Name("Advanced rifle : points")]
        [Description("Default Game Value: 10")]
        [Slider(2, 20)]
        public int pointsRifleFirearmAdvanced = 10;

        [Name("Fire Starting : reading time")]
        [Description("Default Game Value: 5")]
        [Slider(1, 30)]
        public int timeFireStarting = 5;

        [Name("Fire Starting : points")]
        [Description("Default Game Value: 10")]
        [Slider(2, 20)]
        public int pointsFireStarting = 10;

        [Name("Gunsmithing : reading time")]
        [Description("Default Game Value: 5")]
        [Slider(1, 30)]
        public int timeGunsmithing = 5;

        [Name("Gunsmithing : points")]
        [Description("Default Game Value: 10")]
        [Slider(2, 20)]
        public int pointsGunsmithing = 10;

        [Name("Cooking : reading time")]
        [Description("Default Game Value: 5")]
        [Slider(1, 30)]
        public int timeCooking = 5;

        [Name("Cooking : points")]
        [Description("Default Game Value: 10")]
        [Slider(2, 20)]
        public int pointsCooking = 10;

        [Name("Mending : reading time")]
        [Description("Default Game Value: 5")]
        [Slider(1, 30)]
        public int timeMending = 5;

        [Name("Mending : points")]
        [Description("Default Game Value: 10")]
        [Slider(2, 20)]
        public int pointsMending = 10;

        [Name("Ice Fishing : reading time")]
        [Description("Default Game Value: 5")]
        [Slider(1, 30)]
        public int timeIceFishing = 5;

        [Name("Ice Fishing : points")]
        [Description("Default Game Value: 10")]
        [Slider(2, 20)]
        public int pointsIceFishing = 10;

        [Name("Carcass Harvesting : reading time")]
        [Description("Default Game Value: 10")]
        [Slider(1, 30)]
        public int timeCarcassHarvesting = 10;

        [Name("Carcass Harvesting : points")]
        [Description("Default Game Value: 10")]
        [Slider(2, 20)]
        public int pointsCarcassHarvesting = 10;
    }

    internal static class Settings
    {

        public static ResearchBookTweakerSettings settings;

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

