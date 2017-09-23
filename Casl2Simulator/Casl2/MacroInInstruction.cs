using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// マクロ命令の IN です。
    /// IN 命令は、あらかじめ割り当てた入力装置から、1 レコードの文字データを読み込みます。
    /// </summary>
    internal class MacroInInstruction : Instruction
    {
        #region Fields
        private String m_inputBufferArea;
        private String m_inputLengthArea;
        #endregion

        internal MacroInInstruction()
            : base(Instruction.IN)
        {
            //
        }

        internal String InputBufferArea
        {
            get { return m_inputBufferArea; }
        }

        internal String InputLengthArea
        {
            get { return m_inputLengthArea; }
        }

        /// <summary>
        /// IN 命令のオペランドを解釈します。記述の形式は "入力領域,入力文字長領域" です。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        protected override void ParseSpecificOperand(ReadBuffer buffer)
        {
            m_inputBufferArea = Label.ParseOperand(buffer);
            buffer.SkipExpected(Casl2Defs.Comma);
            m_inputLengthArea = Label.ParseOperand(buffer);
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_InputAreaLengthArea; }
        }

        internal override String[] ExpandMacro(String label)
        {
            String[] result = new String[7];

            // LABEL    PUSH    0,GR1
            //          PUSH    0,GR2
            //          LAD     GR1,IBUF
            //          LAD     GR2,LEN
            //          SVC     1
            //          POP     GR2
            //          POP     GR1
            result[0] = Line.Generate(label, Instruction.PUSH, 0, Operand.GR1);
            result[1] = Line.Generate(null, Instruction.PUSH, 0, Operand.GR2);
            result[2] = Line.Generate(null, Instruction.LAD, Operand.GR1, m_inputBufferArea);
            result[3] = Line.Generate(null, Instruction.LAD, Operand.GR2, m_inputLengthArea);
            result[4] = Line.Generate(null, Instruction.SVC, 1);
            result[5] = Line.Generate(null, Instruction.POP, Operand.GR2);
            result[6] = Line.Generate(null, Instruction.POP, Operand.GR1);

            return result;
        }

        internal void SetLabelsForUnitTest(String inputBufferArea, String inputLengthArea)
        {
            m_inputBufferArea = inputBufferArea;
            m_inputLengthArea = inputLengthArea;
        }
    }
}
