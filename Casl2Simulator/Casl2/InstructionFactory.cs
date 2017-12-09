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
                { Casl2Defs.START, () => new AsmStartInstruction() },
                { Casl2Defs.DC, () => new AsmDcInstruction() },

                { Casl2Defs.IN, () => new MacroInInstruction() },
                { Casl2Defs.RPUSH, () => new MacroRpushInstruction() },

                { Casl2Defs.LD, () => MachineInstruction.MakeLD() }
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
