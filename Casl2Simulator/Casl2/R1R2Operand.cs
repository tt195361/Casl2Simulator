using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドの r1,r2 を表わします。
    /// </summary>
    internal class R1R2Operand : MachineInstructionOperand
    {
        #region Fields
        private readonly RegisterOperand m_r1;
        private readonly RegisterOperand m_r2;
        #endregion

        internal R1R2Operand(RegisterOperand r1, RegisterOperand r2)
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
            return m_r1.ToString() + Casl2Defs.Comma + m_r2.ToString();
        }
    }
}
