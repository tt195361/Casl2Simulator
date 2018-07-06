﻿using System;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="ExecutableModule"/> クラスの単体テストです。
    /// </summary>
    internal class ExecutableModuleTest
    {
        internal static void Check(ExecutableModule expected, ExecutableModule actual, String message)
        {
            MemoryAddressTest.Check(
                expected.LoadAddress, actual.LoadAddress, "LoadAddress: " + message);
            MemoryAddressTest.Check(
                expected.ExecStartAddress, actual.ExecStartAddress, "ExecStartAddress: " + message);
            TestUtils.CheckEnumerable(
                expected.Words, actual.Words, "Words: " + message);
        }
    }
}
