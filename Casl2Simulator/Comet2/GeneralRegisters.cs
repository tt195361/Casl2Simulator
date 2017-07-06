using System;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の 8 個の汎用レジスタ (General Register) を表わします。
    /// </summary>
    internal class GeneralRegisters
    {
        #region Fields
        // GR (汎用レジスタ、General Register) は、GR0 ~ GR7 の 8 個。
        internal const Int32 Count = 8;

        private readonly Register[] m_grArray;
        #endregion

        /// <summary>
        /// <see cref="GeneralRegisters"/> のインスタンスを初期化します。
        /// </summary>
        internal GeneralRegisters()
        {
            m_grArray = new Register[Count];
            m_grArray.ForEach((index, gr) => m_grArray[index] = Register.MakeGR(index));
        }

        /// <summary>
        /// 指定の GR を取得します。
        /// </summary>
        /// <param name="grNumber">取得する GR を指定する値です。</param>
        /// <returns>指定の GR を返します。</returns>
        internal Register this[Int32 grNumber]
        {
            get
            {
                ArgChecker.CheckRange(grNumber, 0, Count - 1, nameof(grNumber));
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
