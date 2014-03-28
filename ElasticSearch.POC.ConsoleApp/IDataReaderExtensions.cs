using System;
using System.Data;

namespace ElasticSearch.POC.ConsoleApp
{
    public static class IDataReaderExtensions
    {
        public static Guid GetGuidValue(this IDataReader reader, string key)
        {
            var value = reader[key];
            if (value is DBNull) return Guid.Empty;
            return (Guid)value;
        }

//        public static short GetShortValue(this IDataReader reader, string key)
//        {
//            var value = reader[key];
//            if (value is DBNull) return 0;
//            return (short)value;
//        }

        public static int GetIntValue(this IDataReader reader, string key)
        {
            var value = reader[key];
            if (value is DBNull) return 0;
            return (int)value;
        }

//        public static string GetDateValue(this IDataReader reader, string key, string format = "dd/MM/yyyy")
//        {
//            var value = reader[key];
//            if (value is DBNull) return "";

//            return ((DateTime)value).ToString(format);
//        }

        public static string GetStringValue(this IDataReader reader, string key)
        {
            var value = reader[key];
            if (value is DBNull) return "";
            return (string)value;
        }

    }
}