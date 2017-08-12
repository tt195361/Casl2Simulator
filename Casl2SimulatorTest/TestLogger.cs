using System;
using System.Text;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest
{
    /// <summary>
    /// 単体テストのログを記録する。
    /// </summary>
    internal class TestLogger
    {
        private readonly StringBuilder m_log;

        internal TestLogger()
        {
            m_log = new StringBuilder();
        }

        internal String Log
        {
            get { return m_log.ToString(); }
        }

        internal void Add(String format, params Object[] args)
        {
            String message = String.Format(format, args);
            m_log.AppendLine(message);
        }

        internal static String ExpectedLog(params String[] messages)
        {
            StringBuilder builder = new StringBuilder();
            messages.ForEach((message) => builder.AppendLine(message));
            return builder.ToString();
        }
    }
}
