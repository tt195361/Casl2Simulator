using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の Operating System (OS) です。
    /// </summary>
    internal class Os
    {
        #region Static Fields
        private static readonly Word InitialSp = Word.Zero;
        #endregion

        #region Instance Fields
        private readonly Cpu m_cpu;
        private readonly Memory m_memory;
        #endregion

        internal Os(Cpu cpu, Memory memory)
        {
            m_cpu = cpu;
            m_memory = memory;
        }

        /// <summary>
        /// 指定の実行可能モジュールを CPU で実行します。
        /// </summary>
        /// <param name="exeModule">CPU で実行する実行可能モジュールです。</param>
        internal void Execute(ExecutableModule exeModule)
        {
            SetupMemory(exeModule);
            SetupRegisterSet(exeModule.ExecStartAddress);

            try
            {
                AddHandlers();
                m_cpu.Execute();
            }
            finally
            {
                RemoveHandlers();
            }
        }

        private void SetupMemory(ExecutableModule exeModule)
        {
            // メモリをクリアし、実行するプログラムを書き込む。
            m_memory.Clear();
            m_memory.WriteRange(exeModule.LoadAddress, exeModule.Words);
        }

        private void SetupRegisterSet(MemoryAddress execStartAddress)
        {
            // PR をプログラムの開始アドレスに、SP をスタック領域の最後のアドレス + 1 に、それぞれ設定する。
            CpuRegisterSet registerSet = m_cpu.RegisterSet;
            registerSet.Reset();
            registerSet.PR.Value = execStartAddress.GetValueAsWord();
            registerSet.SP.Value = InitialSp;
        }

        private void AddHandlers()
        {
            m_cpu.ReturningFromSubroutine += OnReturingFromSubroutine;
            m_cpu.CallingSuperVisor += OnCallingSuperVisor;
        }

        private void RemoveHandlers()
        {
            m_cpu.ReturningFromSubroutine -= OnReturingFromSubroutine;
            m_cpu.CallingSuperVisor -= OnCallingSuperVisor;
        }

        internal void OnReturingFromSubroutine(Object sender, ReturningFromSubroutineEventArgs e)
        {
            // RET 命令で SP が初期値ならば、実行を終了する。
            CpuRegister sp = e.SP;
            e.Cancel = (sp.Value == InitialSp);
        }

        internal void OnCallingSuperVisor(Object sender, CallingSuperVisorEventArgs e)
        {
            //
        }
    }
}
