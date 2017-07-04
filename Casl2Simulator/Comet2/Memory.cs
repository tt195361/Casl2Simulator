using System;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の主記憶を表わします。主記憶の容量は 65,536 語で、アドレスは 0 ~ 65,535 番地です。
    /// </summary>
    internal class Memory
    {
        #region Fields
        private const Int32 Size = 65536;

        private const Int32 MinAddress = 0;
        private const Int32 MaxAddress = 65535;

        private readonly Word[] m_contents;
        #endregion

        /// <summary>
        /// COMET II のメモリの新しいインスタンスを初期化します。
        /// </summary>
        internal Memory()
        {
            m_contents = new Word[Size];
        }

        /// <summary>
        /// 指定アドレスの語を読み出します。
        /// </summary>
        /// <param name="address">読み出すアドレスの値です。</param>
        /// <returns>指定アドレスから読み出した語を返します。</returns>
        internal Word Read(Int32 address)
        {
            CheckAddress(address);
            return m_contents[address];
        }

        /// <summary>
        /// 指定アドレスに指定の語を書き込みます。
        /// </summary>
        /// <param name="address">書き込むアドレスの値です。</param>
        /// <param name="word">指定アドレスに書き込む語です。</param>
        internal void Write(Int32 address, Word word)
        {
            CheckAddress(address);
            m_contents[address] = word;
        }

        private void CheckAddress(Int32 address)
        {
            ArgChecker.CheckRange(address, MinAddress, MaxAddress, nameof(address));
        }
    }
}
