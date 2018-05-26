using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// プログラム内で定義されたラベルと各プログラムで定義された実行開始点を用いて
    /// ラベルが参照するアドレスを解決します。
    /// </summary>
    internal class LabelAddressResolver
    {
        #region Instance Members
        private readonly LabelTable m_labelTable;
        private readonly EntryPointTable m_entryPointTable;
        #endregion

        internal LabelAddressResolver(LabelTable lblTable, EntryPointTable entryPointTable)
        {
            m_labelTable = lblTable;
            m_entryPointTable = entryPointTable;
        }

        internal LabelTable LabelTable
        {
            get { return m_labelTable; }
        }

        internal EntryPointTable EntryPointTable
        {
            get { return m_entryPointTable; }
        }

        /// <summary>
        /// 指定のラベルが参照するアドレスを解決します。
        /// </summary>
        /// <param name="label">参照するアドレスを解決するラベルです。</param>
        /// <returns>指定のラベルが参照するアドレスを返します。</returns>
        internal MemoryAddress ResolveAddressFor(Label label)
        {
            // プログラム内で定義されていれば、そのアドレス。
            LabelDefinition labelDef = m_labelTable.FindDefinitionFor(label);
            if (labelDef != null)
            {
                return labelDef.AbsAddress;
            }

            // プログラム内で定義されていなければ、他のプログラムの入口名のアドレス。
            EntryPoint entryPoint = m_entryPointTable.Find(label);
            if (entryPoint != null)
            {
                return entryPoint.ExecStartAddress;
            }

            String message = String.Format(Resources.MSG_LabelOrEntryNameNotDefined, label.Name);
            throw new Casl2SimulatorException(message);
        }
    }
}
