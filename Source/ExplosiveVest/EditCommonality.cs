using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace ExplosiveVest
{
    [StaticConstructorOnStartup]
    public class EditCommonality
    {
        static EditCommonality()
        {
            SettingsModel model = LoadedModManager.GetMod<SettingsWindow>().GetSettings<SettingsModel>();

            float bomberGroupCommonality = PercentToRange(model.bomberGroupCommonality, 5f, 30f);

            float bomberOneCommonality = PercentToRange(model.bomberOneCommonality, 2f, 24f);
            float bomberTwoCommonality = PercentToRange(model.bomberTwoCommonality, 1f, 18f);
            float bomberThreeCommonality = PercentToRange(model.bomberThreeCommonality, 1f, 12f);

            float bomberAntigrainCommonality = PercentToRange(model.bomberAntigrainCommonality, 1f, 15f);


            foreach (FactionDef faction in DefDatabase<FactionDef>.AllDefs)
            {
                if (faction.defName == "PirateBandBase"
                    || faction.defName == "PirateWaster"
                    || faction.defName == "PirateYttakin")
                {
                    foreach (PawnGroupMaker group in faction.pawnGroupMakers)
                    {
                        if (!IsBomberGroup(group))
                            continue;

                        if (IsAntigrainGroup(group))
                        {
                            group.commonality = bomberAntigrainCommonality;
                        }
                        else
                        {
                            SetRegularGroup(group, bomberGroupCommonality, bomberOneCommonality, bomberTwoCommonality, bomberThreeCommonality);
                        }
                    }
                }
            }
        }

        static void SetRegularGroup(PawnGroupMaker group, float commonality, float b1, float b2, float b3)
        {
            group.commonality = commonality;

            foreach (var option in group.options)
            {
                switch (option.kind.defName)
                {
                    case "SuicideBomberOne":
                        option.selectionWeight = b1;
                        break;

                    case "SuicideBomberTwo":
                        option.selectionWeight = b2;
                        break;

                    case "SuicideBomberThree":
                        option.selectionWeight = b3;
                        break;
                }
            }
        }

        static bool IsAntigrainGroup(PawnGroupMaker group)
        {
            return group.options.Any(option =>
                option.kind.defName == "SuicideBomberAntigrain"
            );
        }

        static bool IsBomberGroup(PawnGroupMaker group)
        {
            foreach (var option in group.options)
            {
                string defName = option.kind.defName;

                if (defName == "SuicideBomberOne"
                    || defName == "SuicideBomberTwo"
                    || defName == "SuicideBomberThree"
                    || defName == "SuicideBomberAntigrain")
                {
                    return true;
                }
            }

            return false;
        }

        static float PercentToRange(float percent, float min, float max)
        {
            if (percent <= 0f)
                return 0f;

            percent = Mathf.Clamp01(percent);
            return min + percent * (max - min);
        }

    }
}
