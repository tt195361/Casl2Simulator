using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II CPU の一そろいのレジスタを表わします。
    /// </summary>
    internal class CpuRegisterSet
    {
        #region Instance Fields
        private readonly GeneralRegisters m_generalRegisters;
        private readonly CpuRegister m_stackPointer;
        private readonly CpuRegister m_programRegister;
        private readonly FlagRegister m_flagRegister;
        #endregion

        /// <summary>
        /// <see cref="CpuRegisterSet"/> のインスタンスを初期化します。
        /// </summary>
        internal CpuRegisterSet()
        {
            m_generalRegisters = new GeneralRegisters();
            m_stackPointer = new CpuRegister(RegisterDef.SP);
            m_programRegister = new CpuRegister(RegisterDef.PR);
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
        internal CpuRegister SP
        {
            get { return m_stackPointer; }
        }

        /// <summary>
        /// プログラムレジスタを取得します。
        /// </summary>
        internal CpuRegister PR
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
