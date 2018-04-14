using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 実行開始番地についての情報を保持します。
    /// </summary>
    internal class EntryPoint
    {
        #region Instance Fields
        private readonly Label m_execStartLabel;
        private readonly Label m_exportLabel;
        #endregion

        internal EntryPoint(Label execStartLabel, Label exportLabel)
        {
            m_execStartLabel = execStartLabel;
            m_exportLabel = exportLabel;
        }

        /// <summary>
        /// 実行開始番地を参照するラベルを取得します。
        /// </summary>
        internal Label ExecStartLabel
        {
            get { return m_execStartLabel; }
        }

        /// <summary>
        /// 他のプログラムから入口名として参照できるラベルを取得します。
        /// </summary>
        internal Label ExportLabel
        {
            get { return m_exportLabel; }
        }

        public override String ToString()
        {
            return String.Format("ExecStart={0}, Export={1}", m_execStartLabel, m_exportLabel);
        }
    }
}
