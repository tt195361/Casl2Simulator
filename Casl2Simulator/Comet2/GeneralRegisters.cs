using System;
using System.Linq;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の 8 個の汎用レジスタ (General Register) を表わします。
    /// </summary>
    internal class GeneralRegisters
    {
        #region Instance Fields
        private readonly CpuRegister[] m_grArray;
        #endregion

        /// <summary>
        /// <see cref="GeneralRegisters"/> のインスタンスを初期化します。
        /// </summary>
        internal GeneralRegisters()
        {
            m_grArray = RegisterDef.GrNames
                                   .Select((name) => new CpuRegister(name))
                                   .ToArray();
        }

        /// <summary>
        /// 指定の GR を取得します。
        /// </summary>
        /// <param name="grNumber">取得する GR を指定する値です。</param>
        /// <returns>指定の GR を返します。</returns>
        internal CpuRegister this[Int32 grNumber]
        {
            get
            {
                ArgChecker.CheckRange(grNumber, 0, RegisterDef.GrCount - 1, nameof(grNumber));
                return m_grArray[grNumber];
            }
        }

        /// <summary>
        /// それぞれの汎用レジスタの値を初期化します。
        /// </summary>
        internal void Reset()
        {
            m_grArray.ForEach((gr) => gr.Reset());
        }
    }
}
