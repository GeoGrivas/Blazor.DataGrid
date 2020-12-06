using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.DataGrid
{
    public interface IGridFilters
    {
        Task Go(GridFilter GridFilter);
        void AddGridFilter(GridFilter gridFilter);
        int GetGridFiltersCount();
        string GetFilterValue(int id);
        GridFilter GetGridFilter(int id);
    }
}
