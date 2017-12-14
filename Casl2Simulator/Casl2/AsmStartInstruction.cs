using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の START です。
    /// </summary>
    internal class AsmStartInstruction : Instruction
    {
        #region Fields
        private Label m_execStartAddress;
        #endregion

        internal AsmStartInstruction()
            : base(MnemonicDef.START)
        {
            //
        }

        internal override Boolean IsStart()
        {
            return true;
        }

        internal Label ExecStartAddress
        {
            get { return m_execStartAddress; }
        }

        /// <summary>
        /// START 命令のオペランドを解釈します。記述の形式は "[実行開始番地]" です。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        protected override void ParseSpecificOperand(OperandLexer lexer)
        {
            Token token = lexer.CurrentToken;

            if (token.Type == TokenType.EndOfToken)
            {
                m_execStartAddress = null;
            }
            else if (token.Type == TokenType.Label)
            {
                lexer.MoveNext();
                m_execStartAddress = new Label(token.StrValue);
            }
            // 解釈しなかった残りの字句要素は、Instruction.DoParseOperand() で取り扱う。
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_ExecStartAddr; }
        }

        internal override Int32 GetCodeWordCount()
        {
            return 0;
        }

        internal override void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            // TODO: 実装する。
            throw new NotImplementedException();

            // 開始アドレス
            // Exports: 外部モジュールから参照できるアドレス。
        }

        protected override String OperandString()
        {
            if (m_execStartAddress == null)
            {
                return String.Empty;
            }
            else
            {
                return m_execStartAddress.ToString();
            }
        }
    }
}
