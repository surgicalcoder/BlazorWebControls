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
}