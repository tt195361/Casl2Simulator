using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    using OperandParseFunc = Func<OperandLexer, MachineInstructionOperand>;

    /// <summary>
    /// 機械語命令を表わします。
    /// </summary>
    internal class MachineInstruction : ProgramInstruction
    {
        /// <summary>
        /// オペランドが r,adr,x あるいは r1,r2 のどちらかの機械語命令を作成します。
        /// </summary>
        /// <param name="mnemonic">命令のニーモニックを表わす文字列です。</param>
        /// <param name="opcodeRAdrX">オペランドが r,adr,x の場合のオペコードの値です。</param>
        /// <param name="opcodeR1R2">オペランドが r1,r2 の場合のオペコードの値です。</param>
        /// <returns></returns>
        internal static MachineInstruction MakeRAdrXOrR1R2(
            String mnemonic, UInt16 opcodeRAdrX, UInt16 opcodeR1R2)
        {
            return new MachineInstruction(
                mnemonic, Resources.SYN_RAdrXOrR1R2,
                (lexer) => RAdrXOrR1R2Operand.Parse(lexer, opcodeRAdrX, opcodeR1R2));
        }

        /// <summary>
        /// オペランドが r,adr,x の機械語命令を作成します。
        /// </summary>
        /// <param name="mnemonic">命令のニーモニックを表わす文字列です。</param>
        /// <param name="opcode">この命令のオペコードの値です。</param>
        /// <returns></returns>
        internal static MachineInstruction MakeRAdrX(String mnemonic, UInt16 opcode)
        {
            return new MachineInstruction(
                mnemonic, Resources.SYN_RAdrX, (lexer) => RAdrXOperand.Parse(lexer, opcode));
        }

        /// <summary>
        /// オペランドが adr,x の機械語命令を作成します。
        /// </summary>
        /// <param name="mnemonic">命令のニーモニックを表わす文字列です。</param>
        /// <param name="opcode">この命令のオペコードの値です。</param>
        /// <returns></returns>
        internal static MachineInstruction MakeAdrX(String mnemonic, UInt16 opcode)
        {
            return new MachineInstruction(
                mnemonic, Resources.SYN_AdrX, (lexer) => AdrXOperand.Parse(lexer, opcode));
        }

        /// <summary>
        /// オペランドが r の機械語命令を作成します。
        /// </summary>
        /// <param name="mnemonic">命令のニーモニックを表わす文字列です。</param>
        /// <param name="opcode">この命令のオペコードの値です。</param>
        /// <returns></returns>
        internal static MachineInstruction MakeR(String mnemonic, UInt16 opcode)
        {
            return new MachineInstruction(
                mnemonic, Resources.SYN_R, (lexer) => RegisterOperand.Parse(lexer, opcode));
        }

        /// <summary>
        /// オペランドがない機械語命令を作成します。
        /// </summary>
        /// <param name="mnemonic">命令のニーモニックを表わす文字列です。</param>
        /// <param name="opcode">この命令のオペコードの値です。</param>
        /// <returns></returns>
        internal static MachineInstruction MakeNoOperand(String mnemonic, UInt16 opcode)
        {
            return new MachineInstruction(
                mnemonic, Resources.SYN_NoOperand, (lexer) => NoOperand.Parse(lexer, opcode));
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

        protected override Operand ParseSpecificOperand(OperandLexer lexer)
        {
            m_operand = m_operandParseFunc(lexer);
            return m_operand;
        }

        internal override String GenerateLiteralDc(LabelManager lblManager)
        {
            return m_operand.GenerateLiteralDc(lblManager);
        }

        internal override Int32 GetCodeWordCount()
        {
            // オペコードで 1 ワード、オペランドに応じて追加。
            return 1 + m_operand.GetCodeWordCount();
        }

        internal override void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            Word firstWord = MakeFirstWord();
            relModule.AddWord(firstWord);

            m_operand.GenerateCode(lblManager, relModule);
        }

        private Word MakeFirstWord()
        {
            UInt16 opcode = m_operand.Opcode;
            UInt16 rr1 = m_operand.GetRR1();
            UInt16 xr2 = m_operand.GetXR2();
            return InstructionWord.MakeFirstWord(opcode, rr1, xr2);
        }
    }
}
