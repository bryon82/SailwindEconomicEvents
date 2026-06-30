using System.Collections.Generic;
using UnityEngine;
using static EconomicEvents.EE_Plugin;

namespace EconomicEvents
{
    internal class EventsUI : MonoBehaviour
    {
        public static EventsUI Instance { get; private set; }

        internal List<EventPort> LoggedEventPorts { get; set; }

        private Dictionary<string, TextMesh> _eventTMs;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            _eventTMs = new Dictionary<string, TextMesh>();
            LoggedEventPorts = new List<EventPort>();
        }

        public void UpdatePage()
        {
            for (int i = 0; i < 4; i++)
            {
                _eventTMs[$"event{i}"].text = string.Empty;
                _eventTMs[$"event{i}_description"].text = string.Empty;
                _eventTMs[$"event{i}_items"].text = string.Empty;
                _eventTMs[$"event{i}_multiplier"].text = string.Empty;
                _eventTMs[$"event{i}_duration"].text = string.Empty;
            }

            if (LoggedEventPorts.Count == 0)
            {
                _eventTMs[$"event0"].text = "No active events";
                return;
            }

            if (Event.EventsById.TryGetValue(LoggedEventPorts[0].AssignedEvent, out var globalEvent) &&
                globalEvent.SpecificPorts.Length > 0 &&
                globalEvent.SpecificPorts[0] == 999)
            {
                UpdateTexts(0, true);
                return;
            }

            for (int i = 0; i < LoggedEventPorts.Count; i++)
            {
                UpdateTexts(i, false);
            }
        }

        private void UpdateTexts(int i, bool global)
        {
            if (!Event.EventsById.TryGetValue(LoggedEventPorts[i].AssignedEvent, out var eEvent))
            {
                return;
            }

            var items = eEvent.GetItemsDisplay();
            var numDays = LoggedEventPorts[i].DayEventEnds - GameState.day;

            _eventTMs[$"event{i}"].text = global ? $"{eEvent.Name}" : $"{eEvent.Name} in {LoggedEventPorts[i].Name}";
            _eventTMs[$"event{i}_description"].text = eEvent.EventText;
            _eventTMs[$"event{i}_items"].text = global ? $"Items affected: {items}" : $"Items needed: {items}";
            _eventTMs[$"event{i}_multiplier"].text = $"Price multiplier: <b>{eEvent.PriceMult}X</b>";
            _eventTMs[$"event{i}_duration"].text = numDays == 0 ? "<b>Ends today</b>" : $"<b>Ends in {numDays} days</b>";
        }

        internal void SetUIElems(Dictionary<string, TextMesh> eventTMs)
        {
            _eventTMs = eventTMs;
        }

        internal void SaveEventsUI()
        {
            ModData.AddEventPortListEntry($"{PLUGIN_GUID}.LoggedEventPorts", LoggedEventPorts);
        }

        internal void LoadEventsUI()
        {
            LoggedEventPorts = ModData.GetEventPortListEntry($"{PLUGIN_GUID}.LoggedEventPorts");
        }

        internal void LoadLoggedEventPorts(List<EventPort> loggedEventPorts)
        {
            LoggedEventPorts = loggedEventPorts;
        }
    }
}
