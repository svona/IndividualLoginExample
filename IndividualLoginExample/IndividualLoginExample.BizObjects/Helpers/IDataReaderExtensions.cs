using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualLoginExample.BizObjects.Helpers
{
    public static class IDataReaderExtensions
    {
        public static T GetValue<T>(this IDataReader sdr, string columnName, T defaultValue = default(T))
        {
            object result = sdr[columnName];
            return result == null || result == DBNull.Value ? defaultValue : (T)result;
        }
    }
}
