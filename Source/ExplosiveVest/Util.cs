using RimWorld;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace ExplosiveVest
{
    public static class Util
    {
        public static Pawn FindRandomEnemy(Pawn bomber)
        {
            if (bomber?.Map == null)
                return null;

            var enemies = bomber.Map.mapPawns.AllPawnsSpawned
                .Where(p =>
                    p.Faction == Faction.OfPlayer &&
                    !p.Dead &&
                    !p.Downed)
                .ToList();

            return enemies.RandomElementWithFallback();
        }

        public static bool RaidIsActivelyAttacking(Pawn pawn)
        {
            var lord = pawn.GetLord();
            if (lord == null)
                return true;

            if (lord.LordJob is LordJob_Siege)
                return false;

            var duty = pawn.mindState.duty?.def;
            if (duty == DutyDefOf.AssaultColony)
                return true;

            if (duty == DutyDefOf.Defend)
                return false;

            if (lord.CurLordToil is LordToil_DefendPoint)
                return false;

            return true;
        }


    }
}
