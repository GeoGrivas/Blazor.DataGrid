using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.DataGrid
{
    public class GridOrderBy
    {
        public string OrderBy { get; set; }
        public OrderByMethod OrderByMethod { get; set; }
        
    }
    public enum OrderByMethod
    {
        OrderBy,OrderByDescending
    }
}
