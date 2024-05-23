using System;
using Microsoft.AspNetCore.Authorization;

namespace GoLive.Blazor.Controls.MenuItems;

class MenuItemAuthData : IAuthorizeData
{
    public string Policy { get; set; }
    public string Roles { get; set; }
    public string? AuthenticationSchemes
    {
        get => null;
        set => throw new NotSupportedException();
    }
}