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
        private const UInt16 InitialSp = 0x0000;
        #endregion

        internal Os()
        {
            //
        }

        /// <summary>
        /// レジスタとメモリを、指定の実行可能モジュールが実行できるように準備します。
        /// </summary>
        /// <param name="registerSet">実行を準備する COMET II CPU のレジスタです。</param>
        /// <param name="memory">実行を準備する COMET II の主記憶です。</param>
        /// <param name="exeModule">実行する実行可能モジュールです。</param>
        internal void PrepareExecution(CpuRegisterSet registerSet, Memory memory, ExecutableModule exeModule)
        {
            PrepareMemory(memory, exeModule);
            PrepareRegisterSet(registerSet, exeModule.ExecStartAddress);
        }

        private void PrepareMemory(Memory memory, ExecutableModule exeModule)
        {
            // メモリに実行するプログラムを書き込む。
            memory.Reset();
            memory.WriteRange(exeModule.LoadAddress, exeModule.Words);
        }

        private void PrepareRegisterSet(CpuRegisterSet registerSet, MemoryAddress execStartAddress)
        {
            // PR をプログラムの開始アドレスに、SP をスタック領域の最後のアドレス + 1 に、それぞれ設定する。
            registerSet.Reset();
            registerSet.PR.Value = execStartAddress.GetValueAsWord();
            registerSet.SP.SetValue(InitialSp);
        }

        internal Boolean OnReturingFromSubroutine(CpuRegister sp)
        {
            // RET 命令で、SP が初期値でなければ、実行を継続する。SP が初期値ならば、実行を終了する。
            UInt16 spValue = sp.Value.GetAsUnsigned();
            Boolean continueToExecute = (spValue != InitialSp);
            return continueToExecute;
        }

        internal void OnCallingSuperVisor(Word operand, CpuRegisterSet registerSet, Memory memory)
        {
            //
        }
    }
}
