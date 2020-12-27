using System;

namespace DapperDoodle.Exceptions
{
    public class InvalidSqlStatementException : ArgumentException
    {
        public InvalidSqlStatementException() : base("Please provide a valid sql argument as a parameter.")
        {
        }
    }
}