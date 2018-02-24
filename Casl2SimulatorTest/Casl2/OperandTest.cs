using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="Operand"/> クラスの単体テストです。
    /// </summary>
    internal class OperandTest
    {
        internal static T CheckParse<T>(
            Func<OperandLexer, T> parseFunc, String str, Boolean success, String message)
        {
            OperandLexer lexer = OperandLexerTest.MakeFrom(str);
            lexer.MoveNext();
            try
            {
                T result = parseFunc(lexer);
                Assert.IsTrue(success, message);
                return result;
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
                return default(T);
            }
        }
    }
}
