using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令を表わす抽象クラスです。
    /// </summary>
    internal abstract class MachineInstruction : Instruction
    {
        #region Fields
        private MachineInstructionOperand m_machineInstructionOperand;
        #endregion

        protected MachineInstruction(String code)
            : base(code)
        {
            //
        }

        protected MachineInstructionOperand MachineInstructionOperand
        {
            get { return m_machineInstructionOperand; }
        }

        protected override void ParseSpecificOperand(OperandLexer lexer)
        {
            m_machineInstructionOperand = DoParseSpecificOperand(lexer);
        }

        protected abstract MachineInstructionOperand DoParseSpecificOperand(OperandLexer lexer);

        internal override Int32 GetCodeWordCount()
        {
            // オペコードで 1 ワード、オペランドに応じて追加。
            return 1 + MachineInstructionOperand.GetAdditionalWordCount();
        }

        internal override void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            Word firstWord = MakeFirstWord();
            relModule.AddWord(firstWord);

            Word? secondWord = m_machineInstructionOperand.MakeSecondWord(lblManager);
            if (secondWord != null)
            {
                relModule.AddWord(secondWord.Value);
            }
        }

        private Word MakeFirstWord()
        {
            UInt16 opcode = GetOpcode();
            UInt16 rr1 = MachineInstructionOperand.GetRR1();
            UInt16 xr2 = MachineInstructionOperand.GetXR2();
            return InstructionWord.MakeFirstWord(opcode, rr1, xr2);
        }

        protected abstract UInt16 GetOpcode();

        protected override String OperandString()
        {
            return null;
        }
    }
}
