using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令のレジスタオペランドです。
    /// </summary>
    internal class RegisterOperand : MachineInstructionOperand
    {
        /// <summary>
        /// r のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="RegisterOperand"/> オブジェクトを返します。
        /// </returns>
        internal static RegisterOperand Parse(OperandLexer lexer)
        {
            return Parse(lexer, OpcodeDef.Dummy);
        }

        /// <summary>
        /// r のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="opcode">このオペラントの命令の第 1 語のオペコードの値です。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="RegisterOperand"/> オブジェクトを返します。
        /// </returns>
        internal static RegisterOperand Parse(OperandLexer lexer, UInt16 opcode)
        {
            Token token = lexer.ReadCurrentAs(TokenType.RegisterName);
            return new RegisterOperand(opcode, token.StrValue);
        }

        /// <summary>
        /// r のオペランドを解釈します。戻り値は、解釈が成功したかどうかを示します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="r">
        /// 解釈が成功したとき、結果として生成した <see cref="RegisterOperand"/> のオブジェクトを格納します。
        /// 失敗した場合は <see langword="null"/> を格納します。
        /// </param>
        /// <returns>
        /// 解釈に成功した場合は <see langword="true"/> を、失敗した場合は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean TryParse(OperandLexer lexer, out RegisterOperand r)
        {
            Token token = lexer.ReadCurrentIf(TokenType.RegisterName);
            if (token == null)
            {
                r = null;
                return false;
            }
            else
            {
                r = new RegisterOperand(OpcodeDef.Dummy, token.StrValue);
                return true;
            }
        }

        #region Instance Fields
        private readonly ProgramRegister m_register;
        #endregion

        private RegisterOperand(UInt16 opcode, String name)
            : this(opcode, ProgramRegister.GetFor(name))
        {
            //
        }

        private RegisterOperand(UInt16 opcode, ProgramRegister register)
            : base(opcode)
        {
            m_register = register;
        }

        internal ProgramRegister Register
        {
            get { return m_register; }
        }

        internal String Name
        {
            get { return m_register.Name; }
        }

        internal UInt16 Number
        {
            get { return m_register.Number; }
        }

        internal override UInt16 GetRR1()
        {
            return Number;
        }

        public override String ToString()
        {
            return Name;
        }

        internal static RegisterOperand MakeForUnitTest(ProgramRegister register)
        {
            return MakeForUnitTest(OpcodeDef.Dummy, register);
        }

        internal static RegisterOperand MakeForUnitTest(UInt16 opcode, ProgramRegister register)
        {
            return new RegisterOperand(opcode, register);
        }
    }
}
