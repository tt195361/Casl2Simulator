using System;
using Tt195361.Casl2Simulator.Common;
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
        #region Fields
        // 指標レジスタとして指定できる GR は GR1 ~ 7。
        private const Int32 MinIndexReg = 1;
        private const Int32 MaxIndexReg = 7;

        // 指標レジスタが指定されていない場合の XR2 フィールドの値。
        private const UInt16 XR2ForNoX = 0;
        #endregion

        /// <summary>
        /// adr[,x] のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="AdrXOperand"/> オブジェクトを返します。
        /// </returns>
        internal static AdrXOperand Parse(OperandLexer lexer)
        {
            return Parse(lexer, OpcodeDef.Dummy);
        }

        /// <summary>
        /// adr[,x] のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="opcode">このオペラントの命令の第 1 語のオペコードの値です。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="AdrXOperand"/> オブジェクトを返します。
        /// </returns>
        internal static AdrXOperand Parse(OperandLexer lexer, UInt16 opcode)
        {
            IAdrValue adr = ParseAdr(lexer);
            RegisterOperand x = ParseX(lexer);
            return new AdrXOperand(opcode, adr, x);
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
            RegisterOperand x = RegisterOperand.Parse(lexer);
            if (!CanIndex(x))
            {
                String message =
                    String.Format(Resources.MSG_CanNotBeIndexRegister, x.Name, MinIndexReg, MaxIndexReg);
                throw new Casl2SimulatorException(message);
            }

            return x;
        }

        private static Boolean CanIndex(RegisterOperand x)
        {
            return MinIndexReg <= x.Number && x.Number <= MaxIndexReg;
        }

        #region Fields
        private readonly IAdrValue m_adr;
        private readonly RegisterOperand m_x;
        #endregion

        private AdrXOperand(UInt16 opcode, IAdrValue adr, RegisterOperand x)
            : base(opcode)
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
                return XR2ForNoX;
            }
            else
            {
                return m_x.Number;
            }
        }

        internal override Word? MakeSecondWord(LabelManager lblManager)
        {
            UInt16 address = m_adr.GetAddress(lblManager);
            return new Word(address);
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
                return Operand.Join(adrStr, m_x);
            }
        }

        internal static AdrXOperand MakeForUnitTest(IAdrValue adr, RegisterOperand x)
        {
            return MakeForUnitTest(OpcodeDef.Dummy, adr, x);
        }

        internal static AdrXOperand MakeForUnitTest(UInt16 opcode, IAdrValue adr, RegisterOperand x)
        {
            return new AdrXOperand(opcode, adr, x);
        }
    }
}
