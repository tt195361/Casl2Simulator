using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドの r,adr[,x] を表わします。
    /// </summary>
    internal class RAdrXOperand : MachineInstructionOperand
    {
        #region Fields
        private readonly RegisterOperand m_r;
        private readonly AdrXOperand m_adrX;
        #endregion

        internal RAdrXOperand(RegisterOperand r, AdrXOperand adrX)
        {
            m_r = r;
            m_adrX = adrX;
        }

        internal RegisterOperand R
        {
            get { return m_r; }
        }

        internal AdrXOperand AdrX
        {
            get { return m_adrX; }
        }

        public override String ToString()
        {
            return m_r.ToString() + Casl2Defs.Comma + m_adrX.ToString();
        }
    }
}
