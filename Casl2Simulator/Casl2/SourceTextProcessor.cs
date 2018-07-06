using System;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// ソーステキストを処理します。
    /// </summary>
    internal static class SourceTextProcessor
    {
        /// <summary>
        /// 一連のソーステキストを処理し、一連の <see cref="ProgramLine"/> を生成します。
        /// </summary>
        /// <param name="sourceText">処理する一連のソーステキストです。</param>
        /// <param name="lblTable">定義したラベルの一覧を管理する <see cref="LabelTable"/> のオブジェクトです。</param>
        /// <returns>生成した <see cref="ProgramLine"/> の配列を返します。</returns>
        internal static IEnumerable<ProgramLine> Process(IEnumerable<String> sourceText, LabelTable lblTable)
        {
            // ソーステキストを解釈し、チェックし、マクロを展開する。
            // プログラムのラベルを先に登録し、リテラルで生成する DC 命令のラベルと重複しないようにする。
            // ToArray() して、ここで実行させる。
            return sourceText.ParseText()
                             .CheckParsedLines()
                             .ExpandMacro()
                             .RegisterLabels(lblTable)
                             .GenerateLiteralDc(lblTable)
                             .ToArray();
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
    }
}
