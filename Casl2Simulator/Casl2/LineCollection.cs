using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// <see cref="Line"/> のコレクションです。
    /// </summary>
    internal class LineCollection : IEnumerable<Line>
    {
        #region Fields
        private readonly Line[] m_lines;
        #endregion

        internal LineCollection(IEnumerable<Line> lineEnumerable)
        {
            m_lines = lineEnumerable.ToArray();
        }

        public IEnumerator<Line> GetEnumerator()
        {
            return m_lines.Cast<Line>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
