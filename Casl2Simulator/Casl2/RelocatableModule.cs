using System;
using System.Collections.Generic;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラの出力となる再配置可能モジュールです。
    /// </summary>
    internal class RelocatableModule
    {
        #region Fields
        private readonly List<UInt16> m_codeList;
        #endregion

        internal RelocatableModule()
        {
            m_codeList = new List<UInt16>();
        }

        // 実行開始アドレス

        // Exports: このモジュールが定義し、外部モジュールから参照できるアドレス。

        // Imports: このモジュールが参照し、外部モジュールが提供するアドレス。

        // コード

        internal UInt16 GetCurrentOffset()
        {
            return (UInt16)m_codeList.Count;
        }
    }
}
