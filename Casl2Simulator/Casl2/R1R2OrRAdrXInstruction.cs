using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// オペランドが r1,r2 あるいは r,adr,x の機械語命令を表わします。
    /// </summary>
    internal class R1R2OrRAdrXInstruction : MachineInstruction
    {
        #region Fields
        private readonly UInt16 m_opcodeRAdrX;
        private readonly UInt16 m_opcodeR1R2;
        #endregion

        internal R1R2OrRAdrXInstruction(String code, UInt16 opcodeRAdrX, UInt16 opcodeR1R2)
            : base(code)
        {
            m_opcodeRAdrX = opcodeRAdrX;
            m_opcodeR1R2 = opcodeR1R2;
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_R1R2OrRAdrX; }
        }

        protected override MachineInstructionOperand DoParseSpecificOperand(OperandLexer lexer)
        {
            return MachineInstructionOperand.ParseR1R2OrRAdrX(lexer);
        }

        protected override UInt16 GetOpcode()
        {
            if (MachineInstructionOperand.GetType() == typeof(RAdrXOperand))
            {
                return m_opcodeRAdrX;
            }
            else
            {
                return m_opcodeR1R2;
            }
        }
    }
}
