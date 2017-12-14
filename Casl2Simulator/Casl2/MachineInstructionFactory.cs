﻿using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令のオブジェクトを作成します。
    /// </summary>
    internal static class MachineInstructionFactory
    {
        internal static MachineInstruction MakeLd()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.LD, OpcodeDef.LoadEaContents, OpcodeDef.LoadRegister);
        }

        internal static MachineInstruction MakePush()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.PUSH, OpcodeDef.Push);
        }
    }
}
