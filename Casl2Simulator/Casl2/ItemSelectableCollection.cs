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
            : this(items, 0)
        {
            //
        }

        internal ItemSelectableCollection(IEnumerable<T> items, Int32 selectedItemIndex)
        {
            if (items == null || items.Count() == 0)
            {
                String message = String.Format(
                    Resources.MSG_AtLeastOneItemForTheClass, nameof(ItemSelectableCollection<T>));
                throw new Casl2SimulatorException(message);
            }

            m_itemList = new List<T>(items);
            SetSelectedItemIndex(selectedItemIndex);
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
        /// コレクション中の指定の項目を選択します。
        /// </summary>
        /// <param name="itemToSelect">選択する項目です。</param>
        internal void SelectItem(T itemToSelect)
        {
            Int32 selectedItemIndex = m_itemList.IndexOf(itemToSelect);
            if (selectedItemIndex < 0)
            {
                throw new Casl2SimulatorException(Resources.MSG_NoItemInCollection);
            }

            SetSelectedItemIndex(selectedItemIndex);
        }

        /// <summary>
        /// 選択された項目を取得します。
        /// </summary>
        internal T SelectedItem
        {
            get { return m_itemList[m_selectedItemIndex]; }
        }

        private void SetSelectedItemIndex(Int32 selectedItemIndex)
        {
            ArgChecker.CheckRange(selectedItemIndex, 0, m_itemList.Count - 1, nameof(selectedItemIndex));
            m_selectedItemIndex = selectedItemIndex;
        }

        internal Int32 SelectedItemIndex
        {
            get { return m_selectedItemIndex; }
        }
    }

    internal static class ItemSelectableCollectionUtils
    {
        internal static ItemSelectableCollection<T> MakeItemSelectableCollection<T>(
            this IEnumerable<T> items, Int32 selectedItemIndex)
        {
            return new ItemSelectableCollection<T>(items, selectedItemIndex);
        }
    }
}
