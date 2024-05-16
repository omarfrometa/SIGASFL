using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Repositories.Extension
{
    public static class EFExtension
    {
        private static readonly string READONLY_ANNOTATION = "custom:readonly";

        public static EntityTypeBuilder<TEntity> IsReadOnly<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class
        {
            builder.HasAnnotation(READONLY_ANNOTATION, true);
            return builder;
        }

        public static bool IsReadOnly(this IEntityType entity)
        {
            var annotation = entity.FindAnnotation(READONLY_ANNOTATION);
            if (annotation != null)
            {
                return (bool)annotation.Value;
            }

            return false;
        }

        public static IQueryable<TEntity> SelectFromStoreProcedure<TEntity, TRequest>(this DbSet<TEntity> dbset, TRequest spParams, string procedureName)
            where TEntity : class
        {
            var query = new StringBuilder($"{procedureName} ");
            Dictionary<string, object> propValues = GetPropertiesValues(spParams);
            var parameters = propValues.Select(s =>
            {
                query.Append($"{s.Key}={s.Key}, ");
                object objValue = DBNull.Value;
                return new SqlParameter
                {
                    ParameterName = s.Key,
                    Value = s.Value ?? objValue
                };
            }).ToArray();

            return dbset.FromSqlRaw(query.ToString().Trim().TrimEnd(','), parameters);
        }

        public static IQueryable<TEntity> SelectFromStoreProcedure<TEntity>
            (this DbSet<TEntity> dbset, string procedureName) where TEntity : class
        {
            return dbset.FromSqlRaw(procedureName);
        }

        private static Dictionary<string, object> GetPropertiesValues(object obj)
        {
            var result = new Dictionary<string, object>();
            Type type = obj.GetType();
            List<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

            props.ForEach(f =>
            {
                var value = f.GetValue(obj, null);
                result.Add($"@{f.Name}", value);
            });
            return result;
        }
    }
}
