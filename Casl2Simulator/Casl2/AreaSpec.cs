using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 領域を指定するオペランドを取り扱います。
    /// </summary>
    internal class AreaSpec : Operand
    {
        /// <summary>
        /// オペランドを解釈します。記述の形式は "領域,文字長領域" です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="AreaSpec"/> のオブジェクトを返します。
        /// </returns>
        internal static AreaSpec Parse(OperandLexer lexer)
        {
            Label buffer = Label.Parse(lexer);
            lexer.SkipComma();
            Label length = Label.Parse(lexer);
            return new AreaSpec(buffer, length);

            // 解釈しなかった残りの字句要素は、Instruction.ParseOperand() で取り扱う。
        }

        #region Fields
        private readonly Label m_buffer;
        private readonly Label m_length;
        #endregion

        private AreaSpec(Label buffer, Label length)
        {
            m_buffer = buffer;
            m_length = length;
        }

        internal Label Buffer
        {
            get { return m_buffer; }
        }

        internal Label Length
        {
            get { return m_length; }
        }

        public override String ToString()
        {
            return Operand.Join(m_buffer, m_length);
        }

        internal static AreaSpec MakeForUnitTest(String bufferName, String lengthName)
        {
            Label buffer = new Label(bufferName);
            Label length = new Label(lengthName);
            return new AreaSpec(buffer, length);
        }
    }
}
