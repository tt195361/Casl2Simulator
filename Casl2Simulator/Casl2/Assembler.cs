using System;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// ソーステキストをアセンブルし再配置可能モジュールを生成します。
    /// </summary>
    internal static class Assembler
    {
        /// <summary>
        /// 一連のソースファイルをアセンブルし、一連の再配置可能モジュールを生成します。
        /// </summary>
        /// <param name="srcFiles">アセンブルする一連のソースファイルです。</param>
        /// <returns>生成した一連の再配置可能モジュールを返します。</returns>
        internal static ItemSelectableCollection<RelocatableModule> Assemble(
            this ItemSelectableCollection<SourceFile> srcFiles)
        {
            // 一連のソースファイルをアセンブルし、その結果をチェックし、一連の再配置可能モジュールを生成する。
            return srcFiles.DoAssemble()
                           .Check()
                           .MakeItemSelectableCollection(srcFiles.SelectedItemIndex);
        }

        private static IEnumerable<RelocatableModule> DoAssemble(this IEnumerable<SourceFile> srcFiles)
        {
            // ToArray() して、ここで Assemble() を実行させ、再配置可能モジュールを生成する。
            return srcFiles.Select((srcFile) => Assemble(srcFile))
                           .ToArray();
        }

        private static IEnumerable<RelocatableModule> Check(this IEnumerable<RelocatableModule> relModules)
        {
            Int32 nullCount = relModules.Count((relModule) => relModule == null);
            if (0 < nullCount)
            {
                throw new Casl2SimulatorException(Resources.MSG_FailedToAssemble);
            }

            return relModules;
        }

        private static RelocatableModule Assemble(SourceFile srcFile)
        {
            RelocatableModule relModule = new RelocatableModule();
            Boolean processingError = srcFile.ProcessSourceText(relModule.LabelTable);
            if (processingError)
            {
                // ソーステキストの処理でエラーなら、ここで終了。
                return null;
            }
            else
            {
                // ソーステキストの処理に成功すれば、ラベルのオフセットを設定し、コードを生成する。
                srcFile.ProcessedLines
                       .SetLabelOffset(relModule.LabelTable)
                       .GenerateCode(relModule);
                return relModule;
            }
        }

        private static IEnumerable<ProgramLine> SetLabelOffset(
            this IEnumerable<ProgramLine> lines, LabelTable lblTable)
        {
            MemoryOffset offset = MemoryOffset.Zero;
            foreach (ProgramLine line in lines)
            {
                line.SetLabelOffset(lblTable, offset);
                Int32 wordCount = line.GetCodeWordCount();
                offset = offset.Add(wordCount);
            }

            return lines;
        }

        private static void GenerateCode(this IEnumerable<ProgramLine> lines, RelocatableModule relModule)
        {
            lines.ForEach((line) => line.GenerateCode(relModule));
        }

        internal static RelocatableModule AssembleForUnitTest(SourceFile srcFile)
        {
            return Assemble(srcFile);
        }
    }
}
