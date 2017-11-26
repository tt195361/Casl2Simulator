using System;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    internal class RelocatableModuleTest
    {
        internal static void Check(RelocatableModule relModule, Word[] expectedWords, String message)
        {
            Word[] actualWords = relModule.GetWords();
            TestUtils.CheckEnumerable(expectedWords, actualWords, WordTest.Check, message);
        }
    }
}
