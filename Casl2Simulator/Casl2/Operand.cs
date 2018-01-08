using System;
using System.Linq;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 命令のオペランドを表わす抽象クラスです。
    /// </summary>
    internal abstract class Operand
    {
        internal static String Join(params Object[] args)
        {
            return args.Select((arg) => arg.ToString())
                       .MakeList(Casl2Defs.Comma.ToString());
        }

        internal static String ReadItem(ReadBuffer buffer)
        {
            return buffer.ReadWhile((c) => !Operand.EndOfItem(c));
        }

        private static Boolean EndOfItem(Char current)
        {
            return Line.EndOfField(current) || current == Casl2Defs.Comma;
        }

        protected Operand()
        {
            //
        }
    }
}
