using BepInEx.Configuration;

namespace EconomicEvents
{
    internal class Configs
    {
        internal static ConfigEntry<int> eventInRegionBaseChance;
        internal static ConfigEntry<bool> enableGlobalEvents;
        internal static ConfigEntry<int> globalEventChance;

        internal static void InitializeConfigs()
        {
            var config = EE_Plugin.Instance.Config;

            eventInRegionBaseChance = config.Bind(
                "Settings",
                "BaseEventInRegionChance",
                20,
                new ConfigDescription("Base chance for an event to occur in a region.", new AcceptableValueRange<int>(0, 100)));

            enableGlobalEvents = config.Bind(
                "Settings",
                "EnableGlobalEvents",
                true,
                "Enable global events that affect all ports at once. If disabled, only regional events will occur.");

            globalEventChance = config.Bind(
                "Settings",
                "GlobalEventChance",
                5,
                new ConfigDescription("Chance for a global event to occur.", new AcceptableValueRange<int>(0, 100)));
        }
    }
}
