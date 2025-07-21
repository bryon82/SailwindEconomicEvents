using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace EconomicEvents
{
    internal class EventPatches
    {
        [HarmonyPatch(typeof(IslandMarket))]
        private class IslandMarketPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("GetGoodPriceAtSupply")]
            public static void EventPriceAdjust(int goodIndex, IslandMarket __instance, ref int __result)
            {
                var eventPort = EventScheduler.Instance.PortsWithEvents
                    .Where(p => p.Index == __instance.GetPortIndex() && p.IsEventActive())
                    .FirstOrDefault();

                var eEvent = eventPort == null ? null :
                    Event.Events.Where(e => e.Id == eventPort.AssignedEvent).FirstOrDefault();

                if (eventPort != null && eEvent.GoodIndexes.Contains(goodIndex))
                {
                    __result *= eEvent.PriceMult;
                }
            }
        }

        [HarmonyPatch(typeof(EconomyUI))]
        private class EconomyUIPatches
        {
            [HarmonyPatch("OpenUI")]
            public static void Postfix()
            {
                EventsUI.Instance.DisplayEventPorts = EventScheduler.Instance.PortsWithEvents.ToList();
            }
        }
    }
}
