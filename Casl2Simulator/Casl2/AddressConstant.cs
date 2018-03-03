﻿using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アドレス定数です。
    /// </summary>
    internal class AddressConstant : Constant, IAdrCodeGenerator
    {
        #region Instance Fields
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

        public override Int32 GetCodeWordCount()
        {
            return 1;
        }

        public override void GenerateCode(LabelManager lblManager, RelocatableModule relModule)
        {
            relModule.AddReferenceWord(lblManager, m_label);
        }

        public String GenerateLiteralDc(LabelManager lblManager)
        {
            return null;
        }

        protected override String ValueToString()
        {
            return m_label.Name;
        }
    }
}
