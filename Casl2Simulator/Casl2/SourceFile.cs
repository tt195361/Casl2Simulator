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
        private ProgramLine[] m_processedLines;
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
            set { m_processedLines = value.ToArray(); }
        }

        internal Boolean NoErrorLine()
        {
            return CountErrorLine() == 0;
        }

        private Int32 CountErrorLine()
        {
            return m_processedLines.Count((line) => line.HasError());
        }

        internal static SourceFile MakeForUnitTest(String name, String[] sourceText)
        {
            return new SourceFile(name, sourceText);
        }
    }
}
