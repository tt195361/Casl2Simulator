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
        #region Fields
        private Alu.OperationMethod m_operationMethodToTest;
        #endregion

        #region Arithmetic Operation
        /// <summary>
        /// AddArithmetic メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void AddArithmetic()
        {
            m_operationMethodToTest = Alu.AddArithmetic;

            CheckArithmeticOp(20000, 12767, 32767, false, "ちょうど最大 => 結果は計算通り, オーバーフローなし");
            CheckArithmeticOp(20001, 12767, -32768, true, "最大より大きい => 結果は反転、オーバーフローあり");
            CheckArithmeticOp(-10000, -22768, -32768, false, "ちょうど最小 => 結果は計算通り、オーバーフローなし");
            CheckArithmeticOp(-10001, -22768, 32767, true, "最小より小さい => 結果は反転、オーバーフローあり");
        }

        /// <summary>
        /// SubtractArithmetic メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void SubtractArithmetic()
        {
            m_operationMethodToTest = Alu.SubtractArithmetic;

            CheckArithmeticOp(20000, -12767, 32767, false, "ちょうど最大 => 結果は計算通り, オーバーフローなし");
            CheckArithmeticOp(20001, -12767, -32768, true, "最大より大きい => 結果は反転、オーバーフローあり");
            CheckArithmeticOp(-10000, 22768, -32768, false, "ちょうど最小 => 結果は計算通り、オーバーフローなし");
            CheckArithmeticOp(-10001, 22768, 32767, true, "最小より小さい => 結果は反転、オーバーフローあり");
        }

        private void CheckArithmeticOp(
            Int16 i16Val1, Int16 i16Val2, Int16 expectedResult, Boolean expectedOverflow, String message)
        {
            Word word1 = new Word(i16Val1);
            Word word2 = new Word(i16Val2);
            Boolean actualOverflow;
            Word resultWord = m_operationMethodToTest(word1, word2, out actualOverflow);

            Int16 actualResult = resultWord.GetAsSigned();
            Assert.AreEqual(expectedResult, actualResult, "Result: " + message);
            Assert.AreEqual(expectedOverflow, actualOverflow, "Overflow: " + message);
        }
        #endregion // Arithmetic Operation

        #region Logical Operation
        /// <summary>
        /// AddLogical メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void AddLogical()
        {
            m_operationMethodToTest = Alu.AddLogical;

            CheckLogicalOp(0x7fff, 0x0001, 0x8000, false, "足して 0x8000 => 結果は計算通り, オーバーフローなし");
            CheckLogicalOp(0x8000, 0x7fff, 0xffff, false, "ちょうど最大 => 結果は計算通り, オーバーフローなし");
            CheckLogicalOp(0x8000, 0x8000, 0, true, "最大より大きい => 結果は反転, オーバーフローあり");
        }

        /// <summary>
        /// SubtractLogical メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void SubtractLogical()
        {
            m_operationMethodToTest = Alu.SubtractLogical;

            CheckLogicalOp(0x0001, 0x0001, 0x0000, false, "引いて 0 => 結果は計算通り, オーバーフローなし");
            CheckLogicalOp(0x0001, 0x0002, 0xffff, true, "引いたらマイナス => 結果は反転, オーバーフローあり");
        }

        /// <summary>
        /// And メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void And()
        {
            m_operationMethodToTest = Alu.And;

            CheckLogicalOp(
                0x5a5a, 0x5555, 0x5050, false, 
                "両方のビットが 1 なら 1、どちらかのビットが 0 なら 0, オーバーフローしない");
        }

        /// <summary>
        /// Or メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void Or()
        {
            m_operationMethodToTest = Alu.Or;

            CheckLogicalOp(
                0x5a5a, 0x5555, 0x5f5f, false,
                "両方のビットが 0 なら 0、どちらかのビットが 1 なら 1, オーバーフローしない");
        }

        /// <summary>
        /// Xor メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void Xor()
        {
            m_operationMethodToTest = Alu.Xor;

            CheckLogicalOp(
                0x3636, 0x6363, 0x5555, false,
                "両方のビットが同じなら 0、異なっていれば 1, オーバーフローしない");
        }

        private void CheckLogicalOp(
            UInt16 ui16Val1, UInt16 ui16Val2, UInt16 expectedResult, Boolean expectedOverflow, String message)
        {
            Word word1 = new Word(ui16Val1);
            Word word2 = new Word(ui16Val2);
            Boolean actualOverflow;
            Word resultWord = m_operationMethodToTest(word1, word2, out actualOverflow);

            UInt16 actualResult = resultWord.GetAsUnsigned();
            Assert.AreEqual(expectedResult, actualResult, "Result: " + message);
            Assert.AreEqual(expectedOverflow, actualOverflow, "Overflow: " + message);
        }
        #endregion // Logical Operation

        #region Comparison
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
        #endregion // Comparison

        #region Shift
        /// <summary>
        /// ShiftLeftArithmetic メソッドのシフト結果の単体テストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftArithmetic_Result()
        {
            CheckShiftLeftArithmetic_Result(
                0x0001, 0, 0x0001, "0x0001 << 0 => 0x0001: 0 回シフトは元のまま変わらず");
            CheckShiftLeftArithmetic_Result(
                0x0001, 1, 0x0002, "0x0001 << 1 => 0x0002: 1 回シフトするとビット 1 へ 1 ビット移動");
            CheckShiftLeftArithmetic_Result(
                0x0001, 14, 0x4000, "0x0001 << 14 => 0x4000: 14 回シフトするとビット 14 へ移動");
            CheckShiftLeftArithmetic_Result(
                0x0001, 15, 0x0000, "0x0001 << 15 => 0x0000: ビット 15 は符号ビットで、範囲外に出る");

            CheckShiftLeftArithmetic_Result(
                0x8001, 1, 0x8002, "0x8001 << 1 => 0x8002: ビット 15 は符号ビットで、そのまま残る");
            CheckShiftLeftArithmetic_Result(
                0x8001, 14, 0xc000, "0x8001 << 14 => 0xc000: ビット 15 は符号ビットで、そのまま残る");
            CheckShiftLeftArithmetic_Result(
                0x8001, 15, 0x8000, "0x8001 << 15 => 0x8000: ビット 15 は符号ビットで、そのまま残る");
        }

        private void CheckShiftLeftArithmetic_Result(
            UInt16 ui16Val, UInt16 count, UInt16 expected, String message)
        {
            CheckShiftResult(Alu.ShiftLeftArithmetic, ui16Val, count, expected, message);
        }

        /// <summary>
        /// ShiftLeftLogical メソッドのシフト結果の単体テストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftLogical_Result()
        {
            CheckShiftLeftLogical_Result(
                0x0001, 0, 0x0001, "0x0001 << 0 => 0x0001: 0 回シフトは元のまま変わらず");
            CheckShiftLeftLogical_Result(
                0x0001, 1, 0x0002, "0x0001 << 1 => 0x0002: 1 回シフトするとビット 1 へ 1 ビット移動");
            CheckShiftLeftLogical_Result(
                0x0001, 14, 0x4000, "0x0001 << 14 => 0x4000: 14 回シフトするとビット 14 へ移動");
            CheckShiftLeftLogical_Result(
                0x0001, 15, 0x8000, "0x0001 << 15 => 0x8000: 15 回シフトするとビット 15 へ移動");
            CheckShiftLeftLogical_Result(
                0x0001, 16, 0x0000, "0x0001 << 16 => 0x0000: 16 回シフトすると範囲外に出る");
            CheckShiftLeftLogical_Result(
                0x0001, 65535, 0x0000, "0x0001 << 65535 => 0x0000: 16 回以上は何回シフトしても同じ結果");

            CheckShiftLeftLogical_Result(
                0x8001, 1, 0x0002, "0x8001 << 1 => 0x0002: ビット 15 もシフトされる");
        }

        private void CheckShiftLeftLogical_Result(
            UInt16 ui16Val, UInt16 count, UInt16 expected, String message)
        {
            CheckShiftResult(Alu.ShiftLeftLogical, ui16Val, count, expected, message);
        }

        /// <summary>
        /// ShiftRightArithmetic メソッドのシフト結果の単体テストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightArithmetic_Result()
        {
            CheckShiftRightArithmetic_Result(
                0x4000, 0, 0x4000, "0x4000 >> 0 => 0x4000: 0 回シフトは元のまま変わらず");
            CheckShiftRightArithmetic_Result(
                0x4000, 1, 0x2000, "0x4000 >> 1 => 0x2000: 1 回シフトするとビット 13 へ 1 ビット移動");
            CheckShiftRightArithmetic_Result(
                0x4000, 14, 0x0001, "0x4000 >> 14 => 0x0001: 14 回シフトするとビット 0 へ移動");
            CheckShiftRightArithmetic_Result(
                0x4000, 15, 0x0000, "0x4000 >> 15 => 0x0000: 15 回シフトすると範囲外に出る");

            CheckShiftRightArithmetic_Result(
                0xc000, 1, 0xe000, "0xc000 >> 1 => 0xe000: ビット 14 は符号ビットがコピーされる");
            CheckShiftRightArithmetic_Result(
                0xc000, 14, 0xffff, "0xc000 >> 14 => 0xffff: ビット 14 は符号ビットがコピーされる");
        }

        private void CheckShiftRightArithmetic_Result(
            UInt16 ui16Val, UInt16 count, UInt16 expected, String message)
        {
            CheckShiftResult(Alu.ShiftRightArithmetic, ui16Val, count, expected, message);
        }

        /// <summary>
        /// ShiftRightLogical メソッドのシフト結果の単体テストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightLogical_Result()
        {
            CheckShiftRightLogical_Result(
                0x8000, 0, 0x8000, "0x8000 >> 0 => 0x8000: 0 回シフトは元のまま変わらず");
            CheckShiftRightLogical_Result(
                0x8000, 1, 0x4000, "0x8000 >> 1 => 0x4000: 1 回シフトするとビット 14 へ 1 ビット移動");
            CheckShiftRightLogical_Result(
                0x8000, 14, 0x0002, "0x8000 >> 14 => 0x0002: 14 回シフトするとビット 1 へ移動");
            CheckShiftRightLogical_Result(
                0x8000, 15, 0x0001, "0x8000 >> 15 => 0x0001: 15 回シフトするとビット 0 へ移動");
            CheckShiftRightLogical_Result(
                0x8000, 16, 0x0000, "0x8000 >> 16 => 0x0000: 16 回シフトすると範囲外に出る");
        }

        private void CheckShiftRightLogical_Result(
            UInt16 ui16Val, UInt16 count, UInt16 expected, String message)
        {
            CheckShiftResult(Alu.ShiftRightLogical, ui16Val, count, expected, message);
        }

        private void CheckShiftResult(
            Alu.ShiftMethod shiftMethod, UInt16 ui16Val, UInt16 count, UInt16 expected, String message)
        {
            Word word1 = new Word(ui16Val);
            Word word2 = new Word(count);
            UInt16 notUsed;
            Word resultWord = shiftMethod(word1, word2, out notUsed);

            UInt16 actual = resultWord.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// Shift メソッドの最後に送り出されたビットの値の単体テストです。
        /// </summary>
        [TestMethod]
        public void Shift_LastShiftedOutBit()
        {
            CheckShift_LastShiftedOutBit(Alu.ShiftLeftArithmetic, 0xbfff, 1, 0, "SLA はビット 14: 0");
            CheckShift_LastShiftedOutBit(Alu.ShiftLeftArithmetic, 0x4000, 1, 1, "SLA はビット 14: 1");

            CheckShift_LastShiftedOutBit(Alu.ShiftLeftLogical, 0x7fff, 1, 0, "SLL はビット 15: 0");
            CheckShift_LastShiftedOutBit(Alu.ShiftLeftLogical, 0x8000, 1, 1, "SLL はビット 15: 1");

            CheckShift_LastShiftedOutBit(Alu.ShiftRightArithmetic, 0xfffe, 1, 0, "SRA はビット 0: 0");
            CheckShift_LastShiftedOutBit(Alu.ShiftRightArithmetic, 0x0001, 1, 1, "SRA はビット 0: 1");

            CheckShift_LastShiftedOutBit(Alu.ShiftRightLogical, 0xfffe, 1, 0, "SRL はビット 0: 0");
            CheckShift_LastShiftedOutBit(Alu.ShiftRightLogical, 0x0001, 1, 1, "SRL はビット 0: 1");

            CheckShift_LastShiftedOutBit(Alu.ShiftLeftArithmetic, 0xffff, 0, 0, "0 回シフト => 0");
        }

        private void CheckShift_LastShiftedOutBit(
            Alu.ShiftMethod shiftMethod, UInt16 ui16Val, UInt16 count, UInt16 expected, String message)
        {
            Word word1 = new Word(ui16Val);
            Word word2 = new Word(count);
            UInt16 actual;
            Word notUsed = shiftMethod(word1, word2, out actual);
            Assert.AreEqual(expected, actual, message);
        }
        #endregion // Shift
    }
}
