using System;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// オペランドを記述する字句要素の種類を表わします。
    /// </summary>
    internal class TokenType
    {
        #region Static Fields
        internal static readonly TokenType EndOfToken;
        internal static readonly TokenType DecimalConstant;
        internal static readonly TokenType HexaDecimalConstant;
        internal static readonly TokenType StringConstant;
        internal static readonly TokenType RegisterName;
        internal static readonly TokenType Label;
        internal static readonly TokenType Comma;
        internal static readonly TokenType EqualSign;
        #endregion

        static TokenType()
        {
            EndOfToken = new TokenType(Resources.STR_EndOfToken, TypeOnly);
            DecimalConstant = new TokenType(Resources.STR_DecimalConstant, TypeAndDecimalValue);
            HexaDecimalConstant = new TokenType(Resources.STR_HexaDecimalConstant, TypeAndHexValue);
            StringConstant = new TokenType(Resources.STR_StringConstant, TypeAndQuotedStr);
            RegisterName = new TokenType(Resources.STR_RegisterName, TypeAndStr);
            Label = new TokenType(Resources.STR_Label, TypeAndStr);
            Comma = new TokenType("','", TypeOnly);
            EqualSign = new TokenType("'='", TypeOnly);
        }

        private static String TypeOnly(Token token)
        {
            return token.Type.ToString();
        }

        private static String TypeAndDecimalValue(Token token)
        {
            String valueStr = Casl2.DecimalConstant.ValueToString(token.I32Value);
            return String.Format("{0}: {1}", token.Type, valueStr);
        }

        private static String TypeAndHexValue(Token token)
        {
            String valueStr = Casl2.HexaDecimalConstant.ValueToString(token.I32Value);
            return String.Format("{0}: {1}", token.Type, valueStr);
        }

        private static String TypeAndQuotedStr(Token token)
        {
            String valueStr = Casl2.StringConstant.ValueToString(token.StrValue);
            return String.Format("{0}: {1}", token.Type, valueStr);
        }

        private static String TypeAndStr(Token token)
        {
            return String.Format("{0}: {1}", token.Type, token.StrValue);
        }

        #region Fields
        private readonly String m_description;
        private readonly Func<Token, String> m_tokenToStringFunc;
        #endregion

        private TokenType(String description, Func<Token, String> tokenToStringFunc)
        {
            m_description = description;
            m_tokenToStringFunc = tokenToStringFunc;
        }

        internal String TokenToString(Token token)
        {
            return m_tokenToStringFunc(token);
        }

        public override String ToString()
        {
            return m_description;
        }
    }
}
