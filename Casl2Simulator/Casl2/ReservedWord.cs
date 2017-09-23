using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II の予約語を管理します。
    /// </summary>
    internal class ReservedWord
    {
        #region Fields
        private static readonly HashSet<String> m_reservedWords = new HashSet<String>()
        {
            Operand.GR0,
            Operand.GR1,
            Operand.GR2,
            Operand.GR3,
            Operand.GR4,
            Operand.GR5,
            Operand.GR6,
            Operand.GR7,
        };
        #endregion

        internal static Boolean IsReserved(String str)
        {
            return m_reservedWords.Contains(str);
        }

        internal static String GetList()
        {
            return m_reservedWords.MakeList(", ");
        }
    }
}
