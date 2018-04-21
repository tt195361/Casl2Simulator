using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="ProgramChecker"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ProgramCheckerTest
    {
        /// <summary>
        /// Check メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Check()
        {
            DoCheck(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       END"),
                true, "START 命令が 1 つ、END 命令が 1 つ => OK");

            DoCheck(
                TestUtils.MakeArray(
                    "       END"),
                false, "START 命令がない => 例外");
            DoCheck(
                TestUtils.MakeArray(
                    "ENTRY1 START",
                    "ENTRY2 START",
                    "       END"),
                false, "START 命令が 2 つ以上ある => 例外");

            DoCheck(
                TestUtils.MakeArray(
                    "ENTRY  START"),
                false, "END 命令がない => 例外");
            DoCheck(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       END",
                    "       END"),
                false, "END 命令が 2 つ以上ある => 例外");

            DoCheck(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       END",
                    "       LD    GR0,GR1"),
                false, "END 命令の後にさらに命令がある => 例外");
            DoCheck(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       END",
                    "; コメント"),
                true, "END 命令の後にコメント行がある => OK: コメント行は問題ない");
        }

        private void DoCheck(String[] sourceText, Boolean success, String message)
        {
            IEnumerable<ProgramLine> parsedLines = sourceText.Select((text) => ProgramLine.Parse(text));
            try
            {
                ProgramChecker.Check(parsedLines);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
