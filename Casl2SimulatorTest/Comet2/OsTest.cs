using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// <see cref="Os"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class OsTest
    {
        #region Static Fields
        private static MemoryAddress LoadAddress = MemoryAddress.Zero;
        private static MemoryAddress StartAddress = LoadAddress;
        #endregion

        #region Instance Fields
        private Cpu m_cpu;
        private Os m_os;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            Memory memory = new Memory();
            m_cpu = new Cpu(memory);
            m_os = new Os(m_cpu, memory);
        }

        /// <summary>
        /// <see cref="Os.Execute"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Execute()
        {
            Word[] program = WordTest.MakeArray(
                // 処理: GR1 中の '1' のビットの個数を数える。
                // 出力: GR0: GR1 中の '1' のビットの個数。
                0x1010, 0x000F,         // 0000:        LD      GR1,DATA,0  ;
                0x2522,                 // 0002:        SUBA    GR2,GR2     ; Count = 0
                0x3411,                 // 0003:        AND     GR1,GR1     ; 全部のビットが '0'?
                0x6300, 0x000D,         // 0004:        JZE     RETURN      ; 全部のビットが '0' なら終了
                0x1222, 0x0001,         // 0006: MORE   LAD     GR2,1,GR2   ; Count = Count + 1
                0x1201, 0xffff,         // 0008:        LAD     GR0,-1,GR1  ; 最下位の '1' のビット 1 個を
                0x3410,                 // 000A:        AND     GR1,GR0     ;   '0' に変える。
                0x6200, 0x0006,         // 000B:        JNZ     MORE        ; '1' のビットが残っていれば繰返し
                0x1402,                 // 000D: RETURN LD      GR0,GR2     ; GR0 = Count
                0x8100,                 // 000E:        RET                 ; 呼び出しプログラムへ戻る。
                0x1234                  // 000F: DATA   DC      0x1234      ; 0001 0010 0011 0100 => '1' は 5 つ
            );
            ExecutableModule exeModule = new ExecutableModule(LoadAddress, StartAddress, program);

            CheckExecute(exeModule, true, "指定の実行可能モジュールを実行する");

            const UInt16 Expected = 5;
            Word actualWord = m_cpu.RegisterSet.GR[0].Value;
            WordTest.Check(actualWord, Expected, "GR0: GR1 中の '1' のビットの個数");
        }

        private void CheckExecute(ExecutableModule exeModule, Boolean success, String message)
        {
            try
            {
                m_os.Execute(exeModule);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
