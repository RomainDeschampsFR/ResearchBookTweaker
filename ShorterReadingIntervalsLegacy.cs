using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection; 
using System.Collections;

namespace ShorterReadingIntervalsLegacy
{
	public class ShorterReadingIntervalsLegacy : MelonMod
	{
		public override void OnInitializeMelon()
		{
            Debug.Log($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();
        }
    }
}