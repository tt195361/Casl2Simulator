using System;
using System.Collections.Generic;
using System.Linq;

namespace Tt195361.Casl2Simulator.Utils
{
    /// <summary>
    /// 例外に関するメソッドを集めました。
    /// </summary>
    internal static class ExceptionUtils
    {
        internal static String BuildErrorMessage(Exception ex)
        {
            return ex.EnumerateInnerEx()
                     .Select((exp) => exp.Message)
                     .MakeList("; ");
        }

        private static IEnumerable<Exception> EnumerateInnerEx(this Exception ex)
        {
            Exception exToCheck = ex;
            while (exToCheck != null)
            {
                yield return exToCheck;
                exToCheck = exToCheck.InnerException;
            }
        }
    }
}
