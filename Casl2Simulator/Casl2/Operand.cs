using System;
using System.Linq;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 命令のオペランドを表わす抽象クラスです。
    /// </summary>
    internal abstract class Operand : ICodeGenerator
    {
        internal static String Join(params Object[] args)
        {
            return args.Select((arg) => arg.ToString())
                       .MakeList(Casl2Defs.Comma.ToString());
        }

        internal static String ReadItem(ReadBuffer buffer)
        {
            return buffer.ReadWhile((c) => !Operand.EndOfItem(c));
        }

        private static Boolean EndOfItem(Char current)
        {
            return ProgramLine.EndOfField(current) || current == Casl2Defs.Comma;
        }

        protected Operand()
        {
            //
        }

        /// <summary>
        /// このオペランドが生成するコードの語数を取得します。
        /// </summary>
        public virtual Int32 GetCodeWordCount()
        {
            // デフォルトは、コードを生成しない。
            return 0;
        }

        /// <summary>
        /// このオペランドのコードを生成します。
        /// </summary>
        public virtual void GenerateCode(RelocatableModule relModule)
        {
            // デフォルトは、コードを生成しない。
        }
    }
}
