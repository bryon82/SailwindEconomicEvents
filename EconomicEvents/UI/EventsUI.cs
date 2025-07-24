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

        public void UpdateTexts()
        {
            if (LoggedEventPorts.Count == 0)
            {
                _eventTMs[$"event0"].text = "No active events";
                return;
            }

            if (LoggedEventPorts.Count > 4)
            {
                Event eEvent = Event.Events.Where(e => e.Id == LoggedEventPorts[0].AssignedEvent).FirstOrDefault();

                var items = string.Empty;
                foreach (var goodIndex in eEvent.GoodIndexes)
                {
                    var itemIndex = PrefabsDirectory.GoodToItemIndex(goodIndex);
                    var item = PrefabsDirectory.instance.GetItem(itemIndex);
                    item = item ?? PrefabsDirectory.instance.GetItem(goodIndex);
                    items += $"{item.name}, ";
                }

                var numDays = LoggedEventPorts[0].DayEventEnds - GameState.day;

                _eventTMs["event0"].text = $"{eEvent.Name}";
                _eventTMs["event0_description"].text = eEvent.EventText;
                _eventTMs["event0_items"].text = $"Items needed: <b>{items.Substring(0, items.Length - 2)}</b>";
                _eventTMs["event0_multiplier"].text = $"Price multiplier: <b>{eEvent.PriceMult}X</b>";
                _eventTMs["event0_duration"].text = numDays == 0 ? "<b>Ends today</b>" : $"<b>Ends in {numDays} days</b>";

                return;
            }

            for (int i = 0; i < EventRegion.AllRegions.Count; i++)
            {
                Event eEvent = null;
                var title = string.Empty;
                var description = string.Empty;                
                var items = string.Empty;
                var multiplier = string.Empty;
                var duration = string.Empty;

                if (LoggedEventPorts.ElementAtOrDefault(i) != null)
                {
                    eEvent = Event.Events.Where(e => e.Id == LoggedEventPorts[i].AssignedEvent).FirstOrDefault();

                    foreach (var goodIndex in eEvent.GoodIndexes)
                    {
                        var itemIndex = PrefabsDirectory.GoodToItemIndex(goodIndex);
                        var item = PrefabsDirectory.instance.GetItem(itemIndex);
                        items += $"{item.name}, ";
                    }

                    var numDays = LoggedEventPorts[i].DayEventEnds - GameState.day;

                    title = $"{eEvent.Name} in {LoggedEventPorts[i].Name}";
                    description = eEvent.EventText;                    
                    items = $"Items needed: <b>{items.Substring(0, items.Length - 2)}</b>";
                    multiplier = $"Price multiplier: <b>{eEvent.PriceMult}X</b>";
                    duration = numDays == 0 ? "<b>Ends today</b>" : $"<b>Ends in {numDays} days</b>";
                }

                _eventTMs[$"event{i}"].text = title;
                _eventTMs[$"event{i}_description"].text = description;                
                _eventTMs[$"event{i}_items"].text = items;
                _eventTMs[$"event{i}_multiplier"].text = multiplier;
                _eventTMs[$"event{i}_duration"].text = duration;
            }
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
