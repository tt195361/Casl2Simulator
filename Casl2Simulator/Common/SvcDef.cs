using System;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// SVC 命令に関連する値を定義します。
    /// </summary>
    internal class SvcDef
    {
        // オペランドの値
        internal const Int32 InOperand = 1;
        internal const Int32 OutOperand = 2;

        // 使用するレジスタの名前
        internal const String BufferAddrRegister = RegisterDef.GR1;
        internal const String LengthRegister = RegisterDef.GR2;
    }
}
