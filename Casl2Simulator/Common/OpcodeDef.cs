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

        internal const UInt16 AndRAdrX = 0x30;
        internal const UInt16 OrRAdrX = 0x31;
        internal const UInt16 XorRAdrX = 0x32;
        internal const UInt16 AndR1R2 = 0x34;
        internal const UInt16 OrR1R2 = 0x35;
        internal const UInt16 XorR1R2 = 0x36;

        internal const UInt16 CompareArithmeticRAdrX = 0x40;
        internal const UInt16 CompareLogicalRAdrX = 0x41;
        internal const UInt16 CompareArithmeticR1R2 = 0x44;
        internal const UInt16 CompareLogicalR1R2 = 0x45;

        internal const UInt16 ShiftLeftArithmeticRAdrX = 0x50;
        internal const UInt16 ShiftRightArithmeticRAdrX = 0x51;
        internal const UInt16 ShiftLeftLogicalRAdrX = 0x52;
        internal const UInt16 ShiftRightLogicalRAdrX = 0x53;

        internal const UInt16 PushAdrX = 0x70;

        internal const UInt16 Dummy = 0xff;
    }
}
