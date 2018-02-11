using System;
using System.Collections.Generic;
using System.Text;

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

        /// <summary>
        /// 指定の回数だけ指定の引数のない動作を行います。
        /// </summary>
        /// <param name="count">動作を行う回数です。</param>
        /// <param name="action">指定の回数行う引数のない動作です。</param>
        internal static void Times(this Int32 count, Action action)
        {
            for (Int32 index = 0; index < count; index++)
            {
                action();
            }
        }

        /// <summary>
        /// 指定の回数だけ指定の何回目かを示すインデックスの値を引数に取る動作を行います。
        /// </summary>
        /// <param name="count">動作を行う回数です。</param>
        /// <param name="action">指定の回数行う何回目かを示すインデックスの値を引数に取る動作です。</param>
        internal static void Times(this Int32 count, Action<Int32> action)
        {
            for (Int32 index = 0; index < count; index++)
            {
                action(index);
            }
        }

        /// <summary>
        /// それぞれの文字列を <paramref name="delimiter"/> で区切って連結します。
        /// </summary>
        internal static String MakeList(this IEnumerable<String> strEnumerable, String delimiter)
        {
            return strEnumerable.Delimit(delimiter)
                                .ConcatStrings();
        }

        /// <summary>
        /// 指定の<param name="enumerable" />の各項目の間に、<param name="delimiter" />を挿入します。
        /// </summary>
        private static IEnumerable<T> Delimit<T>(this IEnumerable<T> enumerable, T delimiter)
        {
            Boolean afterSecond = false;
            foreach (T item in enumerable)
            {
                if (afterSecond)
                {
                    yield return delimiter;
                }

                yield return item;
                afterSecond = true;
            }
        }

        /// <summary>
        /// それぞれの文字列を連結します。
        /// </summary>
        /// <param name="strEnumerable"><see cref="String"/> 型の列挙子です。</param>
        /// <returns>それぞれの文字列を連結した文字列を返します。</returns>
        internal static String ConcatStrings(this IEnumerable<String> strEnumerable)
        {
            StringBuilder builder = new StringBuilder();
            strEnumerable.ForEach((str) => builder.Append(str));
            return builder.ToString();
        }

        /// <summary>
        /// それぞれの文字を連結します。
        /// </summary>
        /// <param name="charEnumerable"><see cref="Char"/> 型の列挙子です。</param>
        /// <returns>それぞれの文字を連結した文字列を返します。</returns>
        internal static String ConcatChars(this IEnumerable<Char> charEnumerable)
        {
            StringBuilder builder = new StringBuilder();
            charEnumerable.ForEach((c) => builder.Append(c));
            return builder.ToString();
        }
    }
}
