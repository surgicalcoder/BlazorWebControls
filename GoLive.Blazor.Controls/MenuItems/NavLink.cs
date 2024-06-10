using System.Collections.Generic;

namespace GoLive.Blazor.Controls.MenuItems;

public class NavLink : MenuItem
{
    public virtual string Icon { get; set; }
    public virtual string Link { get; set; }

    public virtual string Badge { get; set; }
    public virtual string BadgeClass { get; set; }
    
    public virtual List<NavLink> Children { get; set; } = [];
}