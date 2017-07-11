﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Instruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class InstructionTest
    {
        #region Fields
        private RegisterSet m_registerSet;
        private Memory m_memory;

        // 命令語の次のアドレス。
        private const UInt16 NextAddress = 100;
        private const UInt16 R = 3;
        private const UInt16 X = 4;
        private const UInt16 Adr = 12345;
        private const UInt16 Offset = 23456;
        private const UInt16 EffectiveAddress = Adr + Offset;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new RegisterSet();
            m_memory = new Memory();
        }

        /// <summary>
        /// LoadEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void LoadEaContents()
        {
            CheckEaContentsRegister(
                Instruction.LoadEaContents, 0, 1357, 1357,
                "実効アドレスの内容がレジスタに設定される");
        }

        /// <summary>
        /// Store 命令のテストです。
        /// </summary>
        [TestMethod]
        public void Store()
        {
            CheckEaContentsMemory(
                Instruction.Store, 345, 0, EffectiveAddress, 345,
                "レジスタの内容が実効アドレスに書き込まれる");
        }

        /// <summary>
        /// AddArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddArithmeticEaContents()
        {
            CheckEaContentsRegister(
                Instruction.AddArithmeticEaContents, 1234, 2345, 3579,
                "実効アドレスの内容がレジスタに算術加算される");
        }

        /// <summary>
        /// CompareArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareArithmeticEaContents()
        {
            CheckEaContentsFlags(
                Instruction.CompareArithmeticEaContents, 0xffff, 0x0001, false, true, false,
                "実効アドレスの内容とレジスタを算術比較し FR を設定する。" +
                "-1 (0xffff) < 1 (0x0001) なので、サインフラグが設定され true になる");
        }

        /// <summary>
        /// CompareLogicalEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareLogicalEaContents()
        {
            CheckEaContentsFlags(
                Instruction.CompareLogicalEaContents, 0x0001, 0xffff, false, true, false,
                "実効アドレスの内容とレジスタを論理比較し FR を設定する。" +
                "1 (0x0001) < 65535 (0xffff) なので、サインフラグが設定され true になる");
        }

        /// <summary>
        /// ShiftLeftArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftArithmeticEaContents()
        {
            CheckEaContentsRegister(
                Instruction.ShiftLeftArithmeticEaContents, 0xaaaa, 1, 0xd554,
                "レジスタの内容が実効アドレス回だけ左に算術シフトされる");
        }

        /// <summary>
        /// ShiftRightArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightArithmeticEaContents()
        {
            CheckEaContentsRegister(
                Instruction.ShiftRightArithmeticEaContents, 0xaaaa, 1, 0xd555,
                "レジスタの内容が実効アドレス回だけ右に算術シフトされる");
        }

        /// <summary>
        /// ShiftLeftLogicalEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftLogicalEaContents()
        {
            CheckEaContentsRegister(
                Instruction.ShiftLeftLogicalEaContents, 0x5555, 1, 0xaaaa,
                "レジスタの内容が実効アドレス回だけ左に論理シフトされる");
        }

        /// <summary>
        /// ShiftRightLogicalEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightLogicalEaContents()
        {
            CheckEaContentsRegister(
                Instruction.ShiftRightLogicalEaContents, 0xaaaa, 1, 0x5555,
                "レジスタの内容が実効アドレス回だけ右に論理シフトされる");
        }

        private void CheckEaContentsRegister(
            Instruction instruction, UInt16 regValue, UInt16 eaContents, UInt16 expected, String message)
        {
            ExecuteInstruction(instruction, regValue, eaContents);
            UInt16 actual = m_registerSet.GR[R].Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckEaContentsMemory(
            Instruction instruction, UInt16 regValue, UInt16 eaContents, Int32 address,
            UInt16 expected, String message)
        {
            ExecuteInstruction(instruction, regValue, eaContents);
            Word word = m_memory.Read(address);
            UInt16 actual = word.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckEaContentsFlags(
            Instruction instruction, UInt16 regValue, UInt16 eaContents,
            Boolean expectedOverflow, Boolean expectedSign, Boolean expectedZero, String message)
        {
            ExecuteInstruction(instruction, regValue, eaContents);
            CheckFlag(expectedOverflow, m_registerSet.FR.OF, "Overflow: " + message);
            CheckFlag(expectedSign, m_registerSet.FR.SF, "Sign: " + message);
            CheckFlag(expectedZero, m_registerSet.FR.ZF, "Zero: " + message);
        }

        private void CheckFlag(Boolean expected, Boolean actual, String message)
        {
            Assert.AreEqual(expected, actual, message);
        }

        private void ExecuteInstruction(Instruction instruction, UInt16 regValue, UInt16 eaContents)
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

        /// <summary>
        /// ToString メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            CheckToString(Instruction.LoadEaContents, "LD r,adr,x", "LoadEaContents");
            CheckToString(Instruction.Store, "ST r,adr,x", "Store");

            CheckToString(Instruction.AddArithmeticEaContents, "ADDA r,adr,x", "AddArithmeticEaContents");

            CheckToString(Instruction.CompareArithmeticEaContents, "CPA r,adr,x", "CompareArithmeticEaContents");
            CheckToString(Instruction.CompareLogicalEaContents, "CPL r,adr,x", "CompareLogicalEaContents");

            CheckToString(
                Instruction.ShiftLeftArithmeticEaContents, "SLA r,adr,x", "ShiftLeftArithmeticEaContents");
            CheckToString(
                Instruction.ShiftRightArithmeticEaContents, "SRA r,adr,x", "ShiftRightArithmeticEaContents");
            CheckToString(
                Instruction.ShiftLeftLogicalEaContents, "SLL r,adr,x", "ShiftLeftLogicalEaContents");
            CheckToString(
                Instruction.ShiftRightLogicalEaContents, "SRL r,adr,x", "ShiftRightLogicalEaContents");
        }

        private void CheckToString(Instruction instruction, String expected, String message)
        {
            String actual = instruction.ToString();
            Assert.AreEqual(expected, actual, message);
        }
    }
}
