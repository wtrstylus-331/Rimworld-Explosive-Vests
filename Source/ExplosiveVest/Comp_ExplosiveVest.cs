using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace ExplosiveVest
{
    public class Comp_ExplosiveVest : ThingComp
    {
        private CompProperties_ExplosiveVest properties => props as CompProperties_ExplosiveVest;
        public Pawn wearer => (parent as Apparel)?.Wearer;
        public bool alreadyDetonated = false;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref alreadyDetonated, "alreadyDetonatedEVest", false);
        }

        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            foreach (var g in base.CompGetWornGizmosExtra())
            {
                yield return g;
            }

            if ((wearer != null && !wearer.DeadOrDowned) && wearer.Drafted && (wearer.Faction == Faction.OfPlayer) && (wearer.IsColonist || wearer.IsPrisonerOfColony || wearer.IsSlaveOfColony))
            {
                if (!alreadyDetonated)
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "EV_DetonateVestLabel".Translate(),
                        defaultDesc = "EV_DetontateVestDesc".Translate(),
                        icon = ContentFinder<UnityEngine.Texture2D>.Get("UI/Icons/Detonate_Vest"),
                        action = () =>
                        {
                            this.Detonate();
                        },
                        hotKey = KeyBindingDefOf.Misc8
                    };
                }
            }
        }

        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            base.PostPreApplyDamage(ref dinfo, out absorbed);

            absorbed = false;

            if (alreadyDetonated)
            {
                return;
            }

            if (wearer == null)
            {
                if (parent.Map != null)
                {
                    SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(parent.PositionHeld, parent.MapHeld));
                    if (CanRandomelyDetonate())
                    {
                        Detonate();
                        absorbed = true;
                    }
                    return;
                }

                return;
            }
 
            if (wearer.DeadOrDowned) { return; }

            if (ValidGrounds(dinfo))
            {
                Detonate();
                return;
            }

            if (CanRandomelyDetonate())
            {
                Detonate();
                absorbed = true;
            }
        }

        private bool ValidGrounds(DamageInfo dinfo)
        {
            bool torsoHit = dinfo.HitPart != null && dinfo.HitPart.def == BodyPartDefOf.Torso;

            bool explosiveHit = dinfo.Def != null && dinfo.Def == DamageDefOf.Bomb;

            bool bulletHit = dinfo.Def != null && dinfo.Def == DamageDefOf.Bullet;

            return torsoHit || (torsoHit && bulletHit) || explosiveHit;

        }

        private bool CanRandomelyDetonate()
        {
            float currentHP = parent.HitPoints;
            float maxHP = parent.MaxHitPoints;

            float hpPercent = currentHP / maxHP;
            float chance = (1f - hpPercent) * Math.Max(1f, properties.damChance);

            return Rand.Value < chance;
        }

        public void Detonate()
        {
            IntVec3 pos = parent.PositionHeld;
            Map map = wearer?.MapHeld;

            if (map == null)
                map = parent.MapHeld;

            if (map == null)
                map = parent.Map;

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

            alreadyDetonated = true;
            parent.Destroy(DestroyMode.Vanish);
        }
    }
}
