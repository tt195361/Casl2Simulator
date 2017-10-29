using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// InstructionFactory クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class InstructionFactoryTest
    {
        /// <summary>
        /// Make メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Make()
        {
            CheckMake("START", typeof(AsmStartInstruction), "アセンブラ命令 START");
            CheckMake("DC", typeof(AsmDcInstruction), "アセンブラ命令 DC");

            CheckMake("IN", typeof(MacroInInstruction), "マクロ命令 IN");
            CheckMake("RPUSH", typeof(MacroRpushInstruction), "マクロ命令 RPUSH");

            CheckMake(String.Empty, null, "空文字列 => 例外");
            CheckMake("UNDEF", null, "未定義命令 => 例外");
        }

        private void CheckMake(String mnemonic, Type expectedType, String message)
        {
            try
            {
                Instruction instruction = InstructionFactory.Make(mnemonic);
                Assert.IsNotNull(expectedType, message);
                Assert.IsInstanceOfType(instruction, expectedType, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expectedType, message);
            }
        }
    }
}
