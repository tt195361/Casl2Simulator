using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="AddressConstant"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AddressConstantTest
    {
        #region Instance Fields
        private AddressConstant m_target;
        private LabelManager m_lblManager;

        private readonly MemoryOffset RegisteredOffset = new MemoryOffset(0x1234);
        private readonly MemoryOffset NotRegisteredOffset = new MemoryOffset(0x0000);
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_target = new AddressConstant("LBL001");
            m_lblManager = new LabelManager();
        }

        /// <summary>
        /// <see cref="AddressConstant.GetCodeWordCount"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetCodeWordCount()
        {
            ICodeGeneratorTest.CheckGetCodeWordCount(m_target, 1, "AddressConstant => 1 語生成する");
        }

        /// <summary>
        /// <see cref="AddressConstant.GenerateCode"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            Word[] expectedWords = WordTest.MakeArray(0x0000);
            ICodeGeneratorTest.CheckGenerateCode(
                m_target, m_lblManager, expectedWords,
                "あどでラベルのアドレスに置き換えるために、値が 0x0000 の語が追加される");
        }

        /// <summary>
        /// <see cref="AddressConstant.GenerateLiteralDc"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateLiteralDc()
        {
            ICodeGeneratorTest.CheckGenerateLiteralDc(
                m_target, null, "AddressConstant は DC 命令を生成しない ==> null が返される");
        }

        internal static void Check(AddressConstant expected, AddressConstant actual, String message)
        {
            LabelTest.Check(expected.Label, actual.Label, "Label: " + message);
        }
    }
}
