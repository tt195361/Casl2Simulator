using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 再配置可能モジュールを結合し実行可能モジュールを生成するリンカーです。
    /// </summary>
    internal static class Linker
    {
        #region Static Fields
        private static readonly MemoryAddress LoadAddress = MemoryAddress.Zero;

        private static EntryPointTable m_entryPointTableForUnitTest;
        #endregion

        /// <summary>
        /// 指定の再配置可能モジュールを結合し実行可能モジュールを生成します。
        /// </summary>
        /// <param name="relModules">結合する再配置可能モジュールです。</param>
        /// <returns>生成した実行可能モジュールを返します。</returns>
        internal static ExecutableModule Link(this ItemSelectableCollection<RelocatableModule> relModules)
        {
            EntryPointTable entryPointTable = new EntryPointTable();
            m_entryPointTableForUnitTest = entryPointTable;
            return relModules.AssignLabelAddress()
                             .RegisterEntryPoints(entryPointTable)
                             .ResolveLabelReferences(entryPointTable)
                             .MakeExecutableModule();
        }

        private static ItemSelectableCollection<RelocatableModule> AssignLabelAddress(
            this ItemSelectableCollection<RelocatableModule> relModules)
        {
            MemoryAddress baseAddress = LoadAddress;
            foreach (RelocatableModule relModule in relModules)
            {
                relModule.AssignLabelAddress(baseAddress);

                MemorySize wordsSize = relModule.GetWordsSize();
                baseAddress = baseAddress.Add(wordsSize);
            }

            return relModules;
        }

        private static ItemSelectableCollection<RelocatableModule> RegisterEntryPoints(
            this ItemSelectableCollection<RelocatableModule> relModules, EntryPointTable entryPointTable)
        {
            relModules.ForEach((relModule) => relModule.RegisterEntryPointTo(entryPointTable));
            return relModules;
        }

        private static ItemSelectableCollection<RelocatableModule> ResolveLabelReferences(
            this ItemSelectableCollection<RelocatableModule> relModules, EntryPointTable entryPointTable)
        {
            relModules.ForEach((relModule) => relModule.ResolveLabelReferences(entryPointTable));
            return relModules;
        }

        private static ExecutableModule MakeExecutableModule(
            this ItemSelectableCollection<RelocatableModule> relModules)
        {
            MemoryAddress execStartAddress = GetExecStartAddress(relModules);
            WordCollection linkedWords = MakeLinkedWords(relModules);
            return new ExecutableModule(LoadAddress, execStartAddress, linkedWords);
        }

        private static MemoryAddress GetExecStartAddress(
            ItemSelectableCollection<RelocatableModule> relModules)
        {
            RelocatableModule selectedRelModule = relModules.SelectedItem;
            EntryPoint selectedEntryPoint = selectedRelModule.EntryPoint;
            return selectedEntryPoint.ExecStartAddress;
        }

        private static WordCollection MakeLinkedWords(
            ItemSelectableCollection<RelocatableModule> relModules)
        {
            WordCollection linkedWords = new WordCollection();
            relModules.ForEach((relModule) => linkedWords.Add(relModule.Words));
            return linkedWords;
        }

        internal static EntryPointTable EntryPointTableForUnitTest
        {
            get { return m_entryPointTableForUnitTest; }
        }
    }
}
