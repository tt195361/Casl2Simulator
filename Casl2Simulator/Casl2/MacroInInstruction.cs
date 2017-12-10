using System;
using Tt195361.Casl2Simulator.Common;
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
        private Label m_inputBufferArea;
        private Label m_inputLengthArea;
        #endregion

        internal MacroInInstruction()
            : base(MnemonicDef.IN)
        {
            //
        }

        internal Label InputBufferArea
        {
            get { return m_inputBufferArea; }
        }

        internal Label InputLengthArea
        {
            get { return m_inputLengthArea; }
        }

        /// <summary>
        /// IN 命令のオペランドを解釈します。記述の形式は "入力領域,入力文字長領域" です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        protected override void ParseSpecificOperand(OperandLexer lexer)
        {
            m_inputBufferArea = lexer.ReadCurrentAsLabel();
            lexer.SkipComma();
            m_inputLengthArea = lexer.ReadCurrentAsLabel();
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_InputAreaLengthArea; }
        }

        internal override String[] ExpandMacro(Label label)
        {
            String[] result = new String[7];

            // LABEL    PUSH    0,GR1
            //          PUSH    0,GR2
            //          LAD     GR1,IBUF
            //          LAD     GR2,LEN
            //          SVC     1
            //          POP     GR2
            //          POP     GR1
            result[0] = Line.Generate(label, MnemonicDef.PUSH, 0, RegisterDef.GR1);
            result[1] = Line.Generate(null, MnemonicDef.PUSH, 0, RegisterDef.GR2);
            result[2] = Line.Generate(null, MnemonicDef.LAD, RegisterDef.GR1, m_inputBufferArea.Name);
            result[3] = Line.Generate(null, MnemonicDef.LAD, RegisterDef.GR2, m_inputLengthArea.Name);
            result[4] = Line.Generate(null, MnemonicDef.SVC, 1);
            result[5] = Line.Generate(null, MnemonicDef.POP, RegisterDef.GR2);
            result[6] = Line.Generate(null, MnemonicDef.POP, RegisterDef.GR1);

            return result;
        }

        protected override String OperandString()
        {
            return String.Format("{0}{1}{2}", m_inputBufferArea, Casl2Defs.Comma, m_inputLengthArea);
        }

        internal void SetLabelsForUnitTest(String inputBufferArea, String inputLengthArea)
        {
            m_inputBufferArea = new Label(inputBufferArea);
            m_inputLengthArea = new Label(inputLengthArea);
        }
    }
}
