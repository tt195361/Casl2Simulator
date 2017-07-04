using System;
using System.Collections.Generic;

namespace Tt195361.Casl2Simulator.Utils
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/> に対して使用する汎用のメソッドを集めたクラスです。 
    /// </summary>
    internal static class EnumerableUtils
    {
        /// <summary>
        /// 列挙子のそれぞれの項目について、指定の動作を行います。
        /// </summary>
        /// <typeparam name="T">列挙子の項目の型です。</typeparam>
        /// <param name="enumerable">それぞれの項目を順に列挙する列挙子です。</param>
        /// <param name="action">それぞれの項目に対して行う動作です。引数は項目です。</param>
        internal static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// 列挙子のそれぞれの項目について、インデックス付きで指定の動作を行います。
        /// </summary>
        /// <typeparam name="T">列挙子の項目の型です。</typeparam>
        /// <param name="enumerable">それぞれの項目を順に列挙する列挙子です。</param>
        /// <param name="action">それぞれの項目に対して行う動作です。引数は項目のインデックスと項目です。</param>
        internal static void ForEach<T>(this IEnumerable<T> enumerable, Action<Int32, T> action)
        {
            Int32 index = 0;
            foreach (T item in enumerable)
            {
                action(index, item);
                ++index;
            }
        }
    }
}
