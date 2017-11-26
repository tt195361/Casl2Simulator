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

        // 登録されていないラベルに対して返すオフセット。
        private const UInt16 NotRegisteredOffset = 0x0000;
        #endregion

        internal LabelManager()
        {
            m_labelDictionary = new Dictionary<String, UInt16>();
        }

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
        /// 指定のラベルが登録されているかどうかを返します。
        /// </summary>
        /// <param name="label">登録されているかどうかを調べるラベルです。</param>
        /// <returns>
        /// 指定のラベルが登録されていれば <see langword="true"/> を、
        /// 登録されていなければ <see langword="false"/> を返します。
        /// </returns>
        internal Boolean IsRegistered(Label label)
        {
            return m_labelDictionary.ContainsKey(label.Name);
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
            if (!IsRegistered(label))
            {
                return NotRegisteredOffset;
            }
            else
            {
                return m_labelDictionary[label.Name];
            }
        }
    }
}
