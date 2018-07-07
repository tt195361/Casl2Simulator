using System;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// メモリのオフセットを表わします。
    /// </summary>
    internal struct MemoryOffset
    {
        #region Static Fields
        internal static readonly MemoryOffset Zero = new MemoryOffset(0);
	    #endregion

	    #region Instance Fields
        private readonly UInt16 m_value;
        #endregion

        internal MemoryOffset(UInt16 value)
        {
            m_value = value;
        }

        internal UInt16 Value
        {
            get { return m_value; }
        }

        internal MemoryOffset Add(Int32 addend)
        {
            UInt16 ui16Result = MemoryUtils.Add(m_value, addend);
            return new MemoryOffset(ui16Result);
        }

        public override String ToString()
        {
            return String.Format("#{0:X04}", m_value);
        }
    }
}
