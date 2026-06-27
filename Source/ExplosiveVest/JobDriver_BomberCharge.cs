using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace ExplosiveVest
{
    public class JobDriver_BomberCharge : JobDriver
    {
        private Pawn TargetPawn => job.targetA.Thing as Pawn;

        private const float DetonationRange = 2.75f;

        private static readonly string[] BomberLines =
        {
            "Time to die! ",
            "Aaaagghhh!",
            "I regret nothing!",
            "Have a piece of this!",
            ""
        };

        private string GenerateTextMote()
        {
            return BomberLines.RandomElement();
        }


        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }


        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOn(() => TargetPawn == null || TargetPawn.Dead);

            Toil move = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            move.tickAction = () =>
            {
                if (TargetPawn == null || TargetPawn.Dead)
                {
                    ReadyForNextToil();
                    return;
                }

                float dist = pawn.Position.DistanceTo(TargetPawn.Position);

                if (dist <= DetonationRange)
                {
                    ReadyForNextToil();
                }
            };

            yield return move;

            Toil warning = new Toil();
            warning.initAction = () =>
            {
                MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, GenerateTextMote());
            };
            warning.defaultDuration = 60;
            warning.defaultCompleteMode = ToilCompleteMode.Delay;
            yield return warning;

            Toil explode = new Toil();
            explode.initAction = () => DetonateVest();
            explode.defaultCompleteMode = ToilCompleteMode.Instant;

            yield return explode;
        }

        private void DetonateVest()
        {
            var vest = pawn.apparel?.WornApparel?
                .FirstOrDefault(a => a.TryGetComp<Comp_ExplosiveVest>() != null);

            var comp = vest?.TryGetComp<Comp_ExplosiveVest>();

            if (comp != null && !comp.alreadyDetonated)
            {
                comp.Detonate();
            }
            else
            {
                GenExplosion.DoExplosion(
                    pawn.Position,
                    pawn.Map,
                    4.9f,
                    DamageDefOf.Bomb,
                    pawn,
                    50
                );
            }

            if (!pawn.Dead)
                pawn.Kill(null);
        }
    }
}
