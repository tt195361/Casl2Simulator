using System;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// COMET II に関する定義です。
    /// </summary>
    internal static class Comet2Defs
    {
        // 主記憶のアドレスは 0 ~ 65,535 番地で、容量は 65,536 語です。
        internal const Int32 MinAddress = 0;
        internal const Int32 MemorySize = 65536;

        internal const Int32 MaxAddress = MinAddress + MemorySize - 1;
    }
}
