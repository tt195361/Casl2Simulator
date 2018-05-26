using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

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

        /// <summary>
        /// <see cref="LabelReference.ResolveReferringAddress"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ResolveReferringAddress()
        {
            LabelAddressResolver labelAddrResolver = LabelAddressResolverTest.Make();
            const UInt16 LabelAddress = 0x2468;
            LabelDefinition labelDef = LabelDefinitionTest.Make("LBL001", 0, LabelAddress);
            labelAddrResolver.LabelTable.RegisterForUnitTest(labelDef);

            const Int32 WordCount = 4;
            WordCollection actualWords = WordCollectionTest.MakeWords(WordCount);
            MemoryOffset wordOffset = new MemoryOffset(1);
            LabelReference labelRef = LabelReference.MakeForUnitTest(labelDef.Label, wordOffset);

            labelRef.ResolveReferringAddress(labelAddrResolver, actualWords);

            WordCollection expectedWords = WordCollectionTest.MakeWords(WordCount);
            expectedWords[wordOffset] = new Word(LabelAddress);
            TestUtils.CheckEnumerable(
                expectedWords, actualWords, 
                "ラベルを参照する語の値がそのラベルのアドレスに置き換えられる");
        }

        internal static void Check(LabelReference expected, LabelReference actual, String message)
        {
            LabelTest.Check(expected.ReferringLabel, actual.ReferringLabel, "ReferringLabel: " + message);
            MemoryOffsetTest.Check(expected.WordOffset, actual.WordOffset, "ObjectOffset: " + message);
        }
    }
}
