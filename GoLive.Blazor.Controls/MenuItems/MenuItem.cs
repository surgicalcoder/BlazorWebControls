using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GoLive.Blazor.Controls.MenuItems;

public abstract class MenuItem
{
    public virtual required string Title { get; set; }
    
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    
    [Inject] private IAuthorizationPolicyProvider AuthorizationPolicyProvider { get; set; } = default!;
    
    [Inject] private IAuthorizationService AuthorizationService { get; set; } = default!;

    public List<string> Policies { get; set; } = [];
    public List<string> Roles { get; set; } = [];
    
    /// <summary>
    /// The resource to which access is being controlled.
    /// </summary>
    [Parameter] public object? Resource { get; set; }

    private List<IAuthorizeData> getAuthorizeData()
    {
        if (Policies?.Count == 0 || Roles?.Count == 0)
        {
            return null;
        }

        List<IAuthorizeData> items = Policies.Select(policy => new MenuItemAuthData() { Policy = policy }).Cast<IAuthorizeData>().ToList();
        items.AddRange(Roles.Select(role => new MenuItemAuthData() { Roles = role }));

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
}