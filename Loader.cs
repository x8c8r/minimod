using BepInEx;
using BepInEx.IL2CPP;
using UnhollowerRuntimeLib;
using UnityEngine;
using CodeStage.AntiCheat.Detectors;

namespace MiniMod
{
    [BepInPlugin("xyz.fl1pnatic.minimod", "MiniMod", "2.0.2")]
    [BepInProcess("Crab Game.exe")]
    public class Loader : BasePlugin
    {
        public override void Load()
        {
            ClassInjector.RegisterTypeInIl2Cpp<Main>();
            new GameObject("MiniMod") { hideFlags = (HideFlags)61 }.AddComponent<Main>();
        }
    }
}
