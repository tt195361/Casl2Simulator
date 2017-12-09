using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドの r1,r2 を表わします。
    /// </summary>
    internal class R1R2Operand : MachineInstructionOperand
    {
        internal static Boolean TryParse(OperandLexer lexer, UInt16 opcode, out R1R2Operand r1R2)
        {
            try
            {
                r1R2 = Parse(lexer, opcode);
                return true;
            }
            catch (Exception)
            {
                r1R2 = null;
                return false;
            }
        }

        internal static R1R2Operand Parse(OperandLexer lexer, UInt16 opcode)
        {
            RegisterOperand r1 = RegisterOperand.Parse(lexer);
            lexer.SkipComma();
            RegisterOperand r2 = RegisterOperand.Parse(lexer);
            return new R1R2Operand(opcode, r1, r2);
        }

        #region Fields
        private readonly RegisterOperand m_r1;
        private readonly RegisterOperand m_r2;
        #endregion

        private R1R2Operand(UInt16 opcode, RegisterOperand r1, RegisterOperand r2)
            : base(opcode)
        {
            m_r1 = r1;
            m_r2 = r2;
        }

        internal RegisterOperand R1
        {
            get { return m_r1; }
        }

        internal RegisterOperand R2
        {
            get { return m_r2; }
        }

        internal override UInt16 GetRR1()
        {
            return m_r1.Number;
        }

        internal override UInt16 GetXR2()
        {
            return m_r2.Number;
        }

        public override String ToString()
        {
            return Operand.Join(m_r1, m_r2);
        }

        internal static R1R2Operand MakeForUnitTest(RegisterOperand r1, RegisterOperand r2)
        {
            return MakeForUnitTest(OpcodeDef.Dummy, r1, r2);
        }

        internal static R1R2Operand MakeForUnitTest(UInt16 opcode, RegisterOperand r1, RegisterOperand r2)
        {
            return new R1R2Operand(opcode, r1, r2);
        }
    }
}
