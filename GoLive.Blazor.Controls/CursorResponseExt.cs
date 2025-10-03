using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoLive.Blazor.Controls;

public static class CursorResponseExt
{
    public static async Task<CursorResponse<T>> Response<T>(this IEnumerable<T> data, string? nextCursor = null, string? previousCursor = null, int pageSize = 0, int? total = null, Func<T, Task> RunEachItemAfterPaging = null, Func<List<T>, Task> RunGroupAfterPaging = null)
    {
        return await CursorResponse<T>.CreateAsync(data, nextCursor, previousCursor, pageSize, total, RunEachItemAfterPaging, RunGroupAfterPaging);
    }
        
    public static async Task<CursorResponse<T>> Response<T, T2>(this IEnumerable<T2> data, string? nextCursor, string? previousCursor, int pageSize, Func<T2, T> ConvertItems, int? total = null, Func<T, Task> RunEachItemAfterPaging = null, Func<List<T>, Task> RunGroupAfterPaging = null)
    {
        CursorResponse<T> item = new();

        IEnumerable<T2> inputData;
            
        if (pageSize > 0)
        {
            inputData = data.Take(pageSize).ToList();
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

        item.NextCursor = nextCursor;
        item.PreviousCursor = previousCursor;
        item.PageSize = Math.Max(pageSize, 1);

        return item;
    }    
    
    
    public static async Task<CursorResponse<T>> Response<T>(this IAsyncEnumerable<T> data, string? nextCursor = null, string? previousCursor = null, int pageSize = 0, int? total = null, Func<T, Task> RunEachItemAfterPaging = null, Func<List<T>, Task> RunGroupAfterPaging = null)
    {
        return await CursorResponse<T>.CreateAsync(data, nextCursor, previousCursor, pageSize, total, RunEachItemAfterPaging, RunGroupAfterPaging);
    }
        
    public static async Task<CursorResponse<T>> Response<T, T2>(this IAsyncEnumerable<T2> data, string? nextCursor, string? previousCursor, int pageSize, Func<T2, T> ConvertItems, int? total = null, Func<T, Task> RunEachItemAfterPaging = null, Func<List<T>, Task> RunGroupAfterPaging = null)
    {
        CursorResponse<T> item = new();

        List<T2> inputData;
            
        if (pageSize > 0)
        {
            inputData = await data.Take(pageSize).ToListAsync();
        }
        else
        {
            inputData = await data.ToListAsync();
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

        item.Total = total ?? (await data.CountAsync());

        item.NextCursor = nextCursor;
        item.PreviousCursor = previousCursor;
        item.PageSize = Math.Max(pageSize, 1);

        return item;
    }
}
