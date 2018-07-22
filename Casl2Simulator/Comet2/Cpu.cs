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
        #region Events
        /// <summary>
        /// ReturnFromSubroutine 命令を実行しようとすると発生します。
        /// </summary>
        internal event EventHandler<ReturningFromSubroutineEventArgs> ReturningFromSubroutine;

        /// <summary>
        /// スーパーバイザーを呼び出そうとすると発生します。
        /// </summary>
        internal event EventHandler<CallingSuperVisorEventArgs> CallingSuperVisor;
        #endregion

        #region Instance Fields
        private readonly CpuRegisterSet m_registerSet;
        private readonly Memory m_memory;
        private Boolean m_continueToExecute;
        #endregion

        /// <summary>
        /// 指定の主記憶を使って <see cref="Cpu"/> のインスタンスを初期化します。
        /// </summary>
        /// <param name="memory">COMET II の主記憶です。</param>
        internal Cpu(Memory memory)
        {
            m_registerSet = new CpuRegisterSet();
            m_memory = memory;
        }

        /// <summary>
        /// CPU のレジスタを取得します。
        /// </summary>
        internal CpuRegisterSet RegisterSet
        {
            get { return m_registerSet; }
        }

        /// <summary>
        /// CPU を実行します。
        /// </summary>
        internal void Execute()
        {
            try
            {
                AddHandlers();
                DoExecute();
            }
            finally
            {
                RemoveHandlers();
            }
        }

        private void DoExecute()
        {
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
            if (ReturningFromSubroutine != null)
            {
                ReturningFromSubroutine(this, e);
                m_continueToExecute = !e.Cancel;
            }
        }

        private void OnCallingSuperVisor(Object sender, CallingSuperVisorEventArgs e)
        {
            if (CallingSuperVisor != null)
            {
                CallingSuperVisor(this, e);
            }
        }
    }
}
