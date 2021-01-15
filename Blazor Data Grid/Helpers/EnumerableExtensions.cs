using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Blazor.DataGrid.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<GroupResult<TElement>> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors.Length > 0)
            {
                var selector = groupSelectors.First();

                //reduce the list recursively until zero
                var nextSelectors = groupSelectors.Skip(1).ToArray();
                return
                    elements.GroupBy(selector).Select(
                        g => new GroupResult<TElement>
                        {
                            Key = g.Key,
                            Count = g.Count(),
                            Items = g,
                            SubGroups = g.GroupByMany(nextSelectors)
                        });
            }
            else
            {
                return null;
            }
        }
        public static IQueryable<TModel> OrderByColumns<TModel>(
            this IQueryable<TModel> collection,
            IEnumerable<GridOrderBy> sortedColumns)
        {
            // Basically sortedColumns contains the columns user wants to sort by, and 
            // the sorting direction.
            // For my screenshot, the sortedColumns looks like
            // [
            //     { "cassette", { Order = 1, Direction = SortDirection.Ascending } },
            //     { "slotNumber", { Order = 2, Direction = SortDirection.Ascending } }
            // ]

            bool firstTime = true;

            // The type that represents each row in the table
            var itemType = typeof(TModel);

            // Name the parameter passed into the lamda "x", of the type TModel
            var parameter = Expression.Parameter(itemType, "x");

            // Loop through the sorted columns to build the expression tree
            foreach (var sortedColumn in sortedColumns)
            {
                // Get the property from the TModel, based on the key
                var prop = Expression.Property(parameter, sortedColumn.OrderBy);

                // Build something like x => x.Cassette or x => x.SlotNumber
                var exp = Expression.Lambda(prop, parameter);

                // Based on the sorting direction, get the right method
                string method = String.Empty;
                if (firstTime)
                {
                    method = sortedColumn.OrderByMethod == OrderByMethod.OrderBy
                        ? "OrderBy"
                        : "OrderByDescending";

                    firstTime = false;
                }
                else
                {
                    method = sortedColumn.OrderByMethod == OrderByMethod.OrderBy
                        ? "ThenBy"
                        : "ThenByDescending";
                }

                // itemType is the type of the TModel
                // exp.Body.Type is the type of the property. Again, for Cassette, it's
                //     a String. For SlotNumber, it's a Double.
                Type[] types = new Type[] { itemType, exp.Body.Type };

                // Build the call expression
                // It will look something like:
                //     OrderBy*(x => x.Cassette) or Order*(x => x.SlotNumber)
                //     ThenBy*(x => x.Cassette) or ThenBy*(x => x.SlotNumber)
                var mce = Expression.Call(typeof(Queryable), method, types,
                    collection.Expression, exp);

                // Now you can run the expression against the collection
                collection = collection.Provider.CreateQuery<TModel>(mce);
            }

            return collection;
        }
       
    }

}

