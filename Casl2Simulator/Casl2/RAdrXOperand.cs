using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令オペランドの r,adr[,x] を表わします。
    /// </summary>
    internal class RAdrXOperand : MachineInstructionOperand
    {
        internal static Boolean TryParse(OperandLexer lexer, UInt16 opcode, out RAdrXOperand rAdrX)
        {
            try
            {
                rAdrX = Parse(lexer, opcode);
                return true;
            }
            catch (Exception)
            {
                rAdrX = null;
                return false;
            }
        }

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

        #region Fields
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

        public override void GenerateCode(LabelManager lblManager, RelocatableModule relModule)
        {
            m_adrX.GenerateCode(lblManager, relModule);
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
