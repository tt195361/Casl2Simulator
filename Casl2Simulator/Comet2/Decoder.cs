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
        #region Fields
        /// <summary>
        /// オペコードがキー、命令が値のディクショナリです。
        /// </summary>
        private static readonly
        Dictionary<UInt16, Instruction> m_instructionDictionary = new Dictionary<UInt16, Instruction>
        {
            { 0x10, Instruction.LoadEaContents },
            { 0x11, Instruction.Store },
            { 0x12, Instruction.LoadEffectiveAddress },
            { 0x14, Instruction.LoadRegister },

            { 0x20, Instruction.AddArithmeticEaContents },
            { 0x21, Instruction.SubtractArithmeticEaContents },
            { 0x22, Instruction.AddLogicalEaContents },
            { 0x23, Instruction.SubtractLogicalEaContents },
            { 0x24, Instruction.AddArithmeticRegister },
            { 0x25, Instruction.SubtractArithmeticRegister },
            { 0x26, Instruction.AddLogicalRegister },
            { 0x27, Instruction.SubtractLogicalRegister },

            { 0x30, Instruction.AndEaContents },
            { 0x31, Instruction.OrEaContents },
            { 0x32, Instruction.XorEaContents },
            { 0x34, Instruction.AndRegister },
            { 0x35, Instruction.OrRegister },
            { 0x36, Instruction.XorRegister },

            { 0x40, Instruction.CompareArithmeticEaContents },
            { 0x41, Instruction.CompareLogicalEaContents },
            { 0x44, Instruction.CompareArithmeticRegister },
            { 0x45, Instruction.CompareLogicalRegister },

            { 0x50, Instruction.ShiftLeftArithmeticEaContents },
            { 0x51, Instruction.ShiftRightArithmeticEaContents },
            { 0x52, Instruction.ShiftLeftLogicalEaContents },
            { 0x53, Instruction.ShiftRightLogicalEaContents },

            { 0x61, Instruction.JumpOnMinus },
            { 0x62, Instruction.JumpOnNonZero },
            { 0x63, Instruction.JumpOnZero },
            { 0x64, Instruction.UnconditionalJump },
            { 0x65, Instruction.JumpOnPlus },
            { 0x66, Instruction.JumpOnOverflow },

            { 0x70, Instruction.Push },
            { 0x71, Instruction.Pop },

            { 0x80, Instruction.CallSubroutine },
            { 0x81, Instruction.ReturnFromSubroutine },
        };
        #endregion

        /// <summary>
        /// 指定の命令コードを解読し、その値が表わす命令を取得します。
        /// </summary>
        /// <param name="opcode">命令を表わす命令コードです。</param>
        /// <returns>指定の命令コードが表わす命令を返します。</returns>
        internal static Instruction Decode(UInt16 opcode)
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
