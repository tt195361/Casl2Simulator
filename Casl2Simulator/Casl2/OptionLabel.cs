using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// オプションでラベルを指定するオペランドを取り扱います。
    /// </summary>
    internal class OptionLabel : Operand
    {
        internal static OptionLabel Parse(OperandLexer lexer)
        {
            Token token = lexer.CurrentToken;

            if (token.Type == TokenType.EndOfToken)
            {
                return new OptionLabel(null);
            }
            else if (token.Type == TokenType.Label)
            {
                lexer.MoveNext();
                Label label = new Label(token.StrValue);
                return new OptionLabel(label);
            }
            else
            {
                String message = String.Format(Resources.MSG_OptionLabelParseError, token);
                throw new Casl2SimulatorException(message);
            }

            // 解釈しなかった残りの字句要素は、Instruction.ParseOperand() で取り扱う。
        }

        #region Fields
        private readonly Label m_label;
        #endregion

        private OptionLabel(Label label)
        {
            m_label = label;
        }

        internal Label Label
        {
            get { return m_label; }
        }

        public override String ToString()
        {
            if (m_label == null)
            {
                return String.Empty;
            }
            else
            {
                return m_label.ToString();
            }
        }
    }
}
