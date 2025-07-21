namespace EconomicEvents
{
    internal class EventPort
    {
        internal int Index { get; private set; }
        internal string Name { get; private set; }
        internal int AssignedEvent { get; set; } = -1;
        internal int DayEventStarts { get; set; } = -1;
        internal int DayEventEnds { get; set; } = -1;

        internal EventPort(int index, string name) 
        {
            Index = index;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        internal bool IsEventActive()
        {
            return HasAssignedEvent() && DayEventStarts <= GameState.day;
        }

        internal bool HasAssignedEvent()
        {
            return AssignedEvent >= 0;
        }

    }
}
