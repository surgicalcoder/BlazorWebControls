namespace GoLive.Blazor.Controls.Breadcrumb
{
    public static class BreadcrumbExt
    {
        public static BreadcrumbList Add(this BreadcrumbList item, string Name, string Link)
        {
            item.Items.Add(new BreadcrumbItem{Link=Link, Name = Name});

            return item;
        }        
        
        public static BreadcrumbList Add(this BreadcrumbList item, string Name)
        {
            item.Items.Add(new BreadcrumbItem { Name = Name });

            return item;
        }
    }
}