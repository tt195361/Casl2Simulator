using System;
using Tt195361.Casl2Simulator.Common;
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

        /// <summary>
        /// このオペランドで追加されるワード数を返します。
        /// </summary>
        /// <returns>このオペランドで追加されるワード数を返します。</returns>
        internal virtual Int32 GetAdditionalWordCount()
        {
            // デフォルトは追加なし。
            return 0;
        }

        /// <summary>
        /// このオペランドの r/r1 フィールドの値を取得します。
        /// </summary>
        /// <returns>このオペランドの r/r1 フィールドの値を返します。</returns>
        internal virtual UInt16 GetRR1()
        {
            // デフォルトは 0。
            return 0;
        }

        /// <summary>
        /// このオペランドの x/r2 フィールドの値を取得します。
        /// </summary>
        /// <returns>このオペランドの x/r2 フィールドの値を返します。</returns>
        internal virtual UInt16 GetXR2()
        {
            // デフォルトは 0。
            return 0;
        }

        /// <summary>
        /// このオペランドの第 2 語を返します。
        /// </summary>
        /// <param name="lblManager">
        /// ラベルを管理する <see cref="LabelManager"/> のオブジェクトです。
        /// </param>
        /// <returns>
        /// このオペランドの第 2 語を返します。第 2 語がない場合は <see langword="null"/> を返します。
        /// </returns>
        internal virtual Word? MakeSecondWord(LabelManager lblManager)
        {
            return null;
        }
    }
}
