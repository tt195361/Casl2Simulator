using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドの r1,r2 を表わします。
    /// </summary>
    internal class R1R2Operand : MachineInstructionOperand
    {
        /// <summary>
        /// r1,r2 のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="opcode">このオペラントの命令の第 1 語のオペコードの値です。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="R1R2Operand"/> オブジェクトを返します。
        /// </returns>
        internal static R1R2Operand Parse(OperandLexer lexer, UInt16 opcode)
        {
            RegisterOperand r1 = RegisterOperand.Parse(lexer);
            lexer.SkipComma();
            RegisterOperand r2 = RegisterOperand.Parse(lexer);
            return new R1R2Operand(opcode, r1, r2);
        }

        /// <summary>
        /// r1,r2 のオペランドの r2 の部分を解釈します。戻り値は、解釈が成功したかどうかを示します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="opcode">このオペラントの命令の第 1 語のオペコードの値です。</param>
        /// <param name="r1">
        /// r1,r2 のオペランドの最初の r1 の内容を保持する <see cref="RegisterOperand"/> のオブジェクトです。
        /// </param>
        /// <param name="r1R2">
        /// 解釈が成功したとき、結果として生成した <see cref="R1R2Operand"/> のオブジェクトを格納します。
        /// 失敗した場合は <see langword="null"/> を格納します。
        /// </param>
        /// <returns>
        /// 解釈に成功した場合は <see langword="true"/> を、失敗した場合は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean TryParseR2(
            OperandLexer lexer, UInt16 opcode, RegisterOperand r1, out R1R2Operand r1R2)
        {
            RegisterOperand r2;
            if (!RegisterOperand.TryParse(lexer, out r2))
            {
                r1R2 = null;
                return false;
            }
            else
            {
                r1R2 = new R1R2Operand(opcode, r1, r2);
                return true;
            }
        }

        #region Fields
        private readonly RegisterOperand m_r1;
        private readonly RegisterOperand m_r2;
        #endregion

        private R1R2Operand(UInt16 opcode, RegisterOperand r1, RegisterOperand r2)
            : base(opcode)
        {
            m_r1 = r1;
            m_r2 = r2;
        }

        internal RegisterOperand R1
        {
            get { return m_r1; }
        }

        internal RegisterOperand R2
        {
            get { return m_r2; }
        }

        internal override UInt16 GetRR1()
        {
            return m_r1.Number;
        }

        internal override UInt16 GetXR2()
        {
            return m_r2.Number;
        }

        public override String ToString()
        {
            return Operand.Join(m_r1, m_r2);
        }

        internal static R1R2Operand MakeForUnitTest(RegisterOperand r1, RegisterOperand r2)
        {
            return MakeForUnitTest(OpcodeDef.Dummy, r1, r2);
        }

        internal static R1R2Operand MakeForUnitTest(UInt16 opcode, RegisterOperand r1, RegisterOperand r2)
        {
            return new R1R2Operand(opcode, r1, r2);
        }
    }
}
