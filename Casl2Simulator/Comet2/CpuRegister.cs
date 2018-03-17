using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II CPU のレジスタを表わします。
    /// </summary>
    internal class CpuRegister
    {
        #region Instance Fields
        private readonly String m_name;
        private Word m_value;
        #endregion

        /// <summary>
        /// <see cref="CpuRegister"/> のインスタンスを初期化します。
        /// </summary>
        internal CpuRegister(String name)
        {
            m_name = name;

            Reset();
        }

        /// <summary>
        /// レジスタの値を取得または設定します。
        /// </summary>
        internal Word Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// レジスタの値を初期化します。
        /// </summary>
        internal void Reset()
        {
            SetValue(0);
        }

        /// <summary>
        /// レジスタに指定の値を設定します。
        /// </summary>
        /// <param name="ui16Val">レジスタに設定する <see cref="UInt16"/> 型の値です。</param>
        internal void SetValue(UInt16 ui16Val)
        {
            Value = new Word(ui16Val);
        }

        /// <summary>
        /// レジスタの値に 1 を加えます。
        /// </summary>
        internal void Increment()
        {
            Value = Alu.AddLogical(Value, Word.One);
        }

        /// <summary>
        /// レジスタの値を 1 減らします。
        /// </summary>
        internal void Decrement()
        {
            Value = Alu.SubtractLogical(Value, Word.One);
        }

        /// <summary>
        /// このレジスタを表わす文字列を作成します。
        /// </summary>
        /// <returns>このレジスタを表わす文字列を返します。</returns>
        public override String ToString()
        {
            UInt16 ui16Val = m_value.GetAsUnsigned();
            String str = String.Format("{0}: {1} (0x{1:x04})", m_name, ui16Val);
            return str;
        }
    }
}
