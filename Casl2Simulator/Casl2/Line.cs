using System;
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
        /// <param name="str">解釈する行の文字列です。</param>
        /// <returns>解釈した結果として生成した <see cref="Line"/> のオブジェクトを返します。</returns>
        internal static Line Parse(String str)
        {
            try
            {
                return DoParse(str);
            }
            catch (Casl2SimulatorException ex)
            {
                String errorMessage = ExceptionUtils.BuildErrorMessage(ex);
                return MakeErrorLine(str, errorMessage);
            }
        }

        private static Line DoParse(String str)
        {
            // CASL II の行は、注釈行か命令行のどちらか。
            if (DecideCommentLine(str))
            {
                return MakeCommentLine(str);
            }
            else
            {
                return ParseInstructionLine(str);
            }
        }

        // 注釈行
        //   [空白] {;} [コメント]
        private static Boolean DecideCommentLine(String str)
        {
            // 空白に続いて ';' ならば注釈行。
            ReadBuffer buffer = new ReadBuffer(str);
            buffer.SkipSpace();
            return buffer.Current == Casl2Defs.Semicolon;
        }

        // 命令行
        //   オペランドあり: [ラベル] {空白} {命令コード} {空白} {オペランド} [ {空白} [コメント]]
        //   オペランドなし: [ラベル] {空白} {命令コード} [ {空白} [ {;} [コメント]]]
        private static Line ParseInstructionLine(String str)
        {
            ReadBuffer buffer = new ReadBuffer(str);

            Label label = ParseLabel(buffer);
            Instruction instruction = ParseInstruction(buffer);
            ParseOperand(instruction, buffer);

            return MakeInstructionLine(str, label, instruction);
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

        internal static String Generate(String label, String opcode, params Object[] args)
        {
            String operand = Operand.Join(args);
            return String.Format("{0}\t{1}\t{2}", label, opcode, operand);
        }

        private static Line MakeCommentLine(String str)
        {
            return new Line(str, null, null, null);
        }

        private static Line MakeInstructionLine(String str, Label label, Instruction instruction)
        {
            return new Line(str, label, instruction, null);
        }

        private static Line MakeErrorLine(String str, String errorMessage)
        {
            return new Line(str, null, null, errorMessage);
        }

        #region Fields
        // 行は、ラベル、オペランドを含む命令コード、およびエラーメッセージを持つ。
        private readonly String m_str;
        private readonly Label m_label;
        private readonly Instruction m_instruction;
        private readonly String m_errorMessage;
        #endregion

        private Line(String str, Label label, Instruction instruction, String errorMessage)
        {
            m_str = str;
            m_label = label;
            m_instruction = instruction;
            m_errorMessage = errorMessage;
        }

        internal String Str
        {
            get { return m_str; }
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
