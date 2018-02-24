using System;
using System.ComponentModel;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// <see cref="Operator.ReturningFromSubroutine"/> イベント、あるいは
    /// <see cref="CpuInstruction.ReturningFromSubroutine"/> イベントのデータを提供します。
    /// </summary>
    internal class ReturningFromSubroutineEventArgs : CancelEventArgs
    {
        private readonly Register m_sp;

        internal ReturningFromSubroutineEventArgs(Register sp)
        {
            m_sp = sp;
        }

        internal Register SP
        {
            get { return m_sp; }
        }
    }

    /// <summary>
    /// <see cref="Operator.CallingSuperVisor"/> イベント、あるいは
    /// <see cref="CpuInstruction.CallingSuperVisor"/> イベントのデータを提供します。
    /// </summary>
    internal class CallingSuperVisorEventArgs : EventArgs
    {
        private readonly Word m_operand;

        internal CallingSuperVisorEventArgs(Word operand)
        {
            m_operand = operand;
        }

        internal Word Operand
        {
            get { return m_operand; }
        }
    }
}
