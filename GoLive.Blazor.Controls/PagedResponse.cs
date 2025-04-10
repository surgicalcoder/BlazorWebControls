using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoLive.Blazor.Controls;

public class PagedResponse
{
    public int PageSize { get; set; }

    public int Page { get; set; }

    public int PageCount { get; set; }

    public int Total { get; set; }
}

public class PagedResponse<T> : PagedResponse
{
    public static async Task<PagedResponse<T>> New(IEnumerable<T> data, int pageIndex, int pageSize, int? total = null, Func<T, Task> RunEachItemAfterPaging = null, Func<List<T>, Task> RunGroupAfterPaging = null )
    {
        PagedResponse<T> item = new();

        if (pageIndex > 0 && pageSize > 0)
        {
            item.Data = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        else
        {
            item.Data = data.ToList();
        }

        if (RunGroupAfterPaging != null)
        {
            await RunGroupAfterPaging.Invoke(item.Data);
        }
            
        if (RunEachItemAfterPaging != null)
        {
            await item.Data.ToAsyncEnumerable().ForEachAwaitAsync(async arg => await RunEachItemAfterPaging(arg));
        }

        item.Total = total ?? data.Count();
        item.Page = Math.Max(pageIndex, 1);
        item.PageSize = Math.Max(pageSize, 1);
        item.PageCount = item.PageSize == 0 
            ? 0 
            : (item.Total + item.PageSize - 1) / item.PageSize;

        return item;
    }

    public PagedResponse() { }

    public List<T> Data { get; set; }
}