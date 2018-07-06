using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 再配置可能モジュールを結合し実行可能モジュールを生成するリンカーです。
    /// </summary>
    internal class Linker
    {
        #region Static Fields
        private static readonly MemoryAddress LoadAddress = MemoryAddress.Zero; 
        #endregion

        #region Instance Fields
        private readonly EntryPointTable m_entryPointTable;
        #endregion

        internal Linker()
        {
            m_entryPointTable = new EntryPointTable();
        }

        internal EntryPointTable EntryPointTable
        {
            get { return m_entryPointTable; }
        }

        /// <summary>
        /// 指定の再配置可能モジュールを結合し実行可能モジュールを生成します。
        /// </summary>
        /// <param name="relModules">結合する再配置可能モジュールです。</param>
        /// <returns>生成した実行可能モジュールを返します。</returns>
        internal ExecutableModule Link(ItemSelectableCollection<RelocatableModule> relModules)
        {
            AssignLabelAddress(relModules);
            RegisterEntryPointsIn(relModules);
            ResolveLabelReferencesFor(relModules);
            return MakeExecutableModule(relModules);
        }

        private void AssignLabelAddress(ItemSelectableCollection<RelocatableModule> relModules)
        {
            MemoryAddress baseAddress = LoadAddress;
            foreach (RelocatableModule relModule in relModules)
            {
                relModule.AssignLabelAddress(baseAddress);

                MemorySize wordsSize = relModule.GetWordsSize();
                baseAddress = baseAddress.Add(wordsSize);
            }
        }

        private void RegisterEntryPointsIn(ItemSelectableCollection<RelocatableModule> relModules)
        {
            relModules.ForEach((relModule) => relModule.RegisterEntryPointTo(m_entryPointTable));
        }

        private void ResolveLabelReferencesFor(ItemSelectableCollection<RelocatableModule> relModules)
        {
            relModules.ForEach((relModule) => relModule.ResolveLabelReferences(m_entryPointTable));
        }

        private ExecutableModule MakeExecutableModule(ItemSelectableCollection<RelocatableModule> relModules)
        {
            MemoryAddress execStartAddress = GetExecStartAddress(relModules);
            WordCollection linkedWords = MakeLinkedWords(relModules);
            return new ExecutableModule(LoadAddress, execStartAddress, linkedWords);
        }

        private MemoryAddress GetExecStartAddress(ItemSelectableCollection<RelocatableModule> relModules)
        {
            RelocatableModule selectedRelModule = relModules.SelectedItem;
            return selectedRelModule.EntryPoint.ExecStartAddress;
        }

        private WordCollection MakeLinkedWords(ItemSelectableCollection<RelocatableModule> relModules)
        {
            WordCollection linkedWords = new WordCollection();
            relModules.ForEach((relModule) => linkedWords.Add(relModule.Words));
            return linkedWords;
        }
    }

    internal static class LinkerUtils
    {
        internal static ExecutableModule Link(this ItemSelectableCollection<RelocatableModule> relModules)
        {
            Linker linker = new Linker();
            return linker.Link(relModules);
        }
    }
}
