using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドでオペランドなしを表わします。
    /// </summary>
    internal class NoOperand : MachineInstructionOperand
    {
        private NoOperand(UInt16 opcode)
            : base(opcode)
        {
            //
        }

        internal static NoOperand MakeForUnitTest()
        {
            return new NoOperand(OpcodeDef.Dummy);
        }
    }
}
