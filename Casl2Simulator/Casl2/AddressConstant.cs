using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アドレス定数です。
    /// </summary>
    internal class AddressConstant : Constant
    {
        /// <summary>
        /// 指定の文字がアドレス定数の最初の文字かどうかを判断します。
        /// </summary>
        /// <param name="firstChar">アドレス定数の最初かどうかを判断する対象の文字です。</param>
        /// <returns>
        /// 指定の文字がアドレス定数の最初の文字なら <see langword="true"/> を、
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsStart(Char firstChar)
        {
            return Casl2.Label.IsLabelFirstChar(firstChar);
        }

        /// <summary>
        /// アドレス定数を解釈します。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="AddressConstant"/> クラスのオブジェクトを返します。
        /// </returns>
        internal static AddressConstant Parse(ReadBuffer buffer)
        {
            String label = Casl2.Label.ParseOperand(buffer);
            return new AddressConstant(label);
        }

        #region Fields
        private readonly String m_label;
        #endregion

        private AddressConstant(String label)
        {
            m_label = label;
        }

        internal String Label
        {
            get { return m_label; }
        }

        internal override int GetWordCount()
        {
            return 1;
        }

        internal override void GenerateCode(LabelManager lblManager, RelocatableModule relModule)
        {
            throw new NotImplementedException();
        }
    }
}
