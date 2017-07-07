﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Fetcher クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class FetcherTest
    {
        #region Fields
        private Register m_pr;
        private Memory m_memory;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_pr = Register.MakePR();
            m_memory = new Memory();
        }

        /// <summary>
        /// Fetch メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void Fetch()
        {
            const UInt16 StartAddress = 1000;
            const UInt16 contents1 = 1111;
            const UInt16 contents2 = 2222;
            const UInt16 contents3 = 3333;

            m_memory.Write(StartAddress, contents1, contents2, contents3);
            m_pr.SetValue(StartAddress);

            CheckFetch(contents1, StartAddress + 1, "1000 番地の内容をフェッチ => 1111");
            CheckFetch(contents2, StartAddress + 2, "1001 番地の内容をフェッチ => 2222");
            CheckFetch(contents3, StartAddress + 3, "1002 番地の内容をフェッチ => 3333");
        }

        private void CheckFetch(UInt16 expectedContents, UInt16 expectedPr, String message)
        {
            Word word = Fetcher.Fetch(m_pr, m_memory);

            UInt16 actualContents = word.GetAsUnsigned();
            Assert.AreEqual(expectedContents, actualContents, "Contents: " + message);

            UInt16 actualPr = m_pr.Value.GetAsUnsigned();
            Assert.AreEqual(expectedPr, actualPr, "PR: " + message);
        }
    }
}