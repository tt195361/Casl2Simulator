using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    internal class Register
    {
        #region Static Fields
        internal static Register GR0 = new Register(RegisterDef.GR0, 0);
        internal static Register GR1 = new Register(RegisterDef.GR1, 1);
        internal static Register GR2 = new Register(RegisterDef.GR2, 2);
        internal static Register GR3 = new Register(RegisterDef.GR3, 3);
        internal static Register GR4 = new Register(RegisterDef.GR4, 4);
        internal static Register GR5 = new Register(RegisterDef.GR5, 5);
        internal static Register GR6 = new Register(RegisterDef.GR6, 6);
        internal static Register GR7 = new Register(RegisterDef.GR7, 7);

        private static readonly Dictionary<String, Register> m_registerDictionary;
        #endregion

        static Register()
        {
            Register[] registers = new Register[] { GR0, GR1, GR2, GR3, GR4, GR5, GR6, GR7 };

            m_registerDictionary = new Dictionary<String, Register>();
            foreach (Register register in registers)
            {
                m_registerDictionary.Add(register.Name, register);
            }
        }

        internal static Boolean IsRegisterName(String str)
        {
            return m_registerDictionary.ContainsKey(str);
        }

        internal static Register GetFor(String name)
        {
            if (!IsRegisterName(name))
            {
                String message = String.Format(Resources.MSG_UndefinedRegisterName, name);
                throw new Casl2SimulatorException(message);
            }

            return m_registerDictionary[name];
        }

        #region Fields
        private readonly String m_name;
        private readonly UInt16 m_number;
        #endregion

        private Register(String name, UInt16 number)
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

        public override String ToString()
        {
            return m_name;
        }
    }
}
