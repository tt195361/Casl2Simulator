using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II の定数です。
    /// </summary>
    internal abstract class Constant
    {
        /// <summary>
        /// 定数の並びを解釈します。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="Constant"/> クラスのオブジェクトの配列を返します。
        /// </returns>
        internal static Constant[] ParseList(ReadBuffer buffer)
        {
            List<Constant> constantList = new List<Constant>();

            for ( ; ; )
            {
                Constant constant = Parse(buffer);
                constantList.Add(constant);

                if (buffer.Current != Casl2Defs.Comma)
                {
                    break;
                }

                buffer.MoveNext();
            }

            // 解釈できなかった残りの文字列は、Instruction.DoParseOperand() で取り扱う。
            return constantList.ToArray();
        }

        private static Constant Parse(ReadBuffer buffer)
        {
            Char firstChar = buffer.Current;

            if (Operand.EndOfField(firstChar))
            {
                throw new Casl2SimulatorException(Resources.MSG_ConstantExpected);
            }

            if (NumericConstant.IsDecimalStart(firstChar))
            {
                return NumericConstant.ParseDecimal(buffer);
            }
            else if (NumericConstant.IsHexaDecimalStart(firstChar))
            {
                return NumericConstant.ParseHexaDecimal(buffer);
            }
            else if (StringConstant.IsStart(firstChar))
            {
                return StringConstant.Parse(buffer);
            }
            else if (AddressConstant.IsStart(firstChar))
            {
                return AddressConstant.Parse(buffer);
            }
            else
            {
                String rest = buffer.GetRest();
                String message = String.Format(Resources.MSG_ConstantParseError, rest, firstChar);
                throw new Casl2SimulatorException(message);
            }
        }

        protected Constant()
        {
            //
        }

        /// <summary>
        /// この定数が生成するコードのワード数を取得します。
        /// </summary>
        /// <returns>生成するコードのワード数を返します。</returns>
        internal abstract Int32 GetWordCount();

        /// <summary>
        /// この定数のコードを生成します。
        /// </summary>
        /// <param name="lblManager">
        /// ラベルを管理する <see cref="LabelManager"/> のオブジェクトです。
        /// </param>
        /// <param name="relModule">
        /// 生成したコードを格納する <see cref="RelocatableModule"/> のオブジェクトです。
        /// </param>
        internal abstract void GenerateCode(LabelManager lblManager, RelocatableModule relModule);
    }
}
