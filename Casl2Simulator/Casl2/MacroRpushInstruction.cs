﻿using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// マクロ命令の RPUSH です。
    /// RPUSH 命令は、GR の内容を、GR1, GR2, ..., GR7 の順序でスタックに格納します。
    /// </summary>
    internal class MacroRpushInstruction : ProgramInstruction
    {
        internal MacroRpushInstruction()
            : base(MnemonicDef.RPUSH)
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
            // LABEL    PUSH    0,GR1
            //          PUSH    0,GR2
            //          PUSH    0,GR3
            //          PUSH    0,GR4
            //          PUSH    0,GR5
            //          PUSH    0,GR6
            //          PUSH    0,GR7
            result[0] = ProgramLine.Generate(label, MnemonicDef.PUSH, 0, RegisterDef.GR1);
            result[1] = ProgramLine.Generate(null, MnemonicDef.PUSH, 0, RegisterDef.GR2);
            result[2] = ProgramLine.Generate(null, MnemonicDef.PUSH, 0, RegisterDef.GR3);
            result[3] = ProgramLine.Generate(null, MnemonicDef.PUSH, 0, RegisterDef.GR4);
            result[4] = ProgramLine.Generate(null, MnemonicDef.PUSH, 0, RegisterDef.GR5);
            result[5] = ProgramLine.Generate(null, MnemonicDef.PUSH, 0, RegisterDef.GR6);
            result[6] = ProgramLine.Generate(null, MnemonicDef.PUSH, 0, RegisterDef.GR7);

            return result;
        }
    }
}
