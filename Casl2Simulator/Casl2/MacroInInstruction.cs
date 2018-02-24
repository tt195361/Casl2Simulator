using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// マクロ命令の IN です。
    /// IN 命令は、あらかじめ割り当てた入力装置から、1 レコードの文字データを読み込みます。
    /// </summary>
    internal class MacroInInstruction : ProgramInstruction
    {
        #region Fields
        private AreaSpec m_inputArea;
        #endregion

        internal MacroInInstruction()
            : base(MnemonicDef.IN)
        {
            //
        }

        internal AreaSpec InputArea
        {
            get { return m_inputArea; }
        }

        /// <summary>
        /// IN 命令のオペランドを解釈します。記述の形式は "入力領域,入力文字長領域" です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="Operand"/> クラスのオブジェクトを返します。
        /// </returns>
        protected override Operand ParseSpecificOperand(OperandLexer lexer)
        {
            m_inputArea = AreaSpec.Parse(lexer);
            return m_inputArea;
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_InputArea; }
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
            result[2] = Line.Generate(null, MnemonicDef.LAD, RegisterDef.GR1, InputArea.Buffer.Name);
            result[3] = Line.Generate(null, MnemonicDef.LAD, RegisterDef.GR2, InputArea.Length.Name);
            result[4] = Line.Generate(null, MnemonicDef.SVC, 1);
            result[5] = Line.Generate(null, MnemonicDef.POP, RegisterDef.GR2);
            result[6] = Line.Generate(null, MnemonicDef.POP, RegisterDef.GR1);

            return result;
        }

        internal void SetLabelsForUnitTest(String bufferName, String lengthName)
        {
            m_inputArea = AreaSpec.MakeForUnitTest(bufferName, lengthName);
        }
    }
}
