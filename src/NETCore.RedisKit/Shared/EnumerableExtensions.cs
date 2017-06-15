using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit.Shared
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// 循环方法
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="enumerable">待循环的集合</param>
        /// <param name="action">要执行的方法</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }
    }
}
