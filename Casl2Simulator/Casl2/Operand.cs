using System;
using System.Linq;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    internal class Operand
    {
        #region Fields
        internal const String GR0 = "GR0";
        internal const String GR1 = "GR1";
        internal const String GR2 = "GR2";
        internal const String GR3 = "GR3";
        internal const String GR4 = "GR4";
        internal const String GR5 = "GR5";
        internal const String GR6 = "GR6";
        internal const String GR7 = "GR7";
        #endregion

        internal static String Join(params Object[] args)
        {
            return args.Select((arg) => arg.ToString())
                       .MakeList(Casl2Defs.Comma.ToString());
        }

        internal static Boolean EndOfItem(Char current)
        {
            return EndOfField(current) || current == Casl2Defs.Comma;
        }

        internal static Boolean EndOfField(Char current)
        {
            return Char.IsWhiteSpace(current) || current == ReadBuffer.EndOfStr;
        }
    }
}
