using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.DataGrid.Helpers;

namespace Blazor.DataGrid
{
    public interface IGridAdapter<TRecord>
    {
        Task<List<TRecord>> OnPageChange(int Page, int Take,IEnumerable<GridOrderBy> OrderByList,IEnumerable<GridFilter> Filters);
         Task<IEnumerable<GroupResult<TRecord>>> OnPageChange(int Page, int Take, IEnumerable<GridOrderBy> OrderByList, IEnumerable<GridFilter> Filters, List<string> GroupBy);
        Task OnRecordDelete(TRecord record);
        Task OnRecordEdit(TRecord original,TRecord updated);
        int GetRecordsCount();
    }
}
