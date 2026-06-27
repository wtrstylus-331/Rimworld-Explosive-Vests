using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace ExplosiveVest
{
    [HarmonyPatch(typeof(JobGiver_AIFightEnemy), "TryGiveJob")]
    public static class Patch_FightEnemy
    {
        static bool Prefix(Pawn pawn, ref Job __result)
        {
            if (pawn?.Map == null || !pawn.Spawned)
                return true;

            if (!pawn.kindDef.defName.StartsWith("SuicideBomber"))
            {
                return true;
            }

            if (Rand.RangeInclusive(1, 10) <= 5)
            {
                return true;
            }

            if (!Util.RaidIsActivelyAttacking(pawn))
                return true;

            Pawn target = Util.FindRandomEnemy(pawn);
            if (target == null)
                return true;

            __result = JobMaker.MakeJob(EVWS_JobDefOf.EVWS_BomberCharge, target);
            return false;
        }
    }
}
