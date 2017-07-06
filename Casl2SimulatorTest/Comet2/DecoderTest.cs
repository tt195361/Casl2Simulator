using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Decoder クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class DecoderTest
    {
        /// <summary>
        /// Decode メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void Decode()
        {
            CheckDecode(0x1000, Instruction.LoadEaContents, "0x1000 => LD r,adr,x");
            CheckDecode(0xe000, null, "0xe000 => 未定義");
        }

        private void CheckDecode(UInt16 ui16Val, Instruction expected, String message)
        {
            try
            {
                Word word = new Word(ui16Val);
                Instruction actual = Decoder.Decode(word);
                Assert.IsNotNull(expected, message);
                Assert.AreSame(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expected, message);
            }
        }
    }
}
