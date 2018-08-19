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
        /// 一連のプログラムをアセンブルし、一連の再配置可能モジュールを生成します。
        /// </summary>
        /// <param name="programs">アセンブルする一連のプログラムです。</param>
        /// <returns>生成した一連の再配置可能モジュールを返します。</returns>
        internal static ItemSelectableCollection<RelocatableModule> Assemble(
            this ItemSelectableCollection<Casl2Program> programs)
        {
            // 一連のプログラムをアセンブルし、その結果をチェックし、一連の再配置可能モジュールを生成する。
            return programs.DoAssemble()
                           .Check()
                           .MakeItemSelectableCollection(programs.SelectedItemIndex);
        }

        private static IEnumerable<RelocatableModule> DoAssemble(this IEnumerable<Casl2Program> programs)
        {
            // ToArray() して、ここで Assemble() を実行させ、再配置可能モジュールを生成する。
            return programs.Select((program) => Assemble(program))
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

        private static RelocatableModule Assemble(Casl2Program program)
        {
            RelocatableModule relModule = new RelocatableModule();
            Boolean processingError = program.ProcessTextLines(relModule.LabelTable);
            if (processingError)
            {
                // ソーステキストの処理でエラーなら、ここで終了。
                return null;
            }
            else
            {
                // ソーステキストの処理に成功すれば、ラベルのオフセットを設定し、コードを生成する。
                program.ProcessedLines
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

        internal static RelocatableModule AssembleForUnitTest(Casl2Program program)
        {
            return Assemble(program);
        }
    }
}
