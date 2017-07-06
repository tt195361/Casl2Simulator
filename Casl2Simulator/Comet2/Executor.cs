using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 命令を実行します。
    /// </summary>
    internal static class Executor
    {
        /// <summary>
        /// 命令を実行するメソッドの呼び出し形式を定義するデリゲートです。
        /// </summary>
        /// <param name="rR1Field">命令語の r/r1 フィールドの値です。</param>
        /// <param name="xR2Field">命令語の x/r2 フィールドの値です。</param>
        /// <param name="registerSet">COMET II の一そろいのレジスタです。</param>
        /// <param name="memory">COMET II の主記憶です。</param>
        internal delegate void ExecuteAction(
            UInt16 rR1Field, UInt16 xR2Field, RegisterSet registerSet, Memory memory);

        /// <summary>
        /// "LD r,adr,x" 命令を実行します。
        /// </summary>
        internal static void LoadEaContents(
            UInt16 rR1Field, UInt16 xR2Field, RegisterSet registerSet, Memory memory)
        {
            // r <- (実効アドレス)
            Register r = registerSet.GR[rR1Field];
            Word eaContents = OperandHandler.GetEaContents(xR2Field, registerSet, memory);
            r.Value = eaContents;
        }
    }
}
