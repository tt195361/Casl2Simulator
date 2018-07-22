using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// <see cref="Cpu"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class CpuTest
    {
        #region Static Fields
        private static MemoryAddress LoadAddress = MemoryAddress.Zero;
        private static MemoryAddress StartAddress = LoadAddress;
        #endregion

        #region Instance Fields
        private Memory m_memory;
        private Cpu m_cpu;
        private TestLogger m_logger;
        private TestData<Boolean> m_cancelData;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_memory = new Memory();
            m_cpu = new Cpu(m_memory);
            m_logger = new TestLogger();
            m_cancelData = new TestData<Boolean>(true);

            AddHandlers();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            RemoveHandlers();
        }

        /// <summary>
        /// <see cref="Cpu.Execute"/> で発生する ReturingFromSubroutine イベントのテスト。
        /// </summary>
        [TestMethod]
        public void Execute_ReturingFromSubroutine()
        {
            Word[] program = WordTest.MakeArray(
                0x8000, 0x0003,         // 0000:        CALL    SUB
                0x8100,                 // 0002:        RET
                0x8100                  // 0003: SUB    RET
            );
            // 2 回目の RET で実行を終了する。
            m_cancelData = new TestData<Boolean>(false, true);

            CheckExecute(program, true, "RET 命令で ReturingFromSubroutine イベントが発生する");

            String expected = TestLogger.ExpectedLog(
                "OnReturingFromSubroutine: SP: 65535 (0xffff)",
                "    Setting e.Cancel to \"False\"",
                "OnReturingFromSubroutine: SP: 0 (0x0000)",
                "    Setting e.Cancel to \"True\"");
            String actual = m_logger.Log;
            Assert.AreEqual(
                expected, actual,
                "e.Cancel=False ならば実行を継続し、e.Cancel=True ならば実行を終了する");
        }

        /// <summary>
        /// <see cref="Cpu.Execute"/> で発生する CallingSuperVisor イベントのテスト。
        /// </summary>
        [TestMethod]
        public void Execute_CallingSuperVisor()
        {
            Word[] program = WordTest.MakeArray(
                0xF000, 0x2468,         // 0000:        SVC     #2468
                0x8100                  // 0002:        RET
            );

            CheckExecute(program, true, "SVC 命令で CallingSuperVisor イベントが発生する");

            String expected = TestLogger.ExpectedLog(
                "OnCallingSuperVisor: Operand=9320 (0x2468)",
                "OnReturingFromSubroutine: SP: 0 (0x0000)",
                "    Setting e.Cancel to \"True\"");
            String actual = m_logger.Log;
            Assert.AreEqual(
                expected, actual,
                "e.Operand に SVC 命令のオペランドの値が格納される");
        }

        /// <summary>
        /// <see cref="Cpu.Execute"/> で、未定義命令の場合です。
        /// </summary>
        [TestMethod]
        public void Execute_UndefinedInstruction()
        {
            Word[] program = WordTest.MakeArray(
                0xff00                  // 未定義命令
            );

            CheckExecute(program, false, "未定義命令で例外が発生する");
        }

        private void CheckExecute(Word[] program, Boolean success, String message)
        {
            SetupMemory(program);
            SetupRegisterSet();

            try
            {
                m_cpu.Execute();
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        private void SetupMemory(Word[] program)
        {
            m_memory.Clear();
            m_memory.WriteRange(LoadAddress, program);
        }

        private void SetupRegisterSet()
        {
            CpuRegisterSet registerSet = m_cpu.RegisterSet;
            registerSet.Reset();
            registerSet.PR.Value = StartAddress.GetValueAsWord();
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
            m_logger.Add("OnReturingFromSubroutine: {0}", e.SP);

            Boolean cancel = m_cancelData.GetNext();
            m_logger.Add("    Setting e.Cancel to \"{0}\"", cancel);
            e.Cancel = cancel;
        }

        internal void OnCallingSuperVisor(Object sender, CallingSuperVisorEventArgs e)
        {
            m_logger.Add("OnCallingSuperVisor: Operand={0}", e.Operand);
        }
    }
}
