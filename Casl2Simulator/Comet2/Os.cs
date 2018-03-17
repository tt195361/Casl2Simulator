using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の Operating System (OS) です。
    /// </summary>
    internal class Os
    {
        private const UInt16 ProgramStartAddress = 0x0000;
        private const UInt16 InitialSp = 0x0000;

        internal Os()
        {
            //
        }

        /// <summary>
        /// レジスタとメモリを、指定のプログラムが実行できるように準備します。
        /// </summary>
        /// <param name="registerSet">実行を準備する COMET II CPU のレジスタです。</param>
        /// <param name="memory">実行を準備する COMET II の主記憶です。</param>
        /// <param name="program">実行するプログラムです。</param>
        internal void PrepareExecution(CpuRegisterSet registerSet, Memory memory, UInt16[] program)
        {
            PrepareMemory(memory, program);
            PrepareRegisterSet(registerSet);
        }

        private void PrepareMemory(Memory memory, UInt16[] program)
        {
            // メモリに実行するプログラムを書き込む。
            memory.Reset();
            memory.WriteRange(ProgramStartAddress, program);
        }

        private void PrepareRegisterSet(CpuRegisterSet registerSet)
        {
            // PR をプログラムの開始アドレスに、SP をスタック領域の最後のアドレス + 1 に、それぞれ設定する。
            registerSet.Reset();
            registerSet.PR.SetValue(ProgramStartAddress);
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
