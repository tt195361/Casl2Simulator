using System.Collections.Generic;
using System.Windows.Controls;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2Simulator.Gui
{
    /// <summary>
    /// UserControlProjectExplorer.xaml の相互作用ロジック
    /// </summary>
    public partial class UserControlProjectExplorer : UserControl
    {
        /// <summary>
        /// <see cref="UserControlProjectExplorer"/> のインスタンスを初期化します。
        /// </summary>
        public UserControlProjectExplorer()
        {
            InitializeComponent();

            // ItemsSource の型が IEnumerable なので、それにあわせて要素が 1 つのイテレータを返す。
            _treeView.ItemsSource = ProjectAsEnumerable();
        }

        private IEnumerable<Casl2Project> ProjectAsEnumerable()
        {
            yield return Casl2SimulatorApp.Current.Project;
        }
    }
}
