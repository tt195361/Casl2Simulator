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
            Casl2Defs.GR0,
            Casl2Defs.GR1,
            Casl2Defs.GR2,
            Casl2Defs.GR3,
            Casl2Defs.GR4,
            Casl2Defs.GR5,
            Casl2Defs.GR6,
            Casl2Defs.GR7,
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
