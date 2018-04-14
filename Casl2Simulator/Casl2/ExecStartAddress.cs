using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// オペランドの実行開始アドレスを取り扱います。
    /// </summary>
    internal class ExecStartAddress : Operand
    {
        internal static ExecStartAddress Parse(OperandLexer lexer)
        {
            Token token = lexer.CurrentToken;

            if (token.Type == TokenType.EndOfToken)
            {
                return new ExecStartAddress(null);
            }
            else if (token.Type == TokenType.Label)
            {
                lexer.MoveNext();
                Label label = new Label(token.StrValue);
                return new ExecStartAddress(label);
            }
            else
            {
                String message = String.Format(Resources.MSG_OptionLabelParseError, token);
                throw new Casl2SimulatorException(message);
            }

            // 解釈しなかった残りの字句要素は、Instruction.ParseOperand() で取り扱う。
        }

        #region Instance Fields
        private readonly Label m_label;
        #endregion

        private ExecStartAddress(Label label)
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

        internal static ExecStartAddress MakeForUnitTest(Label label)
        {
            return new ExecStartAddress(label);
        }
    }
}
