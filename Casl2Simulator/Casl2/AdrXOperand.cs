using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドの adr[,x] を表わします。
    /// </summary>
    /// <remarks>
    /// アドレスは、10 進定数、16 進定数、アドレス定数又はリテラルで指定します。
    /// </remarks>
    internal class AdrXOperand : MachineInstructionOperand
    {
        /// <summary>
        /// adr[,x] のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="AdrXOperand"/> オブジェクトを返します。
        /// </returns>
        internal static AdrXOperand Parse(OperandLexer lexer)
        {
            IAdrValue adr = ParseAdr(lexer);
            RegisterOperand x = ParseX(lexer);
            return new AdrXOperand(adr, x);
        }

        private static IAdrValue ParseAdr(OperandLexer lexer)
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
            else if (token.Type == TokenType.Label)
            {
                lexer.MoveNext();
                return new AddressConstant(token.StrValue);
            }
            else if (token.Type == TokenType.EqualSign)
            {
                // リテラルは、一つの 10 進定数、16 進定数又は文字定数の前に等号 (=) を付けて記述する。
                lexer.MoveNext();
                return Literal.Parse(lexer);
            }
            else
            {
                String message = String.Format(Resources.MSG_CouldNotParseAsAdr, token);
                throw new Casl2SimulatorException(message);
            }
        }

        private static RegisterOperand ParseX(OperandLexer lexer)
        {
            if (!lexer.SkipIf(TokenType.Comma))
            {
                return null;
            }
            else
            {
                return ParseIndexRegister(lexer);
            }
        }

        // 指標レジスタとして指定できる GR は GR1 ~ 7。
        private static RegisterOperand ParseIndexRegister(OperandLexer lexer)
        {
            RegisterOperand x = lexer.ReadCurrentAsRegisterOperand();
            if (!x.CanIndex)
            {
                String message =
                    String.Format(Resources.MSG_CanNotBeIndexRegister, x.Name, RegisterOperand.IndexRegisters);
                throw new Casl2SimulatorException(message);
            }

            return x;
        }

        #region Fields
        private readonly IAdrValue m_adr;
        private readonly RegisterOperand m_x;
        #endregion

        private AdrXOperand(IAdrValue adr, RegisterOperand x)
        {
            m_adr = adr;
            m_x = x;
        }

        internal IAdrValue Adr
        {
            get { return m_adr; }
        }

        internal RegisterOperand X
        {
            get { return m_x; }
        }

        internal override Int32 GetAdditionalWordCount()
        {
            // adr で 1 ワード追加する。
            return 1;
        }

        internal override UInt16 GetXR2()
        {
            if (m_x == null)
            {
                return 0;
            }
            else
            {
                return m_x.Number;
            }
        }

        public override String ToString()
        {
            String adrStr = m_adr.ToString();

            if (m_x == null)
            {
                return adrStr;
            }
            else
            {
                return adrStr + Casl2Defs.Comma + m_x.ToString();
            }
        }

        internal static AdrXOperand MakeForUnitTest(IAdrValue adr, RegisterOperand x)
        {
            return new AdrXOperand(adr, x);
        }
    }
}
