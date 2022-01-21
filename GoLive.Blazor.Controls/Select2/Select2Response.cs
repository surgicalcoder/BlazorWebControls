using System.Collections.Generic;

namespace GoLive.Blazor.Controls.Select2
{
    internal class Select2Response
    {
        public Select2Response()
        {
            Results = new List<Select2Item>();
            Pagination = new Select2Pagination(false);
        }

        public List<Select2Item> Results { get; set; }
        public Select2Pagination Pagination { get; }
    }
}