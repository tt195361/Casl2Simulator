using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令のオペランドを表わす抽象クラスです。
    /// </summary>
    internal abstract class MachineInstructionOperand
    {
        /// <summary>
        /// r1,r2 あるいは r,adr[,x] のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="MachineInstructionOperand"/> オブジェクトを返します。
        /// </returns>
        internal static MachineInstructionOperand ParseR1R2OrRAdrX(OperandLexer lexer)
        {
            RegisterOperand r1 = lexer.ReadCurrentAsRegisterOperand();
            lexer.SkipComma();
            MachineInstructionOperand operand = ParseR2OrAdrX(r1, lexer);
            return operand;
        }

        private static MachineInstructionOperand ParseR2OrAdrX(RegisterOperand r1, OperandLexer lexer)
        {
            try
            {
                return DoParseR2OrAdrX(r1, lexer);
            }
            catch (Casl2SimulatorException ex)
            {
                throw new Casl2SimulatorException(Resources.MSG_FailedToParseR2OrAdrX, ex);
            }
        }

        private static MachineInstructionOperand DoParseR2OrAdrX(RegisterOperand r1, OperandLexer lexer)
        {
            Token token = lexer.CurrentToken;
            if (token.Type == TokenType.RegisterName)
            {
                lexer.MoveNext();
                RegisterOperand r2 = RegisterOperand.GetFor(token.StrValue);
                return new R1R2Operand(r1, r2);
            }
            else
            {
                AdrXOperand adrX = AdrXOperand.Parse(lexer);
                return new RAdrXOperand(r1, adrX);
            }
        }

        /// <summary>
        /// r,adr[,x] のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="MachineInstructionOperand"/> オブジェクトを返します。
        /// </returns>
        internal static MachineInstructionOperand ParseRAdrX(OperandLexer lexer)
        {
            RegisterOperand r = lexer.ReadCurrentAsRegisterOperand();
            lexer.SkipComma();
            AdrXOperand adrX = AdrXOperand.Parse(lexer);
            return new RAdrXOperand(r, adrX);
        }

        protected MachineInstructionOperand()
        {
            //
        }
    }
}
