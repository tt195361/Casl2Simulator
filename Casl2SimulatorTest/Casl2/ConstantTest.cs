using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Constant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ConstantTest
    {
        #region Fields
        private static readonly Type NumericConstant = typeof(NumericConstant);
        private static readonly Type StringConstant = typeof(StringConstant);
        private static readonly Type AddressConstant = typeof(AddressConstant);
        #endregion

        /// <summary>
        /// ParseList メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseList()
        {
            CheckParseList("12345", TestUtils.MakeArray(NumericConstant), String.Empty, "10 進定数");
            CheckParseList("#ABCD", TestUtils.MakeArray(NumericConstant), String.Empty, "16 進定数");
            CheckParseList("'abc'", TestUtils.MakeArray(StringConstant), String.Empty, "文字定数");
            CheckParseList("L001", TestUtils.MakeArray(AddressConstant), String.Empty, "アドレス定数");
            CheckParseList(
                "12345,#ABCD,'文字定数',L001", 
                TestUtils.MakeArray(NumericConstant, NumericConstant, StringConstant, AddressConstant),
                String.Empty, "定数の並び");

            CheckParseList(
                String.Empty, null, String.Empty,
                "空文字列でオペランドなし => エラー, 1 つ以上の定数が必要");
            CheckParseList(
                "123,", null, String.Empty,
                "コンマに続く定数なし => エラー, コンマがあれば続いて定数が必要");
            CheckParseList(
                "!@#$", null, String.Empty,
                "定数として解釈できない文字列 => エラー");
            CheckParseList(
                "'abc'123", TestUtils.MakeArray(StringConstant), "123",
                "区切りのコンマがない、定数の後ろに解釈できない文字列がある => 解釈できるところまで解釈する");
        }

        private static void CheckParseList(
            String str, Type[] expectedTypes, String expectedRest, String message)
        {
            ReadBuffer buffer = new ReadBuffer(str);
            try
            {
                Constant[] actualConstants = Constant.ParseList(buffer);
                Assert.IsNotNull(expectedTypes, message);

                TestUtils.CheckTypes(actualConstants, expectedTypes, message);
                String actualRest = buffer.GetRest();
                Assert.AreEqual(expectedRest, actualRest, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expectedTypes, message);
            }
        }

        internal static T CheckParse<T>(
            Func<ReadBuffer, T> parseFunc, String str, Boolean success, String message)
            where T : Constant
        {
            ReadBuffer buffer = new ReadBuffer(str);
            try
            {
                T target = parseFunc(buffer);
                Assert.IsTrue(success, message);
                return target;
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
                return null;
            }
        }
    }
}
