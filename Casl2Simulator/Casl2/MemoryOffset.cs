using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// メモリのオフセットを表わします。
    /// </summary>
    internal struct MemoryOffset
    {
        #region Fields
        internal static readonly MemoryOffset Zero = new MemoryOffset(0);

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
            Int32 i32Val = m_value + addend;
            if (i32Val < Comet2Defs.MinAddress || Comet2Defs.MaxAddress < i32Val)
            {
                String message = String.Format(
                    Resources.MSG_MemoryOffsetOutOfRange, i32Val, this, addend,
                    Comet2Defs.MinAddress, Comet2Defs.MaxAddress);
                throw new Casl2SimulatorException(message);
            }

            UInt16 ui16Val = NumberUtils.ToUInt16(i32Val);
            return new MemoryOffset(ui16Val);
        }

        public override String ToString()
        {
            return String.Format("0x{0:X04}", m_value);
        }
    }
}
