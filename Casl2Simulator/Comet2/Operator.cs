using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 演算を行います。
    /// </summary>
    internal class Operator
    {
        #region Events
        /// <summary>
        /// ReturnFromSubroutine 命令を実行しようとすると発生します。
        /// </summary>
        internal static event EventHandler<ReturningFromSubroutineEventArgs> ReturningFromSubroutine;

        /// <summary>
        /// スーパーバイザーを呼び出そうとすると発生します。
        /// </summary>
        internal static event EventHandler<CallingSuperVisorEventArgs> CallingSuperVisor;
        #endregion

        #region Delegates
        private delegate void OperateAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory);
        #endregion

        #region Load/Store
        /// <summary>
        /// r &lt;- オペランド, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator LoadWithFr = new Operator(LoadWithFrAction);

        private static void LoadWithFrAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
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
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            memory.Write(operand, r.Value);
        }

        /// <summary>
        /// r &lt;- オペランド, -- 実効前の値が保持される。
        /// </summary>
        internal static readonly Operator LoadWithoutFr = new Operator(LoadWithoutFrAction);

        private static void LoadWithoutFrAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
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
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoOperation(Alu.AddArithmetic, r, operand, registerSet.FR, memory);
        }

        /// <summary>
        /// r &lt;- (r) - オペランド, 〇: 設定される。
        /// </summary>
        internal static readonly Operator SubtractArithmetic = new Operator(SubtractArithmeticAction);

        private static void SubtractArithmeticAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoOperation(Alu.SubtractArithmetic, r, operand, registerSet.FR, memory);
        }

        /// <summary>
        /// r &lt;- (r) +L オペランド, 〇: 設定される。
        /// </summary>
        internal static readonly Operator AddLogical = new Operator(AddLogicalAction);

        private static void AddLogicalAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoOperation(Alu.AddLogical, r, operand, registerSet.FR, memory);
        }

        /// <summary>
        /// r &lt;- (r) -L オペランド, 〇: 設定される。
        /// </summary>
        internal static readonly Operator SubtractLogical = new Operator(SubtractLogicalAction);

        private static void SubtractLogicalAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoOperation(Alu.SubtractLogical, r, operand, registerSet.FR, memory);
        }

        private static void DoOperation(
            Alu.OperationMethod operationMethod, CpuRegister r, Word operand, FlagRegister fr, Memory memory)
        {
            Boolean overflowFlag;
            r.Value = operationMethod(r.Value, operand, out overflowFlag);
            fr.SetFlags(r, overflowFlag);
        }
        #endregion // Arithmetic/Logical Operation

        #region Logic
        /// <summary>
        /// r &lt;- (r) AND オペランド, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator And = new Operator(AndAction);

        private static void AndAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoLogic(Alu.And, r, operand, registerSet.FR, memory);
        }

        /// <summary>
        /// r &lt;- (r) OR オペランド, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator Or = new Operator(OrAction);

        private static void OrAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoLogic(Alu.Or, r, operand, registerSet.FR, memory);
        }

        /// <summary>
        /// r &lt;- (r) XOR オペランド, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator Xor = new Operator(XorAction);

        private static void XorAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoLogic(Alu.Xor, r, operand, registerSet.FR, memory);
        }

        private static void DoLogic(
            Alu.OperationMethod operationMethod, CpuRegister r, Word operand, FlagRegister fr, Memory memory)
        {
            Boolean notUsed;
            r.Value = operationMethod(r.Value, operand, out notUsed);

            const Boolean OverflowFlag = false;
            fr.SetFlags(r, OverflowFlag);
        }
        #endregion // Logic

        #region Comparison
        /// <summary>
        /// r と オペランドを算術比較し FR を設定する, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator CompareArithmetic = new Operator(CompareArithmeticAction);

        private static void CompareArithmeticAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoCompare(Alu.CompareArithmetic, r, operand, registerSet.FR);
        }

        /// <summary>
        /// r と オペランドを論理比較し FR を設定する, 〇*1: 設定される。ただし、OF には 0 が設定される。
        /// </summary>
        internal static readonly Operator CompareLogical = new Operator(CompareLogicalAction);

        private static void CompareLogicalAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoCompare(Alu.CompareLogical, r, operand, registerSet.FR);
        }

        private static void DoCompare(
            Alu.CompareMethod compareMethod, CpuRegister r, Word operand, FlagRegister fr)
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
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
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
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
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
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
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
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            DoShift(Alu.ShiftRightLogical, r, operand, registerSet.FR);
        }

        private static void DoShift(
            Alu.ShiftMethod shiftMethod, CpuRegister r, Word operand, FlagRegister fr)
        {
            UInt16 lastShiftedOutBit;
            r.Value = shiftMethod(r.Value, operand, out lastShiftedOutBit);
            Boolean overflowFlag = (lastShiftedOutBit != 0);
            fr.SetFlags(r, overflowFlag);
        }
        #endregion // Shift

        #region Jump
        /// <summary>
        /// SF=1 のとき、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator JumpOnMinus = new Operator(JumpOnMinusAction);

        private static void JumpOnMinusAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            Boolean condition = registerSet.FR.SF;
            DoJump(condition, operand, registerSet.PR);
        }

        /// <summary>
        /// ZF=0 のとき、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator JumpOnNonZero = new Operator(JumpOnNonZeroAction);

        private static void JumpOnNonZeroAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            Boolean condition = !registerSet.FR.ZF;
            DoJump(condition, operand, registerSet.PR);
        }

        /// <summary>
        /// ZF=1 のとき、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator JumpOnZero = new Operator(JumpOnZeroAction);

        private static void JumpOnZeroAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            Boolean condition = registerSet.FR.ZF;
            DoJump(condition, operand, registerSet.PR);
        }

        /// <summary>
        /// 無条件で、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator UnconditionalJump = new Operator(UnconditionalJumpAction);

        private static void UnconditionalJumpAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            const Boolean Condition = true;
            DoJump(Condition, operand, registerSet.PR);
        }

        /// <summary>
        /// SF=0 かつ ZF=0 のとき、オペランドで指定のアドレスに分岐する。
        /// </summary>
        internal static readonly Operator JumpOnPlus = new Operator(JumpOnPlusAction);

        private static void JumpOnPlusAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
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
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            Boolean condition = registerSet.FR.OF;
            DoJump(condition, operand, registerSet.PR);
        }

        private static void DoJump(Boolean condition, Word operand, CpuRegister pr)
        {
            if (condition)
            {
                pr.Value = operand;
            }
        }
        #endregion // Jump

        #region Stack Operation
        /// <summary>
        /// SP &lt;- (SP) -L 1; (SP) &lt;- 実効アドレス, -- 実効前の値が保持される。
        /// </summary>
        internal static readonly Operator Push = new Operator(PushAction);

        private static void PushAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            PushValue(registerSet.SP, memory, operand);
        }

        private static void PushValue(CpuRegister sp, Memory memory, Word value)
        {
            sp.Decrement();
            memory.Write(sp.Value, value);
        }

        /// <summary>
        /// r &lt;- ( (SP) ); SP &lt;- (SP) +L 1, -- 実効前の値が保持される。
        /// </summary>
        internal static readonly Operator Pop = new Operator(PopAction);

        private static void PopAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            r.Value = PopValue(registerSet.SP, memory);
        }

        private static Word PopValue(CpuRegister sp, Memory memory)
        {
            Word value = memory.Read(sp.Value);
            sp.Increment();
            return value;
        }
        #endregion // Stack Operation

        #region Call/Return
        /// <summary>
        /// SP &lt;- (SP) -L 1; (SP) &lt;- (PR); PR &lt;- 実効アドレス, -- 実効前の値が保持される。
        /// </summary>
        internal static readonly Operator CallSubroutine = new Operator(CallSubroutineAction);

        private static void CallSubroutineAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            CpuRegister pr = registerSet.PR;
            PushValue(registerSet.SP, memory, pr.Value);
            pr.Value = operand;
        }

        /// <summary>
        /// PR &lt;- ( (SP) ); SP &lt;- (SP) +L 1, -- 実効前の値が保持される。
        /// </summary>
        internal static readonly Operator ReturnFromSubroutine = new Operator(ReturnFromSubroutineAction);

        private static void ReturnFromSubroutineAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            Boolean cancel = OnReturningFromSubroutine(registerSet.SP);
            if (!cancel)
            {
                CpuRegister pr = registerSet.PR;
                pr.Value = PopValue(registerSet.SP, memory);
            }
        }

        private static Boolean OnReturningFromSubroutine(CpuRegister sp)
        {
            Boolean cancel = false;

            if (ReturningFromSubroutine != null)
            {
                ReturningFromSubroutineEventArgs e = new ReturningFromSubroutineEventArgs(sp);
                ReturningFromSubroutine(null, e);
                cancel = e.Cancel;
            }

            return cancel;
        }
        #endregion // Call/Return

        #region Others
        /// <summary>
        /// 実効アドレスを引数として割出しを行う。実行後の GR と FR は不定となる。
        /// </summary>
        internal static readonly Operator SuperVisorCall = new Operator(SuperVisorCallAction);

        private static void SuperVisorCallAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            OnCallingSuperVisor(operand);
        }

        private static void OnCallingSuperVisor(Word operand)
        {
            if (CallingSuperVisor != null)
            {
                CallingSuperVisorEventArgs e = new CallingSuperVisorEventArgs(operand);
                CallingSuperVisor(null, e);
            }
        }

        /// <summary>
        /// 何もしない。
        /// </summary>
        internal static readonly Operator NoOperation = new Operator(NoOperationAction);

        private static void NoOperationAction(
            CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            //
        }
        #endregion // Others

        #region Instance Fields
        private readonly OperateAction m_operateAction;
        #endregion

        // このクラスのインスタンスは、このクラス内からのみ作成できます。
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
        internal void Operate(CpuRegister r, Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            m_operateAction(r, operand, registerSet, memory);
        }
    }
}
