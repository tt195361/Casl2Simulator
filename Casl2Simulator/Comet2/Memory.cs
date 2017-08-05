using System;
using System.Linq;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の主記憶を表わします。
    /// </summary>
    internal class Memory
    {
        #region Fields
        // 主記憶のアドレスは 0 ~ 65,535 番地で、容量は 65,536 語です。
        private const Int32 MinAddress = 0;
        private const Int32 Size = 65536;


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
            Enumerable
                .Range(MinAddress, Size)
                .ForEach((i32Addr) => Write((UInt16)i32Addr, Word.Zero));
        }

        /// <summary>
        /// 指定の語の内容をアドレスとして、その位置の語を読み出します。
        /// </summary>
        /// <param name="word">その内容をアドレスとして使用する語です。</param>
        /// <returns>指定アドレスから読み出した語を返します。</returns>
        internal Word Read(Word word)
        {
            UInt16 ui16Addr = word.GetAsUnsigned();
            return m_contents[ui16Addr];
        }

        /// <summary>
        /// 指定の語の内容をアドレスとして、その位置に指定の語を書き込みます。
        /// </summary>
        /// <param name="address">その内容をアドレスとして使用する語です。</param>
        /// <param name="word">指定アドレスに書き込む語です。</param>
        internal void Write(Word address, Word word)
        {
            UInt16 ui16Addr = address.GetAsUnsigned();
            Write(ui16Addr, word);
        }

        private void Write(UInt16 ui16Addr, Word word)
        {
            m_contents[ui16Addr] = word;
        }
    }
}
