using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;

namespace GoLive.Blazor.Controls;


public class VisibilityComponent : ComponentBase
{
    
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    
    [Inject] private IAuthorizationPolicyProvider AuthorizationPolicyProvider { get; set; } = default!;
    
    [Inject] private IAuthorizationService AuthorizationService { get; set; } = default!;

    public List<string> Policies { get; set; } = [];
    public List<string> Roles { get; set; } = [];

    public bool Visible = false;

    protected override async Task OnParametersSetAsync()
    {
        Visible = false;
        Visible = await IsRenderable();
    }

    /// <summary>
    /// The resource to which access is being controlled.
    /// </summary>
    [Parameter] public object? Resource { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Visible)
        {
            base.BuildRenderTree(builder);
        }
    }

    private List<IAuthorizeData> getAuthorizeData()
    {
        if (Policies?.Count == 0 || Roles?.Count == 0)
        {
            return null;
        }

        List<IAuthorizeData> items = Policies.Select(policy => new VisibilityControlData() { Policy = policy }).Cast<IAuthorizeData>().ToList();
        items.AddRange(Roles.Select(role => new VisibilityControlData() { Roles = role }));

        return items;
    }

    public async Task<bool> IsRenderable()
    {
        var currentAuthenticationState = await AuthenticationState;
        return await IsAuthorizedAsync(currentAuthenticationState.User);
    }
    
    private async Task<bool> IsAuthorizedAsync(ClaimsPrincipal user)
    {
        var authorizeData = getAuthorizeData();
        if (authorizeData == null)
        {
            return true;
        }
        
        var policy = await AuthorizationPolicy.CombineAsync(AuthorizationPolicyProvider, authorizeData);
        var result = await AuthorizationService.AuthorizeAsync(user, Resource, policy!);
        return result.Succeeded;
    }
    
    class VisibilityControlData : IAuthorizeData
    {
        public string Policy { get; set; }
        public string Roles { get; set; }
        public string? AuthenticationSchemes
        {
            get => null;
            set => throw new NotSupportedException();
        }
    }
}
