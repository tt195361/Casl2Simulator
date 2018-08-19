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
        private readonly Dictionary<Casl2Program, LayoutDocument> m_programDocumentDictionary;
        #endregion

        /// <summary>
        /// <see cref="MainWindow"/> のインスタンスを初期化します。
        /// </summary>
        public MainWindow()
        {
            m_programDocumentDictionary = new Dictionary<Casl2Program, LayoutDocument>();

            InitializeComponent();
        }

        internal void ShowTextEditor(Casl2Program program)
        {
            LayoutDocument layoutDoc = GetTextEditorLayoutDocument(program);

            if (!_documentPane.Children.Contains(layoutDoc))
            {
                _documentPane.Children.Add(layoutDoc);
            }

            layoutDoc.IsSelected = true;
        }

        private LayoutDocument GetTextEditorLayoutDocument(Casl2Program program)
        {
            if (!m_programDocumentDictionary.ContainsKey(program))
            {
                LayoutDocument layoutDoc = MakeTextEditorLayoutDocument(program);
                m_programDocumentDictionary[program] = layoutDoc;
            }

            return m_programDocumentDictionary[program];
        }

        private LayoutDocument MakeTextEditorLayoutDocument(Casl2Program program)
        {
            LayoutDocument layoutDoc = new LayoutDocument()
            {
                Title = program.Name
            };

            UserControlCasl2TextEditor textEditor = new UserControlCasl2TextEditor();
            textEditor.SetText(program.TextLines);

            layoutDoc.Content = textEditor;
            return layoutDoc;
        }
    }
}
