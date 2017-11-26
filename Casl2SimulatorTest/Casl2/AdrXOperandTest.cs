using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// AdrXOperand クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AdrXOperandTest
    {
        #region Fields
        private AdrXOperand m_adrOnly;
        private AdrXOperand m_adrWithX;
        private LabelManager m_lblManager;

        private const UInt16 AdrOnlyValue = 0x1357;
        private const UInt16 AdrWithXValue = 0x2468;
        private readonly RegisterOperand X = RegisterOperandTest.GR7;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_adrOnly = AdrXOperand.MakeForUnitTest(new DecimalConstant(AdrOnlyValue), null);
            m_adrWithX = AdrXOperand.MakeForUnitTest(new DecimalConstant(AdrWithXValue), X);
            m_lblManager = new LabelManager();
        }

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const IAdrValue DontCareAdr = null;
            const RegisterOperand DontCareX = null;

            // adr のみの場合。
            CheckParse(
                "12345", true,
                new DecimalConstant(12345), null, "10 進定数");
            CheckParse(
                "#ABCD", true,
                new HexaDecimalConstant(0xABCD), null, "16 進定数");
            CheckParse(
                LabelTest.ValidLabelName, true,
                new AddressConstant(LabelTest.ValidLabelName), null, "アドレス定数");
            CheckParse(
                "=12345", true,
                Literal.MakeForUnitTest(new DecimalConstant(12345)), null,
                "リテラル");
            CheckParse(
                "'文字定数'", false,
                DontCareAdr, DontCareX, "上記以外 => 例外");

            // adr に続く字句要素がある場合
            CheckParse(
                "23456=", true,
                new DecimalConstant(23456), null, "続きが ',' 以外なら、そこで解釈終了し x なし");
            CheckParse(
                "23456,", false,
                DontCareAdr, DontCareX, "続きが ',' なら、x が必要 => 例外");
            CheckParse(
                "#BCDE,GR1", true,
                new HexaDecimalConstant(0xBCDE), RegisterOperandTest.GR1, "続きが ',' で x を指定");
            CheckParse(
                "#BCDE,GR0", false,
                DontCareAdr, DontCareX, "GR0 は指標レジスタとして使えない => 例外");
            CheckParse(
                "#CDEF,GR2,", true,
                new HexaDecimalConstant(0xCDEF), RegisterOperandTest.GR2, "x の後ろがあってもここでは OK");
        }

        private void CheckParse(
            String str, Boolean success, IAdrValue expectedAdr, RegisterOperand expectedX, String message)
        {
            AdrXOperand actual =
                MachineInstructionOperandTest.CheckParse(AdrXOperand.Parse, str, success, message);
            if (success)
            {
                Check(expectedAdr, expectedX, actual, message);
            }
        }

        /// <summary>
        /// GetXR2 メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetXR2()
        {
            CheckGetXR2(m_adrOnly, 0, "adr のみで X なし => XR2 は 0");
            CheckGetXR2(m_adrWithX, X.Number, "X あり => XR2 は X の値");
        }

        private void CheckGetXR2(AdrXOperand target, UInt16 expected, String message)
        {
            UInt16 actual = target.GetXR2();
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// MakeSecondWord メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void MakeSecondWord()
        {
            CheckMakeSecondWord(
                m_adrOnly, new Word(AdrOnlyValue), "adr の値を第 2 語にする: adr のみ");
            CheckMakeSecondWord(
                m_adrWithX, new Word(AdrWithXValue), "adr の値を第 2 語にする: X あり");
        }

        private void CheckMakeSecondWord(AdrXOperand target, Word? expected, String message)
        {
            Word? actual = target.MakeSecondWord(m_lblManager);
            WordTest.Check(expected, actual, message);
        }

        internal static void Check(AdrXOperand expected, AdrXOperand actual, String message)
        {
            Check(expected.Adr, expected.X, actual, message);
        }

        private static void Check(
            IAdrValue expectedAdr, RegisterOperand expectedX, AdrXOperand actual, String message)
        {
            CheckAdr(expectedAdr, actual.Adr, message);
            CheckX(expectedX, actual.X, message);
        }

        private static void CheckAdr(IAdrValue expected, IAdrValue actual, String message)
        {
            IAdrValueTest.Check(expected, actual, "Adr: " + message);
        }

        private static void CheckX(RegisterOperand expected, RegisterOperand actual, String message)
        {
            String xMessage = "X: " + message;

            if (expected == null)
            {
                Assert.IsNull(actual, xMessage);
            }
            else
            {
                RegisterOperandTest.Check(expected, actual, xMessage);
            }
        }
    }
}
