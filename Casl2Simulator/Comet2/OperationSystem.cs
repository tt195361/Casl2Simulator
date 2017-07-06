namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の実行システムを表わします。
    /// </summary>
    internal class OperationSystem
    {
        #region Fields
        private readonly Memory m_memory;
        private readonly Cpu m_cpu;
        #endregion

        /// <summary>
        /// 実行システムの新しいインスタンスを初期化します。
        /// </summary>
        internal OperationSystem()
        {
            m_memory = new Memory();
            m_cpu = new Cpu();
        }

        /// <summary>
        /// 実行システムを初期化します。
        /// </summary>
        internal void Reset()
        {
            m_memory.Reset();
            m_cpu.Reset();
        }

        /// <summary>
        /// 実行システムを稼働します。
        /// </summary>
        internal void Execute()
        {
            m_cpu.Execute(m_memory);
        }
    }
}
