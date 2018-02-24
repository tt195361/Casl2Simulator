using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="ImportLabel"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ImportLabelTest
    {
        /// <summary>
        /// AddImportLabelWord メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void AddImportLabelWord()
        {
            RelocatableModule relModule = new RelocatableModule();
            Label label = new Label("LBL001");

            const UInt16 One = 1;
            const UInt16 Two = 2;
            const UInt16 Three = 3;
            MemoryOffset CodeOffset = new MemoryOffset(3);
            const UInt16 PlaceHolder = 0;

            relModule.AddWord(new Word(One));
            relModule.AddWord(new Word(Two));
            relModule.AddWord(new Word(Three));

            ImportLabel importLabel = new ImportLabel();
            importLabel.AddImportLabelWord(relModule, label);

            LabelTest.Check(label, importLabel.Label, "参照先のラベルが記録される");
            MemoryOffsetTest.Check(
                CodeOffset, importLabel.CodeOffset, "再配置可能モジュールのコードの位置が記録される");
            RelocatableModuleTest.Check(
                relModule,
                WordTest.MakeArray(One, Two, Three, PlaceHolder),
                "場所を確保するため 0 がコードに追加される");
        }
    }
}
