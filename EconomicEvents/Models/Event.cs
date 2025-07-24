using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EconomicEvents
{
    internal sealed class Event
    {
        internal int Id { get; private set; }
        internal string Name { get; private set; }
        internal string EventText { get; private set; }        
        internal int[] GoodIndexes { get; private set; }
        internal int PriceMult { get; private set; }
        internal int EventDuration { get; private set; }
        internal int[] SpecificPorts { get; private set; }
        internal static List<Event> Events { get; private set; }
        internal static Dictionary<int, int> ItemToCrateConversion { get; } = new Dictionary<int, int>
        {
            { 33, 1 }, // salmon
            { 42, 2 }, // date
            { 54, 4 }, // lamb
            { 46, 6 }, // tuna
            { 52, 7 }, // cheese
            { 53, 8 }, // goat cheese
            { 32, 9 }, // sunspot fish
            { 37, 14 }, // north fish
            { 41, 15 }, // sausages
            { 39, 16 }, // pork
            { 44, 17 }, // banana
            { 36, 18 }, // trout
            { 34, 19 }, // eel
            { 147, 43 }, // orange
            { 144, 44 }, // forest mushroom
            { 146, 46 }, // cave mushroom
            { 149, 54 }, // apple
        };

        private Event(int id, string name, string eventText, int[] goodIndexes, int priceMult, int eventDuration, int[] ports)
        {
            Id = id;
            Name = name;
            EventText = eventText;
            GoodIndexes = goodIndexes;
            PriceMult = priceMult;
            EventDuration = eventDuration;
            SpecificPorts = ports;
        }

        // lumber 217 (47)
        // nails 218 (48)
        // barrel water 10
        // crate medicine 26
        // crate books 30
        // tools 203 (33)
        // crate gems 20
        // crate gold 22
        // crate goods 29
        // crate tea 5
        // barrel wine 13
        // barrel rum 11
        // barrel beer 12
        // barrel mead 206 (36)
        // barrel cider 228 (58)
        // crate iron 21
        // logs 205 (35)

        internal static void InitializeEvents()
        {
            Events = new List<Event>
            {
                new Event
                (
                    0,
                    "Storm Damage",
                    "A terrible storm swept through and the port needs building supplies\nfor repairs.",
                    new int[] { 47, 48 },
                    3,
                    7,
                    new int[] {}
                ),
                new Event
                (
                    1,
                    "Drought",
                    "A drought has stricken the area. The people are thirsty and are\nin urgent need of water.",
                    new int[] { 10 },
                    3,
                    7,
                    new int[] {}
                ),  
                new Event
                (
                    2,
                    "Plague",
                    "A plague is ravaging the port. They are in dire need of more\nmedicine.",
                    new int[] { 26 },
                    2,
                    14,
                    new int[] {}
                ),
                new Event
                (
                    3,
                    "Library Fire",
                    "A fire broke out in the library. They need more books to replace\nthe damged texts.",
                    new int[] { 30 },
                    3,
                    7,
                    new int[] { 5, 23, 26 }
                ),
                new Event
                (
                    4,
                    "Volcano Erupted",
                    "Mt. Malefic erupted and destroyed parts of the town. They need\nbuilding supplies for repairs and medicine for the injured.",
                    new int[] { 47, 48, 26 },
                    3,
                    14,
                    new int[] { 17 }
                ),
                new Event
                (
                    5,
                    "New Mine",
                    "They are making a new mine on the island and will pay a premium\nfor materials and tools.",
                    new int[] { 47, 48, 33 },
                    2,
                    14,
                    new int[] { 19, 0, 28 }
                ),
                new Event
                (
                    6,
                    "Missed Wine Delivery",
                    "A shipment of wine never arrived and the people of the port are\nclamoring for some.",
                    new int[] { 13 },
                    2,
                    14,
                    new int[] {}
                ),
                new Event
                (
                    7,
                    "Gold and Gems Needed",
                    "Interest in relics is rising. They need gold and gems in Aestra\nAbbey to make more.",
                    new int[] { 20, 22 },
                    3,
                    7,
                    new int[] { 26 }
                ),
                new Event
                (
                    8,
                    "Shipwreck",
                    "A ship wrecked was found and the port needs supplies to help the\nsurvivors.",
                    new int[] { 5, 26, 29 },
                    2,
                    14,
                    new int[] {}
                ),
                new Event
                (
                    9,
                    "Festival",
                    "A festival is being held and the port needs supplies for the\ncelebration.",
                    new int[] { 11, 12, 13, 15, 36, 58 },
                    2,
                    7,
                    new int[] {}
                ),
                new Event
                (
                    10,
                    "Shipyard Maintenance",
                    "The shipyard is in need of maintenance and needs supplies for\nthe job.",
                    new int[] { 47, 48, 33, 21, 35 },
                    3,
                    14,
                    new int[] { 0, 9, 15, 22 }
                ),
                new Event
                (
                    11,
                    "New Fishing Grounds",
                    "New fishing grounds have been discovered and the port wants\nsupplies to build more fishing boats.",
                    new int[] { 48, 33, 35 },
                    2,
                    21,
                    new int[] {}
                ),
                new Event
                (
                    12,
                    "New Dish",
                    "A new dish is sweeping the area and the residents can't get enough.\nThe port will buy the ingredients at a high price",
                    Ingredients(),
                    2,
                    7,
                    new int[] {}
                ),
                new Event
                (
                    13,
                    "Silk Shirt Craze",
                    "A new fashion trend has taken hold and the port's clothiers need\nsilk to keep up with demand.",
                    new int[] { 28 },
                    3,
                    7,
                    new int[] {}
                ),
                new Event
                (
                    14,
                    "Global Drought",
                    "A global drought has struck and all ports have a water shortage\ndriving up prices.",
                    new int[] { 10 },
                    4,
                    21,
                    new int[] { 999 }
                ),
                new Event
                (
                    15,
                    "Fishing Hook Shortage",
                    "An explosion at the hook maker has left all ports with a fishing\nhook shortage.",
                    new int[] { 104 },
                    5,
                    14,
                    new int[] { 999 }
                )
            };
        }

        private static int[] Ingredients()
        {
            var ingPool = new List<int>
            {
                1, 2, 3, 4, 6, 7, 8, 9, 14, 15, 16, 17, 18, 19, 25, 31, 32, 42, 43, 44, 46, 54
            };

            var ingOut = new List<int> { 24, 53 };
            for ( int i = 0; i < 3; i++ )
            {
                var selected = Random.Range(0, ingPool.Count);
                ingOut.Add(ingPool.ElementAt(selected));
                ingPool.RemoveAt(selected);
            }

            return ingOut.ToArray();
        }
    }
}
