using System;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// メモリ関連の汎用メソッドを集めたクラスです。
    /// </summary>
    internal class MemoryUtils
    {
        internal static UInt16 Add(Int32 addend1, Int32 addend2)
        {
            Int32 i32Val = addend1 + addend2;
            if (i32Val < Comet2Defs.MinAddress || Comet2Defs.MaxAddress < i32Val)
            {
                String hexAddend1 = ToHexString(addend1);
                String hexAddend2 = ToHexString(addend2);
                String message = String.Format(
                    Resources.MSG_MemoryAddressOutOfRange, i32Val, addend1, hexAddend1, addend2, hexAddend2,
                    Comet2Defs.MinAddress, Comet2Defs.MaxAddress);
                throw new Casl2SimulatorException(message);
            }

            UInt16 ui16Val = NumberUtils.ToUInt16(i32Val);
            return ui16Val;
        }

        private static String ToHexString(Int32 i32val)
        {
            return String.Format(Casl2Defs.HexaDecimalPrintFormat, Casl2Defs.Sharp, i32val);
        }
    }
}
