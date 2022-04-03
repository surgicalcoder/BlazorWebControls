using System;
using System.Collections.Generic;

namespace GoLive.Blazor.Controls
{
    public class DictionaryBindingWrapper
    {
        public DictionaryBindingWrapper(Dictionary<string, string> container, string fieldId)
        {
            FieldId = fieldId;
            Container = container;
        }

        public String Value
        {
            get => Container[FieldId];
            set => Container[FieldId] = value;
        }

        public string FieldId { get; set; }
        public Dictionary<string, string> Container { get; set; }
    }
}