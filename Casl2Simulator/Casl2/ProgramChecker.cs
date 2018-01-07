using System;
using System.Collections.Generic;
using System.Linq;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II プログラムの内容を検査します。
    /// </summary>
    internal static class ProgramChecker
    {
        internal static void Check(IEnumerable<Line> lines)
        {
            CheckStartCount(lines);
            CheckEndCount(lines);
            CheckInstructionAfterEnd(lines);
        }

        private static void CheckStartCount(IEnumerable<Line> lines)
        {
            Int32 startCount = lines.Count((line) => line.IsStart());
            if (startCount == 0)
            {
                throw new Casl2SimulatorException(Resources.MSG_NoStartInstruction);
            }
            else if (1 < startCount)
            {
                throw new Casl2SimulatorException(Resources.MSG_MoreThanOneStartInstruction);
            }
        }

        private static void CheckEndCount(IEnumerable<Line> lines)
        {
            Int32 endCount = lines.Count((line) => line.IsEnd());
            if (endCount == 0)
            {
                throw new Casl2SimulatorException(Resources.MSG_NoEndInstruction);
            }
            else if (1 < endCount)
            {
                throw new Casl2SimulatorException(Resources.MSG_MoreThanOneEndInstruction);
            }
        }

        private static void CheckInstructionAfterEnd(IEnumerable<Line> lines)
        {
            // END 命令までと END 命令自身をスキップし、そのあとに null でない有効な命令があるかチェックする。
            Int32 afterEndInstructionCount = lines.SkipWhile((line) => !line.IsEnd())
                                                  .Skip(1)
                                                  .Count((line) => !line.Instruction.IsNull());
            if (0 < afterEndInstructionCount)
            {
                throw new Casl2SimulatorException(Resources.MSG_InstructionAfterEnd);
            }
        }
    }
}
