using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// <see cref="ProgramLine"/> のコレクションです。
    /// </summary>
    internal class ProgramLineCollection : IEnumerable<ProgramLine>
    {
        #region Instance Fields
        private readonly ProgramLine[] m_lines;
        #endregion

        internal ProgramLineCollection(IEnumerable<ProgramLine> lineEnumerable)
        {
            m_lines = lineEnumerable.ToArray();
        }

        public IEnumerator<ProgramLine> GetEnumerator()
        {
            return m_lines.Cast<ProgramLine>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal Boolean NoErrorLine()
        {
            return CountErrorLine() == 0;
        }

        private Int32 CountErrorLine()
        {
            return m_lines.Count((line) => line.HasError());
        }
    }
}
