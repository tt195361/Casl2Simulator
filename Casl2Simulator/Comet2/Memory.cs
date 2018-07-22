using System;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の主記憶を表わします。
    /// </summary>
    internal class Memory
    {
        #region Instance Fields
        private readonly Word[] m_contents;
        #endregion

        /// <summary>
        /// メモリの新しいインスタンスを初期化します。
        /// </summary>
        internal Memory()
        {
            m_contents = new Word[Comet2Defs.MemorySize];

            Clear();
        }

        /// <summary>
        /// メモリの内容をクリアします。
        /// </summary>
        internal void Clear()
        {
            Enumerable
                .Range(Comet2Defs.MinAddress, Comet2Defs.MemorySize)
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
            return Read(ui16Addr);
        }

        /// <summary>
        /// 指定のアドレスの語を読み出します。
        /// </summary>
        /// <param name="ui16Addr">読み出すアドレスです。</param>
        /// <returns>指定アドレスから読み出した語を返します。</returns>
        internal Word Read(UInt16 ui16Addr)
        {
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

        /// <summary>
        /// 指定のアドレスから、指定の値を順に書き込みます。
        /// </summary>
        /// <param name="address">指定の値を順に書き込むアドレスの値です。</param>
        /// <param name="words">指定のアドレスから書き込む値です。</param>
        internal void WriteRange(MemoryAddress address, IEnumerable<Word> words)
        {
            UInt16 ui16StartAddr = address.Value;
            IEnumerable<UInt16> ui16Values = words.Select((word) => word.GetAsUnsigned());
            ForEach(ui16StartAddr, ui16Values, Write);
        }

        /// <summary>
        /// 指定のアドレスから、指定の値を順に書き込みます。
        /// </summary>
        /// <param name="ui16StartAddr">指定の値を順に書き込むアドレスの値です。</param>
        /// <param name="ui16Values">指定のアドレスから書き込む値です。</param>
        internal void WriteRange(UInt16 ui16StartAddr, params UInt16[] ui16Values)
        {
            ForEach(ui16StartAddr, ui16Values, Write);
        }

        /// <summary>
        /// 指定のアドレスに指定の値を書き込みます。
        /// </summary>
        /// <param name="ui16Addr">指定の値を書き込むアドレスの値です。</param>
        /// <param name="ui16Value">指定のアドレスに書き込む値です。</param>
        internal void Write(UInt16 ui16Addr, UInt16 ui16Value)
        {
            Write(ui16Addr, new Word(ui16Value));
        }

        private void Write(UInt16 ui16Addr, Word word)
        {
            m_contents[ui16Addr] = word;
        }

        /// <summary>
        /// 指定の開始アドレスから順に、指定のそれぞれの値について、指定の動作を行います。
        /// </summary>
        /// <param name="ui16StartAddr">
        /// 実施する動作に引数として渡す開始アドレスです。<paramref name="ui16Values"/> の
        /// それぞれの値に対して、実施する動作に引数として渡すアドレスは、
        /// "<paramref name="ui16StartAddr"/> + <paramref name="ui16Values"/>[] のインデックス"
        /// となります。
        /// </param>
        /// <param name="ui16Values">
        /// 実施する動作に引数として渡す値です。一連のそれぞれの値について、指定の動作を実行します。
        /// </param>
        /// <param name="action">それぞれのアドレスと値について、実施する動作です。</param>
        internal void ForEach(
            UInt16 ui16StartAddr, IEnumerable<UInt16> ui16Values, Action<UInt16, UInt16> action)
        {
            ui16Values.ForEach(
                (index, ui16Value) => action((UInt16)(ui16StartAddr + index), ui16Value));
        }
    }
}
