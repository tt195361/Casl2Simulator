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
        };
        #endregion

        /// <summary>
        /// 指定の語の中のオペコードを解読し、その値が表わす命令を取得します。
        /// </summary>
        /// <param name="word">命令を表わす値を格納した語です。</param>
        /// <returns>指定の語の中のオペコードが表わす命令を返します。</returns>
        internal static Instruction Decode(Word word)
        {
            UInt16 opcode = word.GetBits(15, 8);

            if (!m_instructionDictionary.ContainsKey(opcode))
            {
                String message = String.Format(Resources.MSG_UndefinedOpcode, opcode);
                throw new Casl2SimulatorException(message);
            }

            return m_instructionDictionary[opcode];
        }
    }
}
