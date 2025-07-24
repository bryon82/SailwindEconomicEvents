using HarmonyLib;
using System.Linq;
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
                if (_initialized) return;
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
                    var activeEvents = EventScheduler.Instance.PortsWithEvents.Where(ep => ep.IsEventActive()).ToList();
                    
                    if (!activeEvents.All(ae => EventsUI.Instance.LoggedEventPorts.Any(le => le.Index == ae.Index)))
                        UpdateEventsUI.Instance.ActivateUI(__instance);                    
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
