using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Alu クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AluTest
    {
        /// <summary>
        /// AddArithmetic メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void AddArithmetic()
        {
            CheckAddArithmetic(20000, 12767, 32767, false, "ちょうど最大 => 結果は計算通り, オーバーフローなし");
            CheckAddArithmetic(20001, 12767, -32768, true, "最大より大きい => 結果は反転、オーバーフローあり");
            CheckAddArithmetic(-10000, -22768, -32768, false, "ちょうど最小 => 結果は計算通り、オーバーフローなし");
            CheckAddArithmetic(-10001, -22768, 32767, true, "最小より小さい => 結果は反転、オーバーフローあり");
        }

        private void CheckAddArithmetic(
            Int16 i16Val1, Int16 i16Val2, Int16 expectedResult, Boolean expectedOverflow, String message)
        {
            Word word1 = new Word(i16Val1);
            Word word2 = new Word(i16Val2);
            Boolean actualOverflow;
            Word resultWord = Alu.AddArithmetic(word1, word2, out actualOverflow);

            Int16 actualResult = resultWord.GetAsSigned();
            Assert.AreEqual(expectedResult, actualResult, "Result: " + message);
            Assert.AreEqual(expectedOverflow, actualOverflow, "Overflow: " + message);
        }

        /// <summary>
        /// AddLogical メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void AddLogical()
        {
            CheckAddLogical(0x7fff, 0x0001, 0x8000, false, "足して 0x8000 => 結果は計算通り, オーバーフローなし");
            CheckAddLogical(0x8000, 0x7fff, 0xffff, false, "ちょうど最大 => 結果は計算通り, オーバーフローなし");
            CheckAddLogical(0x8000, 0x8000, 0, true, "最大より大きい => 結果は反転, オーバーフローあり");
        }

        private void CheckAddLogical(
            UInt16 ui16Val1, UInt16 ui16Val2, UInt16 expectedResult, Boolean expectedOverflow, String message)
        {
            Word word1 = new Word(ui16Val1);
            Word word2 = new Word(ui16Val2);
            Boolean actualOverflow;
            Word resultWord = Alu.AddLogical(word1, word2, out actualOverflow);

            UInt16 actualResult = resultWord.GetAsUnsigned();
            Assert.AreEqual(expectedResult, actualResult, "Result: " + message);
            Assert.AreEqual(expectedOverflow, actualOverflow, "Overflow: " + message);
        }

        /// <summary>
        /// CompareArithmetic メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void CompareArithmetic()
        {
            CheckCompareArithmetic(32767, 32766, false, false, "正の数: n1 > n2 => 負でない, 零でない");
            CheckCompareArithmetic(32767, 32767, false, true, "正の数: n1 == n2 => 負でない, 零");
            CheckCompareArithmetic(32766, 32767, true, false, "正の数: n1 < n2 => 負, 零でない");

            CheckCompareArithmetic(-32767, -32768, false, false, "負の数: n1 > n2 => 負でない, 零でない");
            CheckCompareArithmetic(-32768, -32768, false, true, "負の数: n1 == n2 => 負でない, 零");
            CheckCompareArithmetic(-32768, -32767, true, false, "負の数: n1 < n2 => 負, 零でない");
        }

        private void CheckCompareArithmetic(
            Int16 i16Val1, Int16 i16Val2, Boolean expectedSign, Boolean expectedZero, String message)
        {
            Word word1 = new Word(i16Val1);
            Word word2 = new Word(i16Val2);
            Boolean actualSign;
            Boolean actualZero;
            Alu.CompareArithmetic(word1, word2, out actualSign, out actualZero);

            Assert.AreEqual(expectedSign, actualSign, "Sign: " + message);
            Assert.AreEqual(expectedZero, actualZero, "Zero: " + message);
        }

        /// <summary>
        /// CompareLogical メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void CompareLogical()
        {
            CheckCompareLogical(0x8000, 0x7fff, false, false, "0x8000 付近: n1 > n2 => 負でない, 零でない");
            CheckCompareLogical(0x8000, 0x8000, false, true, "0x8000 付近: n1 == n2 => 負でない, 零");
            CheckCompareLogical(0x7fff, 0x8000, true, false, "0x8000 付近: n1 < n2 => 負, 零でない");

            CheckCompareLogical(0xffff, 0xfffe, false, false, "0xffff 付近: n1 > n2 => 負でない, 零でない");
            CheckCompareLogical(0xffff, 0xffff, false, true, "0xffff 付近: n1 == n2 => 負でない, 零");
            CheckCompareLogical(0xfffe, 0xffff, true, false, "0xffff 付近: n1 < n2 => 負, 零でない");
        }

        private void CheckCompareLogical(
            UInt16 ui16Val1, UInt16 ui16Val2, Boolean expectedSign, Boolean expectedZero, String message)
        {
            Word word1 = new Word(ui16Val1);
            Word word2 = new Word(ui16Val2);
            Boolean actualSign;
            Boolean actualZero;
            Alu.CompareLogical(word1, word2, out actualSign, out actualZero);

            Assert.AreEqual(expectedSign, actualSign, "Sign: " + message);
            Assert.AreEqual(expectedZero, actualZero, "Zero: " + message);
        }
    }
}
