using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令のオブジェクトを作成します。
    /// </summary>
    internal static class MachineInstructionFactory
    {
        internal static MachineInstruction MakeNop()
        {
            return MachineInstruction.MakeNoOperand(MnemonicDef.NOP, OpcodeDef.NoOperation);
        }

        internal static MachineInstruction MakeLd()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.LD, OpcodeDef.LoadRAdrX, OpcodeDef.LoadR1R2);
        }

        internal static MachineInstruction MakeSt()
        {
            return MachineInstruction.MakeRAdrX(MnemonicDef.ST, OpcodeDef.StoreRAdrX);
        }

        internal static MachineInstruction MakeLad()
        {
            return MachineInstruction.MakeRAdrX(MnemonicDef.LAD, OpcodeDef.LoadAddressRAdrX);
        }

        internal static MachineInstruction MakeAdda()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.ADDA, OpcodeDef.AddArithmeticRAdrX, OpcodeDef.AddArithmeticR1R2);
        }

        internal static MachineInstruction MakeSuba()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.SUBA, OpcodeDef.SubtractArithmeticRAdrX, OpcodeDef.SubtractArithmeticR1R2);
        }

        internal static MachineInstruction MakeAddl()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.ADDL, OpcodeDef.AddLogicalRAdrX, OpcodeDef.AddLogicalR1R2);
        }

        internal static MachineInstruction MakeSubl()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.SUBL, OpcodeDef.SubtractLogicalRAdrX, OpcodeDef.SubtractLogicalR1R2);
        }

        internal static MachineInstruction MakeAnd()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.AND, OpcodeDef.AndRAdrX, OpcodeDef.AndR1R2);
        }

        internal static MachineInstruction MakeOr()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.OR, OpcodeDef.OrRAdrX, OpcodeDef.OrR1R2);
        }

        internal static MachineInstruction MakeXor()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.XOR, OpcodeDef.XorRAdrX, OpcodeDef.XorR1R2);
        }

        internal static MachineInstruction MakePush()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.PUSH, OpcodeDef.PushAdrX);
        }
    }
}
