using System;
using System.Data;

namespace ElasticSearch.ConsoleApp
{
    public static class IDataReaderExtensions
    {
        public static Guid GetGuidValue(this IDataReader reader, string key)
        {
            var value = reader[key];
            if (value is DBNull) return Guid.Empty;
            return (Guid)value;
        }

        public static int GetIntValue(this IDataReader reader, string key)
        {
            var value = reader[key];
            if (value is DBNull) return 0;
            return int.Parse(value.ToString());
        }

        public static bool GetBooleanValue(this IDataReader reader, string key)
        {
            var value = reader[key];
            if (value is DBNull) return false;
            return (bool)value;
        }

        public static string GetStringValue(this IDataReader reader, string key, string @default = "")
        {
            var value = reader[key];
            if (value is DBNull) return @default;
            return (string)value;
        }

    }
}