using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ExplosiveVest
{
    public class HediffCompProperties_PrimedExplosives : HediffCompProperties
    {
        public float minRadius;
        public float maxRadius;
        public float minDamage;
        public float maxDamage;
        public float fireChance;
        public float damChance;
        public HediffCompProperties_PrimedExplosives() {
            compClass = typeof(Comp_PrimedExplosives);
        }
    }
}
