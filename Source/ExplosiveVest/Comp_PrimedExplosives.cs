using RimWorld;
using System.Collections.Generic;
using Verse;

namespace ExplosiveVest
{
    public class Comp_PrimedExplosives : HediffComp
    {
        private HediffCompProperties_PrimedExplosives properties => props as HediffCompProperties_PrimedExplosives;
        public Pawn wearer => parent.pawn;
        public bool alreadyDetonated = false;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref alreadyDetonated, "alreadyDetonatedEImplant", false);
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if (base.CompGetGizmos() != null)
            {
                foreach (var g in base.CompGetGizmos())
                    yield return g;
            }

            if (wearer is Pawn p
                && !p.DeadOrDowned
                && p.Drafted
                && p.Faction == Faction.OfPlayer
                && (p.IsColonist || p.IsPrisonerOfColony || p.IsSlaveOfColony))
            {
                if (!alreadyDetonated)
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "Detonate",
                        defaultDesc = "Initiate the implant to detonate",
                        icon = ContentFinder<UnityEngine.Texture2D>.Get("UI/Icons/Detonate_Vest", true),
                        action = () =>
                        {
                            this.Detonate();
                        },
                        hotKey = KeyBindingDefOf.Misc8
                    };
                }
            }
        }

        public void Detonate()
        {
            IntVec3 pos = wearer.PositionHeld;
            Map map = wearer?.MapHeld;

            if (map == null)
                map = wearer.MapHeld;

            if (map == null)
                map = wearer.Map;

            if (map == null)
                return;

            if (wearer != null && !wearer.DeadOrDowned)
            {
                pos = wearer.Position;
                map = wearer.Map;
            }

            GenExplosion.DoExplosion(
                pos,
                map,
                Rand.Range(properties.minRadius, properties.maxRadius),
                DamageDefOf.Bomb,
                null,
                damAmount: (int)Rand.Range(properties.minDamage, properties.maxDamage),
                chanceToStartFire: properties.fireChance
            );

            DamageInfo dInfo = new DamageInfo(
                DamageDefOf.Bomb,
                (int)Rand.Range(properties.minRadius, properties.maxRadius),
                0.5f,
                -1,
                null
                );

            alreadyDetonated = true;

            if (wearer != null)
            {
                wearer.Kill(dInfo);
                wearer.health.RemoveHediff(parent);
            }
        }
    }
}
