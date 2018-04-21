using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// マクロの IN 命令と OUT 命令です。
    /// IN 命令は、あらかじめ割り当てた入力装置から、1 レコードの文字データを読み込みます。
    /// OUT 命令は、あらかじめ割り当てた出力装置に、文字データを、1 レコードとして書き出します。
    /// </summary>
    internal class MacroInOutInstruction : ProgramInstruction
    {
        internal static MacroInOutInstruction MakeIn()
        {
            return new MacroInOutInstruction(MnemonicDef.IN, SvcDef.InOperand, Resources.SYN_InputArea);
        }

        internal static MacroInOutInstruction MakeOut()
        {
            return new MacroInOutInstruction(MnemonicDef.OUT, SvcDef.OutOperand, Resources.SYN_OutputArea);
        }

        #region Instance Fields
        private readonly Int32 m_svcOperand;
        private readonly String m_operandSyntax;
        private AreaSpec m_areaSpec;
        #endregion

        private MacroInOutInstruction(String mnemonic, Int32 svcOperand, String operandSyntax)
            : base(mnemonic)
        {
            m_svcOperand = svcOperand;
            m_operandSyntax = operandSyntax;
        }

        internal AreaSpec AreaSpec
        {
            get { return m_areaSpec; }
        }

        /// <summary>
        /// IN/OUT 命令のオペランドを解釈します。記述の形式は "入出力領域,入出力文字長領域" です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="Operand"/> クラスのオブジェクトを返します。
        /// </returns>
        protected override Operand ParseSpecificOperand(OperandLexer lexer)
        {
            m_areaSpec = AreaSpec.Parse(lexer);
            return m_areaSpec;
        }

        protected override String OperandSyntax
        {
            get { return m_operandSyntax; }
        }

        internal override String[] ExpandMacro(Label label)
        {
            String[] result = new String[7];

            // LABEL    PUSH    0,GR1
            //          PUSH    0,GR2
            //          LAD     GR1,BUF
            //          LAD     GR2,LEN
            //          SVC     1 or 2
            //          POP     GR2
            //          POP     GR1
            result[0] = ProgramLine.Generate(label, MnemonicDef.PUSH, 0, SvcDef.BufferAddrRegister);
            result[1] = ProgramLine.Generate(null, MnemonicDef.PUSH, 0, SvcDef.LengthRegister);
            result[2] = ProgramLine.Generate(null, MnemonicDef.LAD, SvcDef.BufferAddrRegister, AreaSpec.Buffer.Name);
            result[3] = ProgramLine.Generate(null, MnemonicDef.LAD, SvcDef.LengthRegister, AreaSpec.Length.Name);
            result[4] = ProgramLine.Generate(null, MnemonicDef.SVC, m_svcOperand);
            result[5] = ProgramLine.Generate(null, MnemonicDef.POP, SvcDef.LengthRegister);
            result[6] = ProgramLine.Generate(null, MnemonicDef.POP, SvcDef.BufferAddrRegister);

            return result;
        }

        internal void SetLabelsForUnitTest(String bufferName, String lengthName)
        {
            m_areaSpec = AreaSpec.MakeForUnitTest(bufferName, lengthName);
        }
    }
}
