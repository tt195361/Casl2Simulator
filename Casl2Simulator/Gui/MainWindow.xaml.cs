using System.Collections.Generic;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2Simulator.Gui
{
    /// <summary>
    /// MainWindow.xaml を操作するロジックです。
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Instance Members
        private readonly Dictionary<SourceFile, LayoutDocument> m_dictionary;
        #endregion

        /// <summary>
        /// <see cref="MainWindow"/> のインスタンスを初期化します。
        /// </summary>
        public MainWindow()
        {
            m_dictionary = new Dictionary<SourceFile, LayoutDocument>();

            InitializeComponent();
        }

        internal void ShowTextEditor(SourceFile srcFile)
        {
            LayoutDocument layoutDoc = GetTextEditorLayoutDocument(srcFile);

            if (!_documentPane.Children.Contains(layoutDoc))
            {
                _documentPane.Children.Add(layoutDoc);
            }

            layoutDoc.IsSelected = true;
        }

        private LayoutDocument GetTextEditorLayoutDocument(SourceFile srcFile)
        {
            if (!m_dictionary.ContainsKey(srcFile))
            {
                LayoutDocument layoutDoc = MakeTextEditorLayoutDocument(srcFile);
                m_dictionary[srcFile] = layoutDoc;
            }

            return m_dictionary[srcFile];
        }

        private LayoutDocument MakeTextEditorLayoutDocument(SourceFile srcFile)
        {
            LayoutDocument layoutDoc = new LayoutDocument();
            layoutDoc.Title = srcFile.Name;

            UserControlCasl2TextEditor textEditor = new UserControlCasl2TextEditor();
            textEditor.SetText(srcFile.SourceText);

            layoutDoc.Content = textEditor;
            return layoutDoc;
        }
    }
}
