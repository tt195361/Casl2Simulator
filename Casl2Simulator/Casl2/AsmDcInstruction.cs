using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の DC です。
    /// </summary>
    internal class AsmDcInstruction : ProgramInstruction
    {
        internal static String Generate(Label label, params Constant[] constants)
        {
            return Line.Generate(label, MnemonicDef.DC, constants);
        }

        #region Instance Fields
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
        /// <returns>
        /// 解釈した結果として生成した <see cref="Operand"/> クラスのオブジェクトを返します。
        /// </returns>
        protected override Operand ParseSpecificOperand(OperandLexer lexer)
        {
            m_constants = ConstantCollection.Parse(lexer);
            return m_constants;
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
            m_constants.GenerateCode(lblManager, relModule);
        }
    }
}
