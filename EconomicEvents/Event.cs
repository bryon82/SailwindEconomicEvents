using System;
using System.Collections.Generic;

namespace EconomicEvents
{
    internal sealed class Event
    {
        internal int Id { get; private set; }
        internal string Name { get; private set; }
        internal int EventDuration { get; private set; }
        internal int[] GoodIndexes { get; private set; }
        internal int PriceMult { get; private set; }
        internal int[] SpecificPorts { get; private set; }
        internal string EventText { get; private set; }
        internal static List<Event> Events { get; private set; }
        internal static List<Event> GlobalEvents { get; private set; }

        private Event(int id, string name, int eventDuration, int[] goodIndexes, int priceMult, int[] ports, string eventText)
        {
            Id = id;
            Name = name;
            EventDuration = eventDuration;
            GoodIndexes = goodIndexes;
            PriceMult = priceMult;
            SpecificPorts = ports;
            EventText = eventText;
        }

        /*
        
            missed shipment
            A fire at the academy
            A new mine discovered in gold rock
            Panic at mt.malefic
        */

        // lumber 217 (47)
        // nails 218 (48)
        // barrel water 10
        // crate medicine 26
        // crate books 30
        // tools 203 (33)
        // crate gems 20
        // crate gold 22
        internal static void InitializeEvents()
        {
            Events = new List<Event>
            {
                new Event(0, "Storm Damage", 21, new int[] { 47, 48 }, 3, null, "A bad storm swept through and the port needs building supplies for repairs."),
                new Event(1, "Drought", 21, new int[] { 10 }, 3, null, "A drought has stricken the area. They need water badly."),  
                new Event(2, "Plague", 14, new int[] { 26 }, 4, null, "A plague is ravaging the port. They are in dire need of medicine."),
                new Event(3, "Damaged Books", 14, new int[] { 30 }, 3, new int[] { 5, 23, 26 }, "A fire broke out in the library and damaged the books. They need more books."),
                new Event(4, "Volcano Erupted", 21, new int[] { 47, 48, 26 }, 3, new int[] { 17 }, "Mt. Malefic erupted and destroyed parts of the town. They need building supplies for repairs and medicine for the injured."),
                new Event(5, "New Mine", 21, new int[] { 47, 48, 33 }, 2, new int[] { 19, 0, 28 }, "They are making a new mine on the island and will pay premium for materials and tools."),
                new Event(6, "Missed Wine Delivery", 14, new int[] { 13 }, 2, null, "A shipment of wine never arrived and the port is clamoring for some."),
                new Event(7, "Church Needs Gold", 14, new int[] { 20, 22 }, 3, new int[] { 26 }, "They need gold and gems in Aestra to make more relics.")
            };
        }
    }
}
