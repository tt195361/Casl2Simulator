using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// r,adr,x あるいは r1,r2 のオペランドを取り扱います。
    /// </summary>
    internal static class RAdrXOrR1R2Operand
    {
        /// <summary>
        /// r,adr[,x] あるいは r1,r2 のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="opcodeRAdrX">オペランドが r,adr[,x] の場合のオペコードの値です。</param>
        /// <param name="opcodeR1R2">オペランドが r1,r2 の場合のオペコードの値です。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="MachineInstructionOperand"/> オブジェクトを返します。
        /// </returns>
        internal static MachineInstructionOperand Parse(
            OperandLexer lexer, UInt16 opcodeRAdrX, UInt16 opcodeR1R2)
        {
            // r,adr[,x] と r1,r2 の両方とも最初は "r," なので、そこまで解釈する。
            RegisterOperand rOrR1 = RegisterOperand.Parse(lexer);
            lexer.SkipComma();

            // lexer の状態を保存する。
            OperandLexerState savedLexerState = lexer.GetState();

            // adr[,x] を解釈してみる。
            RAdrXOperand rAdrX;
            if (RAdrXOperand.TryParseAdrX(lexer, opcodeRAdrX, rOrR1, out rAdrX))
            {
                return rAdrX;
            }

            // adr[,x] でなければ、lexer の状態を元に戻して r2 を解釈してみる。
            lexer.SetState(savedLexerState);
            R1R2Operand r1R2;
            if (R1R2Operand.TryParseR2(lexer, opcodeR1R2, rOrR1, out r1R2))
            {
                return r1R2;
            }

            // r,adr[,x] でも r1,r2 でもない。
            throw new Casl2SimulatorException(Resources.MSG_OperandNeitherRAdrXNorR1R2);
        }
    }
}
