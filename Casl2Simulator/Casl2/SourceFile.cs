using System;
using System.Collections.Generic;
using System.Linq;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// プログラムを記述するソースファイルを表わします。
    /// </summary>
    internal class SourceFile
    {
        #region Instance Fields
        private String m_name;
        private String[] m_sourceText;
        private IEnumerable<ProgramLine> m_processedLines;
        #endregion

        internal SourceFile(String name)
            : this(name, new String[0])
        {
            //
        }

        private SourceFile(String name, String[] sourceText)
        {
            m_name = name;
            m_sourceText = sourceText;
            m_processedLines = new ProgramLine[0];
        }

        internal IEnumerable<String> SourceText
        {
            get { return m_sourceText; }
        }

        internal IEnumerable<ProgramLine> ProcessedLines
        {
            get { return m_processedLines; }
        }

        internal Boolean ProcessSourceText(LabelTable lblTable)
        {
            m_processedLines = SourceTextProcessor.Process(SourceText, lblTable);
            Boolean processingError = (0 < CountErrorLine(m_processedLines));
            return processingError;
        }

        private Int32 CountErrorLine(IEnumerable<ProgramLine> programLines)
        {
            return programLines.Count((line) => line.HasError());
        }

        internal static SourceFile MakeForUnitTest(String name, String[] sourceText)
        {
            return new SourceFile(name, sourceText);
        }
    }
}
