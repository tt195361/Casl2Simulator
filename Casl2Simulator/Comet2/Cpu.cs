using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II CPU です。
    /// </summary>
    internal class Cpu
    {
        #region Instance Fields
        private readonly RegisterSet m_registerSet;
        private readonly Memory m_memory;
        private readonly Os m_os;

        private Boolean m_continueToExecute;
        #endregion

        /// <summary>
        /// 指定の主記憶とオペレーティングシステムを使って <see cref="Cpu"/> のインスタンスを初期化します。
        /// </summary>
        /// <param name="memory">COMET II の主記憶です。</param>
        /// <param name="os">COMET II のオペレーティングシステムです。</param>
        internal Cpu(Memory memory, Os os)
        {
            m_registerSet = new RegisterSet();
            m_memory = memory;
            m_os = os;
        }

        /// <summary>
        /// CPU のレジスタを取得します。
        /// </summary>
        internal RegisterSet RegisterSet
        {
            get { return m_registerSet; }
        }

        /// <summary>
        /// 指定のプログラムを実行します。
        /// </summary>
        /// <param name="program">CPU で実行するプログラムです。</param>
        internal void Execute(UInt16[] program)
        {
            try
            {
                AddHandlers();
                DoExecute(program);
            }
            finally
            {
                RemoveHandlers();
            }
        }

        private void DoExecute(UInt16[] program)
        {
            m_os.PrepareExecution(m_registerSet, m_memory, program);
            m_continueToExecute = true;

            while (m_continueToExecute)
            {
                ExecuteInstruction();
            }
        }

        private void ExecuteInstruction()
        {
            Word pr = m_registerSet.PR.Value;
            Word instruction = m_memory.Read(pr);

            try
            {
                DoExecuteInstruction();
            }
            catch (Exception ex)
            {
                String message = String.Format(
                    Resources.MSG_CpuExecutionError, pr.GetAsUnsigned(), instruction.GetAsUnsigned());
                throw new Casl2SimulatorException(message, ex);
            }
        }

        private void DoExecuteInstruction()
        {
            // PR レジスタが指すアドレスから命令語をフェッチし、デコードします。
            Word firstWord = Fetcher.Fetch(m_registerSet.PR, m_memory);
            UInt16 opcode = InstructionWord.GetOpcode(firstWord);
            CpuInstruction instruction = Decoder.Decode(opcode);

            // 命令語から r/r1 フィールドと x/r2 フィールドを取得し、命令を実行します。
            UInt16 rR1Field = InstructionWord.GetRR1(firstWord);
            UInt16 xR2Field = InstructionWord.GetXR2(firstWord);
            instruction.Execute(rR1Field, xR2Field, m_registerSet, m_memory);
        }

        private void AddHandlers()
        {
            CpuInstruction.ReturningFromSubroutine += OnReturningFromSubroutine;
            CpuInstruction.CallingSuperVisor += OnCallingSuperVisor;
        }

        private void RemoveHandlers()
        {
            CpuInstruction.ReturningFromSubroutine -= OnReturningFromSubroutine;
            CpuInstruction.CallingSuperVisor -= OnCallingSuperVisor;
        }

        private void OnReturningFromSubroutine(Object sender, ReturningFromSubroutineEventArgs e)
        {
            m_continueToExecute = m_os.OnReturingFromSubroutine(e.SP);
            if (!m_continueToExecute)
            {
                e.Cancel = true;
            }
        }

        private void OnCallingSuperVisor(Object sender, CallingSuperVisorEventArgs e)
        {
            m_os.OnCallingSuperVisor(e.Operand, m_registerSet, m_memory);
        }
    }
}
