namespace EconomicEvents
{
    public class GPButtonUpdateEvents : GoPointerButton
    {        
        public override void OnActivate()
        {
            UpdateEventsUI.Instance.UpdateEvents();
            UpdateEventsUI.Instance.DeactivateUI();
        }        
    }
}
