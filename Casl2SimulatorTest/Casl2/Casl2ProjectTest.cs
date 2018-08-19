using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="Casl2Project"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class Casl2ProjectTest
    {
        /// <summary>
        /// <see cref="Casl2Project.Build"/> メソッドで、ビルドに成功する場合のテストです。
        /// </summary>
        [TestMethod]
        public void Build_Success()
        {
            Casl2Program subProgram = Casl2Program.MakeForUnitTest(
                "SUB", 
                TestUtils.MakeArray(
                    "ADDSUB  START",
                    "        ADDA  GR1,GR2",
                    "        RET",
                    "        END"));
            Casl2Program mainProgram = Casl2Program.MakeForUnitTest(
                "MAIN",
                TestUtils.MakeArray(
                    "MAIN    START",
                    "        CALL  ADDSUB",
                    "        RET",
                    "        END"));
            Casl2Project project = Casl2Project.MakeForUnitTest(subProgram, mainProgram);
            project.Programs.SelectItem(mainProgram);

            MemoryAddress expectedLoadAddress = MemoryAddress.Zero;
            MemoryAddress expectedExecStartAddress = new MemoryAddress(2);
            Word[] expectedWords = WordTest.MakeArray(
                // Sub
                0x2412,                     //         ADDA  GR1,GR2
                0x8100,                     //         RET
                // Main
                0x8000, 0x0000,             //         CALL  ADDSUB
                0x8100);                    //         RET
            ExecutableModule expected =
                new ExecutableModule(expectedLoadAddress, expectedExecStartAddress, expectedWords);

            CheckBuild(project, expected, "ビルド成功 => 実行可能モジュールが生成される");
        }

        /// <summary>
        /// <see cref="Casl2Project.Build"/> メソッドで、アセンブルエラーの場合のテストです。
        /// </summary>
        [TestMethod]
        public void Build_AssembleError()
        {
            Casl2Program errorProgram = Casl2Program.MakeForUnitTest(
                "ERR",
                TestUtils.MakeArray(
                    "MAIN    START",
                    "        UNDEF",        // 未定義命令
                    "        END"));
            Casl2Project project = Casl2Project.MakeForUnitTest(errorProgram);

            CheckBuild(project, null, "アセンブルエラー => 例外");
        }

        /// <summary>
        /// <see cref="Casl2Project.Build"/> メソッドで、リンクエラーの場合のテストです。
        /// </summary>
        [TestMethod]
        public void Build_LinkError()
        {
            Casl2Program subProgram = Casl2Program.MakeForUnitTest(
                "SUB",
                TestUtils.MakeArray(
                    "ADDSUB  START",
                    "        ADDA  GR1,GR2",
                    "        RET",
                    "        END"));
            Casl2Program mainProgram = Casl2Program.MakeForUnitTest(
                "MAIN",
                TestUtils.MakeArray(
                    "MAIN    START",
                    "        CALL  UNDEF",      // 未定義ラベルを参照
                    "        RET",
                    "        END"));
            Casl2Project project = Casl2Project.MakeForUnitTest(subProgram, mainProgram);

            CheckBuild(project, null, "リンクエラー => 例外");
        }

        private void CheckBuild(Casl2Project project, ExecutableModule expected, String message)
        {
            try
            {
                ExecutableModule actual = project.Build();
                Assert.IsNotNull(expected, message);
                ExecutableModuleTest.Check(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expected, message);
            }
        }
    }
}
