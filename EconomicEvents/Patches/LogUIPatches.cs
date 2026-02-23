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
                UIBuilder.MakeBookmark(___modeButtons);
                eventsUI = UIBuilder.MakeEventsUI(___reputationUI);
            }

            [HarmonyAfter(new string[] { "com.raddude82.sailadex" })]
            [HarmonyPostfix]
            [HarmonyPatch("SwitchMode")]
            public static void SwitchModePatches(MissionListMode mode)
            {
                eventsUI.SetActive(false);
                if (mode == UIBuilder.EVENTS)
                {
                    eventsUI.SetActive(true);
                    EventsUI.Instance.UpdatePage();
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
