using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// ラベルを管理します。
    /// </summary>
    internal class LabelManager
    {
        #region Fields
        private readonly Dictionary<String, UInt16> m_labelDictionary;
        private Int32 m_literalLabelNumber;

        // 登録されていないラベルに対して返すオフセット。
        private const UInt16 NotRegisteredOffset = 0x0000;
        #endregion

        internal LabelManager()
        {
            m_labelDictionary = new Dictionary<String, UInt16>();
            m_literalLabelNumber = Label.MinLiteralLabelNumber;
        }

        // TODO: Remove
        /// <summary>
        /// 指定のラベルとそのラベルのプログラム内のオフセットを登録します。
        /// </summary>
        /// <param name="label">登録するラベルです。</param>
        /// <param name="offset">登録するラベルのプログラム内のオフセットです。</param>
        internal void Register(Label label, UInt16 offset)
        {
            if (m_labelDictionary.ContainsKey(label.Name))
            {
                String message = String.Format(Resources.MSG_LabelAlreadyDefined, label.Name);
                throw new Casl2SimulatorException(message);
            }

            m_labelDictionary.Add(label.Name, offset);
        }

        /// <summary>
        /// 指定のラベルを登録します。
        /// </summary>
        /// <param name="label">登録するラベルです。</param>
        internal void RegisterLabel(Label label)
        {
            String name = label.Name;
            if (IsRegistered(name))
            {
                String message = String.Format(Resources.MSG_LabelAlreadyDefined, name);
                throw new Casl2SimulatorException(message);
            }

            m_labelDictionary.Add(name, NotRegisteredOffset);
        }

        /// <summary>
        /// 登録したラベルに対してプログラム内のオフセットを設定します。
        /// </summary>
        /// <param name="label"><see cref="RegisterLabel"/>で登録したラベルです。</param>
        /// <param name="offset">指定のラベルに対して設定するプログラム内のオフセットの値です。</param>
        internal void SetOffset(Label label, UInt16 offset)
        {
            String name = label.Name;
            if (!IsRegistered(name))
            {
                String message = String.Format(Resources.MSG_LabelNotDefined, name);
                throw new Casl2SimulatorException(message);
            }

            m_labelDictionary[name] = offset;
        }

        /// <summary>
        /// 指定の名前のラベルが登録されているかどうかを返します。
        /// </summary>
        /// <param name="name">登録されているかどうかを調べるラベルの名前です。</param>
        /// <returns>
        /// 指定の名前のラベルが登録されていれば <see langword="true"/> を、
        /// 登録されていなければ <see langword="false"/> を返します。
        /// </returns>
        internal Boolean IsRegistered(String name)
        {
            return m_labelDictionary.ContainsKey(name);
        }

        /// <summary>
        /// 指定のラベルのオフセットを取得します。
        /// </summary>
        /// <param name="label">オフセットを取得するラベルです。</param>
        /// <returns>
        /// 指定のラベルが登録されていれば、登録されたオフセットを返します。
        /// 登録されていなければ、0 を返します。
        /// </returns>
        internal UInt16 GetOffset(Label label)
        {
            String name = label.Name;
            if (!IsRegistered(name))
            {
                return NotRegisteredOffset;
            }
            else
            {
                return m_labelDictionary[name];
            }
        }

        /// <summary>
        /// リテラルで使用するプログラムのものと重複しないラベルを作成します。
        /// </summary>
        /// <returns>
        /// 作成したラベルを返します。
        /// </returns>
        internal Label MakeLiteralLabel()
        {
            for ( ; m_literalLabelNumber <= Label.MaxLiteralLabelNumber; ++m_literalLabelNumber)
            {
                String name = Label.MakeLiteralLabelName(m_literalLabelNumber);
                if (!IsRegistered(name))
                {
                    // 登録されていないラベル名が作成できれば、それを登録し、番号を次に進める。
                    Label literalLabel = new Label(name);
                    RegisterLabel(literalLabel);
                    ++m_literalLabelNumber;
                    return literalLabel;
                }
            }

            String message = String.Format(Resources.MSG_CouldNotMakeLiteralLabel, Label.LiteralLabelPrefix);
            throw new Casl2SimulatorException(message);
        }
    }
}
