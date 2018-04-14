using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="LabelReference"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LabelReferenceTest
    {
        /// <summary>
        /// <see cref="LabelReference.Make"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Make()
        {
            Label referringLabel = new Label("LBL001");
            WordCollection actualWords = new WordCollection();

            // actualWords に 1 語追加して、語のオフセットを初期値の 0 以外の値にする。
            MemoryOffset wordOffset = new MemoryOffset(1);
            Word addedWord = new Word(0x55AA);
            actualWords.Add(addedWord);

            LabelReference actualLabelRef = LabelReference.Make(referringLabel, actualWords);

            Word[] expectedWords = TestUtils.MakeArray(addedWord, Word.Zero);
            TestUtils.CheckEnumerable(
                expectedWords, actualWords,
                "オブジェクトコードに、参照するラベルのアドレスを入れる場所として、値が 0 の語が追加される");

            LabelReference expectedLabelRef = LabelReference.MakeForUnitTest(referringLabel, wordOffset);
            Check(
                expectedLabelRef, actualLabelRef,
                "作成された labelRef には、参照するラベルとそのアドレスを入れるオフセットが入る");
        }

        internal static void Check(LabelReference expected, LabelReference actual, String message)
        {
            LabelTest.Check(expected.ReferringLabel, actual.ReferringLabel, "ReferringLabel: " + message);
            MemoryOffsetTest.Check(expected.WordOffset, actual.WordOffset, "ObjectOffset: " + message);
        }
    }
}
