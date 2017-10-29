using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// オペランドを記述する字句要素を表わします。
    /// </summary>
    internal class Token
    {
        #region Fields
        private const Int32 DontCareInt32 = 0;
        private const String DontCareString = null;
        #endregion

        internal static Token MakeEndOfToken()
        {
            return new Token(TokenType.EndOfToken, DontCareInt32, DontCareString);
        }

        internal static Token MakeDecimalConstant(Int32 i32Value)
        {
            return new Token(TokenType.DecimalConstant, i32Value, DontCareString);
        }

        internal static Token MakeHexaDecimalConstant(Int32 i32Value)
        {
            return new Token(TokenType.HexaDecimalConstant, i32Value, DontCareString);
        }

        internal static Token MakeStringConstant(String strValue)
        {
            return new Token(TokenType.StringConstant, DontCareInt32, strValue);
        }

        internal static Token MakeRegisterName(String strValue)
        {
            return new Token(TokenType.RegisterName, DontCareInt32, strValue);
        }

        internal static Token MakeLabel(String strValue)
        {
            return new Token(TokenType.Label, DontCareInt32, strValue);
        }

        internal static Token MakeComma()
        {
            return new Token(TokenType.Comma, DontCareInt32, DontCareString);
        }

        internal static Token MakeEqualSign()
        {
            return new Token(TokenType.EqualSign, DontCareInt32, DontCareString);
        }

        #region Fields
        private readonly TokenType m_type;
        private readonly Int32 m_i32Value;
        private readonly String m_strValue;
        #endregion

        private Token(TokenType type, Int32 i32Value, String strValue)
        {
            m_type = type;
            m_i32Value = i32Value;
            m_strValue = strValue;
        }

        internal TokenType Type
        {
            get { return m_type; }
        }

        internal Int32 I32Value
        {
            get { return m_i32Value; }
        }

        internal String StrValue
        {
            get { return m_strValue; }
        }

        public override String ToString()
        {
            return m_type.TokenToString(this);
        }
    }
}
