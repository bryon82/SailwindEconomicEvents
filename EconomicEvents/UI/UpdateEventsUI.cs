using System.Linq;
using UnityEngine;

namespace EconomicEvents
{
    internal class UpdateEventsUI : MonoBehaviour
    {
        internal static UpdateEventsUI Instance { get; private set; }
        internal GameObject UpdateEventsMenu { get; set; }        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            if (UpdateEventsMenu.activeInHierarchy)
            {
                base.transform.rotation = Refs.observerMirror.transform.rotation;
            }
        }

        internal void ActivateUI(PortDude portDude)
        {
            base.transform.position = portDude.transform.position;
            base.transform.rotation = Refs.observerMirror.transform.rotation;
            UpdateEventsMenu.SetActive(true);
        }

        internal void DeactivateUI()
        {
            UpdateEventsMenu.SetActive(false);
        }

        internal void UpdateEvents()
        {
            UISoundPlayer.instance.PlayWritingSound();
            NotificationUi.instance.ShowNotification("Logbook updated");
            EventsUI.Instance.LoggedEventPorts = 
                EventScheduler.Instance.PortsWithEvents
                .Where(ep => ep.IsEventActive())
                .OrderBy(ep => ep.DayEventEnds)
                .ToList();
        }
    }
}
