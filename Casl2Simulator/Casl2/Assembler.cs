using System;
using System.Collections.Generic;
using System.Linq;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// ソーステキストをアセンブルし、再配置可能コードを生成します。
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

            // TODO: ラベルを登録する。リテラルの DC 命令を展開する。
            m_processedLines = new LineCollection(macroExpandedLines);

            Int32 errorCount = m_processedLines.Count((line) => line.HasError());
            return errorCount == 0;
        }

        private IEnumerable<Line> ParseLines(String[] sourceText)
        {
            return sourceText.Select((text) => Line.Parse(text));
        }

        private IEnumerable<Line> ExpandMacro(IEnumerable<Line> parsedLines)
        {
            // マクロ展開の結果は複数行になるので、line.ExpandMacro() は IEnumerable<Line> を返し、
            // Select() の結果の型は IEnumerable<IEnumerable<Line>> になる。
            // SelectMany() で IEnumerable<IEnumerable<Line>> を IEnumerable<Line> にする。
            return parsedLines.Select((line) => line.ExpandMacro())
                              .SelectMany((expandedLines) => expandedLines);
        }

        internal void Assemble(String[] sourceText)
        {
            // TODO: 実装する。
        }
    }
}
