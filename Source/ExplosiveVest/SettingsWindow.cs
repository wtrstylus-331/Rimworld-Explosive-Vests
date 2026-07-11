using UnityEngine;
using Verse;

namespace ExplosiveVest
{
    [StaticConstructorOnStartup]
    public class SettingsWindow : Mod
    {
        public readonly SettingsModel settingModel;

        public static SettingsWindow Instance { get; private set; }

        public SettingsWindow(ModContentPack content)
            : base(content)
        {
            settingModel = GetSettings<SettingsModel>();
            Instance = this;
        }

        public override string SettingsCategory()
        {
            return "Explosive Vests";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            Text.Font = GameFont.Small;

            listing_Standard.Label((TaggedString)string.Format("{0}: {1}", "Bomber Pawn Group Raid Commonality", settingModel.bomberGroupCommonality.ToStringPercent()), -1f, "How common (%) raids consisting of groups of bomber pawns will appear.");
            settingModel.bomberGroupCommonality = listing_Standard.Slider(settingModel.bomberGroupCommonality, 0f, 1f);

            listing_Standard.Label((TaggedString)string.Format("{0}: {1}", "Bomber I Raider Weight", settingModel.bomberOneCommonality.ToStringPercent()), -1f, "Commonality (%) of the Bomber I Raider type in raids consisting of bomber pawns.");
            settingModel.bomberOneCommonality = listing_Standard.Slider(settingModel.bomberOneCommonality, 0f, 1f);

            listing_Standard.Label((TaggedString)string.Format("{0}: {1}", "Bomber II Raider Weight", settingModel.bomberTwoCommonality.ToStringPercent()), -1f, "Commonality (%) of the Bomber II Raider type in raids consisting of bomber pawns.");
            settingModel.bomberTwoCommonality = listing_Standard.Slider(settingModel.bomberTwoCommonality, 0f, 1f);

            listing_Standard.Label((TaggedString)string.Format("{0}: {1}", "Bomber III Raider Weight", settingModel.bomberThreeCommonality.ToStringPercent()), -1f, "Commonality (%) of the Bomber III Raider type in raids consisting of bomber pawns.");
            settingModel.bomberThreeCommonality = listing_Standard.Slider(settingModel.bomberThreeCommonality, 0f, 1f);

            listing_Standard.Label((TaggedString)string.Format("{0}: {1}", "Antigrain Bomber Pawn Group Raid Commonality", settingModel.bomberAntigrainCommonality.ToStringPercent()), -1f, "How common (%) raids consisting of a single antigrain bomber will appear.");
            settingModel.bomberAntigrainCommonality = listing_Standard.Slider(settingModel.bomberAntigrainCommonality, 0f, 1f);

            listing_Standard.GapLine(15f);
            if (listing_Standard.ButtonText("Reset to default settings"))
            {
                settingModel.SetDefault();
            }
            listing_Standard.End();
        }
    }
}
