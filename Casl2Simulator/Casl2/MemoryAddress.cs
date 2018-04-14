using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// メモリのアドレスの値です。
    /// </summary>
    internal struct MemoryAddress
    {
        #region Static Fields
        internal static readonly MemoryAddress Zero = new MemoryAddress(0);
        #endregion

        #region Instance Fields
        private readonly UInt16 m_value;
        #endregion

        internal MemoryAddress(UInt16 value)
        {
            m_value = value;
        }

        internal UInt16 Value
        {
            get { return m_value; }
        }

        /// <summary>
        /// アドレスに指定のオフセットの値を加算し、その結果のアドレスの値を返します。
        /// </summary>
        /// <param name="offset">このアドレスに加算するオフセットの値です。</param>
        /// <returns>加算した結果のアドレスの値を返します。</returns>
        internal MemoryAddress Add(MemoryOffset offset)
        {
            UInt16 ui16Result = MemoryUtils.Add(m_value, offset.Value);
            return new MemoryAddress(ui16Result);
        }

        public override String ToString()
        {
            return String.Format("${0:X04}", m_value);
        }
    }
}
