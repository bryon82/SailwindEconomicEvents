using System.Collections.Generic;
using System.Linq;

namespace EconomicEvents
{
    internal sealed class EventRegion
    {
        private readonly List<EventPort> _ports;

        internal int Index { get; private set; }
        internal string Name { get; private set; }
        internal IReadOnlyList<EventPort> Ports => _ports.AsReadOnly();

        private EventRegion(int index, string name, List<EventPort> ports)
        {
            Index = index;
            Name = name;
            _ports = ports;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public static readonly EventRegion AlAnkh =
            new EventRegion
            (
                0,
                "Al'Ankh",
                new List<EventPort>
                {
                    new EventPort(0, "Gold Rock City"),
                    new EventPort(1, "Al'Nilem"),
                    new EventPort(2, "Neverdin"),
                    new EventPort(3, "Albacore Town"),
                    new EventPort(4, "Alchemist's Island"),
                    new EventPort(5, "Al'Ankh Academy"),
                    new EventPort(6, "Oasis"),
                    new EventPort(31, "Old Ankh Town"),
                    new EventPort(32, "Mirage Mountain"),
                    new EventPort(33, "Saffron Island"),
                }
            );

        public static readonly EventRegion EmeraldArchipelago =
            new EventRegion
            (
                1,
                "Emerald Archipelago",
                new List<EventPort>
                {
                    new EventPort(9, "Dragon Cliffs"),
                    new EventPort(10, "Sanctuary"),
                    new EventPort(11, "Crab Beach"),
                    new EventPort(12, "New Port"),
                    new EventPort(13, "Sage Hills"),
                    new EventPort(14, "Serpent Isle"),
                    new EventPort(30, "Dead Cove"),
                    new EventPort(29, "Turtle Island"),
                }
            );

        public static readonly EventRegion Aestrin =
            new EventRegion
            (
                2,
                "Aestrin",
                new List<EventPort>
                {
                    new EventPort(15, "Fort Aestrin") ,
                    new EventPort(16, "Sunspire"),
                    new EventPort(17, "Mount Malefic"),
                    new EventPort(18, "Siren Song"),
                    new EventPort(19, "Eastwind"),
                    new EventPort(28, "Firefly Grotto"),
                    new EventPort(26, "Aestra Abbey"),
                    new EventPort(27, "Fey Valley"),
                    new EventPort(20, "Happy Bay"),
                    new EventPort(21, "Chronos"),
                }
            );

        public static readonly EventRegion FireFishLagoon =
            new EventRegion
            (
                3,
                "Fire Fish Lagoon",
                new List<EventPort>
                {
                    new EventPort(22, "Kicia Bay"),
                    new EventPort(23, "Fire Fish Town"),
                    new EventPort(24, "On'na"),
                    new EventPort(25, "Sen'na"),
                }
            );

        public static readonly IReadOnlyList<EventRegion> AllRegions = new List<EventRegion>
        {
            AlAnkh,
            EmeraldArchipelago,
            Aestrin,
            FireFishLagoon
        }.AsReadOnly();

        public static IReadOnlyList<EventPort> AllPorts
        {
            get
            {
                var allPorts = new List<EventPort>();
                foreach (var region in AllRegions)
                {
                    allPorts.AddRange(region.Ports);
                }
                return allPorts.AsReadOnly();
            }
        }

        public static bool AnyAssignedEvents()
        {
            return AllRegions.Any(r => r.HasAssignedEvent());
        }

        public bool HasAssignedEvent()
        {
            return Ports.Any(p => p.HasAssignedEvent());
        }
    }
}
