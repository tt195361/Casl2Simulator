using System;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// メモリのサイズを表わします。
    /// </summary>
    internal struct MemorySize
    {
        #region Instance Fields
        private readonly UInt16 m_value;
        #endregion

        internal MemorySize(UInt16 value)
        {
            m_value = value;
        }

        /// <summary>
        /// メモリサイズの値を取得します。
        /// </summary>
        internal UInt16 Value
        {
            get { return m_value; }
        }

        public override String ToString()
        {
            return m_value.ToString();
        }
    }
}
