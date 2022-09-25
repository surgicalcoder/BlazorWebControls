using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FastMember;
using GoLive.Saturn.Data.Entities;

namespace GoLive.Blazor.Controls
{
    public static class UpdateHelperExtension
    {
        public static void UpdateFrom<T>(this T source, T updated, params Expression<Func<T, dynamic>>[] func)
        {
            if (func == null) return;

            var accessor = TypeAccessor.Create(typeof(T));

            foreach (var exp in func)
            {
                var propName = exp.GetPropertyName();

                if (exp.Body.Type.Assembly == typeof(Int32).Assembly)
                {
                    accessor[source, propName] = accessor[updated, propName];
                    continue;
                }

                var check = typeof(IUpdatableFrom<>).MakeGenericType(exp.Body.Type);

                if (check.IsAssignableFrom(exp.Body.Type))
                {
                    try
                    {
                        var method = check.GetMethod("UpdateFrom", BindingFlags.Instance | BindingFlags.Public);
                        method?.Invoke(accessor[source, propName], new object[] { accessor[updated, propName] });
                    }
                    catch (Exception)
                    {
                        accessor[source, propName] = accessor[updated, propName];
                    }
                }
                else
                {
                    accessor[source, propName] = accessor[updated, propName];
                }
            }
        }

        public static void UpdateFromList<T>(this T source, T updated, List<Expression<Func<T, dynamic>>> func)
        {
            if (func == null) return;

            var accessor = TypeAccessor.Create(typeof(T));

            foreach (var exp in func)
            {
                var propName = exp.GetPropertyName();

                if (exp.Body.Type.Assembly == typeof(Int32).Assembly)
                {
                    accessor[source, propName] = accessor[updated, propName];
                    continue;
                }

                var check = typeof(IUpdatableFrom<>).MakeGenericType(exp.Body.Type);

                if (check.IsAssignableFrom(exp.Body.Type))
                {
                    try
                    {
                        var method = check.GetMethod("UpdateFrom", BindingFlags.Instance | BindingFlags.Public);
                        method?.Invoke(accessor[source, propName], new object[] { accessor[updated, propName] });
                    }
                    catch (Exception)
                    {
                        accessor[source, propName] = accessor[updated, propName];
                    }
                }
                else
                {
                    accessor[source, propName] = accessor[updated, propName];
                }
            }
        }


        public static string GetPropertyName<TModel, TValue>(this Expression<Func<TModel, TValue>> propertySelector, char delimiter = '.', char endTrim = ')')
        {
            return (propertySelector.Body as MemberExpression ?? (MemberExpression)((UnaryExpression)propertySelector.Body).Operand).Member.Name;
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64EncodeUrlSafe(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes).Replace('+', '-').Replace('/', '_');
        }
        public static string Base64DecodeUrlSafe(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData.Replace('-','+').Replace('_','/'));
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}