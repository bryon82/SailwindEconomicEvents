using HarmonyLib;
using ModSaveBackups;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
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
                var oldSavesFile = $"{Application.persistentDataPath}/slot{SaveSlots.currentSlot}/com.raddude82.economicevents.save";
                if (File.Exists(oldSavesFile))
                {
                    LogInfo($"Found old save file");
                    RenameOldSaves();
                }

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

        public static void RenameOldSaves()
        {
            string oldSavesDir = $"{Application.persistentDataPath}/slot{SaveSlots.currentSlot}";
            if (Directory.Exists(oldSavesDir))
            {
                foreach (var file in Directory.GetFiles(oldSavesDir))
                {
                    var newFileName = file.Replace("raddude82.economicevents", "raddude.economicevents");
                    LogInfo($"Renaming old economicevents save file {file} to {newFileName}");
                    File.Move(file, newFileName);
                }
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
