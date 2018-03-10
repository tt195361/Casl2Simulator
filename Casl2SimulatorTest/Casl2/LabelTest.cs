using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="Label"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LabelTest
    {
        #region Static Fields
        internal const String ValidLabelName = "LBL001";
        internal const String InvalidLabelName = "不正なラベル";

        internal static readonly Label ValidLabel = new Label(ValidLabelName);
        #endregion

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const Label DontCare = null;

            CheckParse(String.Empty, true, null, "空文字列 => ラベルはないので null");
            CheckParse(ValidLabelName, true, ValidLabel, "適正なラベル名 => ラベルを生成して返す");
            CheckParse(InvalidLabelName, false, DontCare, "不正なラベル名 => 例外");
        }

        private void CheckParse(String labelField, Boolean success, Label expected, String message)
        {
            try
            {
                Label actual = Label.Parse(labelField);
                Assert.IsTrue(success, message);
                Check(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// コンストラクタのテストです。
        /// </summary>
        [TestMethod]
        public void Ctor()
        {
            CheckCtor("A", true, "半角英大文字 1 文字: 最短");
            CheckCtor("A2B4C6D8", true, "半角英大文字とそれに続く半角数字か半角英大文字で 8 文字: 最長");

            CheckCtor(String.Empty, false, "空文字 => 1 文字より短い => エラー");
            CheckCtor("A23456789", false, "文字数が 8 文字より長い => エラー");
            CheckCtor("Ａ", false, "先頭が半角英大文字でない => エラー");
            CheckCtor("A_", false, "以降に半角英大文字か数字以外 (_) => エラー");
            CheckCtor("Ab", false, "以降に半角英大文字か数字以外 (b) => エラー");
            CheckCtor("AＢ", false, "以降に半角英大文字か数字以外 (Ｂ) => エラー");
            CheckCtor("A８", false, "以降に半角英大文字か数字以外 (８) => エラー");
            CheckCtor("GR0", false, "予約語 => エラー");
        }

        private void CheckCtor(String name, Boolean success, String message)
        {
            try
            {
                Label actual = new Label(name);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        internal static void Check(Label expected, Label actual, String message)
        {
            if (expected == null)
            {
                Assert.IsNull(actual, message);
            }
            else
            {
                Assert.IsNotNull(actual, message);
                Assert.AreEqual(expected.Name, actual.Name, "Name: " + message);
            }
        }
    }
}
