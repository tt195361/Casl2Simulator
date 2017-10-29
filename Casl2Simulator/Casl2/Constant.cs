using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II の定数です。
    /// </summary>
    internal abstract class Constant
    {
        /// <summary>
        /// 定数の並びを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="Constant"/> オブジェクトの配列を返します。
        /// </returns>
        internal static Constant[] ParseList(OperandLexer lexer)
        {
            List<Constant> constantList = new List<Constant>();

            for ( ; ; )
            {
                Constant constant = Parse(lexer);
                constantList.Add(constant);

                if (!lexer.SkipIf(TokenType.Comma))
                {
                    break;
                }
            }

            // 解釈しなかった残りの字句要素は、Instruction.DoParseOperand() で取り扱う。
            return constantList.ToArray();
        }

        private static Constant Parse(OperandLexer lexer)
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
        internal abstract Int32 GetWordCount();

        /// <summary>
        /// この定数のコードを生成します。
        /// </summary>
        /// <param name="lblManager">
        /// ラベルを管理する <see cref="LabelManager"/> のオブジェクトです。
        /// </param>
        /// <param name="relModule">
        /// 生成したコードを格納する <see cref="RelocatableModule"/> のオブジェクトです。
        /// </param>
        internal abstract void GenerateCode(LabelManager lblManager, RelocatableModule relModule);

        public override String ToString()
        {
            return ValueToString();
        }

        protected abstract String ValueToString();
    }
}
