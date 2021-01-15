using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.DataGrid
{
    public interface IGridColumn
    {
        string GetHeaderText();
        string GetCss();
        string GetFieldName();
        bool IsColumnFilter();
        Type GetValueType();
        FilteringType GetFilteringType();
        string GetFilterDisplayText();
        EventCallback<object> GetEventCallback();
        RenderFragment<object> GetChild();
    }
}
