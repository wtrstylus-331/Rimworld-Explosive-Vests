using RimWorld;
using Verse;

namespace ExplosiveVest
{
    [DefOf]
    public class EVWS_JobDefOf
    {
        public static JobDef EVWS_BomberCharge;

        static EVWS_JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(EVWS_JobDefOf));
        }
    }
}
