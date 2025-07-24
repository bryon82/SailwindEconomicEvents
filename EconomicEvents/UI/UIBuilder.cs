using System.Collections.Generic;
using UnityEngine;
using static EconomicEvents.EE_Plugin;

namespace EconomicEvents
{
    internal class UIBuilder
    {
        private const int HEADER_FONT_SIZE = 55;
        private const int FONT_SIZE = 42;
        
        internal static GameObject MakeEventsUI(GameObject repUI, GameObject modeButtons)
        {
            var bookmarkRep = modeButtons.transform.GetChild(5).gameObject;
            bookmarkRep.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "events & rep";

            var eventsUI = GameObject.Instantiate(repUI);
            Object.Destroy(eventsUI.GetComponent<ReputationUI>());
            eventsUI.transform.parent = repUI.transform.parent;
            eventsUI.transform.localPosition = repUI.transform.localPosition;
            eventsUI.transform.localRotation = repUI.transform.localRotation;
            eventsUI.transform.localScale = repUI.transform.localScale;
            eventsUI.name = "events ui";
            Object.Destroy(eventsUI.transform.GetChild(2).gameObject);
            Object.Destroy(eventsUI.transform.GetChild(1).gameObject);
            eventsUI.AddComponent<EventsUI>();

            var eventTextGO = eventsUI.transform.GetChild(0).gameObject;
            eventTextGO.transform.localPosition = new Vector3(0.835f, -0.085f, -0.007f);
            eventTextGO.GetComponent<TextMesh>().font = eventTextGO.transform.GetChild(1).GetComponent<TextMesh>().font;
            eventTextGO.GetComponent<TextMesh>().fontSize = FONT_SIZE;
            eventTextGO.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
            eventTextGO.GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
            eventTextGO.GetComponent<TextMesh>().text = string.Empty;
            eventTextGO.GetComponent<MeshRenderer>().material = eventTextGO.transform.GetChild(1).GetComponent<MeshRenderer>().material;
            eventTextGO.transform.GetChild(0).gameObject.name = "description";
            eventTextGO.transform.GetChild(0).localPosition = new Vector3(0f, -5f, 0f);
            eventTextGO.transform.GetChild(0).GetComponent<TextMesh>().font = eventTextGO.transform.GetChild(1).GetComponent<TextMesh>().font;
            eventTextGO.transform.GetChild(0).GetComponent<TextMesh>().fontSize = FONT_SIZE;
            eventTextGO.transform.GetChild(0).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
            eventTextGO.transform.GetChild(0).GetComponent<TextMesh>().text = string.Empty;
            eventTextGO.transform.GetChild(0).GetComponent<MeshRenderer>().material = eventTextGO.transform.GetChild(1).GetComponent<MeshRenderer>().material;
            eventTextGO.transform.GetChild(1).gameObject.name = "items";
            eventTextGO.transform.GetChild(1).localPosition = new Vector3(0f, -17f, 0f);
            eventTextGO.transform.GetChild(1).GetComponent<TextMesh>().fontSize = FONT_SIZE;
            eventTextGO.transform.GetChild(1).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
            eventTextGO.transform.GetChild(1).GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
            eventTextGO.transform.GetChild(1).GetComponent<TextMesh>().text = string.Empty;
            eventTextGO.transform.GetChild(2).gameObject.name = "multiplier";
            eventTextGO.transform.GetChild(2).localPosition = new Vector3(0f, -23f, 0f);
            eventTextGO.transform.GetChild(2).GetComponent<TextMesh>().font = eventTextGO.transform.GetChild(1).GetComponent<TextMesh>().font;
            eventTextGO.transform.GetChild(2).GetComponent<TextMesh>().fontSize = FONT_SIZE;
            eventTextGO.transform.GetChild(2).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
            eventTextGO.transform.GetChild(2).GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
            eventTextGO.transform.GetChild(2).GetComponent<MeshRenderer>().material = eventTextGO.transform.GetChild(1).GetComponent<MeshRenderer>().material;
            eventTextGO.transform.GetChild(2).GetComponent<TextMesh>().text = string.Empty;
            eventTextGO.transform.GetChild(2).gameObject.SetActive(true);
            eventTextGO.transform.GetChild(3).gameObject.name = "duration";
            eventTextGO.transform.GetChild(3).localPosition = new Vector3(0f, -29f, 0f);
            eventTextGO.transform.GetChild(3).GetComponent<TextMesh>().font = eventTextGO.transform.GetChild(1).GetComponent<TextMesh>().font;
            eventTextGO.transform.GetChild(3).GetComponent<TextMesh>().fontSize = FONT_SIZE;
            eventTextGO.transform.GetChild(3).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
            eventTextGO.transform.GetChild(3).GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
            eventTextGO.transform.GetChild(3).GetComponent<MeshRenderer>().material = eventTextGO.transform.GetChild(1).GetComponent<MeshRenderer>().material;
            eventTextGO.transform.GetChild(3).GetComponent<TextMesh>().text = string.Empty;
            eventTextGO.transform.GetChild(3).gameObject.SetActive(true);
            var eventTMs = new Dictionary<string, TextMesh>();
                       
                        
            for (int i = 0; i <  EventRegion.AllRegions.Count; i++)
            {
                var name = $"event{i}";
                var xPos = 0.835f;
                var newTextGO = GameObject.Instantiate(eventTextGO);
                Object.Destroy(newTextGO.transform.GetChild(4).gameObject);
                newTextGO.name = name;
                newTextGO.transform.parent = eventTextGO.transform.parent;
                newTextGO.transform.localPosition = new Vector3(xPos, 0.215f - 0.125f * i, eventTextGO.transform.localPosition[2]);
                newTextGO.transform.localRotation = eventTextGO.transform.localRotation;
                newTextGO.transform.localScale = eventTextGO.transform.localScale;
                eventTMs[$"{name}"] = newTextGO.GetComponent<TextMesh>();
                eventTMs[$"{name}_description"] = newTextGO.transform.GetChild(0).GetComponent<TextMesh>();
                eventTMs[$"{name}_items"] = newTextGO.transform.GetChild(1).GetComponent<TextMesh>();
                eventTMs[$"{name}_multiplier"] = newTextGO.transform.GetChild(2).GetComponent<TextMesh>();
                eventTMs[$"{name}_duration"] = newTextGO.transform.GetChild(3).GetComponent<TextMesh>();
            }

            EventsUI.Instance.SetUIElems(eventTMs);

            eventTextGO.name = "events header";
            eventTextGO.GetComponent<TextMesh>().text = "Economic Events";
            eventTextGO.transform.localPosition = new Vector3(0.835f, 0.25f, -0.007f);
            eventTextGO.GetComponent<TextMesh>().fontSize = HEADER_FONT_SIZE;
            eventTextGO.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
            Object.Destroy(eventTextGO.transform.GetChild(4).gameObject);
            Object.Destroy(eventTextGO.transform.GetChild(3).gameObject);
            Object.Destroy(eventTextGO.transform.GetChild(2).gameObject);
            Object.Destroy(eventTextGO.transform.GetChild(1).gameObject);
            Object.Destroy(eventTextGO.transform.GetChild(0).gameObject);

            LogInfo("Loaded events UI");

            return eventsUI;
        }        
    }
}
