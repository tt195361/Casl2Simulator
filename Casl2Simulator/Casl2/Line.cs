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
        /// <returns>解釈した結果として生成した <see cref="Casl2.Line"/> のオブジェクトを返します。</returns>
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
                return MakeInstructionLine(str);
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

        private static Line MakeCommentLine(String str)
        {
            return new Line(str);
        }

        // 命令行
        //   オペランドあり: [ラベル] {空白} {命令コード} {空白} {オペランド} [ {空白} [コメント]]
        //   オペランドなし: [ラベル] {空白} {命令コード} [ {空白} [ {;} [コメント]]]
        private static Line MakeInstructionLine(String str)
        {
            ReadBuffer buffer = new ReadBuffer(str);
            String label = Casl2.Label.ParseLine(buffer);

            buffer.SkipSpace();
            Instruction instruction = Instruction.Parse(buffer);

            buffer.SkipSpace();
            instruction.ParseOperand(buffer);

            return new Line(str, label, instruction);
        }

        private static Line MakeErrorLine(String str, String errorMessage)
        {
            return new Line(str, errorMessage);
        }

        internal static String Generate(String label, String opcode, params Object[] args)
        {
            String operand = Operand.Join(args);
            return String.Format("{0}\t{1}\t{2}", label, opcode, operand);
        }

        #region Fields
        // 行は、ラベル、オペランドを含む命令コード、およびエラーメッセージを持つ。
        private readonly String m_str;
        private readonly String m_label;
        private readonly Instruction m_instruction;
        private readonly String m_errorMessage;
        #endregion

        // コメント行
        private Line(String str)
            : this(str, null, null, null)
        {
            //
        }

        // エラーが発生した行
        private Line(String str, String errorMessage)
            : this(str, null, null, errorMessage)
        {
            //
        }

        // 命令行
        private Line(String str, String label, Instruction instruction)
            : this(str, label, instruction, null)
        {
            //
        }

        private Line(String str, String label, Instruction instruction, String errorMessage)
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

        internal String Label
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
    }
}
