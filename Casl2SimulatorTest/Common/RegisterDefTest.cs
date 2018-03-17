using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2SimulatorTest.Common
{
    /// <summary>
    /// <see cref="RegisterDef"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RegisterDefTest
    {
        /// <summary>
        /// <see cref="RegisterDef.GrNames"/> フィールドのテストです。
        /// </summary>
        [TestMethod]
        public void GrNames()
        {
            String[] expected = { "GR0", "GR1", "GR2", "GR3", "GR4", "GR5", "GR6", "GR7" };
            IEnumerable<String> actual = RegisterDef.GrNames;
            TestUtils.CheckEnumerable(
                expected, actual, "GR (汎用レジスタ、General Register) は、GR0 ~ GR7 の 8 個");
        }

        /// <summary>
        /// <see cref="RegisterDef.GrCount"/> フィールドのテストです。
        /// </summary>
        [TestMethod]
        public void GrCount()
        {
            const Int32 Expected = 8;
            Int32 actual = RegisterDef.GrCount;
            Assert.AreEqual(Expected, actual, "GR (汎用レジスタ、General Register) は 8 個");
        }
    }
}
