﻿using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 演算を行います。
    /// </summary>
    internal class Operator
    {
        private delegate void OperateAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory);

        #region Load/Store
        /// <summary>
        /// r &lt;- オペランド, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator LoadWithFr = new Operator(LoadWithFrAction);

        private static void LoadWithFrAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            r.Value = operand;
            const Boolean OverflowFlag = false;
            registerSet.FR.SetFlags(r, OverflowFlag);
        }

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

        /// <summary>
        /// r &lt;- オペランド, -- 実効前の値が保持される。
        /// </summary>
        internal static readonly Operator LoadWithoutFr = new Operator(LoadWithoutFrAction);

        private static void LoadWithoutFrAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            r.Value = operand;
        }
        #endregion // Load/Store

        #region Arithmetic/Logical Operation
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
        #endregion // Arithmetic/Logical Operation

        #region Comparison
        /// <summary>
        /// r と オペランドを算術比較し FR を設定する, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator CompareArithmetic = new Operator(CompareArithmeticAction);

        private static void CompareArithmeticAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            DoCompare(Alu.CompareArithmetic, r, operand, registerSet.FR);
        }

        /// <summary>
        /// r と オペランドを論理比較し FR を設定する, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator CompareLogical = new Operator(CompareLogicalAction);

        private static void CompareLogicalAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            DoCompare(Alu.CompareLogical, r, operand, registerSet.FR);
        }

        private static void DoCompare(
            Alu.CompareMethod compareMethod, Register r, Word operand, FlagRegister fr)
        {
            Boolean signFlag;
            Boolean zeroFlag;
            compareMethod(r.Value, operand, out signFlag, out zeroFlag);
            const Boolean OverflowFlag = false;
            fr.SetFlags(OverflowFlag, signFlag, zeroFlag);
        }
        #endregion // Comparison

        #region Shift
        /// <summary>
        /// 符号を除き (r) をオペランドで指定したビット数だけ左にシフトする。シフトの結果、
        /// 空いたビット位置には 0 が入る。FR を設定する, 〇*2: 設定される。ただし、OF には
        /// レジスタから最後に送り出されたビットの値が設定される。
        /// </summary>
        internal static readonly Operator ShiftLeftArithmetic = new Operator(ShiftLeftArithmeticAction);

        private static void ShiftLeftArithmeticAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            DoShift(Alu.ShiftLeftArithmetic, r, operand, registerSet.FR);
        }

        /// <summary>
        /// 符号を除き (r) をオペランドで指定したビット数だけ右にシフトする。シフトの結果、
        /// 空いたビット位置には符号と同じものが入る。FR を設定する, 〇*2: 設定される。ただし、OF には
        /// レジスタから最後に送り出されたビットの値が設定される。
        /// </summary>
        internal static readonly Operator ShiftRightArithmetic = new Operator(ShiftRightArithmeticAction);

        private static void ShiftRightArithmeticAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            DoShift(Alu.ShiftRightArithmetic, r, operand, registerSet.FR);
        }

        /// <summary>
        /// 符号を含み (r) をオペランドで指定したビット数だけ左にシフトする。シフトの結果、
        /// 空いたビット位置には 0 が入る。FR を設定する, 〇*2: 設定される。ただし、OF には
        /// レジスタから最後に送り出されたビットの値が設定される。
        /// </summary>
        internal static readonly Operator ShiftLeftLogical = new Operator(ShiftLeftLogicalAction);

        private static void ShiftLeftLogicalAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            DoShift(Alu.ShiftLeftLogical, r, operand, registerSet.FR);
        }

        /// <summary>
        /// 符号を含み (r) をオペランドで指定したビット数だけ右にシフトする。シフトの結果、
        /// 空いたビット位置には 0 が入る。FR を設定する, 〇*2: 設定される。ただし、OF には
        /// レジスタから最後に送り出されたビットの値が設定される。
        /// </summary>
        internal static readonly Operator ShiftRightLogical = new Operator(ShiftRightLogicalAction);

        private static void ShiftRightLogicalAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            DoShift(Alu.ShiftRightLogical, r, operand, registerSet.FR);
        }

        private static void DoShift(
            Alu.ShiftMethod shiftMethod, Register r, Word operand, FlagRegister fr)
        {
            UInt16 lastShiftedOutBit;
            r.Value = shiftMethod(r.Value, operand, out lastShiftedOutBit);
            Boolean overflowFlag = (lastShiftedOutBit != 0);
            fr.SetFlags(r, overflowFlag);
        }
        #endregion

        #region Jump
        /// <summary>
        /// SF=1 のとき、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator JumpOnMinus = new Operator(JumpOnMinusAction);

        private static void JumpOnMinusAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            Boolean condition = registerSet.FR.SF;
            DoJump(condition, operand, registerSet.PR);
        }

        /// <summary>
        /// ZF=0 のとき、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator JumpOnNonZero = new Operator(JumpOnNonZeroAction);

        private static void JumpOnNonZeroAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            Boolean condition = !registerSet.FR.ZF;
            DoJump(condition, operand, registerSet.PR);
        }

        /// <summary>
        /// ZF=1 のとき、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator JumpOnZero = new Operator(JumpOnZeroAction);

        private static void JumpOnZeroAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            Boolean condition = registerSet.FR.ZF;
            DoJump(condition, operand, registerSet.PR);
        }

        /// <summary>
        /// 無条件で、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator UnconditionalJump = new Operator(UnconditionalJumpAction);

        private static void UnconditionalJumpAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            const Boolean Condition = true;
            DoJump(Condition, operand, registerSet.PR);
        }

        /// <summary>
        /// SF=0 かつ ZF=0 のとき、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator JumpOnPlus = new Operator(JumpOnPlusAction);

        private static void JumpOnPlusAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            FlagRegister fr = registerSet.FR;
            Boolean condition = (!fr.SF && !fr.ZF);
            DoJump(condition, operand, registerSet.PR);
        }

        /// <summary>
        /// OF=1 のとき、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator JumpOnOverflow = new Operator(JumpOnOverflowAction);

        private static void JumpOnOverflowAction(
            Register r, Word operand, RegisterSet registerSet, Memory memory)
        {
            Boolean condition = registerSet.FR.OF;
            DoJump(condition, operand, registerSet.PR);
        }

        private static void DoJump(Boolean condition, Word operand, Register pr)
        {
            if (condition)
            {
                pr.Value = operand;
            }
        }
        #endregion // Jump

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
