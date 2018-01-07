using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// メモリのオフセットを計算します。
    /// </summary>
    internal class OffsetCalculator
    {
        internal static UInt16 Add(UInt16 offset, Int32 addend)
        {
            Int32 i32Offset = offset + addend;
            if (Comet2Defs.MaxAddress < i32Offset)
            {
                String message = String.Format(Resources.MSG_ProgramTooBig, Comet2Defs.MemorySize);
                throw new Casl2SimulatorException(message);
            }

            return NumberUtils.ToUInt16(i32Offset);
        }
    }
}
