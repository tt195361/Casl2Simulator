using System;

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
        internal static Word Fetch(Register pr, Memory memory)
        {
            Int32 address = pr.Value.GetAsUnsigned();
            Word word = memory.Read(address);
            pr.Value = pr.Value.AddAsUnsigned(1);
            return word;
        }
    }
}
