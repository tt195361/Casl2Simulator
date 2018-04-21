using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// コードを生成するオブジェクトが提供するインタフェースです。
    /// </summary>
    internal interface ICodeGenerator
    {
        Int32 GetCodeWordCount();
        void GenerateCode(RelocatableModule relModule);
    }

    /// <summary>
    /// adr のコードを生成するオブジェクトが提供するインタフェースです。
    /// </summary>
    internal interface IAdrCodeGenerator : ICodeGenerator
    {
        String GenerateLiteralDc(LabelTable lblTable);
    }
}
