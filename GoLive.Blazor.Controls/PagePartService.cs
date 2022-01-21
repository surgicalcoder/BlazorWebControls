using System;

namespace GoLive.Blazor.Controls
{
    [SingletonService]
    public class PagePartService
    {
        public event Action<string, string> OnShow;
        public void ShowMessage(string section, string item)
        {
            OnShow?.Invoke(section, item);
        }
    }
}