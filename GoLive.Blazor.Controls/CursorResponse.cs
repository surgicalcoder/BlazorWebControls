using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoLive.Blazor.Controls;

/// <summary>
/// Represents a cursor-based paginated response containing navigation tokens and metadata.
/// </summary>
public class CursorResponse
{
    public string? PreviousCursor { get; set; }
    public string? NextCursor { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
}

/// <summary>
/// Represents a cursor-based paginated response with strongly-typed data.
/// </summary>
/// <typeparam name="T">The type of items in the response</typeparam>
public class CursorResponse<T> : CursorResponse
{
    public List<T> Data { get; set; } = new();

    public static async Task<CursorResponse<T>> CreateAsync(
        IEnumerable<T> data,
        string? nextCursor = null,
        string? previousCursor = null,
        int pageSize = 0,
        int? total = null,
        Func<T, Task>? processEachItem = null,
        Func<List<T>, Task>? processGroup = null)
    {
        ArgumentNullException.ThrowIfNull(data);

        var response = new CursorResponse<T>();
        var dataList = pageSize > 0 ? data.Take(pageSize).ToList() : data.ToList();

        response.Data = dataList;
        response.Total = total ?? data.Count();
        response.PageSize = Math.Max(pageSize, 1);
        response.NextCursor = nextCursor;
        response.PreviousCursor = previousCursor;

        if (processGroup != null)
        {
            await processGroup(response.Data);
        }

        if (processEachItem != null)
        {
            await Task.WhenAll(response.Data.Select(processEachItem));
        }

        return response;
    }

    public static async Task<CursorResponse<T>> CreateAsync(
        IAsyncEnumerable<T> data,
        string? nextCursor = null,
        string? previousCursor = null,
        int pageSize = 0,
        int? total = null,
        Func<T, Task>? processEachItem = null,
        Func<List<T>, Task>? processGroup = null)
    {
        ArgumentNullException.ThrowIfNull(data);

        var response = new CursorResponse<T>();
        var dataList = pageSize > 0
            ? await data.Take(Math.Max(pageSize, 1)).ToListAsync()
            : await data.ToListAsync();

        response.Data = dataList;
        response.Total = total ?? await data.CountAsync();
        response.PageSize = Math.Max(pageSize, 1);
        response.NextCursor = nextCursor;
        response.PreviousCursor = previousCursor;

        if (processGroup != null)
        {
            await processGroup(response.Data);
        }

        if (processEachItem != null)
        {
            await Task.WhenAll(response.Data.Select(processEachItem));
        }

        return response;
    }
}