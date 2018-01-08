using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の START です。
    /// </summary>
    internal class AsmStartInstruction : Instruction
    {
        #region Fields
        private OptionLabel m_execStartAddress;
        #endregion

        internal AsmStartInstruction()
            : base(MnemonicDef.START)
        {
            //
        }

        internal override Boolean IsStart()
        {
            return true;
        }

        internal OptionLabel ExecStartAddress
        {
            get { return m_execStartAddress; }
        }

        /// <summary>
        /// START 命令のオペランドを解釈します。記述の形式は "[実行開始番地]" です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="Operand"/> クラスのオブジェクトを返します。
        /// </returns>
        protected override Operand ParseSpecificOperand(OperandLexer lexer)
        {
            m_execStartAddress = OptionLabel.Parse(lexer);
            return m_execStartAddress;
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_ExecStartAddr; }
        }

        internal override Int32 GetCodeWordCount()
        {
            return 0;
        }

        internal override void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            // TODO: 実装する。
            // 開始アドレス
            // Exports: 外部モジュールから参照できるアドレス。
        }
    }
}
