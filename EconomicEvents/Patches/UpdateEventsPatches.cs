using HarmonyLib;
using UnityEngine;

namespace EconomicEvents
{
    internal class UpdateEventsPatches
    {
        private static bool _initialized = false;

        [HarmonyPatch(typeof(BuyItemUI))]
        private class BuyItemUIPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("Awake")]
            public static void CopyMenu(BuyItemUI __instance)
            {
                if (_initialized) 
                    return;

                _initialized = true;

                var updateEventsUI = GameObject.Instantiate(__instance.menu.transform.parent.gameObject);
                updateEventsUI.name = "update events ui";
                Object.Destroy(updateEventsUI.GetComponent<BuyItemUI>());
                updateEventsUI.AddComponent<UpdateEventsUI>();
                var menu = updateEventsUI.transform.GetChild(0);
                Object.Destroy(menu.GetChild(0).GetChild(0).gameObject.GetComponent<GPButtonBuyItem>());
                menu.GetChild(0).GetChild(0).gameObject.AddComponent<GPButtonUpdateEvents>();
                menu.GetChild(0).GetChild(1).GetComponent<TextMesh>().text = "update events";
                menu.GetChild(1).GetChild(1).GetComponent<TextMesh>().text = "Have you heard the latest\nnews?";

                UpdateEventsUI.Instance.UpdateEventsMenu = menu.gameObject;
            }
        }

        [HarmonyPatch(typeof(PortDude))]
        private class PortDudePatches 
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnTriggerEnter")]
            public static void OnTriggerEnterPatch(PortDude __instance, Collider other)
            {
                if (other.CompareTag("Player"))
                {
                    var loggedPorts = EventsUI.Instance.LoggedEventPorts;
                    foreach (var port in EventScheduler.Instance.PortsWithEvents)
                    {
                        if (!port.IsEventActive())                        
                            continue;

                        var found = false;
                        for (var i = 0; i < loggedPorts.Count; i++)
                        {
                            if (loggedPorts[i].Index == port.Index)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            UpdateEventsUI.Instance.ActivateUI(__instance);
                            break;
                        }
                    }
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnTriggerExit")]
            public static void OnTriggerExitPatch(Collider other)
            {
                if (other.CompareTag("Player"))
                {
                    UpdateEventsUI.Instance.DeactivateUI();
                }
            }
        }
    }
}
