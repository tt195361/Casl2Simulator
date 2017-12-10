using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// リテラルを表わします。
    /// </summary>
    /// <remarks>
    /// リテラルは、一つの 10 進定数、16 進定数又は文字定数の前に等号 (=) を付けて記述します。
    /// CASL II は、等号の後の定数をオペランドとする DC 命令を生成し、そのアドレスを adr に値とします。
    /// </remarks>
    internal class Literal : IAdrValue
    {
        /// <summary>
        /// リテラルを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="Literal"/> オブジェクトを返します。
        /// </returns>
        internal static Literal Parse(OperandLexer lexer)
        {
            Constant constant = ParseLiteralConstant(lexer);
            return new Literal(constant);
        }

        private static Constant ParseLiteralConstant(OperandLexer lexer)
        {
            Token token = lexer.CurrentToken;

            if (token.Type == TokenType.DecimalConstant)
            {
                lexer.MoveNext();
                return new DecimalConstant(token.I32Value);
            }
            else if (token.Type == TokenType.HexaDecimalConstant)
            {
                lexer.MoveNext();
                return new HexaDecimalConstant(token.I32Value);
            }
            else if (token.Type == TokenType.StringConstant)
            {
                lexer.MoveNext();
                return new StringConstant(token.StrValue);
            }
            else
            {
                String message = String.Format(Resources.MSG_LiteralParseError, token);
                throw new Casl2SimulatorException(message);
            }
        }

        #region Fields
        private readonly Constant m_constant;
        private Label m_label;
        #endregion

        private Literal(Constant constant)
        {
            m_constant = constant;
        }

        internal Constant Constant
        {
            get { return m_constant; }
        }

        internal Label Label
        {
            get { return m_label; }
        }

        String IAdrValue.GenerateDc(LabelManager lblManager)
        {
            m_label = lblManager.MakeLiteralLabel();
            return AsmDcInstruction.Generate(m_label, m_constant);
        }

        UInt16 IAdrValue.GetAddress(LabelManager lblManager)
        {
            return lblManager.GetOffset(m_label);
        }

        public override String ToString()
        {
            return Casl2Defs.EqualSign + m_constant.ToString();
        }

        internal static Literal MakeForUnitTest(Constant constant)
        {
            return new Literal(constant);
        }
    }
}
