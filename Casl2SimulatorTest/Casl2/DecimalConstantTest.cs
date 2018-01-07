﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// DecimalConstant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class DecimalConstantTest
    {
        #region Fields
        private const Int32 DontCare = 0;
        #endregion

        /// <summary>
        /// IsStart メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsStart()
        {
            CheckIsStart('-', true, "マイナス => true: 10 進定数の最初の文字");
            CheckIsStart('5', true, "数字 => true: 10 進定数の最初の文字");
            CheckIsStart('#', false, "マイナスと数字以外 => false: 10 進定数の最初の文字でない");
        }

        private void CheckIsStart(Char firstChar, Boolean expected, String message)
        {
            Boolean actual = DecimalConstant.IsStart(firstChar);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// Read メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Read()
        {
            CheckRead("123", true, 123, "正の数字");
            CheckRead("-246", true, -246, "負の数字");
            CheckRead("0", true, 0, "零");

            CheckRead("13579abc", true, 13579, "数字として解釈できるところまで解釈する");
            CheckRead("abc", false, DontCare, "最初が数字でない => 例外");
            CheckRead("-abc", false, DontCare, "'-' の後ろが数字でない => 例外");
            CheckRead("1234567", true, 1234567, "Read は範囲チェックしない => OK");
        }

        private void CheckRead(String str, Boolean success, Int32 expected, String message)
        {
            Int32 actual = ConstantTest.CheckRead(DecimalConstant.Read, str, success, message);
            if (success)
            {
                Assert.AreEqual(expected, actual, message);
            }
        }

        /// <summary>
        /// コンストラクタのテストです。
        /// </summary>
        [TestMethod]
        public void Ctor()
        {
            CheckCtor(-32769, false, "最小値より小さい => 例外");
            CheckCtor(-32768, true, "ちょうど最小値 => OK");
            CheckCtor(32767, true, "ちょうど最大値 => OK");
            CheckCtor(32768, false, "最大値より大きい => 例外");
        }

        private void CheckCtor(Int32 value, Boolean success, String message)
        {
            try
            {
                DecimalConstant notUsed = new DecimalConstant(value);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            CheckGenerateCode(0, 0, "ゼロ: 0 => コードはその値: 0");
            CheckGenerateCode(32767, 32767, "正の最大値: 32767 => コードはその値: 32767");
            CheckGenerateCode(-32768, -32768, "負の最大値: -32768 => コードはその値: -32768");
            CheckGenerateCode(-1, -1, "負の最小値: -1 => コードはその値: -1");
        }

        private void CheckGenerateCode(Int32 value, Int16 expected, String message)
        {
            DecimalConstant target = new DecimalConstant(value);
            Word[] expectedWords = WordTest.MakeArray(expected);
            ConstantTest.CheckGenerateCode(target, expectedWords, message);
        }

        /// <summary>
        /// GenerateDc メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateDc()
        {
            IAdrValue target = new DecimalConstant(0);
            LabelManager lblManager = new LabelManager();
            String result = target.GenerateDc(lblManager);
            Assert.IsNull(result, "DecimalConstant は DC 命令を生成しない ==> null が返される");
        }

        /// <summary>
        /// GetAddress メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetAddress()
        {
            CheckGetAddress(0, 0x0000, "ゼロ: 0 => その値 0x0000 がアドレス");
            CheckGetAddress(32767, 0x7fff, "正の最大値: 32767 => その値 0x7fff がアドレス");
            CheckGetAddress(-32768, 0x8000, "負の最大値: -32768 => 符号なしの値 0x8000 がアドレス");
            CheckGetAddress(-1, 0xffff, "負の最小値: -1 => 符号なしの値 0xffff がアドレス");
        }

        private void CheckGetAddress(Int32 value, UInt16 expected, String message)
        {
            DecimalConstant target = new DecimalConstant(value);
            IAdrValueTest.CheckGetAddress(target, expected, message);
        }

        internal static void Check(DecimalConstant expected, DecimalConstant actual, String message)
        {
            Assert.AreEqual(expected.Value, actual.Value, "Value: " + message);
        }
    }
}
