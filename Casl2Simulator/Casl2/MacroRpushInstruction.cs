﻿using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// マクロ命令の RPUSH です。
    /// RPUSH 命令は、GR の内容を、GR1, GR2, ..., GR7 の順序でスタックに格納します。
    /// </summary>
    internal class MacroRpushInstruction : Instruction
    {
        internal MacroRpushInstruction()
            : base(Casl2Defs.RPUSH)
        {
            //
        }

        protected override void ParseSpecificOperand(OperandLexer lexer)
        {
            // オペランドなし
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_NoOperand; }
        }

        internal override String[] ExpandMacro(String label)
        {
            String[] result = new String[7];
            // LABEL    PUSH    0,GR1
            //          PUSH    0,GR2
            //          PUSH    0,GR3
            //          PUSH    0,GR4
            //          PUSH    0,GR5
            //          PUSH    0,GR6
            //          PUSH    0,GR7
            result[0] = Line.Generate(label, Casl2Defs.PUSH, 0, Casl2Defs.GR1);
            result[1] = Line.Generate(null, Casl2Defs.PUSH, 0, Casl2Defs.GR2);
            result[2] = Line.Generate(null, Casl2Defs.PUSH, 0, Casl2Defs.GR3);
            result[3] = Line.Generate(null, Casl2Defs.PUSH, 0, Casl2Defs.GR4);
            result[4] = Line.Generate(null, Casl2Defs.PUSH, 0, Casl2Defs.GR5);
            result[5] = Line.Generate(null, Casl2Defs.PUSH, 0, Casl2Defs.GR6);
            result[6] = Line.Generate(null, Casl2Defs.PUSH, 0, Casl2Defs.GR7);

            return result;
        }
    }
}
