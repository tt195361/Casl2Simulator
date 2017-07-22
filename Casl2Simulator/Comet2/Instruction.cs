using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の命令を表わします。
    /// </summary>
    internal class Instruction
    {
        /// ロード (実効アドレス) 命令
        internal static readonly Instruction LoadEaContents = new Instruction(
            "LD r,adr,x", Operator.LoadWithFr, RegisterHandler.Register, OperandHandler.EaContents);
        /// ストア 命令
        internal static readonly Instruction Store = new Instruction(
            "ST r,adr,x", Operator.Store, RegisterHandler.Register, OperandHandler.EffectiveAddress);
        /// ロード 実効アドレス 命令
        internal static readonly Instruction LoadEffectiveAddress = new Instruction(
            "LAD r,adr,x", Operator.LoadWithoutFr, RegisterHandler.Register, OperandHandler.EffectiveAddress);
        /// ロード レジスタ 命令
        internal static readonly Instruction LoadRegister = new Instruction(
            "LD r1,r2", Operator.LoadWithFr, RegisterHandler.Register, OperandHandler.Register);

        /// 算術加算 (実効アドレス) 命令
        internal static readonly Instruction AddArithmeticEaContents = new Instruction(
            "ADDA r,adr,x", Operator.AddArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 算術減算 (実効アドレス) 命令
        internal static readonly Instruction SubtractArithmeticEaContents = new Instruction(
            "SUBA r,adr,x", Operator.SubtractArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理加算 (実効アドレス) 命令
        internal static readonly Instruction AddLogicalEaContents = new Instruction(
            "ADDL r,adr,x", Operator.AddLogical, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理減算 (実効アドレス) 命令
        internal static readonly Instruction SubtractLogicalEaContents = new Instruction(
            "SUBL r,adr,x", Operator.SubtractLogical, RegisterHandler.Register, OperandHandler.EaContents);
        /// 算術加算 レジスタ 命令
        internal static readonly Instruction AddArithmeticRegister = new Instruction(
            "ADDA r1,r2", Operator.AddArithmetic, RegisterHandler.Register, OperandHandler.Register);
        /// 算術減算 レジスタ 命令
        internal static readonly Instruction SubtractArithmeticRegister = new Instruction(
            "SUBA r1,r2", Operator.SubtractArithmetic, RegisterHandler.Register, OperandHandler.Register);
        /// 論理加算 レジスタ 命令
        internal static readonly Instruction AddLogicalRegister = new Instruction(
            "ADDL r1,r2", Operator.AddLogical, RegisterHandler.Register, OperandHandler.Register);
        /// 論理減算 レジスタ 命令
        internal static readonly Instruction SubtractLogicalRegister = new Instruction(
            "SUBL r1,r2", Operator.SubtractLogical, RegisterHandler.Register, OperandHandler.Register);

        /// 算術比較 (実効アドレス) 命令
        internal static readonly Instruction CompareArithmeticEaContents = new Instruction(
            "CPA r,adr,x", Operator.CompareArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理比較 (実効アドレス) 命令
        internal static readonly Instruction CompareLogicalEaContents = new Instruction(
            "CPL r,adr,x", Operator.CompareLogical, RegisterHandler.Register, OperandHandler.EaContents);

        /// 算術左シフト (実効アドレス) 命令
        internal static readonly Instruction ShiftLeftArithmeticEaContents = new Instruction(
            "SLA r,adr,x", Operator.ShiftLeftArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 算術右シフト (実効アドレス) 命令
        internal static readonly Instruction ShiftRightArithmeticEaContents = new Instruction(
            "SRA r,adr,x", Operator.ShiftRightArithmetic, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理左シフト (実効アドレス) 命令
        internal static readonly Instruction ShiftLeftLogicalEaContents = new Instruction(
            "SLL r,adr,x", Operator.ShiftLeftLogical, RegisterHandler.Register, OperandHandler.EaContents);
        /// 論理右シフト (実効アドレス) 命令
        internal static readonly Instruction ShiftRightLogicalEaContents = new Instruction(
            "SRL r,adr,x", Operator.ShiftRightLogical, RegisterHandler.Register, OperandHandler.EaContents);

        /// 負分岐 命令
        internal static readonly Instruction JumpOnMinus = new Instruction(
            "JMI adr,x", Operator.JumpOnMinus, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// 非零分岐 命令
        internal static readonly Instruction JumpOnNonZero = new Instruction(
            "JNZ adr,x", Operator.JumpOnNonZero, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// 零分岐 命令
        internal static readonly Instruction JumpOnZero = new Instruction(
            "JZE adr,x", Operator.JumpOnZero, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// 無条件分岐 命令
        internal static readonly Instruction UnconditionalJump = new Instruction(
            "JUMP adr,x", Operator.UnconditionalJump, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// 正分岐 命令
        internal static readonly Instruction JumpOnPlus = new Instruction(
            "JPL adr,x", Operator.JumpOnPlus, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);
        /// オーバーフロー分岐 命令
        internal static readonly Instruction JumpOnOverflow = new Instruction(
            "JOV adr,x", Operator.JumpOnOverflow, RegisterHandler.NoRegister, OperandHandler.EffectiveAddress);

        #region Fields
        private readonly String m_str;
        private readonly Operator m_operator;
        private readonly RegisterHandler m_registerHandler;
        private readonly OperandHandler m_operandHandler;
        #endregion

        // このクラスのインスタンスは、このクラス内からのみ作成できます。
        private Instruction(String str, Operator op, RegisterHandler regHandler, OperandHandler oprHandler)
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
        internal void Execute(UInt16 rR1Field, UInt16 xR2Field, RegisterSet registerSet, Memory memory)
        {
            Register r = m_registerHandler.GetRegister(rR1Field, registerSet);
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
