using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GoLive.Blazor.Controls.Pager
{
    public class PagerModel : ComponentBase
    {
        [Parameter] 
        public PagedResponse Result { get; set; }

        [Parameter]
        public int Page { get; set; }
    
        [Parameter]
        public EventCallback<int> PageChanged { get; set; }

        [Parameter]
        [DefaultValue(20)]
        public int PageSize { get; set; }

        [Parameter]
        [DefaultValue(5)]
        public int NumberToShow { get; set; }

        protected int StartIndex { get; private set; } = 0;
        protected int FinishIndex { get; private set; } = 0;

        protected int StartRecord { get; private set; } = 0;
        protected int EndRecord { get; private set; } = 0;

        protected override void OnParametersSet()
        {
            StartIndex = Math.Max(Result.Page - NumberToShow, 1);
            FinishIndex = Math.Min(Result.Page + NumberToShow, Result.PageCount);
            StartRecord = (Result.Page * Result.PageSize) - Result.PageSize + 1;
            EndRecord = Math.Min(Result.Page * Result.PageSize, Result.Total);
            base.OnParametersSet();
        }

        protected async Task PagerButtonClicked(int page)
        {
            Page = page;
            await PageChanged.InvokeAsync(page);
        }
    }
}
