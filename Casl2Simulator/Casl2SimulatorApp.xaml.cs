using System;
using System.Windows;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Gui;

namespace Tt195361.Casl2Simulator
{
    /// <summary>
    /// Casl2SimulatorApp.xaml を操作するロジックです。
    /// </summary>
    public partial class Casl2SimulatorApp : Application
    {
        // 戻り値の型を Casl2SimulatorApp にするため、基底クラス Application の Current プロパティを書き換える。
        internal static new Casl2SimulatorApp Current
        {
            get { return (Casl2SimulatorApp)Application.Current; }
        }

        #region Instance Members
        private Casl2Project m_project;
        private MainWindow m_mainWindow;
        #endregion

        /// <summary>
        /// <see cref="Casl2SimulatorApp"/> のインスタンスを初期化します。
        /// </summary>
        public Casl2SimulatorApp()
        {
            m_project = new Casl2Project();
        }

        internal Casl2Project Project
        {
            get { return m_project; }
        }

        internal new MainWindow MainWindow
        {
            get { return m_mainWindow; }
        }

        private void Casl2SimulatorApp_Startup(Object sender, StartupEventArgs e)
        {
            m_mainWindow = new MainWindow();
            m_mainWindow.Show();
        }
    }
}
