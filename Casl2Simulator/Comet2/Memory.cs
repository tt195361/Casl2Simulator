using System;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の主記憶を表わします。
    /// </summary>
    internal class Memory
    {
        #region Fields
        // 主記憶の容量は 65,536 語です。
        private const Int32 Size = 65536;

        // アドレスは 0 ~ 65,535 番地です。
        private const Int32 MinAddress = 0;
        private const Int32 MaxAddress = 65535;

        private readonly Word[] m_contents;
        #endregion

        /// <summary>
        /// メモリの新しいインスタンスを初期化します。
        /// </summary>
        internal Memory()
        {
            m_contents = new Word[Size];

            Reset();
        }

        /// <summary>
        /// メモリの内容を初期化します。
        /// </summary>
        internal void Reset()
        {
            m_contents.ForEach((index, content) => m_contents[index] = new Word(0));
        }

        /// <summary>
        /// 指定の語の内容をアドレスとして、その位置の語を読み出します。
        /// </summary>
        /// <param name="word">その内容をアドレスとして使用する語です。</param>
        /// <returns>指定アドレスから読み出した語を返します。</returns>
        internal Word Read(Word word)
        {
            Int32 address = word.GetAsUnsigned();
            return Read(address);
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
        /// 指定の開始アドレスから、指定の値を順に書き込みます。
        /// </summary>
        /// <param name="startAddress">最初の値を書き込むアドレスの値です。</param>
        /// <param name="values">指定の開始アドレスから順に書き込む値です。</param>
        internal void Write(Int32 startAddress, params UInt16[] values)
        {
            values.ForEach((index, value) => Write(startAddress + index, new Word(value)));
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
