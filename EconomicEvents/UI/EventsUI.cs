using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            if (Event.Events.FirstOrDefault(e => e.Id == LoggedEventPorts.ElementAt(0).AssignedEvent).SpecificPorts.ElementAtOrDefault(0) == 999)
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
            var eEvent = Event.Events.FirstOrDefault(e => e.Id == LoggedEventPorts.ElementAt(i).AssignedEvent);

            var items = string.Empty;
            foreach (var goodIndex in eEvent.GoodIndexes)
            {
                var itemIndex = PrefabsDirectory.GoodToItemIndex(goodIndex);
                var item = PrefabsDirectory.instance.GetItem(itemIndex);
                item = item ?? PrefabsDirectory.instance.GetItem(goodIndex);
                items += $"<b>{item.name}</b>, ";
            }
            items = items.Substring(0, items.Length - 2);

            var numDays = LoggedEventPorts.ElementAt(i).DayEventEnds - GameState.day;

            _eventTMs[$"event{i}"].text = global ? $"{eEvent.Name}" : $"{eEvent.Name} in {LoggedEventPorts.ElementAt(i).Name}";
            _eventTMs[$"event{i}_description"].text = eEvent.EventText;
            _eventTMs[$"event{i}_items"].text = global ? $"Items affected: {items}" : $"Items needed: {items}";
            _eventTMs[$"event{i}_multiplier"].text = $"Price multiplier: <b>{eEvent.PriceMult}X</b>";
            _eventTMs[$"event{i}_duration"].text = numDays == 0 ? "<b>Ends today</b>" : $"<b>Ends in {numDays} days</b>";
        }

        internal void SetUIElems(Dictionary<string, TextMesh> eventTMs)
        {
            _eventTMs = eventTMs;
        }

        internal void LoadLoggedEventPorts(List<EventPort> loggedEventPorts)
        {
            LoggedEventPorts = loggedEventPorts;
        }
    }
}
