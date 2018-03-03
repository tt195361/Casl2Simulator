using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II の予約語を管理します。
    /// </summary>
    internal class ReservedWord
    {
        #region Static Fields
        private static readonly HashSet<String> m_reservedWords = new HashSet<String>()
        {
            RegisterDef.GR0,
            RegisterDef.GR1,
            RegisterDef.GR2,
            RegisterDef.GR3,
            RegisterDef.GR4,
            RegisterDef.GR5,
            RegisterDef.GR6,
            RegisterDef.GR7,
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
