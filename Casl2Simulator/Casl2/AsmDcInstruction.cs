using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の DC です。
    /// </summary>
    internal class AsmDcInstruction : Instruction
    {
        internal static String Generate(Label label, params Constant[] constants)
        {
            return Line.Generate(label, MnemonicDef.DC, constants);
        }

        #region Fields
        private ConstantCollection m_constants;
        #endregion

        internal AsmDcInstruction()
            : base(MnemonicDef.DC)
        {
            //
        }

        internal ConstantCollection Constants
        {
            get { return m_constants; }
        }

        /// <summary>
        /// DC 命令のオペランドを解釈します。記述の形式は "定数[,定数]..." です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        protected override void ParseSpecificOperand(OperandLexer lexer)
        {
            m_constants = ConstantCollection.Parse(lexer);
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_ConstantList; }
        }

        internal override Int32 GetCodeWordCount()
        {
            return m_constants.GetCodeWordCount();
        }

        internal override void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            m_constants.GenerateCode(label, lblManager, relModule);
        }

        protected override String OperandString()
        {
            return m_constants.ToString();
        }
    }
}
