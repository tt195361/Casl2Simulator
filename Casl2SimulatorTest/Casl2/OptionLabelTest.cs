using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// OptionLabel クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class OptionLabelTest
    {
        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const Label DontCare = null;

            CheckParse(
                String.Empty, true, null,
                "空文字列 => OK, ラベルなし");
            CheckParse(
                LabelTest.ValidLabelName, true, LabelTest.ValidLabel,
                "オペランド 1 つ、有効なラベル => OK, ラベルを指定");
            CheckParse(
                "123", false, DontCare,
                "オペランド 1 つ、ラベル以外 => 例外");
            CheckParse(
                LabelTest.ValidLabelName + ",OPR2", true, LabelTest.ValidLabel,
                "オペランドが 1 より多い => ここでは OK");
        }

        private void CheckParse(String text, Boolean success, Label expected, String message)
        {
            OptionLabel target = OperandTest.CheckParse(OptionLabel.Parse, text, success, message);
            if (success)
            {
                Label actual = target.Label;
                LabelTest.Check(expected, actual, message);
            }
        }
    }
}
