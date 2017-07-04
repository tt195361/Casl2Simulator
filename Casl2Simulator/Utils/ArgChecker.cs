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
        /// 指定の値が、指定の最小から最大までの範囲に入っているかどうか、検査します。
        /// </summary>
        /// <param name="value">検査する値です。</param>
        /// <param name="min">範囲の最小の値です。</param>
        /// <param name="max">範囲の最大の値です。</param>
        /// <param name="name">検査する値の名前です。</param>
        internal static void CheckRange(Int32 value, Int32 min, Int32 max, String name)
        {
            if (value < min || max < value)
            {
                String message = String.Format(Resources.MSG_ArgRangeError, name, value, min, max);
                throw new Casl2SimulatorException(message);
            }
        }
    }
}
