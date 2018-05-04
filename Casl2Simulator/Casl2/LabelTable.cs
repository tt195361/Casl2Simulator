using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 定義したラベルの一覧を管理します。
    /// </summary>
    internal class LabelTable
    {
        #region Instance Fields
        private readonly Dictionary<String, LabelDefinition> m_labelDictionary;
        private Int32 m_literalLabelNumber;
        #endregion

        internal LabelTable()
        {
            m_labelDictionary = new Dictionary<String, LabelDefinition>();
            m_literalLabelNumber = Label.MinLiteralLabelNumber;
        }

        /// <summary>
        /// 定義したラベルの一覧を取得します。
        /// </summary>
        internal IEnumerable<LabelDefinition> LabelDefinitions
        {
            get { return m_labelDictionary.Values; }
        }

        /// <summary>
        /// 指定のラベルを登録します。
        /// </summary>
        /// <param name="label">登録するラベルです。</param>
        internal void Register(Label label)
        {
            String name = label.Name;
            if (IsRegistered(name))
            {
                String message = String.Format(Resources.MSG_LabelAlreadyDefined, name);
                throw new Casl2SimulatorException(message);
            }

            LabelDefinition labelDef = new LabelDefinition(label);
            m_labelDictionary.Add(name, labelDef);
        }

        /// <summary>
        /// 指定のラベルに対応するラベルの定義を取得します。
        /// </summary>
        /// <param name="label">定義を取得するラベルです。</param>
        /// <returns>指定のラベルに対応するラベルの定義を返します。</returns>
        internal LabelDefinition GetDefinitionFor(Label label)
        {
            String name = label.Name;
            if (!IsRegistered(name))
            {
                String message = String.Format(Resources.MSG_LabelNotDefined, name);
                throw new Casl2SimulatorException(message);
            }

            return m_labelDictionary[name];
        }

        private Boolean IsRegistered(String name)
        {
            return m_labelDictionary.ContainsKey(name);
        }

        /// <summary>
        /// リテラルで使用するプログラムのものと重複しないラベルを作成します。
        /// </summary>
        /// <returns>作成したラベルを返します。</returns>
        internal Label MakeLiteralLabel()
        {
            for ( ; m_literalLabelNumber <= Label.MaxLiteralLabelNumber; ++m_literalLabelNumber)
            {
                String name = Label.MakeLiteralLabelName(m_literalLabelNumber);
                if (!IsRegistered(name))
                {
                    // 登録されていないラベル名が作成できれば、それを登録し、番号を次に進める。
                    Label literalLabel = new Label(name);
                    Register(literalLabel);
                    ++m_literalLabelNumber;
                    return literalLabel;
                }
            }

            String message = String.Format(Resources.MSG_CouldNotMakeLiteralLabel, Label.LiteralLabelPrefix);
            throw new Casl2SimulatorException(message);
        }

        /// <summary>
        /// それぞれのラベルの定義に絶対アドレスを割り当てます。
        /// </summary>
        /// <param name="baseAddress">
        /// ラベルが定義された再配置可能モジュールが実行可能モジュールで配置されるアドレスです。
        /// </param>
        internal void AssignLabelAddress(MemoryAddress baseAddress)
        {
            LabelDefinitions.ForEach((labelDef) => labelDef.AssignAbsAddress(baseAddress));
        }
    }
}
