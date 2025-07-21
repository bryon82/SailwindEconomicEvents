using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EconomicEvents
{
    internal class EventsUI : MonoBehaviour
    {
        public static EventsUI Instance { get; private set; }

        internal List<EventPort> DisplayEventPorts { get; set; }

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
            DisplayEventPorts = new List<EventPort>();
        }

        public void UpdateTexts()
        {
            var i = 0;
            foreach (var _ in EventRegion.AllRegions)
            {
                Event eEvent = null;
                string description = string.Empty;
                string items = string.Empty;
                string multiplier = string.Empty;
                string duration = string.Empty;
                if (DisplayEventPorts[i] != null)
                {
                    eEvent = Event.Events.Where(e => e.Id == EventScheduler.Instance.PortsWithEvents[i].AssignedEvent).FirstOrDefault();

                    foreach (var goodIndex in eEvent.GoodIndexes)
                    {
                        var itemIndex = PrefabsDirectory.GoodToItemIndex(goodIndex);
                        var item = PrefabsDirectory.instance.GetItem(itemIndex);
                        items += $"{item.name}, ";
                    }

                    var numDays = EventScheduler.Instance.PortsWithEvents[i].DayEventEnds - GameState.day;                    

                    description = eEvent.EventText;
                    items = $"Items needed: {items.Trim().Substring(0, items.Length - 1)}";
                    multiplier = $"Price multiplier: {eEvent.PriceMult}X";
                    duration = numDays == 0 ? "Ends today" : $"Ends in {numDays} days";

                }
                _eventTMs[$"event{i}"].text = description;
                _eventTMs[$"event{i}_items"].text = items;
                _eventTMs[$"event{i}_multiplier"].text = multiplier;
                _eventTMs[$"event{i}_duration"].text = duration;
            }
        }

        internal void SetUIElems(Dictionary<string, TextMesh> eventTMs)
        {
            _eventTMs = eventTMs;
        }
    }
}
