using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の CPU で実行する命令を表わします。
    /// </summary>
    internal class CpuInstruction
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

        static CpuInstruction()
        {
            Operator.ReturningFromSubroutine += OnReturningFromSubroutine;
            Operator.CallingSuperVisor += OnCallingSuperVisor;
        }

        private static void OnReturningFromSubroutine(Object sender, ReturningFromSubroutineEventArgs e)
        {
            if (ReturningFromSubroutine != null)
            {
                ReturningFromSubroutine(null, e);
            }
        }

        private static void OnCallingSuperVisor(Object sender, CallingSuperVisorEventArgs e)
        {
            if (CallingSuperVisor != null)
            {
                CallingSuperVisor(null, e);
            }
        }
        #endregion // Events

        #region Load/Store
        /// ロード (実効アドレス) 命令
        internal static readonly CpuInstruction LoadEaContents = new CpuInstruction(
            "LD r,adr,x", Operator.LoadWithFr, RegisterHandler.Register, OperandHandler.EaContents);
        /// ストア 命令
        internal static readonly CpuInstruction Store = new CpuInstruction(
            "ST r,adr,x", Operator.Store, RegisterHandler.Register, OperandHandler.EffectiveAddress);
        /// ロード 実効アドレス 命令
        internal static readonly CpuInstruction LoadEffectiveAddress = new CpuInstruction(
            "LAD r,adr,x", Operator.LoadWithoutFr, RegisterHandler.Register, OperandHandler.EffectiveAddress);
        /// ロード レジスタ 命令
        internal static readonly CpuInstruction LoadRegister = new CpuInstruction(
            "LD r1,r2", Operator.LoadWithFr, RegisterHandler.Register, OperandHandler.Register);
        #endregion // Load/Store

        #region Arithmetic/Logical Operation
        /// 算術加算 (実効アドレス) 命令
        internal static readonly CpuInstruction AddArithmeticEaContents = new CpuInstruction(
            "ADDA r,adr,x", Operator.AddArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 算術減算 (実効アドレス) 命令
        internal static readonly CpuInstruction SubtractArithmeticEaContents = new CpuInstruction(
            "SUBA r,adr,x", Operator.SubtractArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理加算 (実効アドレス) 命令
        internal static readonly CpuInstruction AddLogicalEaContents = new CpuInstruction(
            "ADDL r,adr,x", Operator.AddLogical, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理減算 (実効アドレス) 命令
        internal static readonly CpuInstruction SubtractLogicalEaContents = new CpuInstruction(
            "SUBL r,adr,x", Operator.SubtractLogical, RegisterHandler.Register, OperandHandler.EaContents);
        /// 算術加算 レジスタ 命令
        internal static readonly CpuInstruction AddArithmeticRegister = new CpuInstruction(
            "ADDA r1,r2", Operator.AddArithmetic, RegisterHandler.Register, OperandHandler.Register);
        /// 算術減算 レジスタ 命令
        internal static readonly CpuInstruction SubtractArithmeticRegister = new CpuInstruction(
            "SUBA r1,r2", Operator.SubtractArithmetic, RegisterHandler.Register, OperandHandler.Register);
        /// 論理加算 レジスタ 命令
        internal static readonly CpuInstruction AddLogicalRegister = new CpuInstruction(
            "ADDL r1,r2", Operator.AddLogical, RegisterHandler.Register, OperandHandler.Register);
        /// 論理減算 レジスタ 命令
        internal static readonly CpuInstruction SubtractLogicalRegister = new CpuInstruction(
            "SUBL r1,r2", Operator.SubtractLogical, RegisterHandler.Register, OperandHandler.Register);
        #endregion // Arithmetic/Logical Operation

        #region Logic
        /// 論理積 (実効アドレス) 命令
        internal static readonly CpuInstruction AndEaContents = new CpuInstruction(
            "AND r,adr,x", Operator.And, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理和 (実効アドレス) 命令
        internal static readonly CpuInstruction OrEaContents = new CpuInstruction(
            "OR r,adr,x", Operator.Or, RegisterHandler.Register, OperandHandler.EaContents);
        /// 排他的論理和 (実効アドレス) 命令
        internal static readonly CpuInstruction XorEaContents = new CpuInstruction(
            "XOR r,adr,x", Operator.Xor, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理積 レジスタ 命令
        internal static readonly CpuInstruction AndRegister = new CpuInstruction(
            "AND r1,r2", Operator.And, RegisterHandler.Register, OperandHandler.Register);
        /// 論理和 レジスタ 命令
        internal static readonly CpuInstruction OrRegister = new CpuInstruction(
            "OR r1,r2", Operator.Or, RegisterHandler.Register, OperandHandler.Register);
        /// 排他的論理和 レジスタ 命令
        internal static readonly CpuInstruction XorRegister = new CpuInstruction(
            "XOR r1,r2", Operator.Xor, RegisterHandler.Register, OperandHandler.Register);
        #endregion // Logic

        #region Comparison
        /// 算術比較 (実効アドレス) 命令
        internal static readonly CpuInstruction CompareArithmeticEaContents = new CpuInstruction(
            "CPA r,adr,x", Operator.CompareArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理比較 (実効アドレス) 命令
        internal static readonly CpuInstruction CompareLogicalEaContents = new CpuInstruction(
            "CPL r,adr,x", Operator.CompareLogical, RegisterHandler.Register, OperandHandler.EaContents);
        /// 算術比較 レジスタ 命令
        internal static readonly CpuInstruction CompareArithmeticRegister = new CpuInstruction(
            "CPA r1,r2", Operator.CompareArithmetic, RegisterHandler.Register, OperandHandler.Register);
        /// 論理比較 レジスタ 命令
        internal static readonly CpuInstruction CompareLogicalRegister = new CpuInstruction(
            "CPL r1,r2", Operator.CompareLogical, RegisterHandler.Register, OperandHandler.Register);
        #endregion // Comparison

        #region Shift
        /// 算術左シフト (実効アドレス) 命令
        internal static readonly CpuInstruction ShiftLeftArithmeticEaContents = new CpuInstruction(
            "SLA r,adr,x", Operator.ShiftLeftArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 算術右シフト (実効アドレス) 命令
        internal static readonly CpuInstruction ShiftRightArithmeticEaContents = new CpuInstruction(
            "SRA r,adr,x", Operator.ShiftRightArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理左シフト (実効アドレス) 命令
        internal static readonly CpuInstruction ShiftLeftLogicalEaContents = new CpuInstruction(
            "SLL r,adr,x", Operator.ShiftLeftLogical, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理右シフト (実効アドレス) 命令
        internal static readonly CpuInstruction ShiftRightLogicalEaContents = new CpuInstruction(
            "SRL r,adr,x", Operator.ShiftRightLogical, RegisterHandler.Register, OperandHandler.EaContents);
        #endregion // Shift

        #region Jump
        /// 負分岐 命令
        internal static readonly CpuInstruction JumpOnMinus = new CpuInstruction(
            "JMI adr,x", Operator.JumpOnMinus, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// 非零分岐 命令
        internal static readonly CpuInstruction JumpOnNonZero = new CpuInstruction(
            "JNZ adr,x", Operator.JumpOnNonZero, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// 零分岐 命令
        internal static readonly CpuInstruction JumpOnZero = new CpuInstruction(
            "JZE adr,x", Operator.JumpOnZero, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// 無条件分岐 命令
        internal static readonly CpuInstruction UnconditionalJump = new CpuInstruction(
            "JUMP adr,x", Operator.UnconditionalJump, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// 正分岐 命令
        internal static readonly CpuInstruction JumpOnPlus = new CpuInstruction(
            "JPL adr,x", Operator.JumpOnPlus, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// オーバーフロー分岐 命令
        internal static readonly CpuInstruction JumpOnOverflow = new CpuInstruction(
            "JOV adr,x", Operator.JumpOnOverflow, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        #endregion // Jump

        #region Stack Operation
        // プッシュ 命令
        internal static readonly CpuInstruction Push = new CpuInstruction(
            "PUSH adr,x", Operator.Push, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        // ポップ 命令
        internal static readonly CpuInstruction Pop = new CpuInstruction(
            "POP r", Operator.Pop, RegisterHandler.Register, OperandHandler.NoOperand);
        #endregion // Stack Operation

        #region Call/Ret
        // コール 命令
        internal static readonly CpuInstruction CallSubroutine = new CpuInstruction(
            "CALL adr,x", Operator.CallSubroutine, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        // リターン 命令
        internal static readonly CpuInstruction ReturnFromSubroutine = new CpuInstruction(
            "RET", Operator.ReturnFromSubroutine, RegisterHandler.NoRegister, OperandHandler.NoOperand);
        #endregion // Call/Ret

        #region Others
        // スーパバイザコール 命令
        internal static readonly CpuInstruction SuperVisorCall = new CpuInstruction(
            "SVC adr,x", Operator.SuperVisorCall, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        // ノーオペレーション 命令
        internal static readonly CpuInstruction NoOperation = new CpuInstruction(
            "NOP", Operator.NoOperation, RegisterHandler.NoRegister, OperandHandler.NoOperand);
        #endregion // Others

        #region Instance Fields
        private readonly String m_str;
        private readonly Operator m_operator;
        private readonly RegisterHandler m_registerHandler;
        private readonly OperandHandler m_operandHandler;
        #endregion

        // このクラスのインスタンスは、このクラス内からのみ作成できます。
        private CpuInstruction(String str, Operator op, RegisterHandler regHandler, OperandHandler oprHandler)
        {
            m_str = str;
            m_operator = op;
            m_registerHandler = regHandler;
            m_operandHandler = oprHandler;
        }

        /// <summary>
        /// 命令を実行します。
        /// </summary>
        /// <param name="rR1Field">命令語の中の r/r1 フィールドの値です。</param>
        /// <param name="xR2Field">命令語の中の x/r2 フィールドの値です。</param>
        /// <param name="registerSet">COMET II の一そろいのレジスタです。</param>
        /// <param name="memory">COMET II の主記憶です。</param>
        internal void Execute(UInt16 rR1Field, UInt16 xR2Field, CpuRegisterSet registerSet, Memory memory)
        {
            CpuRegister r = m_registerHandler.GetRegister(rR1Field, registerSet);
            Word operand = m_operandHandler.GetOperand(xR2Field, registerSet, memory);
            m_operator.Operate(r, operand, registerSet, memory);
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
