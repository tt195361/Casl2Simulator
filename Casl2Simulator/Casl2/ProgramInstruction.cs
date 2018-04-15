using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II アセンブラ言語のプログラムに記述する命令を表わす抽象クラスです。
    /// </summary>
    internal abstract class ProgramInstruction
    {
        /// <summary>
        /// 命令コードの文字列を解釈します。
        /// </summary>
        /// <param name="instructionField">解釈する文字列です。</param>
        /// <returns>
        /// 解釈した文字列から生成した <see cref="ProgramInstruction"/> クラスのオブジェクトを返します。
        /// </returns>
        internal static ProgramInstruction Parse(String instructionField)
        {
            return ProgramInstructionFactory.Make(instructionField);
        }

        #region Instance Fields
        private readonly String m_mnemonic;
        private Operand m_operand;
        #endregion

        protected ProgramInstruction(String mnemonic)
        {
            m_mnemonic = mnemonic;
        }

        internal String Mnemonic
        {
            get { return m_mnemonic; }
        }

        internal virtual Boolean IsNull()
        {
            return false;
        }

        internal virtual Boolean IsStart()
        {
            return false;
        }

        internal virtual Boolean IsEnd()
        {
            return false;
        }

        /// <summary>
        /// オペランドの文字列を読み込み処理します。オペランドは命令ごとに記述の形式が定義されています。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        internal void ReadOperand(ReadBuffer buffer)
        {
            try
            {
                m_operand = ParseOperand(buffer);
            }
            catch (Casl2SimulatorException ex)
            {
                String message = String.Format(Resources.MSG_OperandParseError, Mnemonic, OperandSyntax);
                throw new Casl2SimulatorException(message, ex);
            }
        }

        private Operand ParseOperand(ReadBuffer buffer)
        {
            OperandLexer lexer = new OperandLexer(buffer);
            String strToParse = lexer.Remaining;
            lexer.MoveNext();
            Operand operand = ParseSpecificOperand(lexer);

            // 解釈していない残りの字句要素があるかチェックする。
            Token token = lexer.CurrentToken;
            if (token.Type != TokenType.EndOfToken)
            {
                String message = String.Format(
                    Resources.MSG_NotParsedStringRemainsInOperand, Mnemonic, strToParse, lexer.Remaining);
                throw new Casl2SimulatorException(message);
            }

            return operand;
        }

        /// <summary>
        /// それぞれの命令に応じてオペランドの内容を解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="Operand"/> クラスのオブジェクトを返します。
        /// </returns>
        protected abstract Operand ParseSpecificOperand(OperandLexer lexer);

        /// <summary>
        /// オペランドの文法を説明する文字列を取得します。
        /// </summary>
        protected abstract String OperandSyntax { get; }

        /// <summary>
        /// マクロの内容を展開します。
        /// </summary>
        /// <param name="label">この命令行に定義されたラベルです。</param>
        /// <returns>
        /// マクロの内容を展開した文字列の配列を返します。
        /// <see langword="null"/> が返された場合は、もとの行をそのまま使います。
        /// </returns>
        internal virtual String[] ExpandMacro(Label label)
        {
            // デフォルトでは、マクロ展開しない。
            return null;
        }

        /// <summary>
        /// リテラルのための DC 命令を生成します。
        /// </summary>
        /// <param name="lblManager">
        /// ラベルを管理する <see cref="LabelManager"/> のオブジェクトです。
        /// </param>
        /// <returns>
        /// 生成した DC 命令の命令行を返します。
        /// DC 命令を生成しない場合は <see langword="null"/> を返します。
        /// </returns>
        internal virtual String GenerateLiteralDc(LabelManager lblManager)
        {
            // デフォルトでは、リテラルを生成しない。
            return null;
        }

        /// <summary>
        /// この命令が生成するコードのワード数を取得します。
        /// </summary>
        /// <returns>生成するコードのワード数を返します。</returns>
        internal virtual Int32 GetCodeWordCount()
        {
            // デフォルトは、コードを生成しない。
            return 0;
        }

        /// <summary>
        /// この命令のコードを生成します。
        /// </summary>
        /// <param name="definedLabel">
        /// この命令行に定義されたラベルです。
        /// ラベルが定義されていない場合は、<see langword="null"/> を渡します。
        /// </param>
        /// <param name="relModule">
        /// 生成したコードを格納する <see cref="RelocatableModule"/> のオブジェクトです。
        /// </param>
        internal virtual void GenerateCode(Label definedLabel, RelocatableModule relModule)
        {
            // デフォルトは、コードを生成しない。
        }

        /// <summary>
        /// この命令を表わす文字列を作成します。
        /// </summary>
        /// <returns>この命令を表わす文字列を返します。</returns>
        public override String ToString()
        {
            String operandString = (m_operand == null) ? null : m_operand.ToString();
            if (String.IsNullOrEmpty(operandString))
            {
                return m_mnemonic;
            }
            else
            {
                return m_mnemonic + " " + operandString;
            }
        }
    }
}
