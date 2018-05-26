using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// メモリのアドレスを表わします。
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

        /// <summary>
        /// アドレスの値を取得します。
        /// </summary>
        internal UInt16 Value
        {
            get { return m_value; }
        }

        /// <summary>
        /// アドレスに指定のオフセットを加算し、その結果のアドレスを返します。
        /// </summary>
        /// <param name="offset">このアドレスに加算するオフセットです。</param>
        /// <returns>加算した結果のアドレスを返します。</returns>
        internal MemoryAddress Add(MemoryOffset offset)
        {
            UInt16 ui16Result = MemoryUtils.Add(m_value, offset.Value);
            return new MemoryAddress(ui16Result);
        }

        /// <summary>
        /// アドレスに指定のサイズを加算し、その結果のアドレスを返します。
        /// </summary>
        /// <param name="size">このアドレスに加算するサイズです。</param>
        /// <returns>加算した結果のアドレスを返します。</returns>
        internal MemoryAddress Add(MemorySize size)
        {
            UInt16 ui16Result = MemoryUtils.Add(m_value, size.Value);
            return new MemoryAddress(ui16Result);
        }

        /// <summary>
        /// アドレスの値を <see cref="Word"/> として取得します。
        /// </summary>
        /// <returns>アドレスの値が入った <see cref="Word"/> のオブジェクトを返します。</returns>
        internal Word GetValueAsWord()
        {
            return new Word(m_value);
        }

        public override String ToString()
        {
            return String.Format("${0:X04}", m_value);
        }
    }
}
