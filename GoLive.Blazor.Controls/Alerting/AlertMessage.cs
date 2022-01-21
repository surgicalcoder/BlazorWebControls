using System.Collections.Generic;

namespace GoLive.Blazor.Controls.Alerting
{
    public class AlertMessage
    {
        public AlertMessage()
        {
            AdditionalProperties = new();
        }
        public string Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Dictionary<string, object> AdditionalProperties { get; set; }
    }
}