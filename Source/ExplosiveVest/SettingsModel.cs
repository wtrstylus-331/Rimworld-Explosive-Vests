using Verse;

namespace ExplosiveVest
{
    public class SettingsModel : ModSettings
    {
        public float bomberOneCommonality = 0.3f;
        public float bomberTwoCommonality = 0.18f;
        public float bomberThreeCommonality = 0.10f;

        public float bomberAntigrainCommonality = 0.05f;

        public float bomberGroupCommonality = 0.15f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref bomberGroupCommonality, "bomberGroupCommonality", 0.15f);

            Scribe_Values.Look(ref bomberOneCommonality, "bomberOneCommonality", 0.3f);
            Scribe_Values.Look(ref bomberTwoCommonality, "bomberTwoCommonality", 0.18f);
            Scribe_Values.Look(ref bomberThreeCommonality, "bomberThreeCommonality", 0.10f);

            Scribe_Values.Look(ref bomberAntigrainCommonality, "bomberAntigrainCommonality", 0.05f);
        }

        public void SetDefault()
        {
            bomberOneCommonality = 0.3f;
            bomberTwoCommonality = 0.18f;
            bomberThreeCommonality = 0.10f;

            bomberAntigrainCommonality = 0.05f;

            bomberGroupCommonality = 0.15f;
        }
    }
}
