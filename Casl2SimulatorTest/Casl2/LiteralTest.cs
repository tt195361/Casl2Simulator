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

        internal static void Check(Literal expected, Literal actual, String message)
        {
            ConstantTest.Check(expected.Constant, actual.Constant, message);
        }
    }
}
