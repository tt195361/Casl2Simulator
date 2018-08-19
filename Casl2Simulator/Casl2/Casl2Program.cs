using System;
using System.Collections.Generic;
using System.Linq;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL2 のプログラムです。
    /// </summary>
    internal class Casl2Program
    {
        #region Instance Fields
        private readonly String m_name;
        private readonly String[] m_textLines;
        private IEnumerable<ProgramLine> m_processedLines;
        #endregion

        internal Casl2Program(String name)
            : this(name, new String[0])
        {
            //
        }

        private Casl2Program(String name, String[] textLines)
        {
            m_name = name;
            m_textLines = textLines;
            m_processedLines = new ProgramLine[0];
        }

        /// <summary>
        /// プログラムの名前を取得します。
        /// </summary>
        public String Name
        {
            get { return m_name; }
        }

        internal IEnumerable<String> TextLines
        {
            get { return m_textLines; }
        }

        internal IEnumerable<ProgramLine> ProcessedLines
        {
            get { return m_processedLines; }
        }

        internal Boolean ProcessTextLines(LabelTable lblTable)
        {
            m_processedLines = TextLineProcessor.Process(TextLines, lblTable);
            Boolean processingError = (0 < CountErrorLine(m_processedLines));
            return processingError;
        }

        private Int32 CountErrorLine(IEnumerable<ProgramLine> programLines)
        {
            return programLines.Count((line) => line.HasError());
        }

        internal static Casl2Program MakeForUnitTest(String name, String[] textLines)
        {
            return new Casl2Program(name, textLines);
        }
    }
}
