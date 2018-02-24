using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="ReservedWord"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ReservedWordTest
    {
        /// <summary>
        /// IsReserved メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsReserved()
        {
            CheckIsReserved("GR0", true, "GR0 => 予約語");
            CheckIsReserved("GR1", true, "GR1 => 予約語");
            CheckIsReserved("GR2", true, "GR2 => 予約語");
            CheckIsReserved("GR3", true, "GR3 => 予約語");
            CheckIsReserved("GR4", true, "GR4 => 予約語");
            CheckIsReserved("GR5", true, "GR5 => 予約語");
            CheckIsReserved("GR6", true, "GR6 => 予約語");
            CheckIsReserved("GR7", true, "GR7 => 予約語");

            CheckIsReserved("GR8", false, "GR8 => 予約語でない");
        }

        private void CheckIsReserved(String str, Boolean expected, String message)
        {
            Boolean actual = ReservedWord.IsReserved(str);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// GetList メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetList()
        {
            String actual = ReservedWord.GetList();
            const String Expected = "GR0, GR1, GR2, GR3, GR4, GR5, GR6, GR7";
            Assert.AreEqual(Expected, actual, "予約語のリストを取得する");
        }
    }
}
