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
        #region Static Fields
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
            IAdrCodeGenerator adr = ParseAdr(lexer);
            RegisterOperand x = ParseX(lexer);
            return new AdrXOperand(opcode, adr, x);
        }

        /// <summary>
        /// adr[,x] のオペランドを解釈します。戻り値は、解釈が成功したかどうかを示します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="adrX">
        /// 解釈が成功したとき、結果として生成した <see cref="AdrXOperand"/> のオブジェクトを格納します。
        /// 失敗した場合は <see langword="null"/> を格納します。
        /// </param>
        /// <returns>
        /// 解釈に成功した場合は <see langword="true"/> を、失敗した場合は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean TryParse(OperandLexer lexer, out AdrXOperand adrX)
        {
            IAdrCodeGenerator adr;
            if (!TryParseAdr(lexer, out adr))
            {
                adrX = null;
                return false;
            }
            else
            {
                RegisterOperand x = ParseX(lexer);
                adrX = new AdrXOperand(OpcodeDef.Dummy, adr, x);
                return true;
            }
        }

        private static IAdrCodeGenerator ParseAdr(OperandLexer lexer)
        {
            IAdrCodeGenerator adr;
            if (!TryParseAdr(lexer, out adr))
            {
                String message = String.Format(Resources.MSG_CouldNotParseAsAdr, lexer.CurrentToken);
                throw new Casl2SimulatorException(message);
            }

            return adr;
        }

        private static Boolean TryParseAdr(OperandLexer lexer, out IAdrCodeGenerator adr)
        {
            Token token = lexer.CurrentToken;

            if (token.Type == TokenType.DecimalConstant)
            {
                lexer.MoveNext();
                adr = new DecimalConstant(token.I32Value);
                return true;
            }
            else if (token.Type == TokenType.HexaDecimalConstant)
            {
                lexer.MoveNext();
                adr = new HexaDecimalConstant(token.I32Value);
                return true;
            }
            else if (token.Type == TokenType.Label)
            {
                lexer.MoveNext();
                adr = new AddressConstant(token.StrValue);
                return true;
            }
            else if (token.Type == TokenType.EqualSign)
            {
                // リテラルは、一つの 10 進定数、16 進定数又は文字定数の前に等号 (=) を付けて記述する。
                lexer.MoveNext();
                adr = Literal.Parse(lexer);
                return true;
            }
            else
            {
                adr = null;
                return false;
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

        #region Instance Fields
        private readonly IAdrCodeGenerator m_adr;
        private readonly RegisterOperand m_x;
        #endregion

        private AdrXOperand(UInt16 opcode, IAdrCodeGenerator adr, RegisterOperand x)
            : base(opcode)
        {
            m_adr = adr;
            m_x = x;
        }

        internal IAdrCodeGenerator Adr
        {
            get { return m_adr; }
        }

        internal RegisterOperand X
        {
            get { return m_x; }
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

        public override Int32 GetCodeWordCount()
        {
            return m_adr.GetCodeWordCount();
        }

        public override void GenerateCode(RelocatableModule relModule)
        {
            m_adr.GenerateCode(relModule);
        }

        public override String GenerateLiteralDc(LabelTable lblTable)
        {
            return m_adr.GenerateLiteralDc(lblTable);
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

        internal static AdrXOperand MakeForUnitTest(IAdrCodeGenerator adr, RegisterOperand x)
        {
            return MakeForUnitTest(OpcodeDef.Dummy, adr, x);
        }

        internal static AdrXOperand MakeForUnitTest(UInt16 opcode, IAdrCodeGenerator adr, RegisterOperand x)
        {
            return new AdrXOperand(opcode, adr, x);
        }
    }
}
