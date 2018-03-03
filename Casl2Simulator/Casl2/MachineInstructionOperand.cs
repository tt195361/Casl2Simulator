using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 機械語命令のオペランドを表わす抽象クラスです。
    /// </summary>
    internal abstract class MachineInstructionOperand : Operand, IAdrCodeGenerator
    {
        #region Instance Fields
        private readonly UInt16 m_opcode;
        #endregion

        protected MachineInstructionOperand(UInt16 opcode)
        {
            m_opcode = opcode;
        }

        internal UInt16 Opcode
        {
            get { return m_opcode; }
        }

        /// <summary>
        /// このオペランドの r/r1 フィールドの値を取得します。
        /// </summary>
        /// <returns>このオペランドの r/r1 フィールドの値を返します。</returns>
        internal virtual UInt16 GetRR1()
        {
            // デフォルトは 0。
            return 0;
        }

        /// <summary>
        /// このオペランドの x/r2 フィールドの値を取得します。
        /// </summary>
        /// <returns>このオペランドの x/r2 フィールドの値を返します。</returns>
        internal virtual UInt16 GetXR2()
        {
            // デフォルトは 0。
            return 0;
        }

        /// <summary>
        /// このオペランドで記述されたリテラルの DC 命令を生成します。
        /// </summary>
        /// <param name="lblManager">
        /// ラベルを管理する <see cref="LabelManager"/> のオブジェクトです。
        /// </param>
        /// <returns>
        /// このオペランドで記述されたリテラルの DC 命令の文字列を返します。
        /// リテラルが記述されていない場合は <see langword="null"/> を返します。
        /// </returns>
        public virtual String GenerateLiteralDc(LabelManager lblManager)
        {
            return null;
        }
    }
}
