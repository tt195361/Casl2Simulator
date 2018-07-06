using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL2 のプログラムを作成するプロジェクトです。
    /// </summary>
    internal class Casl2Project
    {
        #region Static Fields
        private const String InitialSourceFileName = "Program1";
        #endregion

        #region Instance Fields
        private readonly ItemSelectableCollection<SourceFile> m_sourceFiles;
        #endregion

        internal Casl2Project()
            : this(new SourceFile(InitialSourceFileName))
        {
            //
        }

        private Casl2Project(params SourceFile[] srcFiles)
        {
            m_sourceFiles = new ItemSelectableCollection<SourceFile>(srcFiles);
        }

        /// <summary>
        /// プロジェクトに含まれる一連のソースファイルを取得します。
        /// </summary>
        internal ItemSelectableCollection<SourceFile> SourceFiles
        {
            get { return m_sourceFiles; }
        }

        /// <summary>
        /// プロジェクトをビルドし、実行可能ファイルを生成します。
        /// </summary>
        /// <returns>生成した実行可能ファイルを返します。</returns>
        internal ExecutableModule Build()
        {
            return SourceFiles.Assemble()
                              .Link();
        }

        internal static Casl2Project MakeForUnitTest(params SourceFile[] srcFiles)
        {
            return new Casl2Project(srcFiles);
        }
    }
}
