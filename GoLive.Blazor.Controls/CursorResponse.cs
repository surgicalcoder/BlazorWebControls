using System;
using System.Collections.Concurrent;
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
    
    /// <summary>
    /// Global registry for ID extraction functions by type, supporting inheritance
    /// </summary>
    private static readonly ConcurrentDictionary<Type, Func<object, string?>> _globalIdFunctions = new();
    
    /// <summary>
    /// Sets a global ID extraction function for a specific type and all its derived types
    /// </summary>
    /// <typeparam name="TBase">The base type (can be abstract)</typeparam>
    /// <param name="idFunc">Function to extract ID from instances of TBase or its derived types</param>
    public static void SetGlobalIdFunction<TBase>(Func<TBase, string?> idFunc)
    {
        _globalIdFunctions[typeof(TBase)] = obj => idFunc((TBase)obj);
    }
    
    /// <summary>
    /// Gets the appropriate ID extraction function for a given type, checking inheritance chain
    /// </summary>
    /// <param name="type">The type to find an ID function for</param>
    /// <returns>The ID extraction function, or null if none found</returns>
    internal static Func<object, string?>? GetGlobalIdFunction(Type type)
    {
        // Check exact type match first
        if (_globalIdFunctions.TryGetValue(type, out var exactMatch))
            return exactMatch;
            
        // Check inheritance chain
        var currentType = type.BaseType;
        while (currentType != null)
        {
            if (_globalIdFunctions.TryGetValue(currentType, out var inheritedMatch))
                return inheritedMatch;
            currentType = currentType.BaseType;
        }
        
        // Check interfaces
        foreach (var interfaceType in type.GetInterfaces())
        {
            if (_globalIdFunctions.TryGetValue(interfaceType, out var interfaceMatch))
                return interfaceMatch;
        }
        
        return null;
    }
}

/// <summary>
/// Represents a cursor-based paginated response with strongly-typed data.
/// </summary>
/// <typeparam name="T">The type of items in the response</typeparam>
public class CursorResponse<T> : CursorResponse
{
    public List<T> Data { get; set; } = new();

    /// <summary>
    /// Default function to extract ID from items of type T. Used when getIdFunc parameter is not provided.
    /// </summary>
    public static Func<T, string?>? DefaultGetIdFunc { get; set; }

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