using System;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 命令のオペランドを取り扱います。
    /// </summary>
    internal class OperandHandler
    {
        private delegate Word GetOperandAction(UInt16 xR2Field, RegisterSet registerSet, Memory memory);

        #region EaContents
        internal static readonly OperandHandler EaContents = new OperandHandler(GetEaContents);

        private static Word GetEaContents(UInt16 xR2Field, RegisterSet registerSet, Memory memory)
        {
            // 実効アドレスの内容を取得します。
            Word effectiveAddress = GetEffectiveAddress(xR2Field, registerSet, memory);
            Word eaContents = memory.Read(effectiveAddress);
            return eaContents;
        }
        #endregion

        #region EffectiveAddress
        internal static readonly OperandHandler EffectiveAddress = new OperandHandler(GetEffectiveAddress);

        private static Word GetEffectiveAddress(UInt16 xR2Field, RegisterSet registerSet, Memory memory)
        {
            // プログラムカウンタの指すアドレスより adr を取得します。
            Word adr = Fetcher.Fetch(registerSet.PR, memory);

            // x/r2 フィールドの値よりオフセット x の値を取得し、実効アドレス adr +L x を計算します。
            Word x = GetAddressOffset(xR2Field, registerSet);
            Word effectiveAddress = Alu.AddLogical(adr, x);
            return effectiveAddress;
        }

        private static Word GetAddressOffset(UInt16 xR2Field, RegisterSet registerSet)
        {
            ArgChecker.CheckRange(xR2Field, 0, GeneralRegisters.Count - 1, "x/r2");

            if (xR2Field == 0)
            {
                return Word.Zero;
            }
            else
            {
                Register x = registerSet.GR[xR2Field];
                return x.Value;
            }
        }
        #endregion

        #region Fields
        private readonly GetOperandAction m_getOperandAction;
        #endregion

        // このクラスのインスタンスは、このクラス内からのみ作成できます。
        private OperandHandler(GetOperandAction getOperandAction)
        {
            m_getOperandAction = getOperandAction;
        }

        /// <summary>
        /// 命令の演算対象の値を取得します。
        /// </summary>
        /// <param name="xR2Field">命令語の中の x/r2 フィールドの値です。</param>
        /// <param name="registerSet">COMET II の一そろいのレジスタです。</param>
        /// <param name="memory">COMET II の主記憶です。</param>
        /// <returns>命令の演算対象の値を返します。</returns>
        internal Word GetOperand(UInt16 xR2Field, RegisterSet registerSet, Memory memory)
        {
            Word word = m_getOperandAction(xR2Field, registerSet, memory);
            return word;
        }
    }
}
