using System;

namespace EconomicEvents
{
    [Serializable]
    public class EventPort
    {        
        public int Index { get; private set; }
        public string Name { get; private set; }
        public int AssignedEvent { get; set; } = -1;
        public int DayEventStarts { get; set; } = -1;
        public int DayEventEnds { get; set; } = -1;

        public EventPort(int index, string name) 
        {
            Index = index;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public bool IsEventActive()
        {
            return HasAssignedEvent() && DayEventStarts <= GameState.day;
        }

        public bool HasAssignedEvent()
        {
            return AssignedEvent >= 0;
        }

    }
}
