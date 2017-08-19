using System;

namespace Tt195361.Casl2Simulator
{
    /// <summary>
    /// CASL II シミュレータの実行で発生したエラーを表わします。
    /// </summary>
    internal class Casl2SimulatorException : Exception
    {
        /// <summary>
        /// 指定のエラーメッセージを用いて、<see cref="Casl2SimulatorException"/> の
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">発生したエラーを説明するメッセージの文字列です。</param>
        internal Casl2SimulatorException(String message)
            : base(message)
        {
            //
        }

        internal Casl2SimulatorException(String message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
