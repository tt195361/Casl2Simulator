using System;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// ソーステキストをアセンブルし再配置可能コードを生成します。
    /// </summary>
    internal class Assembler
    {
        #region Fields
        private readonly LabelManager m_lblManager;
        private LineCollection m_processedLines;
        #endregion

        internal Assembler()
        {
            m_lblManager = new LabelManager();
        }

        internal LineCollection ProcessedLines
        {
            get { return m_processedLines; }
        }

        internal Boolean ProcessSourceText(String[] sourceText)
        {
            IEnumerable<Line> parsedLines = ParseLines(sourceText);
            ProgramChecker.Check(parsedLines);
            IEnumerable<Line> macroExpandedLines = ExpandMacro(parsedLines);

            // プログラムのラベルを先に登録し、リテラルで生成する DC 命令のラベルと重複しないようにする。
            RegisterLabel(macroExpandedLines);
            IEnumerable<Line> literalDcGeneratedLines = GenerateLiteralDc(macroExpandedLines);
            m_processedLines = new LineCollection(literalDcGeneratedLines);

            return m_processedLines.NoErrorLine();
        }

        private IEnumerable<Line> ParseLines(String[] sourceText)
        {
            return sourceText.Select((text) => Line.Parse(text));
        }

        private IEnumerable<Line> ExpandMacro(IEnumerable<Line> lines)
        {
            // マクロ展開の結果は複数行になるので、line.ExpandMacro() は IEnumerable<Line> を返し、
            // Select() の結果の型は IEnumerable<IEnumerable<Line>> になる。
            // SelectMany() で IEnumerable<IEnumerable<Line>> を IEnumerable<Line> にする。
            return lines.Select((line) => line.ExpandMacro())
                        .SelectMany((expandedLines) => expandedLines);
        }

        private void RegisterLabel(IEnumerable<Line> lines)
        {
            lines.ForEach((line) => line.RegisterLabel(m_lblManager));
        }

        private IEnumerable<Line> GenerateLiteralDc(IEnumerable<Line> lines)
        {
            return DoGeneratedLiteralDc(lines).SelectMany((lineEnumerable) => lineEnumerable);
        }

        private IEnumerable<IEnumerable<Line>> DoGeneratedLiteralDc(IEnumerable<Line> lines)
        {
            // リテラルから生成される DC 命令は、END 命令の直前にまとめて配置される。
            // END 命令の前までに続いて、生成された DC 命令を出力し、その後に END 命令以降を出力する。
            Func<Line, Boolean> notEnd = (line) => !line.IsEnd();
            yield return lines.TakeWhile(notEnd);
            yield return lines.Select((line) => line.GenerateLiteralDc(m_lblManager))
                                                    .Where((generatedLine) => generatedLine != null);
            yield return lines.SkipWhile(notEnd);
        }

        internal void Assemble(String[] sourceText)
        {
            // TODO: 実装する。
        }
    }
}
