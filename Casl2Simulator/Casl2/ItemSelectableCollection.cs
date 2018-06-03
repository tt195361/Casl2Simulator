using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 項目を選択できるコレクションです。
    /// </summary>
    /// <typeparam name="T">コレクションに格納する項目の型です。</typeparam>
    internal class ItemSelectableCollection<T> : IEnumerable<T>
    {
        #region Instance Members
        private readonly List<T> m_itemList;
        private Int32 m_selectedItemIndex;
        #endregion

        internal ItemSelectableCollection(IEnumerable<T> items)
        {
            if (items.Count() == 0)
            {
                String message = String.Format(
                    Resources.MSG_AtLeastOneItemForTheClass, nameof(ItemSelectableCollection<T>));
                throw new Casl2SimulatorException(message);
            }

            m_itemList = new List<T>(items);
            m_selectedItemIndex = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_itemList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Add(T item)
        {
            m_itemList.Add(item);
        }

        /// <summary>
        /// 指定のインデックスの項目を選択します。
        /// </summary>
        /// <param name="selectedItemIndex">選択する項目を指定するインデックスです。</param>
        internal void SelectItem(Int32 selectedItemIndex)
        {
            ArgChecker.CheckRange(selectedItemIndex, 0, m_itemList.Count - 1, nameof(selectedItemIndex));
            m_selectedItemIndex = selectedItemIndex;
        }

        /// <summary>
        /// 選択された項目を取得します。
        /// </summary>
        internal T SelectedItem
        {
            get { return m_itemList[m_selectedItemIndex]; }
        }
    }
}
