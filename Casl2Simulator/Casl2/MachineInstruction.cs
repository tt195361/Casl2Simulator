using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    using OperandParseFunc = Func<OperandLexer, MachineInstructionOperand>;

    /// <summary>
    /// 機械語命令を表わします。
    /// </summary>
    internal class MachineInstruction : Instruction
    {
        internal static MachineInstruction MakeRAdrXOrR1R2(
            String mnemonic, UInt16 opcodeRAdrX, UInt16 opcodeR1R2)
        {
            return new MachineInstruction(
                mnemonic, Resources.SYN_RAdrXOrR1R2,
                (lexer) => RAdrXOrR1R2Operand.Parse(lexer, opcodeRAdrX, opcodeR1R2));
        }

        internal static MachineInstruction MakeAdrX(String mnemonic, UInt16 opcode)
        {
            return new MachineInstruction(
                mnemonic, Resources.SYN_AdrX, (lexer) => AdrXOperand.Parse(lexer, opcode));
        }

        #region Fields
        private readonly String m_operandSyntax;
        private readonly OperandParseFunc m_operandParseFunc;
        private MachineInstructionOperand m_operand;
        #endregion

        private MachineInstruction(String mnemonic, String operandSyntax, OperandParseFunc operandParseFunc)
            : base(mnemonic)
        {
            m_operandSyntax = operandSyntax;
            m_operandParseFunc = operandParseFunc;
        }

        protected override String OperandSyntax
        {
            get { return m_operandSyntax; }
        }

        protected override void ParseSpecificOperand(OperandLexer lexer)
        {
            m_operand = m_operandParseFunc(lexer);
        }

        internal override String GenerateLiteralDc(LabelManager lblManager)
        {
            return m_operand.GenerateLiteralDc(lblManager);
        }

        internal override Int32 GetCodeWordCount()
        {
            // オペコードで 1 ワード、オペランドに応じて追加。
            return 1 + m_operand.GetAdditionalWordCount();
        }

        internal override void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            Word firstWord = MakeFirstWord();
            relModule.AddWord(firstWord);

            Word? secondWord = m_operand.MakeSecondWord(lblManager);
            if (secondWord != null)
            {
                relModule.AddWord(secondWord.Value);
            }
        }

        private Word MakeFirstWord()
        {
            UInt16 opcode = m_operand.Opcode;
            UInt16 rr1 = m_operand.GetRR1();
            UInt16 xr2 = m_operand.GetXR2();
            return InstructionWord.MakeFirstWord(opcode, rr1, xr2);
        }

        protected override String OperandString()
        {
            return null;
        }
    }
}
