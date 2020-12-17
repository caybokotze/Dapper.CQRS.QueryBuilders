using System;
using Dapper;

namespace DapperDiddle
{
    public interface IMySql
    {
        
    }

    public class MySqlStuff : IMySql
    {
        
    }
    
    public static class Builders
    {
        public static string BuildInsert<T>(this T type)
        {
            return "";
        }

        public static string BuildUpdate<T>(this T type)
        {
            return "";
        }

        public static string BuildSelect<T>(this T type)
        {
            return "";
        }
    }
}