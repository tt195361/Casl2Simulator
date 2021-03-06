﻿using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II アセンブラ言語プログラムの行を表わします。
    /// </summary>
    internal class ProgramLine
    {
        /// <summary>
        /// CASL II アセンブラ言語プログラムの行を解釈します。
        /// </summary>
        /// <param name="text">解釈する行の文字列です。</param>
        /// <returns>解釈した結果として生成した <see cref="ProgramLine"/> のオブジェクトを返します。</returns>
        internal static ProgramLine Parse(String text)
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

        private static ProgramLine DoParse(String text)
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
        private static ProgramLine ParseInstructionLine(String text)
        {
            ReadBuffer buffer = new ReadBuffer(text);

            Label label = ParseLabel(buffer);
            ProgramInstruction instruction = ParseInstruction(buffer);
            ReadOperand(instruction, buffer);

            return MakeInstructionLine(text, label, instruction);
        }

        private static Label ParseLabel(ReadBuffer buffer)
        {
            String labelField = ReadField(buffer);
            return Label.Parse(labelField);
        }

        private static ProgramInstruction ParseInstruction(ReadBuffer buffer)
        {
            buffer.SkipSpace();
            String instructionField = ReadField(buffer);
            return ProgramInstruction.Parse(instructionField);
        }

        private static void ReadOperand(ProgramInstruction instruction, ReadBuffer buffer)
        {
            buffer.SkipSpace();
            // ';' ならば、そのあとはコメントなので、オペランドとして解釈しない。
            if (buffer.Current == Casl2Defs.Semicolon)
            {
                buffer.SkipToEnd();
            }

            instruction.ReadOperand(buffer);
        }

        private static String ReadField(ReadBuffer buffer)
        {
            return buffer.ReadWhile((c) => !EndOfField(c));
        }

        internal static Boolean EndOfField(Char current)
        {
            return Char.IsWhiteSpace(current) || current == ReadBuffer.EndOfStr;
        }

        internal static String Generate(Label label, String mnemonic, params Object[] args)
        {
            String labelName = (label == null) ? String.Empty : label.Name;
            String operand = Operand.Join(args);
            String[] fields = { labelName, mnemonic, operand };
            return fields.MakeList("\t");
        }

        private static ProgramLine MakeCommentLine(String text)
        {
            return new ProgramLine(text, null, NullInstruction.Instance, null);
        }

        private static ProgramLine MakeInstructionLine(String text, Label label, ProgramInstruction instruction)
        {
            return new ProgramLine(text, label, instruction, null);
        }

        private static ProgramLine MakeErrorLine(String text, String errorMessage)
        {
            return new ProgramLine(text, null, NullInstruction.Instance, errorMessage);
        }

        #region Instance Fields
        // アセンブラプログラムの行は、ラベル、オペランドを含む命令コード、およびエラーメッセージを持つ。
        private readonly String m_text;
        private readonly Label m_label;
        private readonly ProgramInstruction m_instruction;
        private readonly String m_errorMessage;
        #endregion

        private ProgramLine(String text, Label label, ProgramInstruction instruction, String errorMessage)
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

        internal ProgramInstruction Instruction
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
            return m_instruction.IsStart();
        }

        internal Boolean IsEnd()
        {
            return m_instruction.IsEnd();
        }

        internal IEnumerable<ProgramLine> ExpandMacro()
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
                    ProgramLine parsedLine = Parse(text);
                    yield return parsedLine;
                }
            }
        }

        internal void RegisterLabel(LabelTable lblTable)
        {
            if (m_label != null)
            {
                lblTable.Register(m_label);
            }
        }

        internal ProgramLine GenerateLiteralDc(LabelTable lblTable)
        {
            String generatedText = m_instruction.GenerateLiteralDc(lblTable);
            if (generatedText == null)
            {
                return null;
            }
            else
            {
                return Parse(generatedText);
            }
        }

        internal void SetLabelOffset(LabelTable lblTable, MemoryOffset offset)
        {
            if (m_label != null)
            {
                LabelDefinition labelDef = lblTable.GetDefinitionFor(m_label);
                labelDef.SetRelOffset(offset);
            }
        }

        internal Int32 GetCodeWordCount()
        {
            return m_instruction.GetCodeWordCount();
        }

        internal void GenerateCode(RelocatableModule relModule)
        {
            m_instruction.GenerateCode(m_label, relModule);
        }

        public override String ToString()
        {
            return m_text;
        }
    }
}
