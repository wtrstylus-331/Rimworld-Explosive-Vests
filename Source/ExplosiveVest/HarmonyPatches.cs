using HarmonyLib;
using Verse;

namespace ExplosiveVest
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static Harmony _harmonyInstance;

        public static Harmony HarmonyInstance => _harmonyInstance;

        static HarmonyPatches()
        {
            _harmonyInstance = new Harmony("com.waterstylus331.explosivevest");
            _harmonyInstance.PatchAll();
        }
    }
}
