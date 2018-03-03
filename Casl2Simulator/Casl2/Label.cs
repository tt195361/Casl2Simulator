using System;
using System.Linq;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II アセンブラ言語のラベルを取り扱います。
    /// </summary>
    internal class Label
    {
        #region Static Fields
        internal const Int32 MinLiteralLabelNumber = 1;
        internal const Int32 MaxLiteralLabelNumber = 9999;
        internal const String LiteralLabelPrefix = "LTRL";
        private const String LiteralLabelFormat = LiteralLabelPrefix + "{0:d04}";

        private const Int32 MinLength = 1;
        private const Int32 MaxLength = 8;
        #endregion

        /// <summary>
        /// アセンブラ行のラベルフィールドのラベルを解釈します。
        /// </summary>
        /// <param name="labelField">解釈する文字列です。</param>
        /// <returns>
        /// 解釈した文字列から生成した <see cref="Label"/> のオブジェクトを返します。
        /// ラベルが指定されていない場合は <see langword="null"/> を返します。
        /// </returns>
        internal static Label Parse(String labelField)
        {
            if (labelField.Length == 0)
            {
                return null;
            }
            else
            {
                return new Label(labelField);
            }
        }

        /// <summary>
        /// オペランドのラベルを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="Label"/> オブジェクトを返します。
        /// </returns>
        internal static Label Parse(OperandLexer lexer)
        {
            Token token = lexer.ReadCurrentAs(TokenType.Label);
            return new Label(token.StrValue);
        }

        /// <summary>
        /// 指定の文字がラベルの最初の文字かどうかを判断します。
        /// </summary>
        /// <param name="firstChar">ラベルの最初かどうかを判断する対象の文字です。</param>
        /// <returns>
        /// 指定の文字がラベルの最初の文字なら <see langword="true"/> を、
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsStart(Char firstChar)
        {
            // 先頭の文字は英大文字。
            return CharUtils.IsHankakuUpper(firstChar);
        }

        /// <summary>
        /// 指定の番号を用いてリテラルで使用するラベルの名前を作成します。
        /// </summary>
        /// <param name="number">ラベル名の一部として使用する番号です。</param>
        /// <returns>作成したラベルの名前を返します。</returns>
        internal static String MakeLiteralLabelName(Int32 number)
        {
            ArgChecker.CheckRange(number, MinLiteralLabelNumber, MaxLiteralLabelNumber, nameof(number));
            return String.Format(LiteralLabelFormat, number);
        }

        #region Instance Fields
        private readonly String m_name;
        #endregion

        internal Label(String name)
        {
            CheckName(name);
            m_name = name;
        }

        internal String Name
        {
            get { return m_name; }
        }

        private void CheckName(String name)
        {
            CheckLength(name);
            CheckFirstChar(name);
            CheckSubsequentChars(name);
            CheckNotReservedWord(name);
        }

        private void CheckLength(String name)
        {
            // 長さは 1 ~ 8 文字。
            Int32 length = name.Length;
            if (length < MinLength)
            {
                throw new Casl2SimulatorException(Resources.MSG_NoLabel);
            }
            else if (MaxLength < length)
            {
                String message = String.Format(
                    Resources.MSG_LabelLengthOutOfRange, name, length, MinLength, MaxLength);
                throw new Casl2SimulatorException(message);
            }
        }

        private void CheckFirstChar(String name)
        {
            // 先頭の文字は英大文字でなければならない。
            // CheckLength で、長さは 1 ~ 8 と確認している。
            Char firstChar = name[0];
            if (!IsStart(firstChar))
            {
                String message = String.Format(Resources.MSG_LabelFirstCharIsNotUppercase, name, firstChar);
                throw new Casl2SimulatorException(message);
            }
        }

        private void CheckSubsequentChars(String name)
        {
            // 以降の文字は、英大文字または数字のいずれでもよい。
            name.Skip(1)
                .ForEach((subsequentChar) => CheckSubsequentChar(name, subsequentChar));
        }

        private void CheckSubsequentChar(String name, Char subsequentChar)
        {
            // 以降の文字は、英大文字または数字のいずれでもよい。
            if (!CharUtils.IsHankakuUpper(subsequentChar) && !CharUtils.IsHankakuDigit(subsequentChar))
            {
                String message = String.Format(
                    Resources.MSG_LabelSubsequentCharIsNeitherUppercaseNorDigit,
                    name, subsequentChar);
                throw new Casl2SimulatorException(message);
            }
        }

        private void CheckNotReservedWord(String name)
        {
            // なお、予約語である GR0 ~ GR7 は、使用できない。
            if (ReservedWord.IsReserved(name))
            {
                String reservedWordList = ReservedWord.GetList();
                String message = String.Format(Resources.MSG_LabelIsReservedWord, name, reservedWordList);
                throw new Casl2SimulatorException(message);
            }
        }

        public override String ToString()
        {
            return m_name;
        }
    }
}
