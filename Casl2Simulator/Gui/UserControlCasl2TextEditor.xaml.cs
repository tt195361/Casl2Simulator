using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Gui
{
    /// <summary>
    /// UserControlCasl2TextEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class UserControlCasl2TextEditor : UserControl
    {
        #region Statics
        private static readonly String Separator = Environment.NewLine;
        #endregion

        /// <summary>
        /// <see cref="UserControlCasl2TextEditor"/> のインスタンスを初期化します。
        /// </summary>
        public UserControlCasl2TextEditor()
        {
            InitializeComponent();
        }

        internal void SetText(IEnumerable<String> textLines)
        {
            _textEditor.Text = textLines.MakeList(Separator);
        }
    }
}
