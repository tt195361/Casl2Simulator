using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドで adr として使用できる項目が提供するインタフェースです。
    /// </summary>
    internal interface IAdrValue
    {
        String GenerateDc(LabelManager lblManager);
        UInt16 GetAddress(LabelManager lblManager);
    }
}
