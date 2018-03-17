using System;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II アセンブラ言語のプログラムに記述するレジスタを表わします。
    /// </summary>
    internal class ProgramRegister
    {
        #region Static Fields
        private static readonly Dictionary<String, ProgramRegister> m_registerDictionary;
        #endregion

        static ProgramRegister()
        {
            m_registerDictionary = new Dictionary<String, ProgramRegister>();
            RegisterDef.GrNames
                       .Select((name, index) => new ProgramRegister(name, NumberUtils.ToUInt16(index)))
                       .ForEach((register) => m_registerDictionary.Add(register.Name, register));
        }

        internal static Boolean IsRegisterName(String str)
        {
            return m_registerDictionary.ContainsKey(str);
        }

        internal static ProgramRegister GetFor(String name)
        {
            if (!IsRegisterName(name))
            {
                String firstGr = RegisterDef.GrNames.First();
                String lastGr = RegisterDef.GrNames.Last();
                String message = String.Format(Resources.MSG_UndefinedRegisterName, name, firstGr, lastGr);
                throw new Casl2SimulatorException(message);
            }

            return m_registerDictionary[name];
        }

        #region Instance Fields
        private readonly String m_name;
        private readonly UInt16 m_number;
        #endregion

        private ProgramRegister(String name, UInt16 number)
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
