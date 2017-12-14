using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の END です。
    /// </summary>
    internal class AsmEndInstruction : Instruction
    {
        internal AsmEndInstruction()
            : base(MnemonicDef.END)
        {
            //
        }

        internal override Boolean IsEnd()
        {
            return true;
        }

        protected override void ParseSpecificOperand(OperandLexer lexer)
        {
            // オペランドなし
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_NoOperand; }
        }

        internal override void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            // END 命令はラベルを指定できないことになっている。
            // 指定しても特に問題はないので、ラベルはチェックしない。
        }

        protected override String OperandString()
        {
            return String.Empty;
        }
    }
}
