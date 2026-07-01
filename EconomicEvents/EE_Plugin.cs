using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace EconomicEvents
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(MODSAVEBACKUPS_GUID, MODSAVEBACKUPS_VERSION)]
    public class EE_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude.economicevents";
        public const string PLUGIN_NAME = "EconomicEvents";
        public const string PLUGIN_VERSION = "1.3.2";

        public const string SAILADEX_GUID = "com.raddude.sailadex";
        public const string MODSAVEBACKUPS_GUID = "com.raddude.modsavebackups";
        public const string MODSAVEBACKUPS_VERSION = "1.2.0";

        internal static EE_Plugin Instance { get; private set; }
        private static ManualLogSource _logger;

        internal static void LogDebug(string message) => _logger.LogDebug(message);
        internal static void LogInfo(string message) => _logger.LogInfo(message);
        internal static void LogWarning(string message) => _logger.LogWarning(message);
        internal static void LogError(string message) => _logger.LogError(message);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _logger = Logger;

            Configs.InitializeConfigs();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);

            gameObject.AddComponent<EventScheduler>();
        }
    }
}
