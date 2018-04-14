using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の END です。
    /// </summary>
    internal class AsmEndInstruction : ProgramInstruction
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

        protected override Operand ParseSpecificOperand(OperandLexer lexer)
        {
            return null;
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_NoOperand; }
        }

        internal override void GenerateCode(
            Label definedLabel, LabelManager lblManager, RelocatableModule relModule)
        {
            // END 命令はラベルを指定できないことになっている。
            // 指定しても特に問題はないので、ラベルはチェックしない。
        }
    }
}
