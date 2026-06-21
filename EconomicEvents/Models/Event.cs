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
        internal HashSet<int> GoodIndexSet { get; private set; }
        internal int PriceMult { get; private set; }
        internal int EventDuration { get; private set; }
        internal int[] SpecificPorts { get; private set; }
        internal static List<Event> Events { get; private set; }
        internal static Dictionary<int, Event> EventsById { get; private set; }
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

        private string _itemsDisplay;

        private Event(int id, string name, string eventText, int[] goodIndexes, int priceMult, int eventDuration, int[] ports)
        {
            Id = id;
            Name = name;
            EventText = eventText;
            GoodIndexes = goodIndexes;
            GoodIndexSet = new HashSet<int>(goodIndexes);
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
        // rabbit furs 220 (50)
        // marble 225 (55)
        // crate copper 23

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
                    "Increased Relic Production",
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
                ),
                new Event
                (
                    16,
                    "Explosive Event",
                    "Moments away from figuring out the how to create gold from everyday\nitems, they suffered an explosion damaging all of their materials.",
                    new int[] { 11, 23, 50, 55 },
                    2,
                    14,
                    new int[] { 4 }
                ),
                new Event
                (
                    17,
                    "New Tavern",
                    "A new tavern is opening up and they need supplies to get it up\nand running.",
                    new int[] { 12, 36, 58 },
                    2,
                    14,
                    new int[] { 9 }
                ),
                new Event
                (
                    18,
                    "New Restaurant",
                    "A new restaurant is opening up and they need supplies to get it up\nand running.",
                    new int[] { 11, 23, 50, 55 },
                    2,
                    14,
                    new int[] { 4 }
                ),
                new Event
                (
                    19,
                    "Naval Blockade",
                    "A naval blockade has cut off trade routes. Smugglers are paying\na premium for goods that can slip through.",
                    new int[] { 11, 13, 28 },
                    3,
                    14,
                    new int[] {}
                ),
                new Event
                (
                    20,
                    "Monastery Brewing",
                    "The local monastery has taken up brewing and needs supplies\nto expand their operation.",
                    new int[] { 12, 36, 58 },
                    2,
                    14,
                    new int[] {}
                ),
                new Event
                (
                    21,
                    "Copper Shortage",
                    "A collapse at the main copper mine has halted production.\nPorts are scrambling for any supply.",
                    new int[] { 23 },
                    3,
                    14,
                    new int[] { 999 }
                ),
                new Event
                (
                    22,
                    "Pirate Raid",
                    "Pirates ransacked the town's stores. The port is restocking\nessentials and will pay well for delivered goods.",
                    new int[] { 10, 26, 29 },
                    2,
                    14,
                    new int[] {}
                ),
                new Event
                (
                    23,
                    "Royal Visit",
                    "Royalty is visiting the port and the locals are scrambling\nto prepare a proper feast and celebration.",
                    new int[] { 11, 13, 36, 58 },
                    3,
                    7,
                    new int[] {}
                ),
                new Event
                (
                    24,
                    "Tanner's Guild Strike",
                    "The tanners have walked off the job. Fur prices have spiked\nas finished leather goods dry up.",
                    new int[] { 50 },
                    3,
                    14,
                    new int[] {}
                ),
                new Event
                (
                    25,
                    "Alchemist Convention",
                    "Alchemists from across the region have gathered. They are\npaying handsomely for rare materials.",
                    new int[] { 20, 22, 23, 55 },
                    3,
                    7,
                    new int[] { 4 }
                ),
                new Event
                (
                    26,
                    "Cathedral Construction",
                    "A grand cathedral is being built and the port needs large\nquantities of stone and materials.",
                    new int[] { 47, 48, 55 },
                    3,
                    21,
                    new int[] {}
                ),
                new Event
                (
                    27,
                    "Flooded Mines",
                    "Flooding has shut down the iron mines. Smiths across the\nregion are paying top coin for any iron available.",
                    new int[] { 21 },
                    4,
                    14,
                    new int[] { 999 }
                ),
                new Event
                (
                    28,
                    "Shipwright Boom",
                    "A surge in merchant activity has every shipyard booked solid.\nThey are buying timber at record prices.",
                    new int[] { 35, 47 },
                    3,
                    21,
                    new int[] {}
                ),
                new Event
                (
                    29,
                    "Scurvy Outbreak",
                    "Scurvy has broken out among the sailors and locals alike.\nFresh fruit is being bought up as fast as it arrives.",
                    new int[] { 17, 44, 54, 2 },
                    3,
                    14,
                    new int[] {}
                ),
                new Event
                (
                    30,
                    "Merchants' Summit",
                    "Trade representatives from across the region have convened.\nLuxury goods are commanding extraordinary prices.",
                    new int[] { 5, 20, 22, 28 },
                    3,
                    7,
                    new int[] {}
                ),
            };

            EventsById = Events.ToDictionary(e => e.Id, e => e);
        }

        internal string GetItemsDisplay()
        {
            if (_itemsDisplay != null)            
                return _itemsDisplay;
            
            var items = new List<string>(GoodIndexes.Length);
            foreach (var goodIndex in GoodIndexes)
            {
                var itemIndex = PrefabsDirectory.GoodToItemIndex(goodIndex);
                var item = PrefabsDirectory.instance.GetItem(itemIndex) ?? PrefabsDirectory.instance.GetItem(goodIndex);
                if (item != null)
                {
                    items.Add($"<b>{item.name}</b>");
                }
            }

            _itemsDisplay = string.Join(", ", items);
            return _itemsDisplay;
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
