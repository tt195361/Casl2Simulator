using System;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// COMET II の命令語を取り扱います。
    /// </summary>
    internal static class InstructionWord
    {
        #region Static Fields
        // 命令の第 1 語の構成。
        private const Int32 OpcodeMSB = 15;
        private const Int32 OpcodeLSB = 8;
        private const Int32 RR1MSB = 7;
        private const Int32 RR1LSB = 4;
        private const Int32 XR2MSB = 3;
        private const Int32 XR2LSB = 0;
        #endregion

        /// <summary>
        /// 命令の第 1 語からオペコードの値を取得します。
        /// </summary>
        /// <param name="firstWord">オペコードの値を取り出す命令の第 1 語です。</param>
        /// <returns>命令の第 1 語から取り出したオペコードの値を返します。</returns>
        internal static UInt16 GetOpcode(Word firstWord)
        {
            return firstWord.GetBits(OpcodeMSB, OpcodeLSB);
        }

        /// <summary>
        /// 命令の第 1 語から r/r1 の値を取得します。
        /// </summary>
        /// <param name="firstWord">r/r1 の値を取り出す命令の第 1 語です。</param>
        /// <returns>命令の第 1 語から取り出した r/r1 の値を返します。</returns>
        internal static UInt16 GetRR1(Word firstWord)
        {
            return firstWord.GetBits(RR1MSB, RR1LSB);
        }

        /// <summary>
        /// 命令の第 1 語から x/r2 の値を取得します。
        /// </summary>
        /// <param name="firstWord">x/r2 の値を取り出す命令の第 1 語です。</param>
        /// <returns>命令の第 1 語から取り出した x/r2 の値を返します。</returns>
        internal static UInt16 GetXR2(Word firstWord)
        {
            return firstWord.GetBits(XR2MSB, XR2LSB);
        }

        /// <summary>
        /// 指定の値から命令の第 1 語を作成します。
        /// </summary>
        /// <param name="opcode">作成する第 1 語のオペコードの値です。</param>
        /// <param name="rr1">作成する第 1 語の r/r1 の値です。</param>
        /// <param name="xr2">作成する第 1 語の x/r2 の値です。</param>
        /// <returns>作成した命令の第 1 語を返します。</returns>
        internal static Word MakeFirstWord(UInt16 opcode, UInt16 rr1, UInt16 xr2)
        {
            UInt16 value = UInt16Utils.SetBits(0, OpcodeMSB, OpcodeLSB, opcode);
            value = UInt16Utils.SetBits(value, RR1MSB, RR1LSB, rr1);
            value = UInt16Utils.SetBits(value, XR2MSB, XR2LSB, xr2);
            return new Word(value);
        }
    }
}
