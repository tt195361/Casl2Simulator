using System;

namespace Tt195361.Casl2Simulator.Utils
{
    internal static class NumberUtils
    {
        /// <summary>
        /// 指定の <see cref="Int16"/> 型の値を <see cref="UInt16"/> 型の値に変換します。
        /// </summary>
        /// <param name="i16Val">変換する <see cref="Int16"/> 型の値です。</param>
        /// <returns>変換された <see cref="UInt16"/> 型の値を返します。</returns>
        internal static UInt16 ToUInt16(Int16 i16Val)
        {
            // オーバーフローチェックなし。
            unchecked
            {
                UInt16 ui16Val = (UInt16)i16Val;
                return ui16Val;
            }
        }

        /// <summary>
        /// 指定の <see cref="Int32"/> 型の値を <see cref="UInt16"/> 型の値に変換します。
        /// </summary>
        /// <param name="i32Val">変換する <see cref="Int32"/> 型の値です。</param>
        /// <returns>変換された <see cref="UInt16"/> 型の値を返します。</returns>
        internal static UInt16 ToUInt16(Int32 i32Val)
        {
            // オーバーフローチェックなし。
            unchecked
            {
                UInt16 ui16Val = (UInt16)i32Val;
                return ui16Val;
            }
        }

        /// <summary>
        /// 指定の <see cref="UInt16"/> 型の値を <see cref="Int16"/> 型の値に変換します。
        /// </summary>
        /// <param name="ui16Val">変換する <see cref="UInt16"/> 型の値です。</param>
        /// <returns>変換された <see cref="Int16"/> 型の値を返します。</returns>
        internal static Int16 ToInt16(UInt16 ui16Val)
        {
            // オーバーフローチェックなし。
            unchecked
            {
                Int16 i16Val = (Int16)ui16Val;
                return i16Val;
            }
        }

        /// <summary>
        /// 指定の <see cref="Int32"/> 型の値を <see cref="Int16"/> 型の値に変換します。
        /// </summary>
        /// <param name="i32Val">変換する <see cref="Int32"/> 型の値です。</param>
        /// <returns>変換された <see cref="Int16"/> 型の値を返します。</returns>
        internal static Int16 ToInt16(Int32 i32Val)
        {
            // オーバーフローチェックなし。
            unchecked
            {
                Int16 i16Val = (Int16)i32Val;
                return i16Val;
            }
        }

        /// <summary>
        /// 指定の <see cref="Int32"/> 型の値を <see cref="Int16"/> 型の値に変換すると
        /// オーバーフローするかどうかをチェックします。
        /// </summary>
        /// <param name="i32Val">チェックする <see cref="Int32"/> 型の値です。</param>
        /// <returns>
        /// オーバーフローが発生する場合は <see langword="true"/> を、
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean CheckInt16Overflow(Int32 i32Val)
        {
            // オーバーフローチェックあり。
            checked
            {
                try
                {
                    Int16 notUsed = (Int16)i32Val;
                    return false;
                }
                catch (OverflowException)
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 指定の <see cref="Int32"/> 型の値を <see cref="UInt16"/> 型の値に変換すると
        /// オーバーフローするかどうかをチェックします。
        /// </summary>
        /// <param name="i32Val">チェックする <see cref="Int32"/> 型の値です。</param>
        /// <returns>
        /// オーバーフローが発生する場合は <see langword="true"/> を、
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean CheckUInt16Overflow(Int32 i32Val)
        {
            // オーバーフローチェックあり。
            checked
            {
                try
                {
                    UInt16 notUsed = (UInt16)i32Val;
                    return false;
                }
                catch (OverflowException)
                {
                    return true;
                }
            }
        }
    }
}
