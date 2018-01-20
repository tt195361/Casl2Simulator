using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// HexaDecimalConstant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class HexaDecimalConstantTest
    {
        #region Fields
        private HexaDecimalConstant m_target;

        private const Int32 DontCare = 0;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_target = new HexaDecimalConstant(0);
        }

        /// <summary>
        /// IsStart メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsStart()
        {
            CheckIsStart('#', true, "'#' => true: 16 進定数の最初の文字");
            CheckIsStart('!', false, "'#' 以外 => false: 16 進定数の最初の文字でない");
        }

        private void CheckIsStart(Char firstChar, Boolean expected, String message)
        {
            Boolean actual = HexaDecimalConstant.IsStart(firstChar);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// Read メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Read()
        {
            CheckRead("#5AC3", true, 0x5AC3, "'#' で始まり 4 桁の 16 進数が続く => OK");

            CheckRead("!", false, DontCare, "'#' で始まらない => 例外");
            CheckRead("#123", false, DontCare, "16 進数が 3 桁しかない => 例外");
            CheckRead("#12345", false, DontCare, "16 進数が 5 桁ある => 例外");
            CheckRead("#abcd", false, DontCare, "小文字は 16 進数でない => 例外");
        }

        private void CheckRead(String str, Boolean success, Int32 expected, String message)
        {
            Int32 actual = ConstantTest.CheckRead(HexaDecimalConstant.Read, str, success, message);
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
            CheckCtor(-1, false, "最小値より小さい => 例外");
            CheckCtor(0, true, "ちょうど最小値 => OK");
            CheckCtor(65535, true, "ちょうど最大値 => OK");
            CheckCtor(65536, false, "最大値より大きい => 例外");
        }

        private void CheckCtor(Int32 value, Boolean success, String message)
        {
            try
            {
                HexaDecimalConstant notUsed = new HexaDecimalConstant(value);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// GetCodeWordCount メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetCodeWordCount()
        {
            ICodeGeneratorTest.CheckGetCodeWordCount(m_target, 1, "HexaDecimalConstant => 1 語生成する");
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            CheckGenerateCode(0, 0x0000, "最小値 0 => コードはその値 0x0000");
            CheckGenerateCode(65535, 0xffff, "最大値 65535 => コードはその値 0xffff");
        }

        private void CheckGenerateCode(Int32 value, UInt16 expected, String message)
        {
            HexaDecimalConstant target = new HexaDecimalConstant(value);
            Word[] expectedWords = WordTest.MakeArray(expected);
            ICodeGeneratorTest.CheckGenerateCode(target, expectedWords, message);
        }

        /// <summary>
        /// GenerateLiteralDc メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateLiteralDc()
        {
            ICodeGeneratorTest.CheckGenerateLiteralDc(
                m_target, null, "HexaDecimalConstant は DC 命令を生成しない ==> null が返される");
        }

        internal static void Check(HexaDecimalConstant expected, HexaDecimalConstant actual, String message)
        {
            Assert.AreEqual(expected.Value, actual.Value, "Value: " + message);
        }
    }
}
