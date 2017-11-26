﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tt195361.Casl2Simulator.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Tt195361.Casl2Simulator.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} の値 {1} が {2} の値 {3} より小さいです。{0} の値は {2} の値より大きいか、等しくなければなりません。.
        /// </summary>
        internal static string MSG_ArgGreaterEqual {
            get {
                return ResourceManager.GetString("MSG_ArgGreaterEqual", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} の値 {1} が範囲外です。{0} の値は、&quot;{2} &lt;= {0} &lt;= {3}&quot; の範囲でなければなりません。.
        /// </summary>
        internal static string MSG_ArgRangeError {
            get {
                return ResourceManager.GetString("MSG_ArgRangeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 指定のレジスタ {0} は指標レジスタとして使えません。指標レジスタとして用いる GR は、記号 GR{1}～GR{2} で指定してください。.
        /// </summary>
        internal static string MSG_CanNotBeIndexRegister {
            get {
                return ResourceManager.GetString("MSG_CanNotBeIndexRegister", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} を定数として解釈できませんでした。10 進定数 (n)、16 進定数 (#h)、文字定数 (&apos;文字列&apos;)、アドレス定数 (ラベル) のいずれかを記述してください。.
        /// </summary>
        internal static string MSG_ConstantParseError {
            get {
                return ResourceManager.GetString("MSG_ConstantParseError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 字句要素 {0} をアドレスとして解釈できませんでした。アドレスは、10 進定数、16 進定数、アドレス定数又はリテラルで指定してください。.
        /// </summary>
        internal static string MSG_CouldNotParseAsAdr {
            get {
                return ResourceManager.GetString("MSG_CouldNotParseAsAdr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 文字 &apos;{0}&apos; で始まる句ををオペランドの字句要素として解釈できませんでした。オペランドの字句要素には、10 進定数 (n)、16 進定数 (#h)、文字定数 (&apos;文字列&apos;)、アドレス定数 (ラベル)、レジスタ名 (GRn)、リテラル (=定数) があります。.
        /// </summary>
        internal static string MSG_CouldNotParseAsToken {
            get {
                return ResourceManager.GetString("MSG_CouldNotParseAsToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to アドレス #{0:x04} の命令 #{1:x04} の実行でエラーが発生しました。詳細は InnerException を参照してください。.
        /// </summary>
        internal static string MSG_CpuExecutionError {
            get {
                return ResourceManager.GetString("MSG_CpuExecutionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to オペランド &quot;r1,r2&quot; 又は &quot;r,adr[,x]&quot; で、r2 あるいは adr を解釈できませんでした。レジスタ名かアドレスを指定してください。詳細は InnerException を参照してください。.
        /// </summary>
        internal static string MSG_FailedToParseR2OrAdrX {
            get {
                return ResourceManager.GetString("MSG_FailedToParseR2OrAdrX", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 指定の命令コード &quot;{0}&quot; は定義されていません。.
        /// </summary>
        internal static string MSG_InstructionNotDefined {
            get {
                return ResourceManager.GetString("MSG_InstructionNotDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 指定の文字 &apos;{0}&apos; は 10 進定数を記述する半角数字 (0~9) ではありません。10 進定数には半角数字を使用してください。.
        /// </summary>
        internal static string MSG_InvalidCharForDecimalConstant {
            get {
                return ResourceManager.GetString("MSG_InvalidCharForDecimalConstant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 16 進定数として指定の文字列 &quot;{0}&quot; の桁数が 4 桁ではなく {1} 桁です。16 進定数は #h の形式で、h は 4 桁の 16 進数 (0~9, A~F) で指定します。.
        /// </summary>
        internal static string MSG_InvalidHexConstantDigitCount {
            get {
                return ResourceManager.GetString("MSG_InvalidHexConstantDigitCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ラベル &quot;{0}&quot; はすでに定義されています。ラベルの名前はプログラムの中で重複しないようにしてください。.
        /// </summary>
        internal static string MSG_LabelAlreadyDefined {
            get {
                return ResourceManager.GetString("MSG_LabelAlreadyDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ラベル &quot;{0}&quot; の先頭の文字 &apos;{1}&apos; が半角英大文字ではありません。ラベルの先頭の文字は半角英大文字にしてください。.
        /// </summary>
        internal static string MSG_LabelFirstCharIsNotUppercase {
            get {
                return ResourceManager.GetString("MSG_LabelFirstCharIsNotUppercase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ラベル &quot;{0}&quot; は予約語です。ラベルには予約語を使わないでください。予約語は {1} です。 .
        /// </summary>
        internal static string MSG_LabelIsReservedWord {
            get {
                return ResourceManager.GetString("MSG_LabelIsReservedWord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ラベル &quot;{0}&quot; の長さ {1} 文字は有効な範囲外です。ラベルの長さは {2} ~ {3} 文字にしてください。.
        /// </summary>
        internal static string MSG_LabelLengthOutOfRange {
            get {
                return ResourceManager.GetString("MSG_LabelLengthOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ラベル &quot;{0}&quot; の文字 &apos;{1}&apos; が、半角英大文字でも半角数字でもありません。ラベルの 2 文字目以降は、半角英大文字または半角数字にしてください。.
        /// </summary>
        internal static string MSG_LabelSubsequentCharIsNeitherUppercaseNorDigit {
            get {
                return ResourceManager.GetString("MSG_LabelSubsequentCharIsNeitherUppercaseNorDigit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} をリテラルとして解釈できませんでした。リテラルは、10 進定数、16 進定数、または文字定数の前に等号 (=) を付けて記述します。.
        /// </summary>
        internal static string MSG_LiteralParseError {
            get {
                return ResourceManager.GetString("MSG_LiteralParseError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 指定の文字定数 &quot;{0}&quot; で、閉じ側のアポストロフィ (&apos;) がありません。.
        /// </summary>
        internal static string MSG_NoCloseQuoteInStrConstant {
            get {
                return ResourceManager.GetString("MSG_NoCloseQuoteInStrConstant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 命令コードがありません。命令行には、オプションのラベルと空白に続いて、命令コードがなければなりません。.
        /// </summary>
        internal static string MSG_NoInstructionInInstructionLine {
            get {
                return ResourceManager.GetString("MSG_NoInstructionInInstructionLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ラベルを予期していましたが指定されていませんでした。ラベルを指定してください。.
        /// </summary>
        internal static string MSG_NoLabel {
            get {
                return ResourceManager.GetString("MSG_NoLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 次の文字として &apos;{0}&apos; を予期していましたが、実際の文字は &apos;{1}&apos; でした。.
        /// </summary>
        internal static string MSG_NotExpectedChar {
            get {
                return ResourceManager.GetString("MSG_NotExpectedChar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to オペランドの字句要素として {0} を予期していましたが、実際は {1} でした。{0} を記述してください。.
        /// </summary>
        internal static string MSG_NotExpectedToken {
            get {
                return ResourceManager.GetString("MSG_NotExpectedToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} 命令のオペランドで、{1} から後ろが解釈できずに残りました。記述形式を確認してください。.
        /// </summary>
        internal static string MSG_NotParsedTokenRemainsInOperand {
            get {
                return ResourceManager.GetString("MSG_NotParsedTokenRemainsInOperand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} 命令のオペランドの解釈で問題が発生しました。オペランドの記述形式は {1} です。確認してください。.
        /// </summary>
        internal static string MSG_OperandParseError {
            get {
                return ResourceManager.GetString("MSG_OperandParseError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 命令コード 0x{0:x02} は未定義です。.
        /// </summary>
        internal static string MSG_UndefinedOpcode {
            get {
                return ResourceManager.GetString("MSG_UndefinedOpcode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 指定の文字列 &quot;{0}&quot; はレジスタ名ではありません。レジスタ名は、記号 GR0~GR7 で指定してください。.
        /// </summary>
        internal static string MSG_UndefinedRegisterName {
            get {
                return ResourceManager.GetString("MSG_UndefinedRegisterName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 10 進定数.
        /// </summary>
        internal static string STR_DecimalConstant {
            get {
                return ResourceManager.GetString("STR_DecimalConstant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 文字列の終わり.
        /// </summary>
        internal static string STR_EndOfStr {
            get {
                return ResourceManager.GetString("STR_EndOfStr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to オペランドの終わり.
        /// </summary>
        internal static string STR_EndOfToken {
            get {
                return ResourceManager.GetString("STR_EndOfToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 16 進定数.
        /// </summary>
        internal static string STR_HexaDecimalConstant {
            get {
                return ResourceManager.GetString("STR_HexaDecimalConstant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ラベル.
        /// </summary>
        internal static string STR_Label {
            get {
                return ResourceManager.GetString("STR_Label", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to レジスタ名.
        /// </summary>
        internal static string STR_RegisterName {
            get {
                return ResourceManager.GetString("STR_RegisterName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 文字定数.
        /// </summary>
        internal static string STR_StringConstant {
            get {
                return ResourceManager.GetString("STR_StringConstant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;定数[,定数]...&quot;.
        /// </summary>
        internal static string SYN_ConstantList {
            get {
                return ResourceManager.GetString("SYN_ConstantList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;[実行開始番地]&quot;.
        /// </summary>
        internal static string SYN_ExecStartAddr {
            get {
                return ResourceManager.GetString("SYN_ExecStartAddr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;入力領域,入力文字長領域&quot;.
        /// </summary>
        internal static string SYN_InputAreaLengthArea {
            get {
                return ResourceManager.GetString("SYN_InputAreaLengthArea", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to オペランドなし.
        /// </summary>
        internal static string SYN_NoOperand {
            get {
                return ResourceManager.GetString("SYN_NoOperand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to r1,r2 あるいは r,adr[,x].
        /// </summary>
        internal static string SYN_R1R2OrRAdrX {
            get {
                return ResourceManager.GetString("SYN_R1R2OrRAdrX", resourceCulture);
            }
        }
    }
}
