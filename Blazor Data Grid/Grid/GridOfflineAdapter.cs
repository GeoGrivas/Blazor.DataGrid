using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blazor.DataGrid.Helpers;

namespace Blazor.DataGrid
{
    public class GridOfflineAdapter<TRecord> : IGridAdapter<TRecord>
    {
        private int RecordsCount;
        private List<TRecord> Records;


        public GridOfflineAdapter(List<TRecord> records)
        {
            Records = records;
        }

        public int GetRecordsCount()
        {
            return RecordsCount;
        }

       
        public Expression<Func<TItem, object>> GroupByExpression<TItem>(string[] propertyNames)
        {
            var properties = propertyNames.Select(name => typeof(TItem).GetProperty(name)).ToArray();
            var propertyTypes = properties.Select(p => p.PropertyType).ToArray();
            var tupleTypeDefinition = typeof(Tuple).Assembly.GetType("System.Tuple`" + properties.Length);
            var tupleType = tupleTypeDefinition.MakeGenericType(propertyTypes);
            var constructor = tupleType.GetConstructor(propertyTypes);
            var param = Expression.Parameter(typeof(TItem), "item");
            var body = Expression.New(constructor, properties.Select(p => Expression.Property(param, p)));
            var expr = Expression.Lambda<Func<TItem, object>>(body, param);
            return expr;
        }
        public async Task<List<TRecord>> OnPageChange(int Page, int Take, IEnumerable<GridOrderBy> OrderByList, IEnumerable<GridFilter> Filters)
        {
            await Task.Delay(100);
            var filteredRecords = Records;
            var entityType = typeof(TRecord);
            var records = Records;
            if (Filters.Any(x => !string.IsNullOrWhiteSpace(x.Value)))
            {
                var parameterExpression = Expression.Parameter(entityType);
                Expression rootExpression = null;
                foreach (var filter in Filters.Where(x => !string.IsNullOrWhiteSpace(x.Value)))
                {
                    var property = Expression.Property(parameterExpression, filter.FieldName);

                    var propertyType = ((PropertyInfo)property.Member).PropertyType;
                    Expression expression;
                    if (filter.FilteringType == FilteringType.Contains)
                    {
                        var indexOf = Expression.Call(property, "IndexOf", null, Expression.Constant(filter.Value, typeof(string)), Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
                        expression = Expression.GreaterThanOrEqual(indexOf, Expression.Constant(0));
                    }
                    else
                    {
                        var converter = TypeDescriptor.GetConverter(propertyType); // 1

                        if (!converter.CanConvertFrom(typeof(string))) // 2
                            throw new NotSupportedException();

                        var propertyValue = converter.ConvertFromInvariantString(filter.Value); // 3
                        var constant = Expression.Constant(propertyValue);
                        var valueExpression = Expression.Convert(constant, propertyType); // 4
                        if (filter.FilteringType == FilteringType.GreaterThan)
                        {
                            expression = Expression.GreaterThan(property, constant);
                        }
                        else if (filter.FilteringType == FilteringType.GreaterThanOrEqual)
                        {
                            expression = Expression.GreaterThanOrEqual(property, constant);
                        }
                        else if (filter.FilteringType == FilteringType.LessThan)
                        {
                            expression = Expression.LessThan(property, constant);
                        }
                        else if (filter.FilteringType == FilteringType.LessThanOrEqual)
                        {
                            expression = Expression.LessThanOrEqual(property, constant);
                        }
                        else if (filter.FilteringType == FilteringType.NotEquals)
                        {
                            expression = Expression.NotEqual(property, constant);
                        }
                        else
                        {
                            expression = Expression.Equal(property, constant);
                        }

                    }
                    if (rootExpression == null)
                        rootExpression = expression;
                    else
                        rootExpression = Expression.And(expression, rootExpression);
                }
                var final = Expression.Lambda<Func<TRecord, bool>>(body: rootExpression, parameters: parameterExpression).Compile();
                filteredRecords = Records.Where(final).ToList();


            }
            RecordsCount = filteredRecords.Count();
            if (OrderByList.Count() > 0)
            {
                filteredRecords = filteredRecords.AsQueryable().OrderByColumns<TRecord>(OrderByList).ToList();
                //filteredRecords = filteredRecords.BuildOrderBys(OrderByList.ToArray()).ToList();
            }
            return filteredRecords.Skip(Page * Take).Take(Take).ToList();
        }
      
        public async Task OnRecordDelete(TRecord record)
        {
            await Task.Delay(500);
            Records.Remove(record);
        }

        public async Task OnRecordEdit(TRecord OriginalRecord, TRecord UpdatedRecord)
        {
            await Task.Delay(500);
            var properties = OriginalRecord.GetType().GetProperties();
            foreach (var property in properties)
            {
                property.SetValue(OriginalRecord, property.GetValue(UpdatedRecord));
            }
        }

        public async Task<IEnumerable<GroupResult<TRecord>>> OnPageChange(int Page, int Take, IEnumerable<GridOrderBy> OrderByList, IEnumerable<GridFilter> Filters, List<string> GroupBy)
        {

            await Task.Delay(100);
            var filteredRecords = Records;
            var entityType = typeof(TRecord);
            var records = Records;
            if (Filters.Any(x => !string.IsNullOrWhiteSpace(x.Value)))
            {
                var parameterExpression = Expression.Parameter(entityType);
                Expression rootExpression = null;
                foreach (var filter in Filters.Where(x => !string.IsNullOrWhiteSpace(x.Value)))
                {
                    var property = Expression.Property(parameterExpression, filter.FieldName);

                    var propertyType = ((PropertyInfo)property.Member).PropertyType;
                    Expression expression;
                    if (filter.FilteringType == FilteringType.Contains)
                    {
                        var indexOf = Expression.Call(property, "IndexOf", null, Expression.Constant(filter.Value, typeof(string)), Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
                        expression = Expression.GreaterThanOrEqual(indexOf, Expression.Constant(0));
                    }
                    else
                    {
                        var converter = TypeDescriptor.GetConverter(propertyType); // 1

                        if (!converter.CanConvertFrom(typeof(string))) // 2
                            throw new NotSupportedException();

                        var propertyValue = converter.ConvertFromInvariantString(filter.Value); // 3
                        var constant = Expression.Constant(propertyValue);
                        var valueExpression = Expression.Convert(constant, propertyType); // 4
                        if (filter.FilteringType == FilteringType.GreaterThan)
                        {
                            expression = Expression.GreaterThan(property, constant);
                        }
                        else if (filter.FilteringType == FilteringType.GreaterThanOrEqual)
                        {
                            expression = Expression.GreaterThanOrEqual(property, constant);
                        }
                        else if (filter.FilteringType == FilteringType.LessThan)
                        {
                            expression = Expression.LessThan(property, constant);
                        }
                        else if (filter.FilteringType == FilteringType.LessThanOrEqual)
                        {
                            expression = Expression.LessThanOrEqual(property, constant);
                        }
                        else if (filter.FilteringType == FilteringType.NotEquals)
                        {
                            expression = Expression.NotEqual(property, constant);
                        }
                        else
                        {
                            expression = Expression.Equal(property, constant);
                        }

                    }
                    if (rootExpression == null)
                        rootExpression = expression;
                    else
                        rootExpression = Expression.And(expression, rootExpression);
                }
                var final = Expression.Lambda<Func<TRecord, bool>>(body: rootExpression, parameters: parameterExpression).Compile();
                filteredRecords = Records.Where(final).ToList();


            }
            RecordsCount = filteredRecords.Count();
            //var orderByList = OrderByList.ToList();
            GroupBy.Reverse();
            var orderByList = OrderByList.ToList();
            foreach (var groupBy in GroupBy)
            {
                if (orderByList.Select(x => x.OrderBy).Contains(groupBy))
                {
                    var orderBy = orderByList.First(x => x.OrderBy == groupBy);
                    orderByList.Remove(orderBy);
                    orderByList.Insert(0, orderBy);
                }
                else
                {
                    orderByList.Insert(0, new GridOrderBy { OrderBy = groupBy, OrderByMethod = OrderByMethod.OrderBy });
                }
            }
            if (orderByList.Count() > 0)
            {
                var type = typeof(TRecord);
                bool isFirstOrdering = true;
                var ordered = filteredRecords.AsQueryable();
                var parameter = Expression.Parameter(type);

                foreach (var orderBy in orderByList)
                {
                    var property = type.GetProperty(orderBy.OrderBy);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    var typeArguments = new Type[] { type, property.PropertyType };
                    string methodName = "";
                    if (isFirstOrdering)
                        methodName = orderBy.OrderByMethod == OrderByMethod.OrderBy ? "OrderBy" : "OrderByDescending";
                    else
                        methodName = orderBy.OrderByMethod == OrderByMethod.OrderBy ? "ThenBy" : "ThenByDescending";
                    var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, ordered.Expression, Expression.Quote(orderByExp));
                    ordered = ordered.Provider.CreateQuery<TRecord>(resultExp);
                    isFirstOrdering = false;
                }
                filteredRecords = ordered.ToList();
            }
            GroupBy.Reverse();

            var test = filteredRecords.Skip(Page * Take).Take(Take);
            List<Func<TRecord, object>> groupSelectors = new List<Func<TRecord, object>>();
            foreach (var x in GroupBy)
            {
                var str1 = new string[1] { x };
                groupSelectors.Add(GroupByExpression<TRecord>(str1).Compile());
            }
            return test.GroupByMany(groupSelectors.ToArray());
        }
    }
}
