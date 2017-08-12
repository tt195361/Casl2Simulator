using System;
using System.ComponentModel;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// <see cref="Operator.ReturningFromSubroutine"/> イベント、あるいは
    /// <see cref="Instruction.ReturningFromSubroutine"/> イベントのデータを提供します。
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
    /// <see cref="Instruction.CallingSuperVisor"/> イベントのデータを提供します。
    /// </summary>
    internal class CallingSuperVisorEventArgs : EventArgs
    {
        private readonly Word m_operand;
        private readonly RegisterSet m_registerSet;
        private readonly Memory m_memory;

        internal CallingSuperVisorEventArgs(Word operand, RegisterSet registerSet, Memory memory)
        {
            m_operand = operand;
            m_registerSet = registerSet;
            m_memory = memory;
        }

        internal Word Operand
        {
            get { return m_operand; }
        }

        internal RegisterSet RegisterSet
        {
            get { return m_registerSet; }
        }

        internal Memory Memory
        {
            get { return m_memory; }
        }
    }
}
