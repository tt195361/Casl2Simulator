using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II の定数の並びです。
    /// </summary>
    internal class ConstantCollection : Operand, IEnumerable<Constant>
    {
        /// <summary>
        /// 定数の並びを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="ConstantCollection"/> オブジェクトを返します。
        /// </returns>
        internal static ConstantCollection Parse(OperandLexer lexer)
        {
            List<Constant> constantList = new List<Constant>();

            for ( ; ; )
            {
                Constant constant = Constant.Parse(lexer);
                constantList.Add(constant);

                if (!lexer.SkipIf(TokenType.Comma))
                {
                    break;
                }
            }

            // 解釈しなかった残りの字句要素は、Instruction.ReadOperand() で取り扱う。
            return new ConstantCollection(constantList.ToArray());
        }

        #region Fields
        private readonly Constant[] m_constants;
        #endregion

        private ConstantCollection(Constant[] constants)
        {
            m_constants = constants;
        }

        public IEnumerator<Constant> GetEnumerator()
        {
            // https://stackoverflow.com/questions/1272673/obtain-generic-enumerator-from-an-array
            return m_constants.Cast<Constant>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal Int32 GetCodeWordCount()
        {
            return m_constants.Select((constant) => constant.GetWordCount())
                              .Sum();
        }

        internal void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            m_constants.ForEach((constant) => constant.GenerateCode(lblManager, relModule));
        }

        public override String ToString()
        {
            return m_constants.Select((constant) => constant.ToString())
                              .ConcatStrings();
        }
    }
}
