using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 命令コードを解読します。
    /// </summary>
    internal static class Decoder
    {
        #region Static Fields
        /// <summary>
        /// オペコードがキー、命令が値のディクショナリです。
        /// </summary>
        private static readonly Dictionary<UInt16, CpuInstruction> m_instructionDictionary;
        #endregion

        static Decoder()
        {
            m_instructionDictionary = new Dictionary<UInt16, CpuInstruction>()
            {
                { 0x00, CpuInstruction.NoOperation },

                { 0x10, CpuInstruction.LoadEaContents },
                { 0x11, CpuInstruction.Store },
                { 0x12, CpuInstruction.LoadEffectiveAddress },
                { 0x14, CpuInstruction.LoadRegister },

                { 0x20, CpuInstruction.AddArithmeticEaContents },
                { 0x21, CpuInstruction.SubtractArithmeticEaContents },
                { 0x22, CpuInstruction.AddLogicalEaContents },
                { 0x23, CpuInstruction.SubtractLogicalEaContents },
                { 0x24, CpuInstruction.AddArithmeticRegister },
                { 0x25, CpuInstruction.SubtractArithmeticRegister },
                { 0x26, CpuInstruction.AddLogicalRegister },
                { 0x27, CpuInstruction.SubtractLogicalRegister },

                { 0x30, CpuInstruction.AndEaContents },
                { 0x31, CpuInstruction.OrEaContents },
                { 0x32, CpuInstruction.XorEaContents },
                { 0x34, CpuInstruction.AndRegister },
                { 0x35, CpuInstruction.OrRegister },
                { 0x36, CpuInstruction.XorRegister },

                { 0x40, CpuInstruction.CompareArithmeticEaContents },
                { 0x41, CpuInstruction.CompareLogicalEaContents },
                { 0x44, CpuInstruction.CompareArithmeticRegister },
                { 0x45, CpuInstruction.CompareLogicalRegister },

                { 0x50, CpuInstruction.ShiftLeftArithmeticEaContents },
                { 0x51, CpuInstruction.ShiftRightArithmeticEaContents },
                { 0x52, CpuInstruction.ShiftLeftLogicalEaContents },
                { 0x53, CpuInstruction.ShiftRightLogicalEaContents },

                { 0x61, CpuInstruction.JumpOnMinus },
                { 0x62, CpuInstruction.JumpOnNonZero },
                { 0x63, CpuInstruction.JumpOnZero },
                { 0x64, CpuInstruction.UnconditionalJump },
                { 0x65, CpuInstruction.JumpOnPlus },
                { 0x66, CpuInstruction.JumpOnOverflow },

                { 0x70, CpuInstruction.Push },
                { 0x71, CpuInstruction.Pop },

                { 0x80, CpuInstruction.CallSubroutine },
                { 0x81, CpuInstruction.ReturnFromSubroutine },

                { 0xf0, CpuInstruction.SuperVisorCall },
            };
        }

        /// <summary>
        /// 指定の命令コードを解読し、その値が表わす命令を取得します。
        /// </summary>
        /// <param name="opcode">命令を表わす命令コードです。</param>
        /// <returns>指定の命令コードが表わす命令を返します。</returns>
        internal static CpuInstruction Decode(UInt16 opcode)
        {
            if (!m_instructionDictionary.ContainsKey(opcode))
            {
                String message = String.Format(Resources.MSG_UndefinedOpcode, opcode);
                throw new Casl2SimulatorException(message);
            }

            return m_instructionDictionary[opcode];
        }
    }
}
