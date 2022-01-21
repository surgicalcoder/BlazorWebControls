using System.Collections.Generic;

namespace GoLive.Blazor.Controls.Breadcrumb
{
    public class BreadcrumbList
    {
        public BreadcrumbList()
        {
            Items = new List<BreadcrumbItem>();
        }
        public List<BreadcrumbItem> Items { get; set; }
        
    }
}