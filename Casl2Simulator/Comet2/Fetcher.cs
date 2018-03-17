using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// メモリから値をフェッチします。
    /// </summary>
    internal static class Fetcher
    {
        /// <summary>
        /// PR レジスタが指すアドレスの内容を読み出しその値を返します。
        /// PR レジスタの内容に 1 を加えます。 
        /// </summary>
        /// <param name="pr">Program Register (PR) です。</param>
        /// <param name="memory">COMET II の主記憶です。</param>
        /// <returns>PR レジスタが指すアドレスの内容を返します。</returns>
        internal static Word Fetch(CpuRegister pr, Memory memory)
        {
            Word word = memory.Read(pr.Value);
            pr.Increment();
            return word;
        }
    }
}
