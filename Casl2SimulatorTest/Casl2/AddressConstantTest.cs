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
        #region Fields
        private AddressConstant m_target;
        private AddressConstant m_registered;
        private AddressConstant m_notRegistered;
        private LabelManager m_lblManager;

        private readonly MemoryOffset RegisteredOffset = new MemoryOffset(0x1234);
        private readonly MemoryOffset NotRegisteredOffset = new MemoryOffset(0x0000);
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_target = new AddressConstant("LBL001");
            m_registered = new AddressConstant("REGED");
            m_notRegistered = new AddressConstant("NOTREGED");

            m_lblManager = new LabelManager();
            m_lblManager.RegisterForUnitTest(m_registered.Label, RegisteredOffset);
        }

        /// <summary>
        /// GetCodeWordCount メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetCodeWordCount()
        {
            ICodeGeneratorTest.CheckGetCodeWordCount(m_target, 1, "AddressConstant => 1 語生成する");
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            CheckGenerateCode(
                m_registered, RegisteredOffset.Value,
                "登録されたラベル => コードはそのラベルのオフセットの値");
            CheckGenerateCode(
                m_notRegistered, NotRegisteredOffset.Value,
                "登録されていないラベル => コードは 0");
        }

        private void CheckGenerateCode(AddressConstant target, UInt16 expected, String message)
        {
            Word[] expectedWords = WordTest.MakeArray(expected);
            ICodeGeneratorTest.CheckGenerateCode(target, m_lblManager, expectedWords, message);
        }

        /// <summary>
        /// GenerateLiteralDc メソッドのテストです。
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
