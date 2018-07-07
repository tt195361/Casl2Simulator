using System.Collections.Generic;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// COMET II CPU で動作するプログラムを格納する実行可能モジュールです。
    /// </summary>
    internal class ExecutableModule
    {
        #region Instance Members
        private readonly MemoryAddress m_loadAddress;
        private readonly MemoryAddress m_execStartAddress;
        private readonly IEnumerable<Word> m_words;
        #endregion

        internal ExecutableModule(
            MemoryAddress loadAddress, MemoryAddress execStartAddress, IEnumerable<Word> words)
        {
            m_loadAddress = loadAddress;
            m_execStartAddress = execStartAddress;
            m_words = words;
        }

        /// <summary>
        /// プログラムの語をロードするアドレスを取得します。
        /// </summary>
        internal MemoryAddress LoadAddress
        {
            get { return m_loadAddress; }
        }

        /// <summary>
        /// プログラムの実行を開始するアドレスを取得します。
        /// </summary>
        internal MemoryAddress ExecStartAddress
        {
            get { return m_execStartAddress; }
        }

        /// <summary>
        /// プログラムの語を取得します。
        /// </summary>
        internal IEnumerable<Word> Words
        {
            get { return m_words; }
        }
    }
}
