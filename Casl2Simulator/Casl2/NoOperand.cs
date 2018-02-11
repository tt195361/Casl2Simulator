using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドでオペランドなしを表わします。
    /// </summary>
    internal class NoOperand : MachineInstructionOperand
    {
        /// <summary>
        /// オペランドがない命令のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="opcode">このオペラントの命令の第 1 語のオペコードの値です。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="NoOperand"/> オブジェクトを返します。
        /// </returns>
        internal static NoOperand Parse(OperandLexer lexer, UInt16 opcode)
        {
            return new NoOperand(opcode);
        }

        private NoOperand(UInt16 opcode)
            : base(opcode)
        {
            //
        }

        public override String ToString()
        {
            return String.Empty;
        }

        internal static NoOperand MakeForUnitTest()
        {
            return new NoOperand(OpcodeDef.Dummy);
        }
    }
}
