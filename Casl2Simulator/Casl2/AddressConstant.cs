using System;

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

        internal AddressConstant(String label)
        {
            m_label = new Label(label);
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
            throw new NotImplementedException();
        }

        String IAdrValue.GenerateDc(LabelManager lblManager)
        {
            throw new NotImplementedException();
        }

        UInt16 IAdrValue.GetAddress(LabelManager lblManager)
        {
            throw new NotImplementedException();
        }

        protected override String ValueToString()
        {
            return m_label.Name;
        }
    }
}
