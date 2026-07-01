using System;
using System.Collections.Generic;
using System.Text;
using static EconomicEvents.EE_Plugin;

namespace EconomicEvents
{
    internal class ModData
    {
        public static void AddDictEntry<K, T>(string dataName, Dictionary<K, T> data)
        {
            var sb = new StringBuilder();
            foreach (var kvp in data)
                sb.AppendLine($"{kvp.Key}|{kvp.Value}");
            var dataString = sb.ToString();

            if (GameState.modData.ContainsKey(dataName))
                GameState.modData[dataName] = dataString;
            else
                GameState.modData.Add(dataName, dataString);
        }

        public static void GetDictEntry<K, T>(string dataName, Dictionary<K, T> gameDict)
        {
            if (!GameState.modData.ContainsKey(dataName))
            {
                LogWarning($"LoadDictFromModData: {dataName} not found in modData");
                return;
            }

            SaveLoadPatches.loadedNewData = true;
            var dataString = GameState.modData[dataName];
            var lines = dataString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Trim().Split('|');
                if (parts.Length < 2)
                    continue;

                var key = (K)Convert.ChangeType(parts[0], typeof(K));

                if (!gameDict.ContainsKey(key))
                {
                    LogWarning($"LoadDictFromModData: {key} not found in game");
                    continue;
                }

                gameDict[key] = (T)Convert.ChangeType(parts[1], typeof(T));
            }
        }

        public static void AddEventPortListEntry(string dataName, List<EventPort> data)
        {
            var sb = new StringBuilder();
            foreach (var item in data)
                sb.AppendLine($"{item.SaveString()}");
            var dataString = sb.ToString();
            if (GameState.modData.ContainsKey(dataName))
                GameState.modData[dataName] = dataString;
            else
                GameState.modData.Add(dataName, dataString);
        }

        public static List<EventPort> GetEventPortListEntry(string dataName)
        {
            var result = new List<EventPort>();
            if (!GameState.modData.ContainsKey(dataName))
            {
                LogWarning($"GetModDataEntry: {dataName} not found in modData");
                return result;
            }
            var dataString = GameState.modData[dataName];
            var lines = dataString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Trim().Split('|');
                if (parts.Length < 5)
                    continue;
                var port = new EventPort(int.Parse(parts[0]), parts[1])
                {
                    AssignedEvent = int.Parse(parts[2]),
                    DayEventStarts = int.Parse(parts[3]),
                    DayEventEnds = int.Parse(parts[4])
                };
                result.Add(port);
            }
            return result;
        }
    }
}
