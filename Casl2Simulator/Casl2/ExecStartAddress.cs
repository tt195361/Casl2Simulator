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

        #region Fields
        private readonly Label m_label;
        private UInt16 m_codeOffset;
        #endregion

        private ExecStartAddress(Label label)
        {
            m_label = label;
        }

        internal Label Label
        {
            get { return m_label; }
        }

        internal UInt16 CodeOffset
        {
            get { return m_codeOffset; }
        }

        internal void CalculateCodeOffset(LabelManager lblManager, RelocatableModule relModule)
        {
            m_codeOffset = DoCalculateCodeOffset(lblManager, relModule);
        }

        internal UInt16 DoCalculateCodeOffset(LabelManager lblManager, RelocatableModule relModule)
        {
            // 実行開始番地は、そのプログラム内で定義されたラベルで指定する。指定がある場合
            // はその番地から、省略した場合は START 命令の次の命令から、実行を開始する。
            if (m_label == null)
            {
                return relModule.GetCodeOffset();
            }
            else
            {
                if (!lblManager.IsRegistered(m_label))
                {
                    String message = String.Format(Resources.MSG_StartLabelNotDefined, m_label.Name);
                    throw new Casl2SimulatorException(message);
                }

                return lblManager.GetOffset(m_label);
            }
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
