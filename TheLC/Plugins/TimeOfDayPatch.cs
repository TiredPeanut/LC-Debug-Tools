using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLC.Plugins
{
    [HarmonyPatch(typeof(TimeOfDay))]
    public sealed class TimeOfDayPatch
    {
        [HarmonyPatch("MoveGlobalTime")]
        [HarmonyPostfix]
        public static void InfiniteDeadline(ref float ___timeUntilDeadline)
        {
            if (ModSettings.InfiniteDeadlineConfig.Value)
                ___timeUntilDeadline = 10000f;
        }
    }
}
