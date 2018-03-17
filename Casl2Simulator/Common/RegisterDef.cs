using System;
using System.Collections.Generic;
using System.Linq;

namespace Tt195361.Casl2Simulator.Common
{
    internal static class RegisterDef
    {
        // GR (汎用レジスタ、General Register) は、GR0 ~ GR7 の 8 個。
        internal const String GR0 = "GR0";
        internal const String GR1 = "GR1";
        internal const String GR2 = "GR2";
        internal const String GR3 = "GR3";
        internal const String GR4 = "GR4";
        internal const String GR5 = "GR5";
        internal const String GR6 = "GR6";
        internal const String GR7 = "GR7";

        internal static readonly IEnumerable<String> GrNames = new String[]
        {
            GR0, GR1, GR2, GR3, GR4, GR5, GR6, GR7,
        };

        internal static readonly Int32 GrCount = GrNames.Count();

        internal const String PR = "PR";
        internal const String SP = "SP";
    }
}
