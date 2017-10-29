using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令のレジスタオペランドです。
    /// </summary>
    internal class RegisterOperand
    {
        #region Static Fields
        internal static readonly String IndexRegisters = String.Format("{0}~{1}", Casl2Defs.GR1, Casl2Defs.GR7);

        private static readonly Dictionary<String, RegisterOperand> m_registerOperandDictionary;
        #endregion

        static RegisterOperand()
        {
            m_registerOperandDictionary = new Dictionary<String, RegisterOperand>
            {
                { Casl2Defs.GR0, new RegisterOperand(Casl2Defs.GR0, 0, false) },
                { Casl2Defs.GR1, new RegisterOperand(Casl2Defs.GR1, 1, true) },
                { Casl2Defs.GR2, new RegisterOperand(Casl2Defs.GR2, 2, true) },
                { Casl2Defs.GR3, new RegisterOperand(Casl2Defs.GR3, 3, true) },
                { Casl2Defs.GR4, new RegisterOperand(Casl2Defs.GR4, 4, true) },
                { Casl2Defs.GR5, new RegisterOperand(Casl2Defs.GR5, 5, true) },
                { Casl2Defs.GR6, new RegisterOperand(Casl2Defs.GR6, 6, true) },
                { Casl2Defs.GR7, new RegisterOperand(Casl2Defs.GR7, 7, true) },
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
        private readonly Int32 m_number;
        private readonly Boolean m_canIndex;
        #endregion

        private RegisterOperand(String name, Int32 number, Boolean canIndex)
        {
            m_name = name;
            m_number = number;
            m_canIndex = canIndex;
        }

        internal String Name
        {
            get { return m_name; }
        }

        internal Int32 Number
        {
            get { return m_number; }
        }

        /// <summary>
        /// 指標レジスタとして使えるかどうかを取得します。
        /// </summary>
        internal Boolean CanIndex
        {
            get { return m_canIndex; }
        }

        public override String ToString()
        {
            return Name;
        }
    }
}
