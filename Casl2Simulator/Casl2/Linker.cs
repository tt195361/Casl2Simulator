using System.Collections.Generic;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 再配置可能モジュールを結合し実行可能モジュールを生成するリンカーです。
    /// </summary>
    internal class Linker
    {
        /// <summary>
        /// 指定の再配置可能モジュールを結合し実行可能モジュールを生成します。
        /// </summary>
        /// <param name="relModules">結合する再配置可能モジュールです。</param>
        /// <returns>生成した実行可能モジュールを返します。</returns>
        internal ExecutableModule Link(IEnumerable<RelocatableModule> relModules)
        {
            AssignLabelAddress(relModules);

            return null;
        }

        private void AssignLabelAddress(IEnumerable<RelocatableModule> relModules)
        {
            MemoryAddress baseAddress = MemoryAddress.Zero;
            foreach (RelocatableModule relModule in relModules)
            {
                relModule.AssignLabelAddress(baseAddress);

                MemorySize wordsSize = relModule.GetWordsSize();
                baseAddress = baseAddress.Add(wordsSize);
            }
        }
    }
}
