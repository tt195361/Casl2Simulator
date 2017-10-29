using System;
using System.Linq;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の DC です。
    /// </summary>
    internal class AsmDcInstruction : Instruction
    {
        #region Fields
        private Constant[] m_constants;
        #endregion

        internal AsmDcInstruction()
            : base(Casl2Defs.DC)
        {
            //
        }

        internal Constant[] Constants
        {
            get { return m_constants; }
        }

        /// <summary>
        /// DC 命令のオペランドを解釈します。記述の形式は "定数[,定数]..." です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        protected override void ParseSpecificOperand(OperandLexer lexer)
        {
            m_constants = Constant.ParseList(lexer);
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_ConstantList; }
        }

        internal override Int32 GetCodeWordCount()
        {
            return m_constants.Select((constant) => constant.GetWordCount())
                              .Sum();
        }

        internal override void GenerateCode(String label, LabelManager lblManager, RelocatableModule relModule)
        {
            m_constants.ForEach((constant) => constant.GenerateCode(lblManager, relModule));
        }
    }
}
