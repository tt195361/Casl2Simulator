using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// <see cref="ProgramInstruction"/> のオブジェクトを生成します。
    /// </summary>
    internal class ProgramInstructionFactory
    {
        #region Static Fields
        private static readonly Dictionary<String, Func<ProgramInstruction>> m_factoryMethodDictionary;
        #endregion

        static ProgramInstructionFactory()
        {
            m_factoryMethodDictionary = new Dictionary<String, Func<ProgramInstruction>>()
            {
                { MnemonicDef.START, () => new AsmStartInstruction() },
                { MnemonicDef.END, () => new AsmEndInstruction() },
                { MnemonicDef.DS, () => new AsmDsInstruction() },
                { MnemonicDef.DC, () => new AsmDcInstruction() },

                { MnemonicDef.IN, () => MacroInOutInstruction.MakeIn() },
                { MnemonicDef.OUT, () => MacroInOutInstruction.MakeOut() },
                { MnemonicDef.RPUSH, () => new MacroRpushInstruction() },
                { MnemonicDef.RPOP, () => new MacroRpopInstruction() },

                { MnemonicDef.NOP, () => MachineInstructionFactory.MakeNop() },

                { MnemonicDef.LD, () => MachineInstructionFactory.MakeLd() },
                { MnemonicDef.ST, () => MachineInstructionFactory.MakeSt() },
                { MnemonicDef.LAD, () => MachineInstructionFactory.MakeLad() },

                { MnemonicDef.ADDA, () => MachineInstructionFactory.MakeAdda() },
                { MnemonicDef.SUBA, () => MachineInstructionFactory.MakeSuba() },
                { MnemonicDef.ADDL, () => MachineInstructionFactory.MakeAddl() },
                { MnemonicDef.SUBL, () => MachineInstructionFactory.MakeSubl() },

                { MnemonicDef.AND, () => MachineInstructionFactory.MakeAnd() },
                { MnemonicDef.OR, () => MachineInstructionFactory.MakeOr() },
                { MnemonicDef.XOR, () => MachineInstructionFactory.MakeXor() },

                { MnemonicDef.CPA, () => MachineInstructionFactory.MakeCpa() },
                { MnemonicDef.CPL, () => MachineInstructionFactory.MakeCpl() },

                { MnemonicDef.SLA, () => MachineInstructionFactory.MakeSla() },
                { MnemonicDef.SRA, () => MachineInstructionFactory.MakeSra() },
                { MnemonicDef.SLL, () => MachineInstructionFactory.MakeSll() },
                { MnemonicDef.SRL, () => MachineInstructionFactory.MakeSrl() },

                { MnemonicDef.JMI, () => MachineInstructionFactory.MakeJmi() },
                { MnemonicDef.JNZ, () => MachineInstructionFactory.MakeJnz() },
                { MnemonicDef.JZE, () => MachineInstructionFactory.MakeJze() },
                { MnemonicDef.JUMP, () => MachineInstructionFactory.MakeJump() },
                { MnemonicDef.JPL, () => MachineInstructionFactory.MakeJpl() },
                { MnemonicDef.JOV, () => MachineInstructionFactory.MakeJov() },

                { MnemonicDef.PUSH, () => MachineInstructionFactory.MakePush() },
                { MnemonicDef.POP, () => MachineInstructionFactory.MakePop() },

                { MnemonicDef.CALL, () => MachineInstructionFactory.MakeCall() },
                { MnemonicDef.RET, () => MachineInstructionFactory.MakeRet() },

                { MnemonicDef.SVC, () => MachineInstructionFactory.MakeSvc() },
            };
        }

        internal static ProgramInstruction Make(String instructionField)
        {
            if (instructionField.Length == 0)
            {
                String message = Resources.MSG_NoInstructionInInstructionLine;
                throw new Casl2SimulatorException(message);
            }

            if (!m_factoryMethodDictionary.ContainsKey(instructionField))
            {
                String message = String.Format(Resources.MSG_InstructionNotDefined, instructionField);
                throw new Casl2SimulatorException(message);
            }

            Func<ProgramInstruction> factoryMethod = m_factoryMethodDictionary[instructionField];
            ProgramInstruction instruction = factoryMethod();
            return instruction;
        }
    }
}
