using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
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
                String hexAddend1 = HexaDecimalConstant.ValueToString(addend1);
                String hexAddend2 = HexaDecimalConstant.ValueToString(addend2);
                String message = String.Format(
                    Resources.MSG_MemoryAddressOutOfRange, i32Val, addend1, hexAddend1, addend2, hexAddend2,
                    Comet2Defs.MinAddress, Comet2Defs.MaxAddress);
                throw new Casl2SimulatorException(message);
            }

            UInt16 ui16Val = NumberUtils.ToUInt16(i32Val);
            return ui16Val;
        }
    }
}
