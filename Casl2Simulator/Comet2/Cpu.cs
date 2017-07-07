using System;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II CPU を表わします。
    /// </summary>
    internal class Cpu
    {
        #region Fields
        private readonly RegisterSet m_registerSet;
        #endregion

        /// <summary>
        /// <see cref="Cpu"/> のインスタンスを初期化します。
        /// </summary>
        internal Cpu()
        {
            m_registerSet = new RegisterSet();

            Reset();
        }

        /// <summary>
        /// CPU を初期化します。
        /// </summary>
        internal void Reset()
        {
            m_registerSet.Reset();
        }

        /// <summary>
        /// CPU を実行します。
        /// </summary>
        /// <param name="memory">COMET II の主記憶です。</param>
        internal void Execute(Memory memory)
        {
            // TODO: プログラムが終了するまで実行できるようにする。

            // PR レジスタが指すアドレスから命令語をフェッチし、デコードします。
            Word firstWord = Fetcher.Fetch(m_registerSet.PR, memory);
            UInt16 opcode = firstWord.GetBits(15, 8);
            Instruction instruction = Decoder.Decode(opcode);

            // 命令語から r/r1 フィールドと x/r2 フィールドを取得し、命令を実行します。
            UInt16 rR1Field = firstWord.GetBits(7, 4);
            UInt16 xR2Field = firstWord.GetBits(3, 0);
            instruction.Execute(rR1Field, xR2Field, m_registerSet, memory);
        }
    }
}
