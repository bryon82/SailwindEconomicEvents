using HarmonyLib;
using UnityEngine;
using static EconomicEvents.Configs;

namespace EconomicEvents
{
    internal class LogUIPatches
    {
        private static GameObject eventsUI;

        [HarmonyPatch(typeof(MissionListUI))]
        private class MissionListUIPatches
        {
            

            [HarmonyPrefix]
            [HarmonyPatch("Start")]
            public static void StartPatch(GameObject ___modeButtons, GameObject ___reputationUI)
            {
                eventsUI = UIBuilder.MakeEventsUI(___reputationUI, ___modeButtons);
            }
        }
    }
}
