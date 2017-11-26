using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令のレジスタオペランドです。
    /// </summary>
    internal class RegisterOperand : MachineInstructionOperand
    {
        #region Static Fields
        private static readonly Dictionary<String, RegisterOperand> m_registerOperandDictionary;
        #endregion

        static RegisterOperand()
        {
            m_registerOperandDictionary = new Dictionary<String, RegisterOperand>
            {
                { Casl2Defs.GR0, new RegisterOperand(Casl2Defs.GR0, 0) },
                { Casl2Defs.GR1, new RegisterOperand(Casl2Defs.GR1, 1) },
                { Casl2Defs.GR2, new RegisterOperand(Casl2Defs.GR2, 2) },
                { Casl2Defs.GR3, new RegisterOperand(Casl2Defs.GR3, 3) },
                { Casl2Defs.GR4, new RegisterOperand(Casl2Defs.GR4, 4) },
                { Casl2Defs.GR5, new RegisterOperand(Casl2Defs.GR5, 5) },
                { Casl2Defs.GR6, new RegisterOperand(Casl2Defs.GR6, 6) },
                { Casl2Defs.GR7, new RegisterOperand(Casl2Defs.GR7, 7) },
            };
        }

        internal static Boolean IsRegisterName(String str)
        {
            return m_registerOperandDictionary.ContainsKey(str);
        }

        internal static RegisterOperand GetFor(String name)
        {
            if (!IsRegisterName(name))
            {
                String message = String.Format(Resources.MSG_UndefinedRegisterName, name);
                throw new Casl2SimulatorException(message);
            }

            return m_registerOperandDictionary[name];
        }

        #region Fields
        private readonly String m_name;
        private readonly UInt16 m_number;
        #endregion

        private RegisterOperand(String name, UInt16 number)
        {
            m_name = name;
            m_number = number;
        }

        internal String Name
        {
            get { return m_name; }
        }

        internal UInt16 Number
        {
            get { return m_number; }
        }

        internal override UInt16 GetRR1()
        {
            return Number;
        }

        public override String ToString()
        {
            return Name;
        }
    }
}
