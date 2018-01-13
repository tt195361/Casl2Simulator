using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Relocation クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RelocationTest
    {
        /// <summary>
        /// AddRelocationWord メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void AddRelocationWord()
        {
            RelocatableModule relModule = new RelocatableModule();
            LabelManager lblManager = new LabelManager();

            Label label = new Label("LBL001");
            const UInt16 LabelOffset = 0xABCD;
            lblManager.RegisterForUnitTest(label, LabelOffset);

            const UInt16 One = 1;
            const UInt16 Two = 2;
            const UInt16 Three = 3;
            const UInt16 RelocationCodeOffset = 3;

            // Code の語を追加し、CodeOffset を 0 でない値にして、テストでチェックしやすくする。
            relModule.AddWord(new Word(One));
            relModule.AddWord(new Word(Two));
            relModule.AddWord(new Word(Three));

            Relocation relocation = new Relocation();
            relocation.AddRelocationWord(relModule, lblManager, label);

            Assert.AreEqual(
                RelocationCodeOffset, relocation.CodeOffset,
                "再配置するコードの位置が記録されている");
            RelocatableModuleTest.Check(
                relModule,
                WordTest.MakeArray(One, Two, Three, LabelOffset),
                "ラベルのオフセットがコードに追加される");
        }
    }
}
