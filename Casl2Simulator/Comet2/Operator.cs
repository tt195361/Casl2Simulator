using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 演算を行います。
    /// </summary>
    internal class Operator
    {
        private delegate void OperateAction(Register r, Word operand, RegisterSet registerSet);

        #region Load
        /// <summary>
        /// r &lt;- オペランド, 〇*1: 設定されることを示す。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator Load = new Operator(LoadAction);

        private static void LoadAction(Register r, Word operand, RegisterSet registerSet)
        {
            r.Value = operand;
            const Boolean OverflowFlag = false;
            registerSet.FR.SetFlags(r, OverflowFlag);
        }
        #endregion

        #region AddArithmetic
        /// <summary>
        /// r &lt;- (r) + オペランド, 〇: 設定されることを示す。
        /// </summary>
        internal static readonly Operator AddArithmetic = new Operator(AddArithmeticAction);

        private static void AddArithmeticAction(Register r, Word operand, RegisterSet registerSet)
        {
            Boolean overflowFlag;
            r.Value = Alu.AddArithmetic(r.Value, operand, out overflowFlag);
            registerSet.FR.SetFlags(r, overflowFlag);
        }
        #endregion

        #region Fields
        private readonly OperateAction m_operateAction;
        #endregion

        private Operator(OperateAction operateAction)
        {
            m_operateAction = operateAction;
        }

        /// <summary>
        /// 演算を実行します。
        /// </summary>
        /// <param name="r">演算に使用するレジスタです。</param>
        /// <param name="operand">演算対象の値を保持する語です。</param>
        /// <param name="registerSet">COMET II の一そろいのレジスタです。</param>
        internal void Operate(Register r, Word operand, RegisterSet registerSet)
        {
            m_operateAction(r, operand, registerSet);
        }
    }
}
