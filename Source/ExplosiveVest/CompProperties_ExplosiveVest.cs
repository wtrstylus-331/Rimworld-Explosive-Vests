using Verse;

namespace ExplosiveVest
{
    public class CompProperties_ExplosiveVest : CompProperties
    {
        public float minRadius;
        public float maxRadius;
        public float minDamage;
        public float maxDamage;
        public float fireChance;
        public float damChance;
        public CompProperties_ExplosiveVest()
        {
            compClass = typeof(Comp_ExplosiveVest);
        }
    }
}
