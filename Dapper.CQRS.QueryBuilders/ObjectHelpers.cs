using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Dapper.CQRS.QueryBuilders
{
    public static class ObjectHelpers
    {
        public static DataTable ReturnClassAsDataTable<T>(this object instance) where T : class
        {
            var type = instance.GetType();
            return type.GetProperties()
                .Aggregate(
                    new DataTable(),
                    (acc, cur) =>
                    {
                        acc.Columns.Add(cur.Name,
                            Nullable.GetUnderlyingType(cur.PropertyType)
                            ?? cur.PropertyType);
                        return acc;
                    });
        }

        public static List<PropertyInfo> GetObjectProperties(this object obj)
        {
            var objType = obj.GetType();
            return new List<PropertyInfo>(objType.GetProperties());
        }

        public static void RemovePropertiesFromDatatable(
            this DataTable dataTable,
            IEnumerable<PropertyInfo> properties)
        {
            foreach (var item in properties)
            {
                dataTable.Columns.Remove(item.Name);
            }
        }
    }
}