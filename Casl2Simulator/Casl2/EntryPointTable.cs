using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 各再配置可能モジュールの実行開始点の一覧を管理します。
    /// </summary>
    internal class EntryPointTable
    {
        #region Instance Fields
        private readonly Dictionary<String, EntryPoint> m_entryPointDictionary;
        #endregion

        internal EntryPointTable()
        {
            m_entryPointDictionary = new Dictionary<String, EntryPoint>();
        }

        internal IEnumerable<EntryPoint> EntryPoints
        {
            get { return m_entryPointDictionary.Values; }
        }

        /// <summary>
        /// 指定の実行開始点を一覧に登録します。
        /// </summary>
        /// <param name="entryPoint">一覧に登録する実行開始点です。</param>
        internal void Register(EntryPoint entryPoint)
        {
            String entryName = entryPoint.EntryLabel.Name;
            if (IsRegistered(entryName))
            {
                String message = String.Format(Resources.MSG_EntryNameAlreadyDefined, entryName);
                throw new Casl2SimulatorException(message);
            }

            m_entryPointDictionary.Add(entryName, entryPoint);
        }

        private Boolean IsRegistered(String name)
        {
            return m_entryPointDictionary.ContainsKey(name);
        }
    }
}
