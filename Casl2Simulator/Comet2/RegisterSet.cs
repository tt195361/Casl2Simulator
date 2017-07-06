namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の一そろいのレジスタを表わします。
    /// </summary>
    internal class RegisterSet
    {
        #region Fields
        private readonly GeneralRegisters m_generalRegisters;
        private readonly Register m_stackPointer;
        private readonly Register m_programRegister;
        private readonly FlagRegister m_flagRegister;
        #endregion

        /// <summary>
        /// <see cref="RegisterSet"/> のインスタンスを初期化します。
        /// </summary>
        internal RegisterSet()
        {
            m_generalRegisters = new GeneralRegisters();
            m_stackPointer = Register.MakeSP();
            m_programRegister = Register.MakePR();
            m_flagRegister = new FlagRegister();

            Reset();
        }

        /// <summary>
        /// 汎用レジスタの組を取得します。
        /// </summary>
        internal GeneralRegisters GR
        {
            get { return m_generalRegisters; }
        }

        /// <summary>
        /// スタックポインタを取得します。
        /// </summary>
        internal Register SP
        {
            get { return m_stackPointer; }
        }

        /// <summary>
        /// プログラムレジスタを取得します。
        /// </summary>
        internal Register PR
        {
            get { return m_programRegister; }
        }

        /// <summary>
        /// フラグレジスタを取得します。
        /// </summary>
        internal FlagRegister FR
        {
            get { return m_flagRegister; }
        }

        /// <summary>
        /// それぞれのレジスタの値を初期化します。
        /// </summary>
        internal void Reset()
        {
            m_generalRegisters.Reset();
            m_stackPointer.Reset();
            m_programRegister.Reset();
            m_flagRegister.Reset();
        }
    }
}
