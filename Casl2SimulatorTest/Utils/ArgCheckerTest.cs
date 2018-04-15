using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest.Utils
{
    /// <summary>
    /// <see cref="ArgChecker"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ArgCheckerTest
    {
        /// <summary>
        /// <see cref="ArgChecker.CheckRange"/> メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void CheckRange()
        {
            CheckCheckRange(-1, 0, 32767, false, "値が最小より小さい => 失敗");
            CheckCheckRange(0, 0, 32767, true, "値が最小と等しい => 成功");
            CheckCheckRange(32767, 0, 32767, true, "値が最大と等しい => 成功");
            CheckCheckRange(32768, 0, 32767, false, "値が最大より大きい => 失敗");
        }

        private void CheckCheckRange(Int32 value, Int32 min, Int32 max, Boolean success, String message)
        {
            Check(
                () => ArgChecker.CheckRange(value, min, max, nameof(value)),
                success, message);
        }

        /// <summary>
        /// <see cref="ArgChecker.CheckGreaterEqual メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void CheckGreaterEqual()
        {
            CheckCheckGreaterEqual(7, 6, true, "7 > 6 => 成功");
            CheckCheckGreaterEqual(6, 6, true, "6 == 6 => 成功");
            CheckCheckGreaterEqual(5, 6, false, "5 < 6 => 失敗");
        }

        private void CheckCheckGreaterEqual(
            Int32 greaterValue, Int32 lesserValue, Boolean success, String message)
        {
            Check(
                () => ArgChecker.CheckGreaterEqual(
                        greaterValue, lesserValue, nameof(greaterValue), nameof(lesserValue)),
                success, message);
        }

        /// <summary>
        /// <see cref="ArgChecker.CheckNotNull メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void CheckNotNull()
        {
            Object noneNullObj = new Object();

            CheckCheckNotNull(noneNullObj, true, "null でないオブジェクト => OK");
            CheckCheckNotNull(null, false, "null => 例外");
        }

        private void CheckCheckNotNull(Object obj, Boolean success, String message)
        {
            Check(
                () => ArgChecker.CheckNotNull(obj, nameof(obj)),
                success, message);
        }

        private void Check(Action checkAction, Boolean success, String message)
        {
            try
            {
                checkAction();
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
