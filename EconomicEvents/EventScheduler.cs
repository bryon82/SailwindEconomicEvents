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
        private readonly List<Event> _eventBuffer = new List<Event>();

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
                if (GameState.day <= port.DayEventEnds)
                    return false;

                port.DayEventStarts = -1;
                port.DayEventEnds = -1;
                port.AssignedEvent = -1;

                var loggedPorts = EventsUI.Instance.LoggedEventPorts;
                for (var i = loggedPorts.Count - 1; i >= 0; i--)
                {
                    if (loggedPorts[i].Index == port.Index)
                    {
                        loggedPorts.RemoveAt(i);
                        break;
                    }
                }

                return true;
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
            _eventBuffer.Clear();
            foreach (var eEvent in Event.Events)
            {
                if (eEvent.SpecificPorts.Contains(999))
                {
                    _eventBuffer.Add(eEvent);
                }
            }

            if (_eventBuffer.Count == 0)            
                return;            

            var selectedEvent = _eventBuffer[Random.Range(0, _eventBuffer.Count)];
            var dayEventStarts = GameState.day + Random.Range(0, 10);
            LogDebug($"event scheduled in all ports on day {GameState.day}");

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

            _eventBuffer.Clear();
            foreach (var eEvent in Event.Events)
            {
                if (eEvent.SpecificPorts.Length == 0 || eEvent.SpecificPorts.Contains(selectedPort.Index))                
                    _eventBuffer.Add(eEvent);                
            }

            if (_eventBuffer.Count == 0)            
                return;            

            var selectedEvent = _eventBuffer[Random.Range(0, _eventBuffer.Count)];

            var dayEventStarts = GameState.day + Random.Range(0, 10);
            selectedPort.AssignedEvent = selectedEvent.Id;
            selectedPort.DayEventStarts = dayEventStarts;
            selectedPort.DayEventEnds = dayEventStarts + selectedEvent.EventDuration;

            PortsWithEvents.Add(selectedPort);
        }

        internal void LoadPortsWithEvents(List<EventPort> portsWithEvents)
        {
            PortsWithEvents = portsWithEvents;
            foreach (var port in PortsWithEvents)
            {
                var matchedPort = EventRegion.FindPort(port.Index);
                if (matchedPort != null)
                {
                    matchedPort.AssignedEvent = port.AssignedEvent;
                    matchedPort.DayEventStarts = port.DayEventStarts;
                    matchedPort.DayEventEnds = port.DayEventEnds;
                }
            }
        }

        internal void LoadEventsScheduler()
        {
            PortsWithEvents = ModData.GetEventPortListEntry($"{PLUGIN_GUID}.PortsWithEvents");
            ModData.GetDictEntry($"{PLUGIN_GUID}.RegionChance", RegionChance);
        }

        internal void SaveEventsScheduler()
        {
            ModData.AddEventPortListEntry($"{PLUGIN_GUID}.PortsWithEvents", PortsWithEvents);
            ModData.AddDictEntry($"{PLUGIN_GUID}.RegionChance", RegionChance);
        }

        internal void LoadRegionChance(Dictionary<int, int> regionChance)
        {
            SaveLoadPatches.LoadDictionary(regionChance, RegionChance);
        }
    }
}
