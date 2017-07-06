using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 命令のオペランドを取り扱います。
    /// </summary>
    internal class OperandHandler
    {
        /// <summary>
        /// adr と x/r2 フィールドの指定により、実効アドレスを取得します。
        /// </summary>
        /// <param name="xR2Field">命令語の中の x/r2 フィールドの値です。</param>
        /// <param name="registerSet">COMET II の一そろいのレジスタです。</param>
        /// <param name="memory">COMET II の主記憶です。</param>
        /// <returns></returns>
        internal static Word GetEffectiveAddress(UInt16 xR2Field, RegisterSet registerSet, Memory memory)
        {
            // プログラムカウンタの指すアドレスより adr を取得します。
            Word adr = Fetcher.Fetch(registerSet.PR, memory);

            // x/r2 フィールドの値よりオフセット x の値を取得し、実効アドレス adr +L x を計算します。
            UInt16 x = GetAddressOffset(xR2Field, registerSet);
            Word effectiveAddress = adr.AddAsUnsigned(x);
            return effectiveAddress;
        }

        private static UInt16 GetAddressOffset(UInt16 xR2Field, RegisterSet registerSet)
        {
            if (xR2Field == 0)
            {
                return 0;
            }
            else
            {
                Register x = registerSet.GR[xR2Field];
                return x.Value.GetAsUnsigned();
            }
        }

        /// <summary>
        /// adr と x/r2 フィールドの指定により、実効アドレスの内容を取得します。
        /// </summary>
        /// <param name="xR2Field">命令語の中の x/r2 フィールドの値です。</param>
        /// <param name="registerSet">COMET II の一そろいのレジスタです。</param>
        /// <param name="memory">COMET II の主記憶です。</param>
        /// <returns></returns>
        internal static Word GetEaContents(UInt16 xR2Field, RegisterSet registerSet, Memory memory)
        {
            Word effectiveAddress = GetEffectiveAddress(xR2Field, registerSet, memory);
            Word eaContents = memory.Read(effectiveAddress);
            return eaContents;
        }
    }
}
