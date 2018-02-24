using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="AreaSpec"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AreaSpecTest
    {
        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            Label expectedBuffer = new Label("LBL001");
            Label expectedLength = new Label("LBL002");
            const Label DontCare = null;

            CheckParse(
                String.Empty, false, DontCare, DontCare,
                "オペランドなし => エラー, 2 つのラベルが必要");
            CheckParse(
                "LBL001", false, DontCare, DontCare,
                "ラベル 1 つ => エラー, 2 つのラベルが必要");
            CheckParse(
                "LBL001,", false, DontCare, DontCare,
                "ラベル 1 つとコンマ => エラー, 2 つのラベルが必要");
            CheckParse(
                "LBL001,LBL002", true, expectedBuffer, expectedLength,
                "ラベル 2 つ => OK");
            CheckParse(
                "LBL001,LBL002,", true, expectedBuffer, expectedLength,
                "ラベル 2 つのあとにまだ文字がある => ここでは OK");
        }

        private void CheckParse(
            String str, Boolean success, Label expectedBuffer, Label expectedLength, String message)
        {
            AreaSpec actual = OperandTest.CheckParse(AreaSpec.Parse, str, success, message);
            if (success)
            {
                LabelTest.Check(expectedBuffer, actual.Buffer, message);
                LabelTest.Check(expectedLength, actual.Length, message);
            }
        }
    }
}
