using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II アセンブラ言語の命令を表わす抽象クラスです。
    /// </summary>
    internal abstract class Instruction
    {
        #region Static Fields
        internal const String START = "START";
        internal const String DC = "DC";

        internal const String IN = "IN";
        internal const String RPUSH = "RPUSH";

        internal const String LAD = "LAD";
        internal const String PUSH = "PUSH";
        internal const String POP = "POP";
        internal const String SVC = "SVC";

        private static readonly Dictionary<String, Func<Instruction>> m_factoryMethodDictionary;
        #endregion

        static Instruction()
        {
            m_factoryMethodDictionary = new Dictionary<String, Func<Instruction>>()
            {
                { START, () => new AsmStartInstruction() },
                { DC, () => new AsmDcInstruction() },

                { IN, () => new MacroInInstruction() },
                { RPUSH, () => new MacroRpushInstruction() },
            };
        }

        /// <summary>
        /// 命令コードの文字列を解釈します。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>解釈した結果として生成した <see cref="Instruction"/> クラスのオブジェクトを返します。</returns>
        internal static Instruction Parse(ReadBuffer buffer)
        {
            String instructionField = buffer.ReadNoneSpace();
            if (instructionField.Length == 0)
            {
                String message = Resources.MSG_NoInstructionInInstructionLine;
                throw new Casl2SimulatorException(message);
            }

            if (!m_factoryMethodDictionary.ContainsKey(instructionField))
            {
                String message = String.Format(Resources.MSG_InstructionNotDefined, instructionField);
                throw new Casl2SimulatorException(message);
            }

            Func<Instruction> factoryMethod = m_factoryMethodDictionary[instructionField];
            Instruction instruction = factoryMethod();
            return instruction;
        }

        #region Fields
        private readonly String m_name;
        #endregion

        protected Instruction(String name)
        {
            m_name = name;
        }

        internal String Name
        {
            get { return m_name; }
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
                String message = String.Format(Resources.MSG_OperandParseError, Name, OperandSyntax);
                throw new Casl2SimulatorException(message, ex);
            }
        }

        private void DoParseOperand(ReadBuffer buffer)
        {
            // ';' ならば、そのあとはコメントなので、オペランドとして解釈しない。
            if (buffer.Current == Casl2Defs.Semicolon)
            {
                buffer.SkipToEnd();
            }

            ParseSpecificOperand(buffer);

            // 解釈していない残りの文字列があるかチェックする。
            if (!Operand.EndOfField(buffer.Current))
            {
                String notParsedString = buffer.GetRest();
                String message = String.Format(
                    Resources.MSG_NotParsedStringRemainsInOperand, Name, notParsedString);
                throw new Casl2SimulatorException(message);
            }
        }

        /// <summary>
        /// それぞれの命令に応じてオペランドの内容を解釈します。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        protected abstract void ParseSpecificOperand(ReadBuffer buffer);

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
        internal virtual void GenerateCode(String label, LabelManager lblManager, RelocatableModule relModule)
        {
            // デフォルトは、コードを生成しない。
        }

        /// <summary>
        /// この命令を表わす文字列を作成します。
        /// </summary>
        /// <returns>この命令を表わす文字列を返します。</returns>
        public override String ToString()
        {
            return m_name;
        }
    }
}
