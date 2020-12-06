using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.DataGrid.Helpers
{
    public class GroupResult<TItem>
    {
        public object Key { get; set; }
        public int Count { get; set; }
        public IEnumerable<TItem> Items { get; set; }
        public IEnumerable<GroupResult<TItem>> SubGroups { get; set; }
        public override string ToString()
        { return string.Format("{0} ({1})", Key, Count); }
    }
}
