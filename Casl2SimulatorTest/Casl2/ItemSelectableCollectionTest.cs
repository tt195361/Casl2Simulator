using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="ItemSelectableCollection&lt;T&gt;"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ItemSelectableCollectionTest
    {
        #region Instance Members
        private ItemSelectableCollection<RelocatableModule> m_target;
        private RelocatableModule m_relModule1;
        private RelocatableModule m_relModule2;
        private RelocatableModule m_relModule3;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_relModule1 = new RelocatableModule();
            m_relModule2 = new RelocatableModule();
            m_relModule3 = new RelocatableModule();
            m_target = Make(m_relModule1, m_relModule2, m_relModule3);
        }

        /// <summary>
        /// <see cref="ItemSelectableCollection&lt;T&gt;"/> クラスのコンストラクタの引数 items のテストです。
        /// </summary>
        [TestMethod]
        public void Ctor_Items()
        {
            RelocatableModule[] oneItem = TestUtils.MakeArray<RelocatableModule>(m_relModule1);
            RelocatableModule[] noItem = TestUtils.MakeArray<RelocatableModule>();
            RelocatableModule[] nullItem = null;

            CheckCtor_Items(oneItem, true, "項目あり => OK");
            CheckCtor_Items(noItem, false, "項目なし => 例外");
            CheckCtor_Items(nullItem, false, "null => 例外");
        }

        private void CheckCtor_Items(RelocatableModule[] items, Boolean success, String message)
        {
            const Int32 SelectedItemIndex = 0;
            CheckCtor(items, SelectedItemIndex, success, message);
        }

        /// <summary>
        /// <see cref="ItemSelectableCollection&lt;T&gt;"/> クラスのコンストラクタの
        /// 引数 selectedItemIndex のテストです。
        /// </summary>
        [TestMethod]
        public void Ctor_SelectedItemIndex()
        {
            RelocatableModule[] items = TestUtils.MakeArray(m_relModule1, m_relModule2, m_relModule3);

            CheckCtor(items, -1, false, "下限より小さい => 例外");
            CheckCtor(items, 0, true, "ちょうど下限 => OK");
            CheckCtor(items, 2, true, "ちょうど上限 => OK");
            CheckCtor(items, 3, false, "上限より大きい => 例外");
        }

        private void CheckCtor(
            RelocatableModule[] items, Int32 selectedItemIndex, Boolean success, String message)
        {
            try
            {
                ItemSelectableCollection<RelocatableModule> notUsed =
                    new ItemSelectableCollection<RelocatableModule>(items, selectedItemIndex);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="ItemSelectableCollection&lt;T&gt;.SelectItem(T)"/> のテストです。
        /// </summary>
        [TestMethod]
        public void SelectItem()
        {
            RelocatableModule notInCollection = new RelocatableModule();
            RelocatableModule inCollection = m_relModule2;

            CheckSelectItem(null, false, "null => 例外");
            CheckSelectItem(notInCollection, false, "コレクションに含まれない項目 => 例外");
            CheckSelectItem(inCollection, true, "コレクションに含まれる項目 => OK");
        }

        private void CheckSelectItem(RelocatableModule itemToSelect, Boolean success, String message)
        {
            try
            {
                m_target.SelectItem(itemToSelect);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="ItemSelectableCollection&lt;T&gt;.SelectedItemIndex"/> のテストです。
        /// </summary>
        [TestMethod]
        public void SelectedItemIndex()
        {
            CheckSelectedItemIndex(m_relModule1, 0, "最初の項目 => 0");
            CheckSelectedItemIndex(m_relModule2, 1, "2 番目の項目 => 1");
            CheckSelectedItemIndex(m_relModule3, 2, "3 番目の項目 => 2");
        }

        private void CheckSelectedItemIndex(RelocatableModule itemToSelect, Int32 expected, String message)
        {
            m_target.SelectItem(itemToSelect);
            Int32 actual = m_target.SelectedItemIndex;
            Assert.AreEqual(expected, actual, message);
        }

        internal static ItemSelectableCollection<T> Make<T>(params T[] itemArray)
        {
            return new ItemSelectableCollection<T>(itemArray);
        }
    }
}
