using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// オペランドの字句を解析します。
    /// </summary>
    internal class OperandLexer
    {
        #region Fields
        private readonly ReadBuffer m_buffer;
        private Token m_currentToken;
        #endregion

        internal OperandLexer(ReadBuffer buffer)
        {
            m_buffer = buffer;
            // ここでは、最初のトークンを読まない。解釈できずに例外になる可能性があるため。
        }

        /// <summary>
        /// 現在の <see cref="Token"/> オブジェクトを取得します。
        /// </summary>
        internal Token CurrentToken
        {
            get { return m_currentToken; }
        }

        /// <summary>
        /// 次の <see cref="Token"/> を読み込み、<see cref="CurrentToken"/> を更新します。
        /// </summary>
        internal void MoveNext()
        {
            m_currentToken = ReadNext();
        }

        private Token ReadNext()
        {
            Char firstChar = m_buffer.Current;
            if (Line.EndOfField(firstChar))
            {
                return Token.MakeEndOfToken();
            }
            else if (firstChar == Casl2Defs.Comma)
            {
                m_buffer.MoveNext();
                return Token.MakeComma();
            }
            else if (firstChar == Casl2Defs.EqualSign)
            {
                m_buffer.MoveNext();
                return Token.MakeEqualSign();
            }
            else if (DecimalConstant.IsStart(firstChar))
            {
                Int32 i32Value = DecimalConstant.Read(m_buffer);
                return Token.MakeDecimalConstant(i32Value);
            }
            else if (HexaDecimalConstant.IsStart(firstChar))
            {
                Int32 i32Value = HexaDecimalConstant.Read(m_buffer);
                return Token.MakeHexaDecimalConstant(i32Value);
            }
            else if (StringConstant.IsStart(firstChar))
            {
                String strValue = StringConstant.Read(m_buffer);
                return Token.MakeStringConstant(strValue);
            }
            else if (Label.IsStart(firstChar))
            {
                // ラベルはレジスタ名の場合もある。オペランドの字句要素の区切りまで読み込む。
                String strValue = Operand.ReadItem(m_buffer);
                if (RegisterOperand.IsRegisterName(strValue))
                {
                    return Token.MakeRegisterName(strValue);
                }
                else
                {
                    return Token.MakeLabel(strValue);
                }
            }
            else
            {
                String message = String.Format(Resources.MSG_CouldNotParseAsToken, firstChar);
                throw new Casl2SimulatorException(message);
            }
        }

        internal Label ReadCurrentAsLabel()
        {
            Token token = ReadCurrentAs(TokenType.Label);
            return new Label(token.StrValue);
        }

        internal RegisterOperand ReadCurrentAsRegisterOperand()
        {
            Token token = ReadCurrentAs(TokenType.RegisterName);
            return RegisterOperand.GetFor(token.StrValue);
        }

        internal void SkipComma()
        {
            Token notUsed = ReadCurrentAs(TokenType.Comma);
        }

        private Token ReadCurrentAs(TokenType expectedType)
        {
            TokenType actualType = CurrentToken.Type;
            if (expectedType != actualType)
            {
                String message = String.Format(Resources.MSG_NotExpectedToken, expectedType, CurrentToken);
                throw new Casl2SimulatorException(message);
            }

            Token expectedToken = CurrentToken;
            MoveNext();
            return expectedToken;
        }

        internal Boolean SkipIf(TokenType type)
        {
            if (CurrentToken.Type != type)
            {
                return false;
            }
            else
            {
                MoveNext();
                return true;
            }
        }

        public override String ToString()
        {
            String str = String.Format("{0}; '{1}'", m_currentToken, m_buffer);
            return str;
        }
    }
}
