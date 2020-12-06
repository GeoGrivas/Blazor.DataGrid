using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Blazor.DataGrid
{
    public interface IGrid
    {
        void AddColumn(IGridColumn column);
        Type GetType();
        void SelectRecord(EventArgs e,object record);
        PropertyInfo[] GetProperties();
        List<IGridColumn> GetColumns();
        //object GetSelectedRecord();
        bool IsRecordSelected(object record);
        Task GoToPage(int x);
        bool ShouldRenderCheckbox();
        Task OnOrderClicked(GridOrderBy orderBy);
        Task RemoveOrdering(string field);
        string GetOrderSymbol(string Field);
        bool HasOrdering();
        void SetDraggingValue(string value);
    }
}
