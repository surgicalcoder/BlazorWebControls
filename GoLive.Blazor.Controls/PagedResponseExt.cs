using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoLive.Blazor.Controls;

public static class PagedResponseExt
{
    public static async Task<PagedResponse<T>> Response<T>(this IEnumerable<T> data, int pageIndex, int pageSize, int? total = null, Func<T, Task> RunEachItemAfterPaging = null, Func<List<T>, Task> RunGroupAfterPaging = null)
    {
        return await PagedResponse<T>.New(data, pageIndex, pageSize, total, RunEachItemAfterPaging, RunGroupAfterPaging);
    }
        
    public static async Task<PagedResponse<T>> Response<T, T2>(this IEnumerable<T2> data, int pageIndex, int pageSize, Func<T2, T> ConvertItems, int? total = null, Func<T, Task> RunEachItemAfterPaging = null, Func<List<T>, Task> RunGroupAfterPaging = null)
    {
        PagedResponse<T> item = new();

        IEnumerable<T2> inputData;
            
        if (pageIndex > 0 && pageSize > 0)
        {
            inputData = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        else
        {
            inputData = data.ToList();
        }
            
        item.Data = inputData.Select(ConvertItems).ToList();

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
        item.PageCount = Math.Min((item.Total + item.PageSize - 1) / item.PageSize, 1);

        return item;
    }
}