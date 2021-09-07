using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AwesomeMvcDemo.Utils
{
    public static class ExprHelper
    {
        /// <summary>
        /// Get property info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> lambda)
        {
            var member = lambda.Body as MemberExpression;
            var prop = member.Member as PropertyInfo;
            return prop;
        }

        /// <summary>
        /// Get lambda expression for Enumerable.Count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpr">property to call Count() on</param>
        /// <returns></returns>
        public static LambdaExpression GetCountLambda<T>(Expression<Func<T, object>> propertyExpr)
        {
            var o = Expression.Parameter(typeof(T), "o");
            var collProp = GetProperty(propertyExpr);

            var propAccess = Expression.MakeMemberAccess(o, collProp);
            var genericTypeArgument = collProp.PropertyType.GenericTypeArguments[0];

            var countMethodInfo = typeof (Enumerable).GetMethods().Single(
                method => method.Name == "Count" && method.IsStatic && method.GetParameters().Length == 1);

            // make a generic method call for System.Linq.Enumerable.Count<> for the type of this collection
            var localCountMethodInfo = countMethodInfo.MakeGenericMethod(genericTypeArgument);

            var call = Expression.Call(localCountMethodInfo, propAccess);

            return Expression.Lambda(call, o);
        }
    }
}