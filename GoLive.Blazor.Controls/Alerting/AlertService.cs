using Microsoft.AspNetCore.Components;

namespace GoLive.Blazor.Controls.Alerting
{
    [SingletonService]
    public class AlertService
    {
        public EventCallback<AlertItem> OnShow;
        public void ShowAlert(AlertMessage item, bool Autohide=true, int Autohidetime=5)
        {
            OnShow.InvokeAsync(new AlertItem(item, Autohide, Autohidetime));
        }

        public class AlertItem
        {
            public AlertMessage Item { get; set; }
            public bool AutoHide { get; set; }
            public int AutohideTime { get; set; }
            public AlertItem() { }
            public AlertItem(AlertMessage item, bool autoHide, int autohideTime)
            {
                Item = item;
                AutoHide = autoHide;
                AutohideTime = autohideTime;
            }
        }
    }
}