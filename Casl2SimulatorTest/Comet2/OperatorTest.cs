using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// <see cref="Operator"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class OperatorTest
    {
        #region Instance Fields
        private CpuRegisterSet m_registerSet;
        private Memory m_memory;
        private CpuRegister m_gr;
        private CpuRegister m_pr;
        private FlagRegister m_fr;
        private TestLogger m_logger;
        private Boolean m_cancelRet;

        private const UInt16 SpValue = 0x2468;
        private const UInt16 SpValueMinusOne = SpValue - 1;
        private const UInt16 SpValuePlusOne = SpValue + 1;

        private const Boolean DontCareBool = false;
        private const UInt16 DontCareUInt16 = 0;
        #endregion // Fields

        #region TestInitialize/TestCleanup
        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new CpuRegisterSet();
            m_memory = new Memory();
            m_gr = m_registerSet.GR[1];
            m_pr = m_registerSet.PR;
            m_fr = m_registerSet.FR;
            m_logger = new TestLogger();

            Operator.ReturningFromSubroutine += OnReturningFromSubroutine;
            Operator.CallingSuperVisor += OnCallingSuperVisor;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Operator.ReturningFromSubroutine -= OnReturningFromSubroutine;
            Operator.CallingSuperVisor -= OnCallingSuperVisor;
        }
        #endregion // TestInitialize/TestCleanup

        #region Load/Store
        /// <summary>
        /// <see cref="Operator.LoadWithFr"/> のテストです。
        /// </summary>
        [TestMethod]
        public void LoadWithFr()
        {
            Operator target = Operator.LoadWithFr;

            CheckRegisterResult(target, 0, 13579, 13579, "オペランドの値がレジスタに設定される");

            CheckOverflowFlag(target, 0, 0xffff, false, "OF は常に false");

            CheckSignFlag(target, 0, 0x7fff, false, "正の値をロード => SF は false");
            CheckSignFlag(target, 0, 0x8000, true, "負の値をロード => SF は true");

            CheckZeroFlag(target, 0, 1, false, "0 以外をロード => ZF は false");
            CheckZeroFlag(target, 0, 0, true, "0 をロード => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.Store"/> のテストです。
        /// </summary>
        [TestMethod]
        public void Store()
        {
            Operator target = Operator.Store;

            CheckMemoryResult(target, 1234, 56789, 1234, "レジスタの値がオペランドで指定のアドレスに書き込まれる");
        }

        /// <summary>
        /// <see cref="Operator.LoadWithoutFr"/> のテストです。
        /// </summary>
        [TestMethod]
        public void LoadWithoutFr()
        {
            Operator target = Operator.LoadWithoutFr;

            const Boolean OverflowFlagSet = true;
            const Boolean SignFlagSet = false;
            const Boolean ZeroFlagSet = true;
            m_fr.SetFlags(OverflowFlagSet, SignFlagSet, ZeroFlagSet);

            CheckRegisterResult(target, 0, 0x8000, 0x8000, "オペランドの値がレジスタに設定される");

            Assert.AreEqual(OverflowFlagSet, m_fr.OF, "OF の値が保持される");
            Assert.AreEqual(SignFlagSet, m_fr.SF, "SF の値が保持される");
            Assert.AreEqual(ZeroFlagSet, m_fr.ZF, "ZF の値が保持される");
        }
        #endregion // Load/Store

        #region Arithmetic/Logical Operation
        /// <summary>
        /// <see cref="Operator.AddArithmetic"/> のテストです。
        /// </summary>
        [TestMethod]
        public void AddArithmetic()
        {
            Operator target = Operator.AddArithmetic;

            CheckRegisterResult(target, 1111, 2222, 3333, "算術加算の値がレジスタに設定される");

            CheckOverflowFlag(target, 0x7ffe, 0x0001, false, "オーバーフローしない => OF は false");
            CheckOverflowFlag(target, 0x7fff, 0x0001, true, "オーバーフローする => OF は true");

            CheckSignFlag(target, 0xffff, 0x0002, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x0001, 0xfffe, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0x0002, 0xffff, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x0001, 0xffff, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.SubtractArithmetic"/> のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractArithmetic()
        {
            Operator target = Operator.SubtractArithmetic;

            CheckRegisterResult(target, 23456, 12345, 11111, "算術減算の値がレジスタに設定される");

            CheckOverflowFlag(target, 0x7ffe, 0xffff, false, "オーバーフローしない => OF は false");
            CheckOverflowFlag(target, 0x7fff, 0xffff, true, "オーバーフローする => OF は true");

            CheckSignFlag(target, 0xffff, 0xfffe, false, "結果が正の値 => SF は false");
            CheckSignFlag(target, 0x0001, 0x0002, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0x0002, 0x0001, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x0001, 0x0001, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.AddLogical"/> のテストです。
        /// </summary>
        [TestMethod]
        public void AddLogical()
        {
            Operator target = Operator.AddLogical;

            CheckRegisterResult(target, 0x7fff, 0x8000, 0xffff, "論理加算の値がレジスタに設定される");

            CheckOverflowFlag(target, 0x7fff, 0x8000, false, "オーバーフローしない => OF は false");
            CheckOverflowFlag(target, 0x7fff, 0x8001, true, "オーバーフローする => OF は true");

            CheckSignFlag(target, 0xfffe, 0x0002, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0xfffe, 0x0001, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0x0002, 0xffff, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x0001, 0xffff, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.SubtractLogical"/> のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractLogical()
        {
            Operator target = Operator.SubtractLogical;

            CheckRegisterResult(target, 0xffff, 0x8000, 0x7fff, "論理減算の値がレジスタに設定される");

            CheckOverflowFlag(target, 0x7fff, 0x7fff, false, "オーバーフローしない => OF は false");
            CheckOverflowFlag(target, 0x7fff, 0x8000, true, "オーバーフローする => OF は true");

            CheckSignFlag(target, 0x8000, 0x0001, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x7fff, 0xffff, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0x0002, 0x0001, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x0001, 0x0001, true, "結果が 0 => ZF は true");
        }
        #endregion // Arithmetic/Logical Operation

        #region Logic
        /// <summary>
        /// <see cref="Operator.And"/> のテストです。
        /// </summary>
        [TestMethod]
        public void And()
        {
            Operator target = Operator.And;

            CheckRegisterResult(target, 0xc3a5, 0xa5c3, 0x8181, "論理積の値がレジスタに設定される");

            CheckOverflowFlag(target, 0xffff, 0xffff, false, "OF は常に false");

            CheckSignFlag(target, 0xffff, 0x7fff, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0xffff, 0x8000, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0x5a5a, 0x5a5a, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x5a5a, 0xa5a5, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.Or"/> のテストです。
        /// </summary>
        [TestMethod]
        public void Or()
        {
            Operator target = Operator.Or;

            CheckRegisterResult(target, 0xc3a5, 0xa5c3, 0xe7e7, "論理和の値がレジスタに設定される");

            CheckOverflowFlag(target, 0xffff, 0xffff, false, "OF は常に false");

            CheckSignFlag(target, 0x7fff, 0x7fff, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x8000, 0x8000, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0x5a5a, 0xa5a5, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x0000, 0x0000, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.Xor"/> のテストです。
        /// </summary>
        [TestMethod]
        public void Xor()
        {
            Operator target = Operator.Xor;

            CheckRegisterResult(target, 0xc3a5, 0xa5c3, 0x6666, "排他的論理和の値がレジスタに設定される");

            CheckOverflowFlag(target, 0xaaaa, 0x5555, false, "OF は常に false");

            CheckSignFlag(target, 0xaaaa, 0xaaaa, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0xaaaa, 0x5555, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0x5a5a, 0xa5a5, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x5a5a, 0x5a5a, true, "結果が 0 => ZF は true");
        }
        #endregion // Logic

        #region Comparison
        /// <summary>
        /// <see cref="Operator.CompareArithmetic"/> のテストです。
        /// </summary>
        [TestMethod]
        public void CompareArithmetic()
        {
            Operator target = Operator.CompareArithmetic;

            CheckRegisterResult(target, 11111, 22222, 11111, "レジスタの値は変わらない");

            CheckOverflowFlag(target, 0, 0x1234, false, "OF は常に false");

            CheckSignFlag(target, 0x0001, 0xffff, false, "結果が正の値 (1 > -1) => SF は false");
            CheckSignFlag(target, 0x0001, 0x0001, false, "結果が 0 (1 == 1) => SF は false");
            CheckSignFlag(target, 0xffff, 0x0001, true, "結果が負の値 (-1 < 1) => SF は true");

            CheckZeroFlag(target, 0x1111, 0x2222, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x1111, 0x1111, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.CompareLogical"/> のテストです。
        /// </summary>
        [TestMethod]
        public void CompareLogical()
        {
            Operator target = Operator.CompareLogical;

            CheckRegisterResult(target, 33333, 44444, 33333, "レジスタの値は変わらない");

            CheckOverflowFlag(target, 0, 0x1234, false, "OF は常に false");

            CheckSignFlag(target, 0xffff, 0x0001, false, "結果が正の値 (65535 > 1) => SF は false");
            CheckSignFlag(target, 0x0001, 0x0001, false, "結果が 0 (1 == 1) => SF は false");
            CheckSignFlag(target, 0x0001, 0xffff, true, "結果が負の値 (1 < 65535) => SF は true");

            CheckZeroFlag(target, 0x1111, 0x2222, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x1111, 0x1111, true, "結果が 0 => ZF は true");
        }
        #endregion // Comparison

        #region Shift
        /// <summary>
        /// <see cref="Operator.ShiftLeftArithmetic"/> のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftArithmetic()
        {
            Operator target = Operator.ShiftLeftArithmetic;

            CheckRegisterResult(target, 0xaaaa, 1, 0xd554, "左に 1 回算術シフトされる");

            CheckOverflowFlag(target, 0xfffe, 15, false, "OF は最後に送り出されたビットの値: 0");
            CheckOverflowFlag(target, 0x0001, 15, true, "OF は最後に送り出されたビットの値: 1");

            CheckSignFlag(target, 0x7fff, 15, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x8000, 15, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0xffff, 15, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x7fff, 15, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.ShiftRightArithmetic"/> のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightArithmetic()
        {
            Operator target = Operator.ShiftRightArithmetic;

            CheckRegisterResult(target, 0xaaaa, 1, 0xd555, "右に 1 回算術シフトされる");

            CheckOverflowFlag(target, 0xfff7, 4, false, "OF は最後に送り出されたビットの値: 0");
            CheckOverflowFlag(target, 0x0008, 4, true, "OF は最後に送り出されたビットの値: 1");

            CheckSignFlag(target, 0x7fff, 15, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x8000, 15, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0xffff, 15, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x7fff, 15, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.ShiftLeftLogical"/> のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftLogical()
        {
            Operator target = Operator.ShiftLeftLogical;

            CheckRegisterResult(target, 0xaaaa, 1, 0x5554, "左に 1 回論理シフトされる");

            CheckOverflowFlag(target, 0xfffe, 16, false, "OF は最後に送り出されたビットの値: 0");
            CheckOverflowFlag(target, 0x0001, 16, true, "OF は最後に送り出されたビットの値: 1");

            CheckSignFlag(target, 0xffff, 16, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x0001, 15, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0xffff, 15, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0xffff, 16, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// <see cref="Operator.ShiftRightLogical"/> のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightLogical()
        {
            Operator target = Operator.ShiftRightLogical;

            CheckRegisterResult(target, 0xaaaa, 1, 0x5555, "右に 1 回論理シフトされる");

            CheckOverflowFlag(target, 0x7fff, 16, false, "OF は最後に送り出されたビットの値: 0");
            CheckOverflowFlag(target, 0x8000, 16, true, "OF は最後に送り出されたビットの値: 1");

            CheckSignFlag(target, 0xffff, 16, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x8000, 0, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0xffff, 15, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0xffff, 16, true, "結果が 0 => ZF は true");
        }
        #endregion // Shift

        #region Jump
        /// <summary>
        /// <see cref="Operator.JumpOnMinus"/> のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnMinus()
        {
            Operator target = Operator.JumpOnMinus;

            CheckJump(target, DontCareBool, true, DontCareBool, true, "SF=1 => 分岐する");
            CheckJump(target, DontCareBool, false, DontCareBool, false, "SF=0 => 分岐しない");
        }

        /// <summary>
        /// <see cref="Operator.JumpOnNonZero"/> のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnNonZero()
        {
            Operator target = Operator.JumpOnNonZero;

            CheckJump(target, DontCareBool, DontCareBool, false, true, "ZF=0 => 分岐する");
            CheckJump(target, DontCareBool, DontCareBool, true, false, "ZF=1 => 分岐しない");
        }

        /// <summary>
        /// <see cref="Operator.JumpOnZero"/> のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnZero()
        {
            Operator target = Operator.JumpOnZero;

            CheckJump(target, DontCareBool, DontCareBool, true, true, "ZF=1 => 分岐する");
            CheckJump(target, DontCareBool, DontCareBool, false, false, "ZF=0 => 分岐しない");
        }

        /// <summary>
        /// <see cref="Operator.UnconditionalJump"/> のテストです。
        /// </summary>
        [TestMethod]
        public void UnconditionalJump()
        {
            Operator target = Operator.UnconditionalJump;

            CheckJump(target, DontCareBool, DontCareBool, DontCareBool, true, "無条件で分岐する");
        }

        /// <summary>
        /// <see cref="Operator.JumpOnPlus"/> のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnPlus()
        {
            Operator target = Operator.JumpOnPlus;

            CheckJump(target, DontCareBool, false, false, true, "SF=0 かつ ZF=0 => 分岐する");
            CheckJump(target, DontCareBool, true, false, false, "SF=1 => 分岐しない");
            CheckJump(target, DontCareBool, false, true, false, "ZF=1 => 分岐しない");
        }

        /// <summary>
        /// <see cref="Operator.JumpOnOverflow"/> のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnOverflow()
        {
            Operator target = Operator.JumpOnOverflow;

            CheckJump(target, true, DontCareBool, DontCareBool, true, "OF=1 => 分岐する");
            CheckJump(target, false, DontCareBool, DontCareBool, false, "OF=0 => 分岐しない");
        }
        #endregion // Jump

        #region Stack Operation
        /// <summary>
        /// <see cref="Operator.Push"/> のテストです。
        /// </summary>
        [TestMethod]
        public void Push()
        {
            CheckPush(0x8000, 0x1234, 0x7fff, "SP の値を 1 減らし、そのアドレスにオペランドの値を書き込む");
            CheckPush(0x0000, 0x5678, 0xffff, "SP が 0x0000 の場合、1 減らした値は 0xffff になる。");
        }

        private void CheckPush(UInt16 spValue, UInt16 oprValue, UInt16 expectedSp, String message)
        {
            CpuRegister sp = m_registerSet.SP;
            sp.SetValue(spValue);

            Operate(Operator.Push, DontCareUInt16, oprValue);

            CheckRegister(sp, expectedSp, "SP の値が 1 減る: " + message);
            CheckMemory(expectedSp, oprValue, "SP の指すアドレスにオペランドの値が書き込まれる: " + message);
        }

        /// <summary>
        /// <see cref="Operator.Pop"/> のテストです。
        /// </summary>
        [TestMethod]
        public void Pop()
        {
            CheckPop(0x7fff, 0x2468, 0x8000, "SP の指すアドレスから値をレジスタに読み込み、SP の値を 1 増やす");
            CheckPop(0xffff, 0x1357, 0x0000, "SP が 0xffff の場合、1 増やした値は 0x0000 になる。");
        }

        private void CheckPop(UInt16 spValue, UInt16 memValue, UInt16 expectedSp, String message)
        {
            CpuRegister sp = m_registerSet.SP;
            sp.SetValue(spValue);
            m_memory.Write(spValue, memValue);

            Operate(Operator.Pop, DontCareUInt16, DontCareUInt16);

            CheckRegister(m_gr, memValue, "SP の指すアドレスから値が指定のレジスタに読み込まれる: " + message);
            CheckRegister(sp, expectedSp, "SP の値が 1 増える: " + message);
        }
        #endregion // Stack Operation

        #region Call/Return
        /// <summary>
        /// <see cref="Operator.CallSubroutine"/> のテストです。
        /// </summary>
        [TestMethod]
        public void CallSubroutine()
        {
            const UInt16 PrValue = 0x1357;
            const UInt16 OprValue = 0x9bdf;
            SP.SetValue(SpValue);
            PR.SetValue(PrValue);

            Operate(Operator.CallSubroutine, DontCareUInt16, OprValue);

            CpuRegisterTest.Check(SP, SpValueMinusOne, "SP の値が 1 減る");
            MemoryTest.Check(m_memory, SpValueMinusOne, PrValue, "(SP - 1) に PR の値が書き込まれる");
            CpuRegisterTest.Check(PR, OprValue, "PR にオペランドの値が設定される");
        }

        /// <summary>
        /// ReturnFromSubroutine でキャンセルしない場合の単体テストです。
        /// </summary>
        [TestMethod]
        public void ReturnFromSubroutine_NotCancel()
        {
            const UInt16 MemValue = 0xace0;
            SP.SetValue(SpValue);
            m_memory.Write(SpValue, MemValue);
            m_cancelRet = false;

            Operate(Operator.ReturnFromSubroutine, DontCareUInt16, DontCareUInt16);

            CpuRegisterTest.Check(PR, MemValue, "PR に SP の指すアドレスの値が設定される");
            CpuRegisterTest.Check(SP, SpValuePlusOne, "SP の値が 1 増える");

            String expectedLog = TestLogger.ExpectedLog("OnReturningFromSubroutine: 'SP: 9320 (0x2468)'");
            String actualLog = m_logger.Log;
            Assert.AreEqual(expectedLog, actualLog, "ReturningFromSubroutine イベントが発生する");
        }

        /// <summary>
        /// ReturnFromSubroutine でキャンセルする場合の単体テストです。
        /// </summary>
        [TestMethod]
        public void ReturnFromSubroutine_Cancel()
        {
            const UInt16 PrValue = 0xcdef;
            PR.SetValue(PrValue);
            SP.SetValue(SpValue);
            m_cancelRet = true;

            Operate(Operator.ReturnFromSubroutine, DontCareUInt16, DontCareUInt16);

            CpuRegisterTest.Check(PR, PrValue, "PR の値はそのまま");
            CpuRegisterTest.Check(SP, SpValue, "SP の値はそのまま");

            String expectedLog = TestLogger.ExpectedLog("OnReturningFromSubroutine: 'SP: 9320 (0x2468)'");
            String actualLog = m_logger.Log;
            Assert.AreEqual(expectedLog, actualLog, "ReturningFromSubroutine イベントが発生する");
        }
        #endregion // Call/Return

        #region Others
        /// <summary>
        /// <see cref="Operator.SuperVisorCall"/> のテストです。
        /// </summary>
        [TestMethod]
        public void SuperVisorCall()
        {
            const UInt16 OprValue = 0xabcd;
            Operate(Operator.SuperVisorCall, DontCareUInt16, OprValue);

            String expected = TestLogger.ExpectedLog("OnCallingSuperVisor: operand='43981 (0xabcd)'");
            String actual = m_logger.Log;
            Assert.AreEqual(expected, actual, "CallingSuperVisor イベントが発生する");
        }

        /// <summary>
        /// <see cref="Operator.NoOperation"/> のテストです。
        /// </summary>
        [TestMethod]
        public void NoOperation()
        {
            Operate(Operator.NoOperation, DontCareUInt16, DontCareUInt16);
            // 何もしない。
        }
        #endregion // Others

        #region Check
        private void CheckRegisterResult(
            Operator op, UInt16 regValue, UInt16 oprValue, UInt16 expected, String message)
        {
            Operate(op, regValue, oprValue);
            CheckRegister(m_gr, expected, message);
        }

        private void CheckMemoryResult(
            Operator op, UInt16 regValue, UInt16 oprValue, UInt16 expected, String message)
        {
            Operate(op, regValue, oprValue);
            CheckMemory(oprValue, expected, message);
        }

        private void CheckOverflowFlag(
            Operator op, UInt16 regValue, UInt16 oprValue, Boolean expected, String message)
        {
            Operate(op, regValue, oprValue);
            Boolean actual = m_fr.OF;
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckSignFlag(
            Operator op, UInt16 regValue, UInt16 oprValue, Boolean expected, String message)
        {
            Operate(op, regValue, oprValue);
            Boolean actual = m_fr.SF;
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckZeroFlag(
            Operator op, UInt16 regValue, UInt16 oprValue, Boolean expected, String message)
        {
            Operate(op, regValue, oprValue);
            Boolean actual = m_fr.ZF;
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckJump(
            Operator op, Boolean overflowFlag, Boolean signFlag, Boolean zeroFlag, Boolean jump, String message)
        {
            const UInt16 PRValue = 0x1111;
            const UInt16 OperandValue = 0x2222;
            const UInt16 DontCare = 0;

            m_fr.SetFlags(overflowFlag, signFlag, zeroFlag);
            m_pr.SetValue(PRValue);
            Operate(op, DontCare, OperandValue);

            UInt16 expected = jump ? OperandValue : PRValue;
            CheckRegister(m_pr, expected, message);
        }

        private void CheckRegister(CpuRegister reg, UInt16 expected, String message)
        {
            CpuRegisterTest.Check(reg, expected, message);
        }

        private void CheckMemory(UInt16 address, UInt16 expected, String message)
        {
            MemoryTest.Check(m_memory, address, expected, message);
        }

        private void Operate(Operator op, UInt16 regValue, UInt16 oprValue)
        {
            m_gr.Value = new Word(regValue);
            Word operand = new Word(oprValue);
            op.Operate(m_gr, operand, m_registerSet, m_memory);
        }

        private CpuRegister SP
        {
            get { return m_registerSet.SP; }
        }

        private CpuRegister PR
        {
            get { return m_registerSet.PR; }
        }
        #endregion // Check

        #region EventHandlers
        private void OnReturningFromSubroutine(Object sender, ReturningFromSubroutineEventArgs e)
        {
            m_logger.Add("OnReturningFromSubroutine: '{0}'", e.SP);
            e.Cancel = m_cancelRet;
        }

        private void OnCallingSuperVisor(Object sender, CallingSuperVisorEventArgs e)
        {
            m_logger.Add("OnCallingSuperVisor: operand='{0}'", e.Operand);
        }
        #endregion // EventHandlers
    }
}
