using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Literal クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LiteralTest
    {
        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const Constant DontCare = null;

            CheckParse("12345", true, new DecimalConstant(12345), "10 進定数 => OK");
            CheckParse("#ABCD", true, new HexaDecimalConstant(0xABCD), "16 進定数 => OK");
            CheckParse("'#$!%'", true, new StringConstant("#$!%"), "文字定数 => OK");
            CheckParse(LabelTest.ValidLabelName, false, DontCare, "それ以外 => リテラルに使えない");
        }

        private void CheckParse(String str, Boolean success, Constant expected, String message)
        {
            Literal literal = MachineInstructionOperandTest.CheckParse(Literal.Parse, str, success, message);
            if (success)
            {
                Constant actual = literal.Constant;
                ConstantTest.Check(expected, actual, message);
            }
        }

        /// <summary>
        /// IAdrValue.GenerateDc メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateDc()
        {
            CheckGenerateDc(
                Literal.MakeForUnitTest(new DecimalConstant(12345)),
                "LTRL0001\tDC\t12345",
                "10 進定数 => 10 進定数の DC 命令が生成される。");
            CheckGenerateDc(
                Literal.MakeForUnitTest(new HexaDecimalConstant(0xFEDC)),
                "LTRL0001\tDC\t#FEDC",
                "16 進定数 => 16 進定数の DC 命令が生成される。");
            CheckGenerateDc(
                Literal.MakeForUnitTest(new StringConstant("!@#$%")),
                "LTRL0001\tDC\t'!@#$%'",
                "文字定数 => 文字定数の DC 命令が生成される。");
        }

        private void CheckGenerateDc(IAdrValue literal, String expected, String message)
        {
            LabelManager lblManager = new LabelManager();
            String actual = literal.GenerateDc(lblManager);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// IAdrValue.GetAddress メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetAddress()
        {
            const UInt16 LabelOffset = 1357;
            Literal literal = Literal.MakeForUnitTest(new DecimalConstant(2468));
            IAdrValue iAdrValue = literal;
            LabelManager lblManager = new LabelManager();

            String notUsed = iAdrValue.GenerateDc(lblManager);
            lblManager.SetOffset(literal.Label, LabelOffset);

            IAdrValueTest.CheckGetAddress(
                iAdrValue, lblManager, LabelOffset,
                "生成した DC 命令のラベルに設定したオフセットが返される");
        }

        internal static void Check(Literal expected, Literal actual, String message)
        {
            ConstantTest.Check(expected.Constant, actual.Constant, message);
        }
    }
}
