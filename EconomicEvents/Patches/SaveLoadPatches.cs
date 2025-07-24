using HarmonyLib;
using ModSaveBackups;
using System;
using System.Collections.Generic;
using System.Linq;
using static EconomicEvents.EE_Plugin;

namespace EconomicEvents
{
    internal class SaveLoadPatches
    {
        [HarmonyPatch(typeof(SaveLoadManager))]
        private class SaveLoadManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("SaveModData")]
            public static void DoSaveGamePatch()
            {
                var saveContainer = new EconomicEventsSaveContainer
                {
                    loggedEventPorts = EventsUI.Instance.LoggedEventPorts.ToList(),

                    portsWithEvents = EventScheduler.Instance.PortsWithEvents.ToList(),

                    regionChance = EventScheduler.Instance.RegionChance.ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value)
                };

                ModSave.Save(Instance.Info, saveContainer);
            }

            [HarmonyPostfix]
            [HarmonyPatch("LoadModData")]
            public static void LoadModDataPatch()
            {
                if (!ModSave.Load(Instance.Info, out EconomicEventsSaveContainer saveContainer))
                {
                    LogWarning("Save file loading failed. If this is the first time loading this save with this mod, this is normal.");
                    return;
                }

                if (saveContainer.loggedEventPorts != null)
                    EventsUI.Instance.LoadLoggedEventPorts(saveContainer.loggedEventPorts);

                if (saveContainer.portsWithEvents != null)
                    EventScheduler.Instance.LoadPortsWithEvents(saveContainer.portsWithEvents);

                if (saveContainer.regionChance != null)
                    EventScheduler.Instance.LoadRegionChance(saveContainer.regionChance);
            }
        }

        public static void LoadDictionary<T>(Dictionary<int, T> saveDict, Dictionary<int, T> gameDict)
        {
            foreach (KeyValuePair<int, T> item in saveDict)
            {
                if (!gameDict.ContainsKey(item.Key))
                {
                    LogWarning($"LoadData: {item.Key} not found in game");
                    continue;
                }
                gameDict[item.Key] = item.Value;
            }
        }
    }

    [Serializable]
    public class EconomicEventsSaveContainer
    {
        public List<EventPort> loggedEventPorts;
        public List<EventPort> portsWithEvents;
        public Dictionary<int, int> regionChance;        
    }
}
