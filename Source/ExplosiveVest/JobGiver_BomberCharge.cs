using Verse;
using Verse.AI;

namespace ExplosiveVest
{
    public class JobGiver_BomberCharge : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (!pawn.kindDef.defName.StartsWith("SuicideBomber"))
                return null;

            if (!Util.RaidIsActivelyAttacking(pawn))
                return null;

            Pawn target = Util.FindRandomEnemy(pawn);
            if (target == null)
                return null;

            return JobMaker.MakeJob(EVWS_JobDefOf.EVWS_BomberCharge, target);
        }
    }
}
