using System;
using System.Collections.Generic;
using System.Linq;
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
            srcFile.ProcessedLines = srcFile.SourceText
                                            .ProcessSourceText(relModule.LabelTable);
            if (!srcFile.NoErrorLine())
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

        private static IEnumerable<ProgramLine> ProcessSourceText(
            this IEnumerable<String> sourceText, LabelTable lblTable)
        {
            // ソーステキストを解釈し、チェックし、マクロを展開する。
            // プログラムのラベルを先に登録し、リテラルで生成する DC 命令のラベルと重複しないようにする。
            return sourceText.ParseText()
                             .CheckParsedLines()
                             .ExpandMacro()
                             .RegisterLabels(lblTable)
                             .GenerateLiteralDc(lblTable);
        }

        private static IEnumerable<ProgramLine> ParseText(this IEnumerable<String> sourceText)
        {
            // ここで ToArray() して内容を実行させる。IEnumerable<ProgramLine> のままにしておくと、
            // 遅延評価で必要になるたびに実行される。
            return sourceText.Select((text) => ProgramLine.Parse(text))
                             .ToArray();
        }

        private static IEnumerable<ProgramLine> CheckParsedLines(this IEnumerable<ProgramLine> parsedLines)
        {
            ProgramChecker.Check(parsedLines);
            return parsedLines;
        }

        private static IEnumerable<ProgramLine> ExpandMacro(this IEnumerable<ProgramLine> lines)
        {
            // マクロ展開の結果は複数行になるので、line.ExpandMacro() は IEnumerable<ProgramLine> を返し、
            // Select() の結果の型は IEnumerable<IEnumerable<ProgramLine>> になる。
            // SelectMany() で IEnumerable<IEnumerable<ProgramLine>> を IEnumerable<ProgramLine> にする。
            return lines.Select((line) => line.ExpandMacro())
                        .SelectMany((expandedLines) => expandedLines);
        }

        private static IEnumerable<ProgramLine> RegisterLabels(
            this IEnumerable<ProgramLine> lines, LabelTable lblTable)
        {
            lines.ForEach((line) => line.RegisterLabel(lblTable));
            return lines;
        }

        private static IEnumerable<ProgramLine> GenerateLiteralDc(
            this IEnumerable<ProgramLine> lines, LabelTable lblTable)
        {
            return lines.DoGeneratedLiteralDc(lblTable)
                        .SelectMany((lineEnumerable) => lineEnumerable);
        }

        private static IEnumerable<IEnumerable<ProgramLine>> DoGeneratedLiteralDc(
            this IEnumerable<ProgramLine> lines, LabelTable lblTable)
        {
            // リテラルから生成される DC 命令は、END 命令の直前にまとめて配置される。
            // END 命令の前までに続いて、生成された DC 命令を出力し、その後に END 命令以降を出力する。
            Func<ProgramLine, Boolean> notEnd = (line) => !line.IsEnd();
            yield return lines.TakeWhile(notEnd);
            yield return lines.Select((line) => line.GenerateLiteralDc(lblTable))
                                                    .Where((generatedLine) => generatedLine != null);
            yield return lines.SkipWhile(notEnd);
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
