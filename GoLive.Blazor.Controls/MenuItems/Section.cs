using System.Collections.Generic;

namespace GoLive.Blazor.Controls.MenuItems;

public class Section : MenuItem
{
    public virtual List<NavLink> Children { get; set; } = [];
}