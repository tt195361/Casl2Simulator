using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の命令を表わします。
    /// </summary>
    internal class Instruction
    {
        #region Static Fields
        internal static readonly Instruction LoadEaContents =
            new Instruction("LD r,adr,x", Executor.LoadEaContents);
        #endregion

        #region Instance Fields
        private readonly String m_str;
        private readonly Executor.ExecuteAction m_executeAction;
        #endregion

        // このクラスのインスタンスは、クラス外から作成できません。
        private Instruction(String str, Executor.ExecuteAction executeAction)
        {
            m_str = str;
            m_executeAction = executeAction;
        }

        /// <summary>
        /// 命令を実行します。
        /// </summary>
        /// <param name="rR1Field">命令語の中の r/r1 フィールドの値です。</param>
        /// <param name="xR2Field">命令語の中の x/r2 フィールドの値です。</param>
        /// <param name="registerSet">COMET II の一そろいのレジスタです。</param>
        /// <param name="memory">COMET II の主記憶です。</param>
        internal void Execute(UInt16 rR1Field, UInt16 xR2Field, RegisterSet registerSet, Memory memory)
        {
            // それぞれの命令に応じた処理を実行します。
            m_executeAction(rR1Field, xR2Field, registerSet, memory);
        }

        /// <summary>
        /// この命令を表わす文字列を作成します。
        /// </summary>
        /// <returns>この命令を表わす文字列を返します。</returns>
        public override String ToString()
        {
            return m_str;
        }
    }
}
