using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラ命令の START です。
    /// </summary>
    internal class AsmStartInstruction : Instruction
    {
        #region Fields
        private String m_execStartAddress;
        #endregion

        internal AsmStartInstruction()
            : base(Instruction.START)
        {
            //
        }

        internal String ExecStartAddress
        {
            get { return m_execStartAddress; }
        }

        /// <summary>
        /// START 命令のオペランドを解釈します。記述の形式は "[実行開始番地]" です。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        protected override void ParseSpecificOperand(ReadBuffer buffer)
        {
            if (Operand.EndOfField(buffer.Current))
            {
                m_execStartAddress = null;
            }
            else
            {
                m_execStartAddress = Label.ParseOperand(buffer);
            }
        }

        protected override String OperandSyntax
        {
            get { return Resources.SYN_ExecStartAddr; }
        }

        internal override Int32 GetCodeWordCount()
        {
            return 0;
        }

        internal override void GenerateCode(String label, LabelManager lblManager, RelocatableModule relModule)
        {
            // TODO: 実装する。
            throw new NotImplementedException();

            // 開始アドレス
            // Exports: 外部モジュールから参照できるアドレス。
        }
    }
}
