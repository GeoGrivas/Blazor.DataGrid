using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.DataGrid
{
    public class GridFilter
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string FieldName { get; set; }
        public bool IsExtra { get; set; }
        public FilteringType FilteringType { get; set; }
        public Type Type { get; set; }
        public string DisplayText { get; set; }
    }
}
