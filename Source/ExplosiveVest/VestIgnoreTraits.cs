using RimWorld;
using Verse;

namespace ExplosiveVest
{
    public class VestIgnoreTraits : StatPart
    {
        public override string ExplanationPart(StatRequest req)
        {
            Pawn pawn = req.Thing as Pawn;
            if (pawn == null) return null;

            if (pawn.story?.traits != null &&
                (pawn.story.traits.HasTrait(TraitDefOf.Psychopath) ||
                 pawn.story.traits.HasTrait(TraitDefOf.Bloodlust)))
            {
                return "Pawn unaffected by wearing explosive vest.";
            }

            return null;
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!req.HasThing) { return; }

            Pawn pawn = req.Thing as Pawn;
            if (pawn == null) return;

            bool wearingVest = pawn.apparel?.WornApparel
                .Any(a => a.def.defName.StartsWith("EVWS_")) ?? false;

            if (!wearingVest)
                return;

            if (pawn.story?.traits != null)
            {
                if (pawn.story.traits.HasTrait(TraitDefOf.Psychopath) ||
                    pawn.story.traits.HasTrait(TraitDefOf.Bloodlust))
                {
                    UpdateVal(pawn, ref val);
                }
            }
        }

        private void UpdateVal(Pawn pawn, ref float val)
        {
            if (pawn.apparel.WornApparel.Any(a => a.def.defName == "EVWS_VestTierOne"))
            {
                val -= 0.10f;
            }

            if (pawn.apparel.WornApparel.Any(a => a.def.defName == "EVWS_VestTierTwo"))
            {
                val -= 0.15f;
            }

            if (pawn.apparel.WornApparel.Any(a => a.def.defName == "EVWS_VestTierThree"))
            {
                val -= 0.20f;
            }

            if (pawn.apparel.WornApparel.Any(a => a.def.defName == "EVWS_AntigrainVest"))
            {
                val -= 0.25f;
            }

            if (pawn.apparel.WornApparel.Any(a => a.def.defName == "EVWS_VestKid"))
            {
                val -= 0.12f;
            }
        }
    }
}
