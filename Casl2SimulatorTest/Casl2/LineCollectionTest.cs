using System;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="LineCollection"/> クラスの単体テストです。
    /// </summary>
    internal class LineCollectionTest
    {
        internal static void Check(IEnumerable<Line> lines, IEnumerable<String> expectedText, String message)
        {
            IEnumerable<String> actualText = lines.Select((line) => line.Text);
            TestUtils.CheckEnumerable(expectedText, actualText, message);
        }
    }
}
