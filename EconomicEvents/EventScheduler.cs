using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EconomicEvents
{
    internal class EventScheduler : MonoBehaviour
    {
        public static EventScheduler Instance { get; private set; }
        internal List<EventPort> PortsWithEvents { get; private set; }
        
        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            PortsWithEvents = new List<EventPort>();
            Sun.OnNewDay += UpdateEvents;
        }


        public void UpdateEvents()
        {
            foreach (EventPort port in PortsWithEvents)
            {
                if (GameState.day > port.DayEventEnds)
                {
                    port.DayEventStarts = -1;
                    port.DayEventEnds = -1;
                    port.AssignedEvent = -1;
                    PortsWithEvents.Remove(port);
                    var displayPort = EventsUI.Instance.DisplayEventPorts.Where(ep => ep.Index == port.Index).FirstOrDefault();
                    if (displayPort != null) 
                        EventsUI.Instance.DisplayEventPorts.Remove(displayPort);
                }                    
            }

            ScheduleEvents();
        }

        public void ScheduleEvents()
        {
            if (GameState.day % 30 == 0)
            {
                foreach(EventRegion region in EventRegion.AllRegions)
                {
                    if (!region.HasAssignedEvent() && Random.Range(0, 100) > region.)
                        ScheduleEventInRegion(region);
                }                
            }
        }

        public void ScheduleEventInRegion(EventRegion region)
        {
            var selectedPort = region.Ports[Random.Range(0, region.Ports.Count)];
            
            var eventPool = Event.Events
                .Where(e => e.SpecificPorts.Contains(selectedPort.Index) || e.SpecificPorts == null)
                .ToList();
            var selectedEvent = eventPool[Random.Range(0, eventPool.Count)];

            var dayEventStarts = GameState.day + Random.Range(0, 10);
            selectedPort.AssignedEvent = selectedEvent.Id;
            selectedPort.DayEventStarts = dayEventStarts;
            selectedPort.DayEventEnds = dayEventStarts + selectedEvent.EventDuration;

            PortsWithEvents.Add(selectedPort);
        }
    }
}
