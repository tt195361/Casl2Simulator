using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// CASL2 のプログラムを作成するプロジェクトです。
    /// </summary>
    internal class Casl2Project
    {
        #region Static Fields
        private const String InitialProjectName = "Project";
        private const String InitialProgramName = "Program1";
        #endregion

        #region Instance Fields
        private readonly String m_name;
        private readonly ItemSelectableCollection<Casl2Program> m_programs;
        #endregion

        internal Casl2Project()
            : this(new Casl2Program(InitialProgramName))
        {
            //
        }

        private Casl2Project(params Casl2Program[] programs)
        {
            m_name = InitialProjectName;
            m_programs = new ItemSelectableCollection<Casl2Program>(programs);
        }

        /// <summary>
        /// プロジェクトの名前を取得します。
        /// </summary>
        public String Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// プロジェクトに含まれる一連のプログラムを取得します。
        /// </summary>
        public ItemSelectableCollection<Casl2Program> Programs
        {
            get { return m_programs; }
        }

        /// <summary>
        /// プロジェクトをビルドし、実行可能ファイルを生成します。
        /// </summary>
        /// <returns>生成した実行可能ファイルを返します。</returns>
        internal ExecutableModule Build()
        {
            return Programs.Assemble()
                           .Link();
        }

        internal static Casl2Project MakeForUnitTest(params Casl2Program[] programs)
        {
            return new Casl2Project(programs);
        }
    }
}
