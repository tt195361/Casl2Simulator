using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// コメント行などの命令がない行のための命令を表わします。
    /// </summary>
    internal class NullInstruction : Instruction
    {
        private static NullInstruction m_instance = new NullInstruction();

        internal static NullInstruction Instance
        {
            get { return m_instance; }
        }

        private NullInstruction()
            : base(MnemonicDef.NULL)
        {
            //
        }

        internal override Boolean IsNull()
        {
            return true;
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_NoOperand; }
        }

        protected override String OperandString()
        {
            return String.Empty;
        }

        protected override void ParseSpecificOperand(OperandLexer lexer)
        {
            // オペランドなし
        }
    }
}
