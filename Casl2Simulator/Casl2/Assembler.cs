using System;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// ソーステキストをアセンブルし再配置可能モジュールを生成します。
    /// </summary>
    internal class Assembler
    {
        #region Instance Fields
        private readonly RelocatableModule m_relModule;
        private LineCollection m_processedLines;
        #endregion

        internal Assembler()
        {
            m_relModule = new RelocatableModule();
        }

        /// <summary>
        /// ソーステキストをアセンブルして生成した再配置可能モジュールを取得します。
        /// </summary>
        internal RelocatableModule RelModule
        {
            get { return m_relModule; }
        }

        /// <summary>
        /// ソーステキストを処理したテキスト行のコレクションを取得します。
        /// 実施する処理は、マクロ展開とリテラルの DC 命令の生成です。
        /// </summary>
        internal LineCollection ProcessedLines
        {
            get { return m_processedLines; }
        }

        internal Boolean Assemble(String[] sourceText)
        {
            m_processedLines = ProcessSourceText(sourceText);
            if (!m_processedLines.NoErrorLine())
            {
                // ソーステキストの処理でエラーなら、ここで終了。
                return false;
            }
            else
            {
                // ソーステキストの処理に成功すれば、ラベルのオフセットを設定し、コードを生成する。
                SetLabelOffset(m_processedLines);
                GenerateCode(m_processedLines);
                return true;
            }
        }

        private LineCollection ProcessSourceText(String[] sourceText)
        {
            // ここで ToArray() して内容を実行させる。IEnumerable<Line> のままにしておくと、
            // 遅延評価で必要になるたびに実行される。
            Line[] parsedLines = ParseLines(sourceText).ToArray();
            ProgramChecker.Check(parsedLines);
            IEnumerable<Line> macroExpandedLines = ExpandMacro(parsedLines);

            // プログラムのラベルを先に登録し、リテラルで生成する DC 命令のラベルと重複しないようにする。
            RegisterLabels(macroExpandedLines);
            IEnumerable<Line> literalDcGeneratedLines = GenerateLiteralDc(macroExpandedLines);
            return new LineCollection(literalDcGeneratedLines);
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

        private void RegisterLabels(IEnumerable<Line> lines)
        {
            lines.ForEach((line) => line.RegisterLabel(m_relModule.LabelTable));
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
            yield return lines.Select((line) => line.GenerateLiteralDc(m_relModule.LabelTable))
                                                    .Where((generatedLine) => generatedLine != null);
            yield return lines.SkipWhile(notEnd);
        }

        private void SetLabelOffset(IEnumerable<Line> lines)
        {
            MemoryOffset offset = MemoryOffset.Zero;
            foreach (Line line in lines)
            {
                line.SetLabelOffset(m_relModule.LabelTable, offset);
                Int32 wordCount = line.GetCodeWordCount();
                offset = offset.Add(wordCount);
            }
        }

        private void GenerateCode(IEnumerable<Line> lines)
        {
            lines.ForEach((line) => line.GenerateCode(m_relModule));
        }
    }
}
