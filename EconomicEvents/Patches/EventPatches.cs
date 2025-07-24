using HarmonyLib;
using System.Linq;

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

        [HarmonyPatch(typeof(Shopkeeper))]
        private class ShopkeeperPatches 
        {
            [HarmonyPostfix]
            [HarmonyPatch("GetPrice")]
            public static void EventPriceAdjust(ShipItem item, Shopkeeper __instance, ref int __result)
            {
                if (item.IsBulk())                
                    return;
                
                var eventPort = EventScheduler.Instance.PortsWithEvents
                    .Where(p => p.Index == Refs.islands[__instance.GetComponentInParent<IslandSceneryScene>().parentIslandIndex].GetComponentInChildren<Port>().portIndex && p.IsEventActive())
                    .FirstOrDefault();
                var eEvent = eventPort == null ? null :
                    Event.Events.Where(e => e.Id == eventPort.AssignedEvent).FirstOrDefault();
                if (eventPort != null && 
                    (eEvent.GoodIndexes.Contains(item.gameObject.GetComponent<SaveablePrefab>().prefabIndex) ||
                    eEvent.GoodIndexes.Contains(Event.ItemToCrateConversion.TryGetValue(item.gameObject.GetComponent<SaveablePrefab>().prefabIndex, out int crateIndex) ? crateIndex : -1)
                    ))
                {
                    __result *= eEvent.PriceMult;
                }
            }
        }
    }
}
