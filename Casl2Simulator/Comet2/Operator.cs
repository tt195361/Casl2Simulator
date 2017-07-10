using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 演算を行います。
    /// </summary>
    internal class Operator
    {
        private delegate void OperateAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory);
        private delegate void CompareAction(
            Word word1, Word word2, out Boolean sign, out Boolean zero);

        #region Load
        /// <summary>
        /// r &lt;- オペランド, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator Load = new Operator(LoadAction);

        private static void LoadAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            r.Value = operand;
            const Boolean OverflowFlag = false;
            registerSet.FR.SetFlags(r, OverflowFlag);
        }
        #endregion

        #region Store
        /// <summary>
        /// 実効アドレス &lt;- (r), -- 実効前の値が保持される。
        /// </summary>
        internal static readonly Operator Store = new Operator(StoreAction);

        private static void StoreAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            UInt16 effectiveAddress = operand.GetAsUnsigned();
            memory.Write(effectiveAddress, r.Value);
        }
        #endregion

        #region AddArithmetic
        /// <summary>
        /// r &lt;- (r) + オペランド, 〇: 設定される。
        /// </summary>
        internal static readonly Operator AddArithmetic = new Operator(AddArithmeticAction);

        private static void AddArithmeticAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            Boolean overflowFlag;
            r.Value = Alu.AddArithmetic(r.Value, operand, out overflowFlag);
            registerSet.FR.SetFlags(r, overflowFlag);
        }
        #endregion

        #region CompareArithmetic
        /// <summary>
        /// r と オペランドを算術比較し FR を設定する, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator CompareArithmetic = new Operator(CompareArithmeticAction);

        private static void CompareArithmeticAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            Compare(Alu.CompareArithmetic, r, operand, registerSet.FR);
        }
        #endregion

        #region CompareLogical
        /// <summary>
        /// r と オペランドを論理比較し FR を設定する, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator CompareLogical = new Operator(CompareLogicalAction);

        private static void CompareLogicalAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            Compare(Alu.CompareLogical, r, operand, registerSet.FR);
        }
        #endregion

        #region Compare
        private static void Compare(
            CompareAction cmpAction, Register r, Word operand, FlagRegister fr)
        {
            Boolean signFlag;
            Boolean zeroFlag;
            cmpAction(r.Value, operand, out signFlag, out zeroFlag);
            const Boolean OverflowFlag = false;
            fr.SetFlags(OverflowFlag, signFlag, zeroFlag);
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
        /// <param name="memory">COMET II の主記憶です。</param>
        internal void Operate(Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            m_operateAction(r, operand, registerSet, memory);
        }
    }
}
