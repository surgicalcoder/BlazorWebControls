using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GoLive.Blazor.Controls.Pager;

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

    protected int StartIndex { get; private set; }
    protected int FinishIndex { get; private set; }
    protected int StartRecord { get; private set; }
    protected int EndRecord { get; private set; }

    protected override void OnParametersSet()
    {
        if (Result == null)
        {
            return;
        }

        StartIndex = Math.Max(Result.Page - NumberToShow, 1);
        FinishIndex = Math.Min(Result.Page + NumberToShow, Result.PageCount);
        StartRecord = Result.Page * Result.PageSize - Result.PageSize + 1;
        this.EndRecord = Math.Min(this.Result.Page * this.Result.PageSize, this.Result.Total);
        if (this.EndRecord == 0 && this.Result.Total > 0)
        {
            this.EndRecord = this.Result.Total;
        }
    }

    protected async Task PagerButtonClicked(int page)
    {
        Page = page;
        await PageChanged.InvokeAsync(page);
    }
}