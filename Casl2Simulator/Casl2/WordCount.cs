using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 語数を指定するオペランドを取り扱います。
    /// </summary>
    internal class WordCount : Operand
    {
        /// <summary>
        /// オペランドを解釈します。記述の形式は "語数" です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="WordCount"/> のオブジェクトを返します。
        /// </returns>
        internal static WordCount Parse(OperandLexer lexer)
        {
            // 語数は, 10 進定数 (≧ 0) で指定する。
            Token token = lexer.ReadCurrentAs(TokenType.DecimalConstant);
            return new WordCount(token.I32Value);
        }

        #region Fields
        private readonly Int32 m_value;

        private const Int32 MinValue = 0;
        private const Int32 MaxValue = Comet2Defs.MaxAddress;
        #endregion

        private WordCount(Int32 value)
        {
            if (value < MinValue || MaxValue < value)
            {
                String message = String.Format(Resources.MSG_WordCountOutOfRange, value, MinValue, MaxValue);
                throw new Casl2SimulatorException(message);
            }

            m_value = value;
        }

        internal Int32 Value
        {
            get { return m_value; }
        }

        public override String ToString()
        {
            return m_value.ToString();
        }

        internal static WordCount MakeForUnitTest(Int32 value)
        {
            return new WordCount(value);
        }
    }
}
