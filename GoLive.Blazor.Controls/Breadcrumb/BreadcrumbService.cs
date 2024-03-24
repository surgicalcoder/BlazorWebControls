using System;

namespace GoLive.Blazor.Controls.Breadcrumb
{
    [RegisterSingleton]
    public class BreadcrumbService : IDisposable
    {

        public event Action<BreadcrumbList> Breadcrumbs;
        public void SetBreadcrumb(BreadcrumbList items)
        {
            Breadcrumbs?.Invoke(items);
        }

        public void Clear()
        {
            Breadcrumbs?.Invoke(null);
        }

        public void Dispose()
        {
            Breadcrumbs?.Invoke(null);
        }
    }
}
