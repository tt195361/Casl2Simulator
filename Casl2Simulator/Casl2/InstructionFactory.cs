using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// <see cref="Instruction"/> のオブジェクトを生成します。
    /// </summary>
    internal class InstructionFactory
    {
        #region Fields
        private static readonly Dictionary<String, Func<Instruction>> m_factoryMethodDictionary;
        #endregion

        static InstructionFactory()
        {
            m_factoryMethodDictionary = new Dictionary<String, Func<Instruction>>()
            {
                { MnemonicDef.START, () => new AsmStartInstruction() },
                { MnemonicDef.END, () => new AsmEndInstruction() },
                { MnemonicDef.DS, () => new AsmDsInstruction() },
                { MnemonicDef.DC, () => new AsmDcInstruction() },

                { MnemonicDef.IN, () => new MacroInInstruction() },
                { MnemonicDef.RPUSH, () => new MacroRpushInstruction() },

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

                { MnemonicDef.PUSH, () => MachineInstructionFactory.MakePush() },
            };
        }

        internal static Instruction Make(String instructionField)
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

            Func<Instruction> factoryMethod = m_factoryMethodDictionary[instructionField];
            Instruction instruction = factoryMethod();
            return instruction;
        }
    }
}
