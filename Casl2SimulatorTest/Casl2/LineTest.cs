using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Line クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LineTest
    {
        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            CheckParse(
                "; コメント",
                true, null, null, "空白なしで ';' => コメント行");
            CheckParse(
                " ; コメント",
                true, null, null, "空白の続いて ';' => コメント行");

            CheckParse(
                "LABEL DC 123",
                true, "LABEL", typeof(AsmDcInstruction), "ラベルがある場合");
            CheckParse(
                "      DC 123",
                true, null, typeof(AsmDcInstruction), "ラベルがない場合");

            CheckParse(
                "LABEL START",
                true, "LABEL", typeof(AsmStartInstruction), "オペランドなしで、そのまま終了");
            CheckParse(
                "LABEL START ",
                true, "LABEL", typeof(AsmStartInstruction), "オペランドなしで、空白があって終了");
            CheckParse(
                "LABEL START ;コメント",
                true, "LABEL", typeof(AsmStartInstruction), "オペランドなしで、';' でコメント開始");
            CheckParse(
                "LABEL START EXECSTRT",
                true, "LABEL", typeof(AsmStartInstruction), "オペランドありで、そのまま終了");
            CheckParse(
                "LABEL START EXECSTRT コメント",
                true, "LABEL", typeof(AsmStartInstruction), "オペランドありで、コメントあり");

            CheckParse(
                "LABEL DC ' ' コメント",
                true, "LABEL", typeof(AsmDcInstruction), "文字定数中に空白がある場合");
            CheckParse(
                "LABEL DC 10,',',#ABCD コメント",
                true, "LABEL", typeof(AsmDcInstruction), "文字定数中にコンマがある場合");

            CheckParse(
                String.Empty,
                false, null, null, "空行 => エラー");
        }

        private void CheckParse(
            String str, Boolean success, String expectedLabel, Type expectedInstructionType, String message)
        {
            Line line = Line.Parse(str);

            if (!success)
            {
                Assert.IsNotNull(line.ErrorMessage, message);
            }
            else
            {
                Assert.IsNull(line.ErrorMessage, message);

                String actualLabel = line.Label;
                Assert.AreEqual(expectedLabel, actualLabel, message);

                if (expectedInstructionType == null)
                {
                    Assert.IsNull(line.Instruction, message);
                }
                else
                {
                    Assert.IsInstanceOfType(line.Instruction, expectedInstructionType, message);
                }
            }
        }
    }
}
