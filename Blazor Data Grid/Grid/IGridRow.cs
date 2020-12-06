using Blazor.DataGrid.Grid;

namespace Blazor.DataGrid
{
    public class GridRow<T>
    {
        public Grid<T> Grid { get; set; }
        public T Record { get; set; }
    }
}
