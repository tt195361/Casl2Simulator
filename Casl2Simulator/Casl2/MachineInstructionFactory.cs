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

        internal static MachineInstruction MakeCpa()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.CPA, OpcodeDef.CompareArithmeticRAdrX, OpcodeDef.CompareArithmeticR1R2);
        }

        internal static MachineInstruction MakeCpl()
        {
            return MachineInstruction.MakeRAdrXOrR1R2(
                MnemonicDef.CPL, OpcodeDef.CompareLogicalRAdrX, OpcodeDef.CompareLogicalR1R2);
        }

        internal static MachineInstruction MakeSla()
        {
            return MachineInstruction.MakeRAdrX(MnemonicDef.SLA, OpcodeDef.ShiftLeftArithmeticRAdrX);
        }

        internal static MachineInstruction MakeSra()
        {
            return MachineInstruction.MakeRAdrX(MnemonicDef.SRA, OpcodeDef.ShiftRightArithmeticRAdrX);
        }

        internal static MachineInstruction MakeSll()
        {
            return MachineInstruction.MakeRAdrX(MnemonicDef.SLL, OpcodeDef.ShiftLeftLogicalRAdrX);
        }

        internal static MachineInstruction MakeSrl()
        {
            return MachineInstruction.MakeRAdrX(MnemonicDef.SRL, OpcodeDef.ShiftRightLogicalRAdrX);
        }

        internal static MachineInstruction MakeJmi()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.JMI, OpcodeDef.JumpOnMinusAdrX);
        }

        internal static MachineInstruction MakeJnz()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.JNZ, OpcodeDef.JumpOnNonZeroAdrX);
        }

        internal static MachineInstruction MakeJze()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.JZE, OpcodeDef.JumpOnZeroAdrX);
        }

        internal static MachineInstruction MakeJump()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.JUMP, OpcodeDef.UnconditionalJumpAdrX);
        }

        internal static MachineInstruction MakeJpl()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.JPL, OpcodeDef.JumpOnPlusAdrX);
        }

        internal static MachineInstruction MakeJov()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.JOV, OpcodeDef.JumpOnOverflowAdrX);
        }

        internal static MachineInstruction MakePush()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.PUSH, OpcodeDef.PushAdrX);
        }

        internal static MachineInstruction MakePop()
        {
            return MachineInstruction.MakeR(MnemonicDef.POP, OpcodeDef.PopR);
        }

        internal static MachineInstruction MakeCall()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.CALL, OpcodeDef.CallAdrX);
        }

        internal static MachineInstruction MakeRet()
        {
            return MachineInstruction.MakeNoOperand(MnemonicDef.RET, OpcodeDef.Ret);
        }

        internal static MachineInstruction MakeSvc()
        {
            return MachineInstruction.MakeAdrX(MnemonicDef.SVC, OpcodeDef.SupervisorCallAdrX);
        }
    }
}
