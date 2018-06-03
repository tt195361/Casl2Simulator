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
        /// <see cref="ItemSelectableCollection&lt;T&gt;.ItemSelectableCollection(System.Collections.Generic.IEnumerable{T})"/> のテストです。
        /// </summary>
        [TestMethod]
        public void Ctor()
        {
            RelocatableModule[] oneItem = new RelocatableModule[] { m_relModule1 };
            RelocatableModule[] noItems = new RelocatableModule[] { };

            CheckCtor(oneItem, true, "項目あり => OK");
            CheckCtor(noItems, false, "項目なし => 例外");
        }

        private void CheckCtor(RelocatableModule[] relModules, Boolean success, String message)
        {
            try
            {
                ItemSelectableCollection<RelocatableModule> notUsed =
                    new ItemSelectableCollection<RelocatableModule>(relModules);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="ItemSelectableCollection&lt;T&gt;.SelectItem"/> のテストです。
        /// </summary>
        [TestMethod]
        public void SelectItem()
        {
            CheckSelectItem(-1, false, "最小より小さい => 例外");
            CheckSelectItem(0, true, "ちょうど最小 => OK");
            CheckSelectItem(2, true, "ちょうど最大 => OK");
            CheckSelectItem(3, false, "最大より大きい => 例外");
        }

        private void CheckSelectItem(Int32 selectedItemIndex, Boolean success, String message)
        {
            try
            {
                m_target.SelectItem(selectedItemIndex);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="ItemSelectableCollection&lt;T&gt;.SelectedItem"/> のテストです。
        /// </summary>
        [TestMethod]
        public void SelectedItem()
        {
            CheckGetSelectedItem(0, m_relModule1, "最初の項目を選択 => 最初の項目を取得する");
            CheckGetSelectedItem(1, m_relModule2, "途中の項目を選択 => 選択された項目を取得する");
            CheckGetSelectedItem(2, m_relModule3, "最後の項目を選択 => 最後の項目を取得する");
        }

        private void CheckGetSelectedItem(Int32 selectedItemIndex, RelocatableModule expected, String message)
        {
            m_target.SelectItem(selectedItemIndex);
            RelocatableModule actual = m_target.SelectedItem;
            Assert.AreSame(expected, actual, message);
        }

        internal static ItemSelectableCollection<T> Make<T>(params T[] itemArray)
        {
            return new ItemSelectableCollection<T>(itemArray);
        }
    }
}
