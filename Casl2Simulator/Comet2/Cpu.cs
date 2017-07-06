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
            Instruction instruction = Decoder.Decode(firstWord);

            // 命令語から r/r1 フィールドと x/r2 フィールドを取得し、命令を実行します。
            UInt16 rR1Field = GetRR1Field(firstWord);
            UInt16 xR2Field = GetXR2Field(firstWord);
            instruction.Execute(rR1Field, xR2Field, m_registerSet, memory);
        }

        private UInt16 GetRR1Field(Word firstWord)
        {
            UInt16 rR1Field = firstWord.GetBits(7, 4);
            ArgChecker.CheckRange(rR1Field, 0, GeneralRegisters.Count - 1, "r/r1");
            return rR1Field;
        }

        private UInt16 GetXR2Field(Word firstWord)
        {
            UInt16 xR2Field = firstWord.GetBits(3, 0);
            ArgChecker.CheckRange(xR2Field, 0, GeneralRegisters.Count - 1, "x/r2");
            return xR2Field;
        }
    }
}
