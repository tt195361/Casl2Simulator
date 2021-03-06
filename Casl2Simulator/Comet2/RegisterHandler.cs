﻿using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 命令で使用するレジスタを取り扱います。
    /// </summary>
    internal class RegisterHandler
    {
        private delegate CpuRegister GetRegisterAction(UInt16 rR1Field, CpuRegisterSet registerSet);

        #region Register
        /// <summary>
        /// "r/r1" フィールドの指定するレジスタを返します。レジスタを使用する命令に使います。
        /// </summary>
        internal static readonly RegisterHandler Register = new RegisterHandler(GetSpecifiedRegister);

        private static CpuRegister GetSpecifiedRegister(UInt16 rR1Field, CpuRegisterSet registerSet)
        {
            ArgChecker.CheckRange(rR1Field, 0, RegisterDef.GrCount - 1, "r/r1");

            return registerSet.GR[rR1Field];
        }
        #endregion

        #region NoRegister
        /// <summary>
        /// 戻り値として <see langword="null"/> を返します。レジスタを使用しない命令に使います。
        /// </summary>
        internal static readonly RegisterHandler NoRegister = new RegisterHandler(GetNoRegister);

        private static CpuRegister GetNoRegister(UInt16 rR1Field, CpuRegisterSet registerSet)
        {
            return null;
        }
        #endregion

        #region Instance Fields
        private readonly GetRegisterAction m_getRegisterAction;
        #endregion

        // このクラスのインスタンスは、このクラス内からのみ作成できます。
        private RegisterHandler(GetRegisterAction getRegisterAction)
        {
            m_getRegisterAction = getRegisterAction;
        }

        /// <summary>
        /// 命令で指定するレジスタを取得します。
        /// </summary>
        /// <param name="rR1Field">命令語の中の r/r1 フィールドの値です。</param>
        /// <param name="registerSet">COMET II の一そろいのレジスタです。</param>
        /// <returns>
        /// 命令で指定するレジスタを返します。
        /// 指定するレジスタがない場合は <see langword="null"/> を返します。
        /// </returns>
        internal CpuRegister GetRegister(UInt16 rR1Field, CpuRegisterSet registerSet)
        {
            CpuRegister r = m_getRegisterAction(rR1Field, registerSet);
            return r;
        }
    }
}
