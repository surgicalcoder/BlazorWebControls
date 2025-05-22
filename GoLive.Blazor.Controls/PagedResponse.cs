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
            await Task.WhenAll(item.Data.Select(async itemToProcess => await RunEachItemAfterPaging(itemToProcess)));
        }

        item.Total = total ?? data.Count();
        item.Page = Math.Max(pageIndex, 1);
        item.PageSize = Math.Max(pageSize, 1);
        item.PageCount = Math.Max((item.Total + item.PageSize - 1) / item.PageSize, 1);

        return item;
    }

    public static async Task<PagedResponse<T>> New(IAsyncEnumerable<T> data, int pageIndex, int pageSize, int? total = null, Func<T, Task> RunEachItemAfterPaging = null, Func<List<T>, Task> RunGroupAfterPaging = null)
    {
        PagedResponse<T> pagedItem = new();
        var skipCount = (Math.Max(pageIndex, 1) - 1) * Math.Max(pageSize, 1);
        var takeCount = Math.Max(pageSize, 1);
        var pagedData = new List<T>();

        if (pageIndex > 0 && pageSize > 0)
        {
            pagedData = await data.Skip(skipCount).Take(takeCount).ToListAsync();
        }
        else
        {
            pagedData = await data.ToListAsync();
        }

        pagedItem.Data = pagedData;

        if (RunGroupAfterPaging != null)
        {
            await RunGroupAfterPaging.Invoke(pagedItem.Data);
        }

        if (RunEachItemAfterPaging != null)
        {
            await Task.WhenAll(pagedItem.Data.Select(async item => await RunEachItemAfterPaging(item)));
        }

        int actualTotal;

        if (total.HasValue)
        {
            actualTotal = total.Value;
        }
        else
        {
            actualTotal = await data.CountAsync();
        }

        pagedItem.Total = actualTotal;
        pagedItem.Page = Math.Max(pageIndex, 1);
        pagedItem.PageSize = Math.Max(pageSize, 1);
        pagedItem.PageCount = Math.Max((pagedItem.Total + pagedItem.PageSize - 1) / pagedItem.PageSize, 1);

        return pagedItem;
    }

    public PagedResponse() { }

    public List<T> Data { get; set; }
}