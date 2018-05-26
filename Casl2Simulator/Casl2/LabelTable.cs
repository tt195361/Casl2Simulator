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
        /// <param name="label">取得する定義を指定するラベルです。</param>
        /// <returns>指定のラベルに対応するラベルの定義を返します。</returns>
        internal LabelDefinition GetDefinitionFor(Label label)
        {
            LabelDefinition labelDef = FindDefinitionFor(label);
            if (labelDef == null)
            {
                String message = String.Format(Resources.MSG_LabelNotDefined, label.Name);
                throw new Casl2SimulatorException(message);
            }

            return labelDef;
        }

        /// <summary>
        /// 指定のラベルに対応するラベルの定義を探します。
        /// </summary>
        /// <param name="label">探し出す定義を指定するラベルです。</param>
        /// <returns>
        /// ラベルの定義が見つかった場合は、そのラベルの定義を返します。
        /// 見つからなかった場合は <see langword="null"/> を返します。
        /// </returns>
        internal LabelDefinition FindDefinitionFor(Label label)
        {
            String name = label.Name;
            if (!IsRegistered(name))
            {
                return null;
            }
            else
            {
                return m_labelDictionary[name];
            }
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

        internal void RegisterForUnitTest(LabelDefinition labelDef)
        {
            String name = labelDef.Label.Name;
            m_labelDictionary.Add(name, labelDef);
        }
    }
}
