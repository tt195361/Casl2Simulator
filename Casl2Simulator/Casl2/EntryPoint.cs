using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// プログラムの実行開始点についての情報を保持します。
    /// </summary>
    internal class EntryPoint
    {
        #region Instance Fields
        private readonly Label m_execStartLabel;
        private readonly Label m_entryLabel;
        private MemoryAddress m_execStartAddress;
        #endregion

        internal EntryPoint(Label execStartLabel, Label entryLabel)
        {
            m_execStartLabel = execStartLabel;
            m_entryLabel = entryLabel;
            m_execStartAddress = MemoryAddress.Zero;
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
        internal Label EntryLabel
        {
            get { return m_entryLabel; }
        }

        /// <summary>
        /// 実行開始アドレスを取得します。
        /// </summary>
        internal MemoryAddress ExecStartAddress
        {
            get { return m_execStartAddress; }
        }

        /// <summary>
        /// 実行開始番地を参照するラベルのアドレスを解決し、実行開始アドレスを設定します。
        /// </summary>
        /// <param name="lblTable">
        /// 再配置可能モジュールで定義されたラベルを管理する <see cref="LabelTable"/> のオブジェクトです。
        /// </param>
        internal void ResolveExecStartAddress(LabelTable lblTable)
        {
            LabelDefinition labelDef = GetExecStartLabelDefinition(lblTable);
            m_execStartAddress = labelDef.AbsAddress;
        }

        private LabelDefinition GetExecStartLabelDefinition(LabelTable lblTable)
        {
            try
            {
                return lblTable.GetDefinitionFor(m_execStartLabel);
            }
            catch (Exception innerEx)
            {
                String message = String.Format(
                    Resources.MSG_CouldNotGetExecStartLabelDefinition, m_execStartLabel);
                throw new Casl2SimulatorException(message, innerEx);
            }
        }

        public override String ToString()
        {
            return String.Format(
                "ExecStart={0}: {1}, Entry={2}", m_execStartLabel, m_execStartAddress, m_entryLabel);
        }

        internal void SetExecStartAddressForUnitTest(MemoryAddress execStartAddress)
        {
            m_execStartAddress = execStartAddress;
        }
    }
}
