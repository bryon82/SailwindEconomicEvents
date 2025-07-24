using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EconomicEvents.EE_Plugin;
using static EconomicEvents.Configs;

namespace EconomicEvents
{
    internal class EventScheduler : MonoBehaviour
    {
        public static EventScheduler Instance { get; private set; }
        internal List<EventPort> PortsWithEvents { get; private set; }
        internal Dictionary<int, int> RegionChance { get; private set; }

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            RegionChance = new Dictionary<int, int>();
            foreach (EventRegion region in EventRegion.AllRegions)
            {
                RegionChance[region.Index] = eventInRegionBaseChance.Value;
            }
            Event.InitializeEvents();
            PortsWithEvents = new List<EventPort>();
            Sun.OnNewDay += UpdateEvents;
        }


        public void UpdateEvents()
        {
            PortsWithEvents.RemoveAll(port =>
            {
                if (GameState.day > port.DayEventEnds)
                {
                    port.DayEventStarts = -1;
                    port.DayEventEnds = -1;
                    port.AssignedEvent = -1;
                    var displayPort = EventsUI.Instance.LoggedEventPorts.Where(ep => ep.Index == port.Index).FirstOrDefault();
                    if (displayPort != null)
                        EventsUI.Instance.LoggedEventPorts.Remove(displayPort);

                    return true;
                }

                return false;
            });

            ScheduleEvents();
        }

        public void ScheduleEvents()
        {
            if (GameState.day % 14 == 0)
            {
                if (enableGlobalEvents.Value && !EventRegion.AnyAssignedEvents() && Random.Range(0, 100) < globalEventChance.Value)
                {
                    ScheduleGlobalEvent();
                    return;
                }

                foreach(EventRegion region in EventRegion.AllRegions)
                {
                    if (!region.HasAssignedEvent() && Random.Range(0, 100) < RegionChance[region.Index])
                    {
                        ScheduleEventInRegion(region);
                        RegionChance[region.Index] = eventInRegionBaseChance.Value;
                        LogDebug($"event scheduled in {region.Name} region on day {GameState.day}");
                    }
                        
                    if (!region.HasAssignedEvent())
                    {
                        RegionChance[region.Index] += Random.Range(5, 15);
                        LogDebug($"{region.Name} chance increased to {RegionChance[region.Index]} on day {GameState.day}");
                    }                        
                }                
            }
        }

        public void ScheduleGlobalEvent()
        {
            var eventPool = Event.Events
               .Where(e => e.SpecificPorts.Contains(999))
               .ToList();
            var selectedEvent = eventPool.ElementAt(Random.Range(0, eventPool.Count));
            var dayEventStarts = GameState.day + Random.Range(0, 10);

            foreach (var port in EventRegion.AllPorts)
            {
                port.AssignedEvent = selectedEvent.Id;
                port.DayEventStarts = dayEventStarts;
                port.DayEventEnds = dayEventStarts + selectedEvent.EventDuration;
                PortsWithEvents.Add(port);
            }
        }

        public void ScheduleEventInRegion(EventRegion region)
        {
            var selectedPort = region.Ports[Random.Range(0, region.Ports.Count)];            

            var eventPool = Event.Events
                .Where(e => e.SpecificPorts.Contains(selectedPort.Index) || e.SpecificPorts.Length == 0)
                .ToList();
            var selectedEvent = eventPool.ElementAt(Random.Range(0, eventPool.Count));

            var dayEventStarts = GameState.day + Random.Range(0, 10);
            selectedPort.AssignedEvent = selectedEvent.Id;
            selectedPort.DayEventStarts = dayEventStarts;
            selectedPort.DayEventEnds = dayEventStarts + selectedEvent.EventDuration;

            PortsWithEvents.Add(selectedPort);
        }

        internal void LoadPortsWithEvents(List<EventPort> portsWithEvents)
        {
            PortsWithEvents = portsWithEvents;
        }

        internal void LoadRegionChance(Dictionary<int, int> regionChance)
        {
            SaveLoadPatches.LoadDictionary(regionChance, RegionChance);
        }
    }
}
