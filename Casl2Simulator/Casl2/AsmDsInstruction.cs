using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の DS です。
    /// </summary>
    internal class AsmDsInstruction : Instruction
    {
        #region Fields
        private WordCount m_wordCount;
        #endregion

        internal AsmDsInstruction()
            : base(MnemonicDef.DS)
        {
            //
        }

        internal WordCount WordCount
        {
            get { return m_wordCount; }
        }

        /// <summary>
        /// DS 命令のオペランドを解釈します。記述の形式は "語数" です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="Operand"/> クラスのオブジェクトを返します。
        /// </returns>
        protected override Operand ParseSpecificOperand(OperandLexer lexer)
        {
            m_wordCount = WordCount.Parse(lexer);
            return m_wordCount;
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_WordCount; }
        }

        internal override Int32 GetCodeWordCount()
        {
            return m_wordCount.Value;
        }

        internal override void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            // 指定した語数の領域を確保する。指定の語数分の 0 を追加する。
            relModule.AddWords(Word.Zero, m_wordCount.Value);
        }

        internal void SetWordCountValueForUnitTest(Int32 wordCountValue)
        {
            m_wordCount = WordCount.MakeForUnitTest(wordCountValue);
        }
    }
}
