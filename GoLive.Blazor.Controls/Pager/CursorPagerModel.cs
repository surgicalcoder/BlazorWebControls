using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GoLive.Blazor.Controls.Pager;

public class CursorPagerModel : ComponentBase
{
    [Parameter]
    public CursorResponse Result { get; set; }

    [Parameter]
    public string CurrentCursor { get; set; }

    [Parameter]
    public EventCallback<string> CursorChanged { get; set; }

    [Parameter]
    [DefaultValue(20)]
    public int PageSize { get; set; }

    [Parameter]
    [DefaultValue(5)]
    public int NumberToShow { get; set; }

    protected bool HasPrevious { get; private set; }
    protected bool HasNext { get; private set; }
    protected int StartRecord { get; private set; }
    protected int EndRecord { get; private set; }

    protected override void OnParametersSet()
    {
        if (Result == null)
        {
            return;
        }

        HasPrevious = !string.IsNullOrEmpty(Result.PreviousCursor);
        HasNext = !string.IsNullOrEmpty(Result.NextCursor);
        StartRecord = 1;
        this.EndRecord = Math.Min(this.Result.PageSize, this.Result.Total);
        if (this.EndRecord == 0 && this.Result.Total > 0)
        {
            this.EndRecord = this.Result.Total;
        }
    }

    protected async Task PreviousButtonClicked()
    {
        if (HasPrevious)
        {
            CurrentCursor = Result.PreviousCursor;
            await CursorChanged.InvokeAsync(Result.PreviousCursor);
        }
    }

    protected async Task NextButtonClicked()
    {
        if (HasNext)
        {
            CurrentCursor = Result.NextCursor;
            await CursorChanged.InvokeAsync(Result.NextCursor);
        }
    }
}
