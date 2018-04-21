using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// マクロ命令の RPOP です。
    /// RPOP 命令は、スタックの内容を順次取り出し、GR7, GR6, ..., GR1 の順序で GR に格納します。
    /// </summary>
    internal class MacroRpopInstruction : ProgramInstruction
    {
        internal MacroRpopInstruction()
            : base(MnemonicDef.RPOP)
        {
            //
        }

        protected override Operand ParseSpecificOperand(OperandLexer lexer)
        {
            return null;
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_NoOperand; }
        }

        internal override String[] ExpandMacro(Label label)
        {
            String[] result = new String[7];
            // LABEL    POP     0,GR7
            //          POP     0,GR6
            //          POP     0,GR5
            //          POP     0,GR4
            //          POP     0,GR3
            //          POP     0,GR2
            //          POP     0,GR1
            result[0] = ProgramLine.Generate(label, MnemonicDef.POP, RegisterDef.GR7);
            result[1] = ProgramLine.Generate(null, MnemonicDef.POP, RegisterDef.GR6);
            result[2] = ProgramLine.Generate(null, MnemonicDef.POP, RegisterDef.GR5);
            result[3] = ProgramLine.Generate(null, MnemonicDef.POP, RegisterDef.GR4);
            result[4] = ProgramLine.Generate(null, MnemonicDef.POP, RegisterDef.GR3);
            result[5] = ProgramLine.Generate(null, MnemonicDef.POP, RegisterDef.GR2);
            result[6] = ProgramLine.Generate(null, MnemonicDef.POP, RegisterDef.GR1);

            return result;
        }
    }
}
