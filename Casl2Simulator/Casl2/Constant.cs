using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II の定数です。
    /// </summary>
    internal abstract class Constant : ICodeGenerator
    {
        internal static Constant Parse(OperandLexer lexer)
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
            else if (token.Type == TokenType.Label)
            {
                lexer.MoveNext();
                return new AddressConstant(token.StrValue);
            }
            else
            {
                String message = String.Format(Resources.MSG_ConstantParseError, token);
                throw new Casl2SimulatorException(message);
            }
        }

        protected Constant()
        {
            //
        }

        /// <summary>
        /// この定数が生成するコードのワード数を取得します。
        /// </summary>
        /// <returns>生成するコードのワード数を返します。</returns>
        public abstract Int32 GetCodeWordCount();

        /// <summary>
        /// この定数のコードを生成します。
        /// </summary>
        /// <param name="lblManager">
        /// ラベルを管理する <see cref="LabelManager"/> のオブジェクトです。
        /// </param>
        /// <param name="relModule">
        /// 生成したコードを格納する <see cref="RelocatableModule"/> のオブジェクトです。
        /// </param>
        public abstract void GenerateCode(LabelManager lblManager, RelocatableModule relModule);

        public override String ToString()
        {
            return ValueToString();
        }

        protected abstract String ValueToString();
    }
}
