using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GoLive.Blazor.Controls.Breadcrumb
{
    public class BreadcrumbBase : ComponentBase, IDisposable
    {
        [Inject]
        protected NavigationManager navManager { get; set; }
        
        [Inject]
        private BreadcrumbService service { get; set; }
        protected BreadcrumbList items;

        protected override async Task OnInitializedAsync()
        {
            service.Breadcrumbs += ServiceOnBreadcrumbs;
        }

        private void ServiceOnBreadcrumbs(BreadcrumbList input)
        {
            items = input;

            try
            {
                InvokeAsync(StateHasChanged);
            }
            catch (Exception) {}
        }

        public void Dispose()
        {
            items = null;
           // InvokeAsync(StateHasChanged);
        }
    }
}
