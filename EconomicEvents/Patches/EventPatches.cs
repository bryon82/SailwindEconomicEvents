using HarmonyLib;

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
                EventPort eventPort = null;
                var portIndex = __instance.GetPortIndex();
                foreach (var port in EventScheduler.Instance.PortsWithEvents)
                {
                    if (port.Index == portIndex && port.IsEventActive())
                    {
                        eventPort = port;
                        break;
                    }
                }

                if (eventPort == null)                
                    return;                

                Event.EventsById.TryGetValue(eventPort.AssignedEvent, out var eEvent);
                if (eEvent != null && eEvent.GoodIndexSet.Contains(goodIndex))
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

                var islandScene = __instance.GetComponentInParent<IslandSceneryScene>();
                var portIndex = Refs.islands[islandScene.parentIslandIndex].GetComponentInChildren<Port>().portIndex;
                EventPort eventPort = null;
                foreach (var port in EventScheduler.Instance.PortsWithEvents)
                {
                    if (port.Index == portIndex && port.IsEventActive())
                    {
                        eventPort = port;
                        break;
                    }
                }

                if (eventPort == null)                
                    return;                

                Event.EventsById.TryGetValue(eventPort.AssignedEvent, out var eEvent);
                if (eEvent == null)                
                    return;                

                var prefabIndex = item.gameObject.GetComponent<SaveablePrefab>().prefabIndex;
                var hasCrateIndex = Event.ItemToCrateConversion.TryGetValue(prefabIndex, out var crateIndex);
                if (eEvent.GoodIndexSet.Contains(prefabIndex) || (hasCrateIndex && eEvent.GoodIndexSet.Contains(crateIndex)))
                {
                    __result *= eEvent.PriceMult;
                }
            }
        }
    }
}
