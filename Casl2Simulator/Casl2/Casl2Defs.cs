using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL II に関する定義です。
    /// </summary>
    internal class Casl2Defs
    {
        #region Characters
        internal const Char Semicolon = ';';
        internal const Char Comma = ',';
        internal const Char SingleQuote = '\'';
        internal const Char Sharp = '#';
        internal const Char Minus = '-';
        internal const Char EqualSign = '=';
        #endregion

        #region Register Names
        internal const String GR0 = "GR0";
        internal const String GR1 = "GR1";
        internal const String GR2 = "GR2";
        internal const String GR3 = "GR3";
        internal const String GR4 = "GR4";
        internal const String GR5 = "GR5";
        internal const String GR6 = "GR6";
        internal const String GR7 = "GR7";
        #endregion

        #region Instruction Mnemonics
        internal const String START = "START";
        internal const String DC = "DC";

        internal const String IN = "IN";
        internal const String RPUSH = "RPUSH";

        internal const String LAD = "LAD";
        internal const String PUSH = "PUSH";
        internal const String POP = "POP";
        internal const String SVC = "SVC";
        #endregion
    }
}
