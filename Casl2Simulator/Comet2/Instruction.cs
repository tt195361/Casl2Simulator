using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の命令を表わします。
    /// </summary>
    internal class Instruction
    {
        /// ロード "LD r,adr,x" 命令
        internal static readonly Instruction LoadEaContents = new Instruction(
            "LD r,adr,x", Operator.Load, RegisterHandler.Register, OperandHandler.EaContents);
        /// ストア "ST r,adr,x" 命令
        internal static readonly Instruction Store = new Instruction(
            "ST r,adr,x", Operator.Store, RegisterHandler.Register, OperandHandler.EffectiveAddress);

        /// 算術加算 "ADDA r,adr,x" 命令
        internal static readonly Instruction AddArithmeticEaContents = new Instruction(
            "ADDA r,adr,x", Operator.AddArithmetic, RegisterHandler.Register, OperandHandler.EaContents);

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
