using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Utils
{
    /// <summary>
    /// 引数の値を検査します。
    /// </summary>
    internal class ArgChecker
    {
        /// <summary>
        /// 指定の検査する値が、指定の最小値から最大値の範囲内かどうか、検査します。
        /// </summary>
        /// <param name="value">検査する値です。</param>
        /// <param name="min">範囲の最小の値です。この値も範囲に含みます。</param>
        /// <param name="max">範囲の最大の値です。この値も範囲に含みます。</param>
        /// <param name="name">検査する値の名前です。</param>
        internal static void CheckRange(Int32 value, Int32 min, Int32 max, String name)
        {
            if (value < min || max < value)
            {
                // TODO: メッセージを変更。
                // {0} として指定の値 {1} は、{2} ~ {3} の範囲外です。範囲内の値を指定してください。
                String message = String.Format(Resources.MSG_ArgRangeError, name, value, min, max);
                throw new Casl2SimulatorException(message);
            }
        }

        /// <summary>
        /// 指定のより大きい値を指定のより小さい値と比較し、より大きいか等しいことを、検査します。
        /// </summary>
        /// <param name="greaterValue">検査するより大きい値です。</param>
        /// <param name="lesserValue">検査するより小さい値です。</param>
        /// <param name="greaterName">より大きい値の名前です。</param>
        /// <param name="lesserName">より小さい値の名前です。</param>
        internal static void CheckGreaterEqual(
            Int32 greaterValue, Int32 lesserValue, String greaterName, String lesserName)
        {
            if (greaterValue < lesserValue)
            {
                String message = String.Format(
                    Resources.MSG_ArgGreaterEqual, greaterName, greaterValue, lesserName, lesserValue);
                throw new Casl2SimulatorException(message);
            }
        }
    }
}
