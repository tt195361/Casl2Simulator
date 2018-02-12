using System;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// COMET II のオペコードを定義します。
    /// </summary>
    internal static class OpcodeDef
    {
        internal const UInt16 NoOperation = 0x00;

        internal const UInt16 LoadRAdrX = 0x10;
        internal const UInt16 StoreRAdrX = 0x11;
        internal const UInt16 LoadAddressRAdrX = 0x12;
        internal const UInt16 LoadR1R2 = 0x14;

        internal const UInt16 AddArithmeticRAdrX = 0x20;
        internal const UInt16 SubtractArithmeticRAdrX = 0x21;
        internal const UInt16 AddLogicalRAdrX = 0x22;
        internal const UInt16 SubtractLogicalRAdrX = 0x23;
        internal const UInt16 AddArithmeticR1R2 = 0x24;
        internal const UInt16 SubtractArithmeticR1R2 = 0x25;
        internal const UInt16 AddLogicalR1R2 = 0x26;
        internal const UInt16 SubtractLogicalR1R2 = 0x27;

        internal const UInt16 PushAdrX = 0x70;

        internal const UInt16 Dummy = 0xff;
    }
}
