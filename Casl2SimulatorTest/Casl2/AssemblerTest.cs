using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Assembler クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AssemblerTest
    {
        #region Fields
        private Assembler m_assembler;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_assembler = new Assembler();
        }

        /// <summary>
        /// ProcessSourceText で、マクロ命令の展開の確認。
        /// </summary>
        [TestMethod]
        public void ProcessSourceText_ExpandMacro()
        {
            String[] sourceText = TestUtils.MakeArray(
                "ENTRY  START",
                "LBL001 RPUSH   ; マクロ命令",
                "       DC 123  ; マクロでない命令",
                "; コメント行",
                "       END");
            Boolean result = m_assembler.ProcessSourceText(sourceText);
            Assert.IsTrue(result, "展開に成功する");

            String[] expectedText = TestUtils.MakeArray(
                "ENTRY  START",
                "LBL001\tPUSH\t0,GR1",
                "\tPUSH\t0,GR2",
                "\tPUSH\t0,GR3",
                "\tPUSH\t0,GR4",
                "\tPUSH\t0,GR5",
                "\tPUSH\t0,GR6",
                "\tPUSH\t0,GR7",
                "       DC 123  ; マクロでない命令",
                "; コメント行",
                "       END");
            LineCollectionTest.Check(
                m_assembler.ProcessedLines, expectedText,
                "マクロ命令の内容が展開される。マクロ命令以外はそのまま");
        }
    }
}
