using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest.Utils
{
    /// <summary>
    /// ArgChecker クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ArgCheckerTest
    {
        /// <summary>
        /// CheckRange メソッドの単体テストです。
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
            try
            {
                ArgChecker.CheckRange(value, min, max, nameof(value));
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
