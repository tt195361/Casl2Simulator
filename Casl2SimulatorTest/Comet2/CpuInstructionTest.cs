using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// <see cref="CpuInstruction"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class CpuInstructionTest
    {
        #region Instance Fields
        private CpuRegisterSet m_registerSet;
        private Memory m_memory;
        private TestLogger m_logger;

        // 命令語の次のアドレス。
        private const UInt16 NextAddress = 100;
        private const UInt16 R = 3;
        private const UInt16 X = 4;
        private const UInt16 R1 = 5;
        private const UInt16 R2 = 6;
        private const UInt16 Adr = 12345;
        private const UInt16 Offset = 23456;
        private const UInt16 EffectiveAddress = Adr + Offset;

        private const UInt16 SpValue = 0x789a;
        private const UInt16 SpValueMinusOne = SpValue - 1;
        private const UInt16 SpValuePlusOne = SpValue + 1;

        private const UInt16 NextAddressPlusOne = NextAddress + 1;
        private const Boolean DontCareBool = false;
        private const UInt16 DontCareUInt16 = 0;
        #endregion

        #region TestInitialize/TestCleanup
        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new CpuRegisterSet();
            m_memory = new Memory();
            m_logger = new TestLogger();

            CpuInstruction.ReturningFromSubroutine += OnReturningFromSubroutine;
            CpuInstruction.CallingSuperVisor += OnCallingSuperVisor;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            CpuInstruction.ReturningFromSubroutine -= OnReturningFromSubroutine;
            CpuInstruction.CallingSuperVisor -= OnCallingSuperVisor;
        }
        #endregion // TestInitialize/TestCleanup

        #region Load/Store
        /// <summary>
        /// <see cref="CpuInstruction.LoadEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void LoadEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.LoadEaContents, DontCareUInt16, 1357, 1357,
                "実効アドレスの内容がレジスタに設定される");
        }

        /// <summary>
        /// <see cref="CpuInstruction.Store"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void Store()
        {
            const UInt16 RegValue = 345;

            CheckEaContentsMemory(
                CpuInstruction.Store, RegValue, DontCareUInt16, EffectiveAddress, RegValue,
                "レジスタの内容が実効アドレスに書き込まれる");
        }

        /// <summary>
        /// <see cref="CpuInstruction.LoadEffectiveAddress"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void LoadEffectiveAddress()
        {
            CheckEaContentsRegister(
                CpuInstruction.LoadEffectiveAddress, DontCareUInt16, DontCareUInt16, EffectiveAddress,
                "実効アドレスがレジスタに設定される");
        }

        /// <summary>
        /// <see cref="CpuInstruction.LoadRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void LoadRegister()
        {
            const UInt16 R2Value = 13579;

            CheckRegisterRegister(
                CpuInstruction.LoadRegister, DontCareUInt16, R2Value, R2Value,
                "指定のレジスタの内容がレジスタに設定される");
        }
        #endregion // Load/Store

        #region Arithmetic/Logical Operation
        /// <summary>
        /// <see cref="CpuInstruction.AddArithmeticEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddArithmeticEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.AddArithmeticEaContents, 32767, 1, 32768,
                "実効アドレスの内容がレジスタに算術加算される");
            Assert.IsTrue(m_registerSet.FR.OF, "算術だと -32768..32767 なのでオーバーフローする");
        }

        /// <summary>
        /// <see cref="CpuInstruction.SubtractArithmeticEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractArithmeticEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.SubtractArithmeticEaContents, 1, 2, 0xffff,
                "実効アドレスの内容がレジスタから算術減算される");
            Assert.IsFalse(m_registerSet.FR.OF, "算術だと -32768..32767 なのでオーバーフローしない");
        }

        /// <summary>
        /// <see cref="CpuInstruction.AddLogicalEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddLogicalEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.AddLogicalEaContents, 32767, 1, 32768,
                "実効アドレスの内容がレジスタに論理加算される");
            Assert.IsFalse(m_registerSet.FR.OF, "論理だと 0..65535 なのでオーバーフローしない");
        }

        /// <summary>
        /// <see cref="CpuInstruction.SubtractLogicalEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractLogicalEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.SubtractLogicalEaContents, 1, 2, 0xffff,
                "実効アドレスの内容がレジスタから論理減算される");
            Assert.IsTrue(m_registerSet.FR.OF, "論理だと 0..65535 なのでオーバーフローする");
        }

        /// <summary>
        /// <see cref="CpuInstruction.AddArithmeticRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddArithmeticRegister()
        {
            CheckRegisterRegister(
                CpuInstruction.AddArithmeticRegister, 10000, 22768, 32768,
                "レジスタ2の内容がレジスタ1に算術加算される");
            Assert.IsTrue(m_registerSet.FR.OF, "算術だと -32768..32767 なのでオーバーフローする");
        }

        /// <summary>
        /// <see cref="CpuInstruction.SubtractArithmeticRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractArithmeticRegister()
        {
            CheckRegisterRegister(
                CpuInstruction.SubtractArithmeticRegister, 0x7ffe, 0x7fff, 0xffff,
                "レジスタ2の内容がレジスタ1から算術減算される");
            Assert.IsFalse(m_registerSet.FR.OF, "算術だと -32768..32767 なのでオーバーフローしない");
        }

        /// <summary>
        /// <see cref="CpuInstruction.AddLogicalRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddLogicalRegister()
        {
            CheckRegisterRegister(
                CpuInstruction.AddLogicalRegister, 10000, 22768, 32768,
                "レジスタ2の内容がレジスタ1に論理加算される");
            Assert.IsFalse(m_registerSet.FR.OF, "論理だと 0..65535 なのでオーバーフローしない");
        }

        /// <summary>
        /// <see cref="CpuInstruction.SubtractLogicalRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractLogicalRegister()
        {
            CheckRegisterRegister(
                CpuInstruction.SubtractLogicalRegister, 0x7ffe, 0x7fff, 0xffff,
                "レジスタ2の内容がレジスタ1から論理減算される");
            Assert.IsTrue(m_registerSet.FR.OF, "論理だと 0..65535 なのでオーバーフローする");
        }
        #endregion // Arithmetic/Logical Operation

        #region Logic
        /// <summary>
        /// <see cref="CpuInstruction.AndEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AndEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.AndEaContents, 0x137f, 0x5555, 0x1155,
                "実効アドレスの内容とレジスタの論理積を求める");
        }

        /// <summary>
        /// <see cref="CpuInstruction.OrEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void OrEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.OrEaContents, 0x137f, 0x5555, 0x577f,
                "実効アドレスの内容とレジスタの論理和を求める");
        }

        /// <summary>
        /// <see cref="CpuInstruction.XorEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void XorEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.XorEaContents, 0x137f, 0x5555, 0x462a,
                "実効アドレスの内容とレジスタの排他的論理和を求める");
        }

        /// <summary>
        /// <see cref="CpuInstruction.AndRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AndRegister()
        {
            CheckRegisterRegister(
                CpuInstruction.AndRegister, 0x8cef, 0x5555, 0x0445,
                "レジスタ2の内容がレジスタ1に論理積される");
        }

        /// <summary>
        /// <see cref="CpuInstruction.OrRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void OrRegister()
        {
            CheckRegisterRegister(
                CpuInstruction.OrRegister, 0x8cef, 0x5555, 0xddff,
                "レジスタ2の内容がレジスタ1に論理和される");
        }

        /// <summary>
        /// <see cref="CpuInstruction.XorRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void XorRegister()
        {
            CheckRegisterRegister(
                CpuInstruction.XorRegister, 0x8cef, 0x5555, 0xd9ba,
                "レジスタ2の内容がレジスタ1に排他的論理和される");
        }
        #endregion

        #region Comparison
        /// <summary>
        /// <see cref="CpuInstruction.CompareArithmeticEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareArithmeticEaContents()
        {
            CheckEaContentsFlags(
                CpuInstruction.CompareArithmeticEaContents, 0xffff, 0x0001, false, true, false,
                "実効アドレスの内容とレジスタを算術比較し FR を設定する。" +
                "-1 (0xffff) < 1 (0x0001) なので、サインフラグが設定され true になる");
        }

        /// <summary>
        /// <see cref="CpuInstruction.CompareLogicalEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareLogicalEaContents()
        {
            CheckEaContentsFlags(
                CpuInstruction.CompareLogicalEaContents, 0x0001, 0xffff, false, true, false,
                "実効アドレスの内容とレジスタを論理比較し FR を設定する。" +
                "1 (0x0001) < 65535 (0xffff) なので、サインフラグが設定され true になる");
        }

        /// <summary>
        /// <see cref="CpuInstruction.CompareArithmeticRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareArithmeticRegister()
        {
            CheckRegisterFlags(
                CpuInstruction.CompareArithmeticRegister, 0x8000, 0x7fff, false, true, false,
                "指定のレジスタの内容を算術比較し FR を設定する。" +
                "-32768 (0x8000) < 32767 (0x7fff) なので、サインフラグが設定され true になる");
        }

        /// <summary>
        /// <see cref="CpuInstruction.CompareLogicalRegister"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareLogicalRegister()
        {
            CheckRegisterFlags(
                CpuInstruction.CompareLogicalRegister, 0x7fff, 0x8000, false, true, false,
                "指定のレジスタの内容を論理比較し FR を設定する。" +
                "32767 (0x7fff) < 32768 (0x8000) なので、サインフラグが設定され true になる");
        }
        #endregion // Comparison

        #region Shift
        /// <summary>
        /// <see cref="CpuInstruction.ShiftLeftArithmeticEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftArithmeticEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.ShiftLeftArithmeticEaContents, 0xaaaa, 1, 0xd554,
                "レジスタの内容が実効アドレス回だけ左に算術シフトされる");
        }

        /// <summary>
        /// <see cref="CpuInstruction.ShiftRightArithmeticEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightArithmeticEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.ShiftRightArithmeticEaContents, 0xaaaa, 1, 0xd555,
                "レジスタの内容が実効アドレス回だけ右に算術シフトされる");
        }

        /// <summary>
        /// <see cref="CpuInstruction.ShiftLeftLogicalEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftLogicalEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.ShiftLeftLogicalEaContents, 0x5555, 1, 0xaaaa,
                "レジスタの内容が実効アドレス回だけ左に論理シフトされる");
        }

        /// <summary>
        /// <see cref="CpuInstruction.ShiftRightLogicalEaContents"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightLogicalEaContents()
        {
            CheckEaContentsRegister(
                CpuInstruction.ShiftRightLogicalEaContents, 0xaaaa, 1, 0x5555,
                "レジスタの内容が実効アドレス回だけ右に論理シフトされる");
        }
        #endregion // Shift

        #region Jump
        /// <summary>
        /// <see cref="CpuInstruction.JumpOnMinus"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnMinus()
        {
            CheckJump(
                CpuInstruction.JumpOnMinus, DontCareBool, true, DontCareBool,
                true, "JumpOnMinus: SF=1 => 分岐する");
            CheckJump(
                CpuInstruction.JumpOnMinus, DontCareBool, false, DontCareBool,
                false, "JumpOnMinus: SF=0 => 分岐しない");
        }

        /// <summary>
        /// <see cref="CpuInstruction.JumpOnNonZero"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnNonZero()
        {
            CheckJump(
                CpuInstruction.JumpOnNonZero, DontCareBool, DontCareBool, false, 
                true, "JumpOnNonZero: ZF=0 => 分岐する");
            CheckJump(
                CpuInstruction.JumpOnNonZero, DontCareBool, DontCareBool, true, 
                false, "JumpOnNonZero: ZF=1 => 分岐しない");
        }

        /// <summary>
        /// <see cref="CpuInstruction.JumpOnZero"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnZero()
        {
            CheckJump(
                CpuInstruction.JumpOnZero, DontCareBool, DontCareBool, true,
                true, "JumpOnZero: ZF=1 => 分岐する");
            CheckJump(
                CpuInstruction.JumpOnZero, DontCareBool, DontCareBool, false,
                false, "JumpOnZero: ZF=0 => 分岐しない");
        }

        /// <summary>
        /// <see cref="CpuInstruction.UnconditionalJump"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void UnconditionalJump()
        {
            CheckJump(
                CpuInstruction.UnconditionalJump, DontCareBool, DontCareBool, DontCareBool,
                true, "UnconditionalJump: 無条件で分岐する");
        }

        /// <summary>
        /// <see cref="CpuInstruction.JumpOnPlus"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnPlus()
        {
            CheckJump(
                CpuInstruction.JumpOnPlus, DontCareBool, false, false,
                true, "JumpOnPlus: SF=0 かつ ZF=0 => 分岐する");
            CheckJump(
                CpuInstruction.JumpOnPlus, DontCareBool, true, true,
                false, "JumpOnPlus: SF=1 あるいは ZF=1 => 分岐しない");
        }

        /// <summary>
        /// <see cref="CpuInstruction.JumpOnOverflow"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnOverflow()
        {
            CheckJump(
                CpuInstruction.JumpOnOverflow, true, DontCareBool, DontCareBool,
                true, "JumpOnOverflow: OF=1 => 分岐する");
            CheckJump(
                CpuInstruction.JumpOnOverflow, false, DontCareBool, DontCareBool,
                false, "JumpOnOverflow: OF=0 => 分岐しない");
        }
        #endregion // Jump

        #region Stack Operation
        /// <summary>
        /// <see cref="CpuInstruction.Push"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void Push()
        {
            SP.SetValue(SpValue);

            ExecuteEaContentsInstruction(CpuInstruction.Push, DontCareUInt16, DontCareUInt16);

            CpuRegisterTest.Check(
                SP, SpValueMinusOne, "SP の値が 1 減る");
            MemoryTest.Check(
                m_memory, SpValueMinusOne, EffectiveAddress,
                "SP の指すアドレスに実効アドレスの値を書き込む");
        }

        /// <summary>
        /// <see cref="CpuInstruction.Pop"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void Pop()
        {
            const UInt16 PopValue = 0xbcde;
            SP.SetValue(SpValue);
            m_memory.Write(SpValue, PopValue);

            ExecuteRegisterInstruction(CpuInstruction.Pop, DontCareUInt16, DontCareUInt16);

            CpuRegisterTest.Check(
                m_registerSet.GR[R1], PopValue, "SP の指すアドレスの値がレジスタに読み込まれる");
            CpuRegisterTest.Check(
                SP, SpValuePlusOne, "SP の値が 1 増える");
        }
        #endregion // Stack Operation

        #region Call/Ret
        /// <summary>
        /// <see cref="CpuInstruction.CallSubroutine"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CallSubroutine()
        {
            SP.SetValue(SpValue);
            ExecuteEaContentsInstruction(CpuInstruction.CallSubroutine, DontCareUInt16, DontCareUInt16);

            CpuRegisterTest.Check(
                SP, SpValueMinusOne, "SP の値が 1 減る");
            MemoryTest.Check(
                m_memory, SpValueMinusOne, NextAddressPlusOne,　"SP の指すアドレスに PR の値を書き込む");
            CpuRegisterTest.Check(
                PR, EffectiveAddress, "PR に実効アドレスの値が設定される");
        }

        /// <summary>
        /// <see cref="CpuInstruction.ReturnFromSubroutine"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ReturnFromSubroutine()
        {
            const UInt16 MemValue = 0x9876;
            SP.SetValue(SpValue);
            m_memory.Write(SpValue, MemValue);

            ExecuteRegisterInstruction(CpuInstruction.ReturnFromSubroutine, DontCareUInt16, DontCareUInt16);

            CpuRegisterTest.Check(
                PR, MemValue, "SP の指すアドレスの値が PR に読み込まれる");
            CpuRegisterTest.Check(
                SP, SpValuePlusOne, "SP の値が 1 増える");

            String expectedLog = TestLogger.ExpectedLog("OnReturningFromSubroutine: 'SP: 30874 (0x789a)'");
            String actualLog = m_logger.Log;
            Assert.AreEqual(expectedLog, actualLog, "ReturningFromSubroutine イベントが発生する");
        }
        #endregion // Call/Ret

        #region Others
        /// <summary>
        /// <see cref="CpuInstruction.SuperVisorCall"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void SuperVisorCall()
        {
            ExecuteEaContentsInstruction(CpuInstruction.SuperVisorCall, DontCareUInt16, DontCareUInt16);

            String expectedLog = TestLogger.ExpectedLog("OnCallingSuperVisor: operand='35801 (0x8bd9)'");
            String actualLog = m_logger.Log;
            Assert.AreEqual(expectedLog, actualLog, "CallingSuperVisor イベントが発生する");
        }

        /// <summary>
        /// <see cref="CpuInstruction.NoOperation"/> 命令のテストです。
        /// </summary>
        [TestMethod]
        public void NoOperation()
        {
            ExecuteRegisterInstruction(CpuInstruction.NoOperation, DontCareUInt16, DontCareUInt16);
            // 何もしない。
        }
        #endregion // Others

        #region Check
        private void CheckEaContentsRegister(
            CpuInstruction instruction, UInt16 regValue, UInt16 eaContents, UInt16 expected, String message)
        {
            ExecuteEaContentsInstruction(instruction, regValue, eaContents);
            CpuRegisterTest.Check(m_registerSet.GR[R], expected, message);
        }

        private void CheckEaContentsMemory(
            CpuInstruction instruction, UInt16 regValue, UInt16 eaContents, UInt16 address,
            UInt16 expected, String message)
        {
            ExecuteEaContentsInstruction(instruction, regValue, eaContents);
            MemoryTest.Check(m_memory, address, expected, message);
        }

        private void CheckEaContentsFlags(
            CpuInstruction instruction, UInt16 regValue, UInt16 eaContents,
            Boolean expectedOverflow, Boolean expectedSign, Boolean expectedZero, String message)
        {
            ExecuteEaContentsInstruction(instruction, regValue, eaContents);
            CheckFlags(expectedOverflow, expectedSign, expectedZero, message);
        }

        private void CheckRegisterFlags(
            CpuInstruction instruction, UInt16 reg1Value, UInt16 reg2Value,
            Boolean expectedOverflow, Boolean expectedSign, Boolean expectedZero, String message)
        {
            ExecuteRegisterInstruction(instruction, reg1Value, reg2Value);
            CheckFlags(expectedOverflow, expectedSign, expectedZero, message);
        }

        private void CheckFlags(
            Boolean expectedOverflow, Boolean expectedSign, Boolean expectedZero, String message)
        {
            CheckFlag(expectedOverflow, m_registerSet.FR.OF, "Overflow: " + message);
            CheckFlag(expectedSign, m_registerSet.FR.SF, "Sign: " + message);
            CheckFlag(expectedZero, m_registerSet.FR.ZF, "Zero: " + message);
        }

        private void CheckFlag(Boolean expected, Boolean actual, String message)
        {
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckJump(
            CpuInstruction instruction, Boolean overflowFlag, Boolean signFlag, Boolean zeroFlag,
            Boolean jump, String message)
        {
            m_registerSet.FR.SetFlags(overflowFlag, signFlag, zeroFlag);
            ExecuteEaContentsInstruction(instruction, DontCareUInt16, DontCareUInt16);

            UInt16 expected = jump ? EffectiveAddress : NextAddressPlusOne;
            CpuRegisterTest.Check(m_registerSet.PR, expected, message);
        }

        private void ExecuteEaContentsInstruction(CpuInstruction instruction, UInt16 regValue, UInt16 eaContents)
        {
            // 命令語の次のアドレスに adr, 実効アドレスの内容、GRx にオフセットの値を書き込みます。
            m_memory.Write(NextAddress, Adr);
            m_memory.Write(EffectiveAddress, eaContents);
            m_registerSet.GR[X].SetValue(Offset);

            // レジスタと PR に値を設定し、命令を実行します。
            m_registerSet.GR[R].SetValue(regValue);
            m_registerSet.PR.SetValue(NextAddress);
            instruction.Execute(R, X, m_registerSet, m_memory);
        }

        private void CheckRegisterRegister(
            CpuInstruction instruction, UInt16 reg1Value, UInt16 reg2Value, UInt16 expected, String message)
        {
            ExecuteRegisterInstruction(instruction, reg1Value, reg2Value);
            CpuRegisterTest.Check(m_registerSet.GR[R1], expected, message);
        }

        private void ExecuteRegisterInstruction(CpuInstruction instruction, UInt16 reg1Value, UInt16 reg2Value)
        {
            // レジスタに値を設定し、命令を実行します。
            m_registerSet.GR[R1].SetValue(reg1Value);
            m_registerSet.GR[R2].SetValue(reg2Value);
            instruction.Execute(R1, R2, m_registerSet, m_memory);
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

        #region ToString
        /// <summary>
        /// ToString メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            CheckToString(CpuInstruction.LoadEaContents, "LD r,adr,x");
            CheckToString(CpuInstruction.Store, "ST r,adr,x");
            CheckToString(CpuInstruction.LoadEffectiveAddress, "LAD r,adr,x");
            CheckToString(CpuInstruction.LoadRegister, "LD r1,r2");

            CheckToString(CpuInstruction.AddArithmeticEaContents, "ADDA r,adr,x");
            CheckToString(CpuInstruction.SubtractArithmeticEaContents, "SUBA r,adr,x");
            CheckToString(CpuInstruction.AddLogicalEaContents, "ADDL r,adr,x");
            CheckToString(CpuInstruction.SubtractLogicalEaContents, "SUBL r,adr,x");
            CheckToString(CpuInstruction.AddArithmeticRegister, "ADDA r1,r2");
            CheckToString(CpuInstruction.SubtractArithmeticRegister, "SUBA r1,r2");
            CheckToString(CpuInstruction.AddLogicalRegister, "ADDL r1,r2");
            CheckToString(CpuInstruction.SubtractLogicalRegister, "SUBL r1,r2");

            CheckToString(CpuInstruction.AndEaContents, "AND r,adr,x");
            CheckToString(CpuInstruction.OrEaContents, "OR r,adr,x");
            CheckToString(CpuInstruction.XorEaContents, "XOR r,adr,x");
            CheckToString(CpuInstruction.AndRegister, "AND r1,r2");
            CheckToString(CpuInstruction.OrRegister, "OR r1,r2");
            CheckToString(CpuInstruction.XorRegister, "XOR r1,r2");

            CheckToString(CpuInstruction.CompareArithmeticEaContents, "CPA r,adr,x");
            CheckToString(CpuInstruction.CompareLogicalEaContents, "CPL r,adr,x");
            CheckToString(CpuInstruction.CompareArithmeticRegister, "CPA r1,r2");
            CheckToString(CpuInstruction.CompareLogicalRegister, "CPL r1,r2");

            CheckToString(CpuInstruction.ShiftLeftArithmeticEaContents, "SLA r,adr,x");
            CheckToString(CpuInstruction.ShiftRightArithmeticEaContents, "SRA r,adr,x");
            CheckToString(CpuInstruction.ShiftLeftLogicalEaContents, "SLL r,adr,x");
            CheckToString(CpuInstruction.ShiftRightLogicalEaContents, "SRL r,adr,x");

            CheckToString(CpuInstruction.JumpOnMinus, "JMI adr,x");
            CheckToString(CpuInstruction.JumpOnNonZero, "JNZ adr,x");
            CheckToString(CpuInstruction.JumpOnZero, "JZE adr,x");
            CheckToString(CpuInstruction.UnconditionalJump, "JUMP adr,x");
            CheckToString(CpuInstruction.JumpOnPlus, "JPL adr,x");
            CheckToString(CpuInstruction.JumpOnOverflow, "JOV adr,x");

            CheckToString(CpuInstruction.Push, "PUSH adr,x");
            CheckToString(CpuInstruction.Pop, "POP r");

            CheckToString(CpuInstruction.CallSubroutine, "CALL adr,x");
            CheckToString(CpuInstruction.ReturnFromSubroutine, "RET");

            CheckToString(CpuInstruction.SuperVisorCall, "SVC adr,x");
            CheckToString(CpuInstruction.NoOperation, "NOP");
        }

        private void CheckToString(CpuInstruction instruction, String expected)
        {
            String actual = instruction.ToString();
            Assert.AreEqual(expected, actual);
        }
        #endregion // ToString

        #region EventHandlers
        private void OnReturningFromSubroutine(Object sender, ReturningFromSubroutineEventArgs e)
        {
            m_logger.Add("OnReturningFromSubroutine: '{0}'", e.SP);
            e.Cancel = false;
        }

        private void OnCallingSuperVisor(Object sender, CallingSuperVisorEventArgs e)
        {
            m_logger.Add("OnCallingSuperVisor: operand='{0}'", e.Operand);
        }
        #endregion // EventHandlers
    }
}
