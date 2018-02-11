using System;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// 命令のニーモニックの定義です。
    /// </summary>
    internal static class MnemonicDef
    {
        // アセンブラ命令
        internal const String START = "START";
        internal const String END = "END";
        internal const String DS = "DS";
        internal const String DC = "DC";

        // マクロ命令
        internal const String IN = "IN";
        internal const String RPUSH = "RPUSH";

        // 機械語命令
        internal const String NOP = "NOP";

        internal const String LD = "LD";
        internal const String ST = "ST";
        internal const String LAD = "LAD";

        internal const String PUSH = "PUSH";
        internal const String POP = "POP";
        internal const String SVC = "SVC";

        // その他
        internal const String NULL = "NULL";
    }
}
