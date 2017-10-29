﻿using System;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 16 進の数値定数です。
    /// </summary>
    internal class HexaDecimalConstant : Constant, IAdrValue
    {
        #region Fields
        private const Int32 MinValue = UInt16.MinValue;
        private const Int32 MaxValue = UInt16.MaxValue;

        private const Int32 DigitCount = 4;
        private const String PrintFormat = "{0}{1:X04}";
        #endregion

        /// <summary>
        /// 指定の文字が 16 進定数の最初の文字かどうかを判断します。
        /// </summary>
        /// <param name="firstChar">16 進定数の最初かどうかを判断する対象の文字です。</param>
        /// <returns>
        /// 指定の文字が 16 進定数の最初の文字なら <see langword="true"/> を、
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsStart(Char firstChar)
        {
            return firstChar == Casl2Defs.Sharp;
        }

        /// <summary>
        /// 16 進定数を読み込みます。16 進定数は #h の形式で記述し、h は 4 桁の 16 進数
        /// (16 新数字は 0 ~ 9, A ~ F) です。
        /// </summary>
        /// <param name="buffer">読み込む文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>読み込んだ <see cref="Int32"/> の値を返します。</returns>
        internal static Int32 Read(ReadBuffer buffer)
        {
            Int32 parseStartIndex = buffer.CurrentIndex;
            buffer.SkipExpected(Casl2Defs.Sharp);

            Int32 digitCount = 0;
            Int32 value = GetValue(buffer, out digitCount);
            if (digitCount != 4)
            {
                String hexStr = buffer.GetToCurrent(parseStartIndex);
                String message = String.Format(Resources.MSG_InvalidHexConstantDigitCount, hexStr, digitCount);
                throw new Casl2SimulatorException(message);
            }

            return value;
        }

        private static Int32 GetValue(ReadBuffer buffer, out Int32 digitCount)
        {
            const Int32 Base = 16;

            Int32 hexValue = 0;
            digitCount = 0;
            for ( ; ; )
            {
                Char c = buffer.Current;
                if (!CharUtils.IsHankakuHexDigit(c))
                {
                    return hexValue;
                }

                Int32 digit = CharUtils.ToDigit(c, Base);
                hexValue = hexValue * Base + digit;
                buffer.MoveNext();
                ++digitCount;
            }
        }

        #region Fields
        private readonly Int32 m_value;
        #endregion

        internal HexaDecimalConstant(Int32 value)
        {
            ArgChecker.CheckRange(value, MinValue, MaxValue, nameof(value));

            m_value = value;
        }

        internal Int32 Value
        {
            get { return m_value; }
        }

        internal override int GetWordCount()
        {
            return 1;
        }

        internal override void GenerateCode(LabelManager lblManager, RelocatableModule relModule)
        {
            // TODO: 実装する。
            throw new NotImplementedException();
        }

        String IAdrValue.GenerateDc(LabelManager lblManager)
        {
            throw new NotImplementedException();
        }

        UInt16 IAdrValue.GetAddress(LabelManager lblManager)
        {
            return NumberUtils.ToUInt16(m_value);
        }

        protected override String ValueToString()
        {
            return ValueToString(m_value);
        }

        internal static String ValueToString(Int32 value)
        {
            return String.Format(PrintFormat, Casl2Defs.Sharp, value);
        }
    }
}
