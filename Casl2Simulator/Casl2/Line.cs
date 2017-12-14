using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II アセンブラ言語の行を表わします。
    /// </summary>
    internal class Line
    {
        /// <summary>
        /// CASL II アセンブラ言語の行を解釈します。
        /// </summary>
        /// <param name="text">解釈する行の文字列です。</param>
        /// <returns>解釈した結果として生成した <see cref="Line"/> のオブジェクトを返します。</returns>
        internal static Line Parse(String text)
        {
            try
            {
                return DoParse(text);
            }
            catch (Casl2SimulatorException ex)
            {
                String errorMessage = ExceptionUtils.BuildErrorMessage(ex);
                return MakeErrorLine(text, errorMessage);
            }
        }

        private static Line DoParse(String text)
        {
            // CASL II の行は、注釈行か命令行のどちらか。
            if (DecideCommentLine(text))
            {
                return MakeCommentLine(text);
            }
            else
            {
                return ParseInstructionLine(text);
            }
        }

        // 注釈行
        //   [空白] {;} [コメント]
        private static Boolean DecideCommentLine(String text)
        {
            // 空白に続いて ';' ならば注釈行。
            ReadBuffer buffer = new ReadBuffer(text);
            buffer.SkipSpace();
            return buffer.Current == Casl2Defs.Semicolon;
        }

        // 命令行
        //   オペランドあり: [ラベル] {空白} {命令コード} {空白} {オペランド} [ {空白} [コメント]]
        //   オペランドなし: [ラベル] {空白} {命令コード} [ {空白} [ {;} [コメント]]]
        private static Line ParseInstructionLine(String text)
        {
            ReadBuffer buffer = new ReadBuffer(text);

            Label label = ParseLabel(buffer);
            Instruction instruction = ParseInstruction(buffer);
            ParseOperand(instruction, buffer);

            return MakeInstructionLine(text, label, instruction);
        }

        private static Label ParseLabel(ReadBuffer buffer)
        {
            String labelField = ReadField(buffer);
            return Label.Parse(labelField);
        }

        private static Instruction ParseInstruction(ReadBuffer buffer)
        {
            buffer.SkipSpace();
            String instructionField = ReadField(buffer);
            return Instruction.Parse(instructionField);
        }

        private static void ParseOperand(Instruction instruction, ReadBuffer buffer)
        {
            buffer.SkipSpace();
            // ';' ならば、そのあとはコメントなので、オペランドとして解釈しない。
            if (buffer.Current == Casl2Defs.Semicolon)
            {
                buffer.SkipToEnd();
            }

            instruction.ParseOperand(buffer);
        }

        private static String ReadField(ReadBuffer buffer)
        {
            return buffer.ReadWhile((c) => !EndOfField(c));
        }

        internal static Boolean EndOfField(Char current)
        {
            return Char.IsWhiteSpace(current) || current == ReadBuffer.EndOfStr;
        }

        internal static String Generate(Label label, String opcode, params Object[] args)
        {
            String name = (label == null) ? String.Empty : label.Name;
            String operand = Operand.Join(args);
            return String.Format("{0}\t{1}\t{2}", name, opcode, operand);
        }

        private static Line MakeCommentLine(String text)
        {
            return new Line(text, null, null, null);
        }

        private static Line MakeInstructionLine(String text, Label label, Instruction instruction)
        {
            return new Line(text, label, instruction, null);
        }

        private static Line MakeErrorLine(String text, String errorMessage)
        {
            return new Line(text, null, null, errorMessage);
        }

        #region Fields
        // 行は、ラベル、オペランドを含む命令コード、およびエラーメッセージを持つ。
        private readonly String m_text;
        private readonly Label m_label;
        private readonly Instruction m_instruction;
        private readonly String m_errorMessage;
        #endregion

        private Line(String text, Label label, Instruction instruction, String errorMessage)
        {
            m_text = text;
            m_label = label;
            m_instruction = instruction;
            m_errorMessage = errorMessage;
        }

        internal String Text
        {
            get { return m_text; }
        }

        internal Label Label
        {
            get { return m_label; }
        }

        internal Instruction Instruction
        {
            get { return m_instruction; }
        }

        internal String ErrorMessage
        {
            get { return m_errorMessage; }
        }

        internal Boolean HasError()
        {
            return m_errorMessage != null;
        }

        internal Boolean IsStart()
        {
            return m_instruction != null && m_instruction.IsStart();
        }

        internal Boolean IsEnd()
        {
            return m_instruction != null && m_instruction.IsEnd();
        }

        internal IEnumerable<Line> ExpandMacro()
        {
            if (m_instruction == null)
            {
                yield return this;
            }
            else
            {
                String[] expandedText = m_instruction.ExpandMacro(m_label);
                if (expandedText == null)
                {
                    yield return this;
                }
                else
                {
                    foreach (String text in expandedText)
                    {
                        Line parsedLine = Parse(text);
                        yield return parsedLine;
                    }
                }
            }
        }

        // TODO: 実装する。
        //internal void RegisterLabel(LabelManager lblManager)
        //{
        //    if (m_label != null)
        //    {
        //        lblManager.RegisterLabel(m_label);
        //    }
        //}

        //internal  Line GenerateLiteralDc(LabelManager lblManager)
        //{
        //    if (m_instruction == null)
        //    {
        //        return null;
        //    }

        //    String generatedText = m_instruction.GenerateLiteralDc(lblManager);
        //    if (generatedText == null)
        //    {
        //        return null;
        //    }

        //    return Parse(generatedText);
        //}

        internal void GenerateCode(LabelManager lblManager, RelocatableModule relModule)
        {
            // このアセンブラ行にラベルがあれば登録する。
            if (m_label != null)
            {
                UInt16 offset = relModule.GetCurrentOffset();
                lblManager.Register(m_label, offset);
            }

            // このアセンブラ行に命令があれば、コードを生成する。
            if (m_instruction != null)
            {
                m_instruction.GenerateCode(m_label, lblManager, relModule);
            }
        }
    }
}
