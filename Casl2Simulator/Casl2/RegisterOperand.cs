using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令のレジスタオペランドです。
    /// </summary>
    internal class RegisterOperand : MachineInstructionOperand
    {
        internal static RegisterOperand Parse(OperandLexer lexer)
        {
            return Parse(lexer, OpcodeDef.Dummy);
        }

        internal static RegisterOperand Parse(OperandLexer lexer, UInt16 opcode)
        {
            Token token = lexer.ReadCurrentAs(TokenType.RegisterName);
            Register r = Register.GetFor(token.StrValue);
            return new RegisterOperand(opcode, r);
        }

        #region Fields
        private readonly Register m_r;
        #endregion

        private RegisterOperand(UInt16 opcode, Register r)
            : base(opcode)
        {
            m_r = r;
        }

        internal Register R
        {
            get { return m_r; }
        }

        internal String Name
        {
            get { return m_r.Name; }
        }

        internal UInt16 Number
        {
            get { return m_r.Number; }
        }

        internal override UInt16 GetRR1()
        {
            return Number;
        }

        public override String ToString()
        {
            return Name;
        }

        internal static RegisterOperand MakeForUnitTest(Register r)
        {
            return MakeForUnitTest(OpcodeDef.Dummy, r);
        }

        internal static RegisterOperand MakeForUnitTest(UInt16 opcode, Register r)
        {
            return new RegisterOperand(opcode, r);
        }
    }
}
