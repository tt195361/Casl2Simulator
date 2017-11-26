using System;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// COMET II のオペコードを定義します。
    /// </summary>
    internal static class Opcode
    {
        internal const UInt16 NoOperation = 0x00;

        internal const UInt16 LoadEaContents = 0x10;
        internal const UInt16 Store = 0x11;
        internal const UInt16 LoadEffectiveAddress = 0x12;
        internal const UInt16 LoadRegister = 0x14;
    }
}
