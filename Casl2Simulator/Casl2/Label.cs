using System;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II アセンブラ言語のラベルを取り扱います。
    /// </summary>
    internal class Label
    {
        /// <summary>
        /// アセンブラ行のラベルを解釈します。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈したラベルの文字列を返します。
        /// ラベルが指定されていない場合は <see langword="null"/> を返します。
        /// </returns>
        internal static String ParseLine(ReadBuffer buffer)
        {
            String label = buffer.ReadNoneSpace();

            if (label.Length == 0)
            {
                return null;
            }
            else
            {
                CheckLabel(label);
                return label;
            }
        }

        /// <summary>
        /// オペランドのラベルを解釈します。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>解釈したラベルの文字列を返します。</returns>
        internal static String ParseOperand(ReadBuffer buffer)
        {
            String label = buffer.ReadWhile((c) => !Operand.EndOfItem(c));
            CheckLabel(label);
            return label;
        }

        private static void CheckLabel(String label)
        {
            CheckLength(label);
            CheckFirstChar(label);
            CheckSubsequentChars(label);
            CheckNotReservedWord(label);
        }

        private static void CheckLength(String label)
        {
            // 長さは 1 ~ 8 文字。
            Int32 length = label.Length;
            if (length < 1)
            {
                throw new Casl2SimulatorException(Resources.MSG_NoLabel);
            }
            else if (8 < length)
            {
                String message = String.Format(Resources.MSG_LabelLengthMustBe1Thru8, label, length);
                throw new Casl2SimulatorException(message);
            }
        }

        private static void CheckFirstChar(String label)
        {
            // 先頭の文字は英大文字でなければならない。
            // CheckLength で、長さは 1 ~ 8 と確認している。
            Char firstChar = label[0];
            if (!IsLabelFirstChar(firstChar))
            {
                String message = String.Format(Resources.MSG_LabelFirstCharIsNotUppercase, label, firstChar);
                throw new Casl2SimulatorException(message);
            }
        }

        private static void CheckSubsequentChars(String label)
        {
            // 以降の文字は、英大文字または数字のいずれでもよい。
            for (Int32 index = 1; index < label.Length; ++index)
            {
                Char subsequentChar = label[index];
                if (!CharUtils.IsHankakuUpper(subsequentChar) && !CharUtils.IsHankakuDigit(subsequentChar))
                {
                    String message = String.Format(
                        Resources.MSG_LabelSubsequentCharIsNeitherUppercaseNorDigit,
                        label, subsequentChar);
                    throw new Casl2SimulatorException(message);
                }
            }
        }

        private static void CheckNotReservedWord(String label)
        {
            // なお、予約語である GR0 ~ GR7 は、使用できない。
            if (ReservedWord.IsReserved(label))
            {
                String reservedWordList = ReservedWord.GetList();
                String message = String.Format(Resources.MSG_LabelIsReservedWord, label, reservedWordList);
                throw new Casl2SimulatorException(message);
            }
        }

        internal static Boolean IsLabelFirstChar(Char firstChar)
        {
            // 先頭の文字は英大文字。
            return CharUtils.IsHankakuUpper(firstChar);
        }
    }
}
