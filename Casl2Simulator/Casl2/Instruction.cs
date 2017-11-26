using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II アセンブラ言語の命令を表わす抽象クラスです。
    /// </summary>
    internal abstract class Instruction
    {
        /// <summary>
        /// 命令コードの文字列を解釈します。
        /// </summary>
        /// <param name="instructionField">解釈する文字列です。</param>
        /// <returns>
        /// 解釈した文字列から生成した <see cref="Instruction"/> クラスのオブジェクトを返します。
        /// </returns>
        internal static Instruction Parse(String instructionField)
        {
            return InstructionFactory.Make(instructionField);
        }

        #region Fields
        private readonly String m_code;
        #endregion

        protected Instruction(String code)
        {
            m_code = code;
        }

        internal String Code
        {
            get { return m_code; }
        }

        /// <summary>
        /// オペランドの文字列を解釈します。オペランドは命令ごとに記述の形式が定義されています。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        internal void ParseOperand(ReadBuffer buffer)
        {
            try
            {
                DoParseOperand(buffer);
            }
            catch (Casl2SimulatorException ex)
            {
                String message = String.Format(Resources.MSG_OperandParseError, Code, OperandSyntax);
                throw new Casl2SimulatorException(message, ex);
            }
        }

        private void DoParseOperand(ReadBuffer buffer)
        {
            OperandLexer lexer = new OperandLexer(buffer);
            lexer.MoveNext();
            ParseSpecificOperand(lexer);

            // 解釈していない残りの字句要素があるかチェックする。
            Token token = lexer.CurrentToken;
            if (token.Type != TokenType.EndOfToken)
            {
                String message = String.Format(Resources.MSG_NotParsedTokenRemainsInOperand, Code, token);
                throw new Casl2SimulatorException(message);
            }
        }

        /// <summary>
        /// それぞれの命令に応じてオペランドの内容を解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        protected abstract void ParseSpecificOperand(OperandLexer lexer);

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
        internal virtual String[] ExpandMacro(String label)
        {
            // デフォルトでは、マクロ展開しない。
            return null;
        }

        /// <summary>
        /// リテラルのための DC 命令を生成します。
        /// </summary>
        /// <param name="lblManager"></param>
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
        /// <param name="label">
        /// この命令行に定義されたラベルです。
        /// ラベルが定義されていない場合は、<see langword="null"/> を渡します。
        /// </param>
        /// <param name="lblManager">
        /// ラベルを管理する <see cref="LabelManager"/> のオブジェクトです。
        /// </param>
        /// <param name="relModule">
        /// 生成したコードを格納する <see cref="RelocatableModule"/> のオブジェクトです。
        /// </param>
        internal virtual void GenerateCode(Label label, LabelManager lblManager, RelocatableModule relModule)
        {
            // デフォルトは、コードを生成しない。
        }

        /// <summary>
        /// この命令を表わす文字列を作成します。
        /// </summary>
        /// <returns>この命令を表わす文字列を返します。</returns>
        public override String ToString()
        {
            return m_code;
        }

        protected abstract String OperandString();
    }
}
