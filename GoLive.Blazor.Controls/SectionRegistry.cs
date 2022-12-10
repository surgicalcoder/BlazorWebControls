using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

namespace GoLive.Blazor.Controls;

public class SectionRegistry
{
    private static ConditionalWeakTable<Dispatcher, SectionRegistry> registries = new();

    private Dictionary<string, List<Action<RenderFragment>>> subscriptions = new();

    public static SectionRegistry GetRegistry(RenderHandle renderHandle)
    {
        return registries.GetOrCreateValue(renderHandle.Dispatcher);
    }

    public void Subscribe(string name, Action<RenderFragment> callback)
    {
        if (!subscriptions.TryGetValue(name, out var existingList))
        {
            existingList = new List<Action<RenderFragment>>();
            subscriptions.Add(name, existingList);
        }

        existingList.Add(callback);
    }

    public void Unsubscribe(string name, Action<RenderFragment> callback)
    {
        if (name != null && subscriptions.TryGetValue(name, out var existingList))
        {
            existingList.Remove(callback);
        }
    }

    public void SetContent(string name, RenderFragment content)
    {
        if (subscriptions.TryGetValue(name, out var existingList))
        {
            foreach (var callback in existingList)
            {
                callback(content);
            }
        }
    }
}