using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の START です。
    /// </summary>
    internal class AsmStartInstruction : ProgramInstruction
    {
        #region Instance Fields
        private ExecStartAddress m_execStartAddress;
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

        internal ExecStartAddress ExecStartAddress
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
            m_execStartAddress = ExecStartAddress.Parse(lexer);
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

        internal override void GenerateCode(
            Label definedLabel, LabelManager lblManager, RelocatableModule relModule)
        {
            if (definedLabel == null)
            {
                throw new Casl2SimulatorException(Resources.MSG_NoLabelForStart);
            }

            // 実行開始番地は、そのプログラム内で定義されたラベルで指定する。
            // また、この命令につけられたラベルは、他のプログラムから入口名として参照できる。
            Label execStartLabel = GetExecStartLabel(definedLabel);
            Label exportLabel = definedLabel;

            EntryPoint entryPoint = new EntryPoint(execStartLabel, exportLabel);
            relModule.SetEntryPoint(entryPoint);
        }

        private Label GetExecStartLabel(Label definedLabel)
        {
            if (m_execStartAddress.Label == null)
            {
                // 省略した場合は START 命令の次の命令 (この命令に定義されたラベルの番地) から、実行を開始する。
                return definedLabel;
            }
            else
            {
                // 指定がある場合はその番地から。
                return m_execStartAddress.Label;
            }
        }

        internal void SetExecStartAddressForUnitTest(ExecStartAddress execStartAddress)
        {
            m_execStartAddress = execStartAddress;
        }
    }
}
