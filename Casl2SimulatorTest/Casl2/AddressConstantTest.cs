using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// AddressConstant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AddressConstantTest
    {
        #region Fields
        private AddressConstant m_registered;
        private AddressConstant m_notRegistered;
        private LabelManager m_lblManager;

        private const UInt16 RegisteredOffset = 0x1234;
        private const UInt16 NotRegisteredOffset = 0x0000;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_registered = new AddressConstant("REGED");
            m_notRegistered = new AddressConstant("NOTREGED");

            m_lblManager = new LabelManager();
            m_lblManager.Register(m_registered.Label, RegisteredOffset);
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            CheckGenerateCode(
                m_registered, RegisteredOffset,
                "登録されたラベル => コードはそのラベルのオフセット");
            CheckGenerateCode(
                m_notRegistered, NotRegisteredOffset,
                "登録されていないラベル => コードは 0");
        }

        private void CheckGenerateCode(AddressConstant target, UInt16 expected, String message)
        {
            Word[] expectedWords = WordTest.MakeArray(expected);
            ConstantTest.CheckGenerateCode(target, m_lblManager, expectedWords, message);
        }

        /// <summary>
        /// GenerateDc メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateDc()
        {
            IAdrValue target = new AddressConstant("LBL001");
            String result = target.GenerateDc(m_lblManager);
            Assert.IsNull(result, "AddressConstant は DC 命令を生成しない ==> null が返される");
        }

        /// <summary>
        /// GetAddress メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetAddress()
        {
            CheckGetAddress(
                m_registered, RegisteredOffset,
                "登録されたラベルのアドレス定数 => アドレスはそのラベルのオフセット");
            CheckGetAddress(
                m_notRegistered, NotRegisteredOffset,
                "登録されていないラベルのアドレス定数 => アドレスは 0");
        }

        private void CheckGetAddress(IAdrValue target, UInt16 expected, String message)
        {
            IAdrValueTest.CheckGetAddress(target, m_lblManager, expected, message);
        }

        internal static void Check(AddressConstant expected, AddressConstant actual, String message)
        {
            LabelTest.Check(expected.Label, actual.Label, "Label: " + message);
        }
    }
}
