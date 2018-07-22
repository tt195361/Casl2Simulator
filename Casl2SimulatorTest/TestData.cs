using System;
using System.Diagnostics;

namespace Tt195361.Casl2SimulatorTest
{
    /// <summary>
    /// テストに使用するデータを格納し、そのデータを順に取得できるようにします。
    /// </summary>
    /// <typeparam name="T">格納するデータの型です。</typeparam>
    internal class TestData<T>
    {
        #region Instance Fiends
        private readonly T[] m_data;
        private Int32 m_dataIndex;
        #endregion

        /// <summary>
        /// 指定のデータを用いて <see cref="TestData"/> のインスタンスを初期化します。
        /// </summary>
        /// <param name="data">テストに使用するデータです。1 つ以上のデータを指定する必要があります。</param>
        internal TestData(params T[] data)
        {
            Debug.Assert(1 <= data.Length, nameof(TestData<T>) + ": 1 つ以上のデータを指定する必要があります");

            m_data = data;
            m_dataIndex = 0;
        }

        /// <summary>
        /// 次に使用するデータを取得します。データを最後まで使用した場合は、最初のデータに戻ります。
        /// </summary>
        /// <returns>次に使用するデータを返します。</returns>
        internal T GetNext()
        {
            T datum = m_data[m_dataIndex];
            m_dataIndex = (m_dataIndex + 1) % m_data.Length;
            return datum;
        }
    }
}
