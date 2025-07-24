using HarmonyLib;
using UnityEngine;

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

            [HarmonyBefore(new string[] { "com.raddude82.sailadex" })]
            [HarmonyPostfix]
            [HarmonyPatch("SwitchMode")]
            public static void SwitchModePatches(MissionListMode mode)
            {
                eventsUI.SetActive(false);
                if (mode == MissionListMode.reputation)
                {
                    eventsUI.SetActive(true);
                    EventsUI.Instance.UpdateTexts();
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("HideUI")]
            public static void HideUIPatches()
            {
                eventsUI.SetActive(false);
            }
        }
    }
}
