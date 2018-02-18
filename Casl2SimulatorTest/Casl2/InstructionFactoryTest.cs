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
            CheckMake("END", typeof(AsmEndInstruction), "アセンブラ命令 END");
            CheckMake("DS", typeof(AsmDsInstruction), "アセンブラ命令 DS");
            CheckMake("DC", typeof(AsmDcInstruction), "アセンブラ命令 DC");

            CheckMake("IN", typeof(MacroInInstruction), "マクロ命令 IN");
            CheckMake("RPUSH", typeof(MacroRpushInstruction), "マクロ命令 RPUSH");

            CheckMake("NOP", typeof(MachineInstruction), "機械語命令 NOP");

            CheckMake("LD", typeof(MachineInstruction), "機械語命令 LD");
            CheckMake("ST", typeof(MachineInstruction), "機械語命令 ST");
            CheckMake("LAD", typeof(MachineInstruction), "機械語命令 LAD");

            CheckMake("ADDA", typeof(MachineInstruction), "機械語命令 ADDA");
            CheckMake("SUBA", typeof(MachineInstruction), "機械語命令 SUBA");
            CheckMake("ADDL", typeof(MachineInstruction), "機械語命令 ADDL");
            CheckMake("SUBL", typeof(MachineInstruction), "機械語命令 SUBL");

            CheckMake("AND", typeof(MachineInstruction), "機械語命令 AND");
            CheckMake("OR", typeof(MachineInstruction), "機械語命令 OR");
            CheckMake("XOR", typeof(MachineInstruction), "機械語命令 XOR");

            CheckMake("CPA", typeof(MachineInstruction), "機械語命令 CPA");
            CheckMake("CPL", typeof(MachineInstruction), "機械語命令 CPL");

            CheckMake("SLA", typeof(MachineInstruction), "機械語命令 SLA");
            CheckMake("SRA", typeof(MachineInstruction), "機械語命令 SRA");
            CheckMake("SLL", typeof(MachineInstruction), "機械語命令 SLL");
            CheckMake("SRL", typeof(MachineInstruction), "機械語命令 SRL");

            CheckMake("PUSH", typeof(MachineInstruction), "機械語命令 PUSH");

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
                Assert.AreEqual(mnemonic, instruction.Mnemonic, "指定のニーモニックと命令のニーモニックが同じ");
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expectedType, message);
            }
        }
    }
}
