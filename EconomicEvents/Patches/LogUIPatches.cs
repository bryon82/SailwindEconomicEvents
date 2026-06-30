using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using static EconomicEvents.EE_Plugin;

namespace EconomicEvents
{
    internal class LogUIPatches
    {
        private static GameObject eventsUI;
        private static GPButtonLogMode[] logModeButtons;

        [HarmonyPatch(typeof(MissionListUI))]
        private class MissionListUIPatches
        {
            [HarmonyAfter(SAILADEX_GUID)]
            [HarmonyPrefix]
            [HarmonyPatch("Start")]
            public static void StartPatch(GameObject ___modeButtons, ref GPButtonLogMode[] ___logModeButtons, GameObject ___reputationUI)
            {
                var newLogModeButtons = new List<GPButtonLogMode>(___logModeButtons);
                logModeButtons = UIBuilder.MakeBookmark(___modeButtons, newLogModeButtons);
                eventsUI = UIBuilder.MakeEventsUI(___reputationUI);
                ___logModeButtons = logModeButtons;
            }

            [HarmonyAfter(SAILADEX_GUID)]
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
