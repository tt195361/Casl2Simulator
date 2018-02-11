using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest.Utils
{
    /// <summary>
    /// EnumerableUtils クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class EnumerableUtilsTest
    {
        #region Fields
        private StringBuilder m_builder;

        private const String ABC = "ABC";
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_builder = new StringBuilder();
        }

        /// <summary>
        /// 引数が項目の action を実行する ForEach のテストです。
        /// </summary>
        [TestMethod]
        public void ForEachWithItem()
        {
            CheckForEachWithItem(
                TestUtils.MakeArray<String>(),
                AppendStr, String.Empty, "項目なし => なにもしない");
            CheckForEachWithItem(
                TestUtils.MakeArray<String>("ABC"),
                AppendStr, "ABC", "項目 1 つ => その項目について実行");
            CheckForEachWithItem(
                TestUtils.MakeArray<String>("ABC", ",DEF", ",GHI"),
                AppendStr, "ABC,DEF,GHI", "項目 3 つ => それぞれの項目について実行");
        }

        private void CheckForEachWithItem(
            IEnumerable<String> strEnumerable, Action<String> action, String expected, String message)
        {
            CheckResult(() => strEnumerable.ForEach((str) => action(str)), expected, message);
        }

        /// <summary>
        /// 引数がインデックスと項目の action を実行する ForEach のテストです。
        /// </summary>
        [TestMethod]
        public void ForEachWithIndexAndItem()
        {
            CheckForEachWithIndexAndItem(
                TestUtils.MakeArray<String>(),
                AppendIndexAndStr, String.Empty, "項目なし => なにもしない");
            CheckForEachWithIndexAndItem(
                TestUtils.MakeArray<String>("AAA"),
                AppendIndexAndStr, "0:AAA", "項目 1 つ => その項目について実行");
            CheckForEachWithIndexAndItem(
                TestUtils.MakeArray<String>("AAA,", "BBB,", "CCC"),
                AppendIndexAndStr, "0:AAA,1:BBB,2:CCC", "項目 3 つ => それぞれの項目について実行");
        }

        private void CheckForEachWithIndexAndItem(
            IEnumerable<String> strEnumerable, Action<Int32, String> action, String expected, String message)
        {
            CheckResult(() => strEnumerable.ForEach((index, str) => action(index, str)), expected, message);
        }

    /// <summary>
    /// 引数のない action を実行する Times のテストです。
    /// </summary>
    [TestMethod]
        public void Times()
        {
            CheckTimes(0, AppendAbc, String.Empty, "0 回 => 空文字列");
            CheckTimes(1, AppendAbc, ABC, "1 回 => 'ABC'");
            CheckTimes(3, AppendAbc, ABC + ABC + ABC, "3 回 => 'ABCABCABC'");
        }

        private void CheckTimes(Int32 count, Action action, String expected, String message)
        {
            CheckResult(() => count.Times(() => action()), expected, message);
        }

        /// <summary>
        /// index を引数に取る action を実行する Times のテストです。
        /// </summary>
        [TestMethod]
        public void TimesWithIndex()
        {
            CheckTimesWithIndex(0, AppendIndex, String.Empty, "0 回 => 空文字列");
            CheckTimesWithIndex(1, AppendIndex, "0", "1 回 => '0'");
            CheckTimesWithIndex(3, AppendIndex, "012", "3 回 => '012'");
        }

        private void CheckTimesWithIndex(Int32 count, Action<Int32> action, String expected, String message)
        {
            CheckResult(() => count.Times((index) => action(index)), expected, message);
        }

        private void CheckResult(Action checkAction, String expected, String message)
        {
            m_builder.Clear();
            checkAction();

            String actual = m_builder.ToString();
            Assert.AreEqual(expected, actual, message);
        }

        private void AppendStr(String str)
        {
            m_builder.Append(str);
        }

        private void AppendIndexAndStr(Int32 index, String str)
        {
            m_builder.Append(index);
            m_builder.Append(':');
            m_builder.Append(str);
        }

        private void AppendAbc()
        {
            m_builder.Append(ABC);
        }

        private void AppendIndex(Int32 index)
        {
            m_builder.Append(index);
        }
    }
}
