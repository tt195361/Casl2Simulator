using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アドレス定数です。
    /// </summary>
    internal class AddressConstant : Constant, IAdrValue
    {
        #region Fields
        private readonly Label m_label;
        #endregion

        internal AddressConstant(String name)
        {
            m_label = new Label(name);
        }

        internal Label Label
        {
            get { return m_label; }
        }

        internal override int GetWordCount()
        {
            return 1;
        }

        internal override void GenerateCode(LabelManager lblManager, RelocatableModule relModule)
        {
            UInt16 offset = GetOffset(lblManager);
            relModule.AddWord(new Word(offset));
        }

        String IAdrValue.GenerateDc(LabelManager lblManager)
        {
            return null;
        }

        UInt16 IAdrValue.GetAddress(LabelManager lblManager)
        {
            return GetOffset(lblManager);
        }

        private UInt16 GetOffset(LabelManager lblManager)
        {
            return lblManager.GetOffset(m_label);
        }

        protected override String ValueToString()
        {
            return m_label.Name;
        }
    }
}
