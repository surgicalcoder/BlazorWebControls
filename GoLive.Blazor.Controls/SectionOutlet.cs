using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GoLive.Blazor.Controls;

public class SectionOutlet : IComponent, IDisposable
{
    private static RenderFragment EmptyRenderFragment = builder => { };
    private string subscribedName;
    private SectionRegistry registry;
    private Action<RenderFragment> onChangeCallback;

    public void Attach(RenderHandle renderHandle)
    {
        onChangeCallback = content => renderHandle.Render(content ?? EmptyRenderFragment);
        registry = SectionRegistry.GetRegistry(renderHandle);
    }

    public Task SetParametersAsync(ParameterView parameters)
    {
        var suppliedName = parameters.GetValueOrDefault<string>("Name");

        if (suppliedName != subscribedName)
        {
            registry.Unsubscribe(subscribedName, onChangeCallback);
            registry.Subscribe(suppliedName, onChangeCallback);
            subscribedName = suppliedName;
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        registry?.Unsubscribe(subscribedName, onChangeCallback);
    }
}