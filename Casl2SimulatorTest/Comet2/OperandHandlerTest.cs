using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// <see cref="OperandHandler"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class OperandHandlerTest
    {
        #region Instance Fields
        private CpuRegisterSet m_registerSet;
        private Memory m_memory;

        // 命令語の次のアドレス。
        private const UInt16 NextAddress = 1;

        private const UInt16 DontCare = 0;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new CpuRegisterSet();
            m_memory = new Memory();
        }

        /// <summary>
        /// <see cref="OperandHandler.EffectiveAddress"/> の単体テストです。
        /// </summary>
        [TestMethod]
        public void EffectiveAddress()
        {
            const UInt16 Adr = 2468;
            const UInt16 Gr6 = 3456;
            const UInt16 X6 = 6;
            const UInt16 Gr7 = 65536 - Adr;
            const UInt16 X7 = 7;

            m_memory.Write(NextAddress, Adr);
            m_registerSet.GR[X6].Value = Gr6;
            m_registerSet.GR[X7].Value = Gr7;

            CheckEffectiveAddress(0, true, Adr, "x/r2 が 0 => adr が実効アドレス");
            CheckEffectiveAddress(X6, true, Adr + Gr6, "x/r2 が 6 => adr + GR6 が実効アドレス");
            CheckEffectiveAddress(X7, true, 0, "adr + GR がオーバーフローの場合");
            CheckEffectiveAddress(8, false, DontCare, "x/r2 が 8 => エラー");
        }

        public void CheckEffectiveAddress(UInt16 xR2Field, Boolean success, UInt16 expected, String message)
        {
            m_registerSet.PR.Value = NextAddress;

            try
            {
                OperandHandler target = OperandHandler.EffectiveAddress;
                Word effectiveAddress = target.GetOperand(xR2Field, m_registerSet, m_memory);
                Assert.IsTrue(success, message);
                WordTest.Check(effectiveAddress, expected, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="OperandHandler.EaContents"/> の単体テストです。
        /// </summary>
        [TestMethod]
        public void EaContents()
        {
            const UInt16 X = 6;
            const UInt16 Adr = 1111;
            const UInt16 Gr6 = 2222;
            const UInt16 EffectiveAddress = Adr + Gr6;
            const UInt16 EaContents = 0xaa55;

            m_memory.Write(NextAddress, Adr);
            m_memory.Write(EffectiveAddress, EaContents);
            m_registerSet.GR[X].Value = Gr6;
            m_registerSet.PR.Value = NextAddress;

            OperandHandler target = OperandHandler.EaContents;
            Word word = target.GetOperand(X, m_registerSet, m_memory);

            const UInt16 Expected = EaContents;
            WordTest.Check(word, Expected, "実効アドレスの内容を取得する");
        }

        /// <summary>
        /// <see cref="OperandHandler.Register"/> の単体テストです。
        /// </summary>
        [TestMethod]
        public void Register()
        {
            const UInt16 X0 = 0;
            const UInt16 X7 = 7;
            const UInt16 GR0Value = 0x0123;
            const UInt16 GR7Value = 0x789a;
            m_registerSet.GR[X0].Value = GR0Value;
            m_registerSet.GR[X7].Value = GR7Value;

            CheckRegister(X0, true, GR0Value, "x/r2 が 0 => GR0 の値");
            CheckRegister(X7, true, GR7Value, "x/r2 が 7 => GR7 の値");
            CheckRegister(8, false, DontCare, "x/r2 が 8 => エラー");
        }

        public void CheckRegister(UInt16 xR2Field, Boolean success, UInt16 expected, String message)
        {
            try
            {
                OperandHandler target = OperandHandler.Register;
                Word r2 = target.GetOperand(xR2Field, m_registerSet, m_memory);
                Assert.IsTrue(success, message);
                WordTest.Check(r2, expected, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
