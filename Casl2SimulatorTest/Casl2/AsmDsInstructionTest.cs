using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="AsmDsInstruction"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AsmDsInstructionTest
    {
        /// <summary>
        /// ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand()
        {
            const Int32 DontCare = 0;

            CheckReadOperand(
                "123", true, 123, "10 進定数 => OK");
            CheckReadOperand(
                String.Empty, false, DontCare,
                "オペランドなし => エラー, 10 進定数が必要");
            CheckReadOperand(
                "123,456", false, DontCare,
                "10 進定数の後にさらに字句要素がある => エラー");
        }

        private void CheckReadOperand(
            String text, Boolean success, Int32 expectedValue, String message)
        {
            AsmDsInstruction target = new AsmDsInstruction();
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
            if (success)
            {
                WordCount expected = WordCount.MakeForUnitTest(expectedValue);
                WordCount actual = target.WordCount;
                WordCountTest.Check(expected, actual, message);
            }
        }

        /// <summary>
        /// GetCodeWordCount メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetCodeWordCount()
        {
            CheckGetCodeWordCount(0, "指定の語数のコードを生成する: 語数 0");
            CheckGetCodeWordCount(1, "指定の語数のコードを生成する: 語数 1");
            CheckGetCodeWordCount(65535, "指定の語数のコードを生成する: 語数 65535");
        }

        private void CheckGetCodeWordCount(Int32 wordCountValue, String message)
        {
            AsmDsInstruction target = MakeTarget(wordCountValue);
            Int32 actual = target.GetCodeWordCount();
            Int32 expected = wordCountValue;
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            CheckGenerateCode(0, "確保する語数分の値が 0 の語が追加される: 語数 0");
            CheckGenerateCode(1, "確保する語数分の値が 0 の語が追加される: 語数 1");
            CheckGenerateCode(65535, "確保する語数分の値が 0 の語が追加される: 語数 65535");
        }

        private void CheckGenerateCode(Int32 wordCountValue, String message)
        {
            AsmDsInstruction target = MakeTarget(wordCountValue);
            const Label label = null;
            LabelManager lblManager = new LabelManager();
            RelocatableModule relModule = new RelocatableModule();

            target.GenerateCode(label, lblManager, relModule);

            // 確保する語数分の 0 の語が追加される。
            Word[] expectedWords = WordTest.MakeArray(Word.Zero, wordCountValue);
            RelocatableModuleTest.Check(relModule, expectedWords, message);
        }

        private AsmDsInstruction MakeTarget(Int32 wordCountValue)
        {
            AsmDsInstruction target = new AsmDsInstruction();
            target.SetWordCountValueForUnitTest(wordCountValue);
            return target;
        }
    }
}
