using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドの r,adr[,x] を表わします。
    /// </summary>
    internal class RAdrXOperand : MachineInstructionOperand
    {
        /// <summary>
        /// r,adr[,x] のオペランドを解釈します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="opcode">このオペラントの命令の第 1 語のオペコードの値です。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="RAdrXOperand"/> オブジェクトを返します。
        /// </returns>
        internal static RAdrXOperand Parse(OperandLexer lexer, UInt16 opcode)
        {
            RegisterOperand r = RegisterOperand.Parse(lexer);
            lexer.SkipComma();
            AdrXOperand adrX = AdrXOperand.Parse(lexer);
            return new RAdrXOperand(opcode, r, adrX);
        }

        /// <summary>
        /// r,adr[,x] のオペランドの adr[,x] の部分を解釈します。戻り値は、解釈が成功したかどうかを示します。
        /// </summary>
        /// <param name="lexer">オペランドの字句を解析する <see cref="OperandLexer"/> のオブジェクトです。</param>
        /// <param name="opcode">このオペラントの命令の第 1 語のオペコードの値です。</param>
        /// <param name="r">
        /// r,adr[,x] のオペランドの最初の r の内容を保持する <see cref="RegisterOperand"/> のオブジェクトです。
        /// </param>
        /// <param name="rAdrX">
        /// 解釈が成功したとき、結果として生成した <see cref="RAdrXOperand"/> のオブジェクトを格納します。
        /// 失敗した場合は <see langword="null"/> を格納します。
        /// </param>
        /// <returns>
        /// 解釈に成功した場合は <see langword="true"/> を、失敗した場合は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean TryParseAdrX(
            OperandLexer lexer, UInt16 opcode, RegisterOperand r, out RAdrXOperand rAdrX)
        {
            AdrXOperand adrX;
            if (!AdrXOperand.TryParse(lexer, out adrX))
            {
                rAdrX = null;
                return false;
            }
            else
            {
                rAdrX = new RAdrXOperand(opcode, r, adrX);
                return true;
            }
        }

        #region Instance Fields
        private readonly RegisterOperand m_r;
        private readonly AdrXOperand m_adrX;
        #endregion

        private RAdrXOperand(UInt16 opcode, RegisterOperand r, AdrXOperand adrX)
            : base(opcode)
        {
            m_r = r;
            m_adrX = adrX;
        }

        internal RegisterOperand R
        {
            get { return m_r; }
        }

        internal AdrXOperand AdrX
        {
            get { return m_adrX; }
        }

        internal override UInt16 GetRR1()
        {
            return m_r.Number;
        }

        internal override UInt16 GetXR2()
        {
            return m_adrX.GetXR2();
        }

        public override Int32 GetCodeWordCount()
        {
            return m_adrX.GetCodeWordCount();
        }

        public override void GenerateCode(RelocatableModule relModule)
        {
            m_adrX.GenerateCode(relModule);
        }

        public override String GenerateLiteralDc(LabelManager lblManager)
        {
            return m_adrX.GenerateLiteralDc(lblManager);
        }

        public override String ToString()
        {
            return Operand.Join(m_r, m_adrX);
        }

        internal static RAdrXOperand MakeForUnitTest(RegisterOperand r, AdrXOperand adrX)
        {
            return MakeForUnitTest(OpcodeDef.Dummy, r, adrX);
        }

        internal static RAdrXOperand MakeForUnitTest(UInt16 opcode, RegisterOperand r, AdrXOperand adrX)
        {
            return new RAdrXOperand(opcode, r, adrX);
        }
    }
}
