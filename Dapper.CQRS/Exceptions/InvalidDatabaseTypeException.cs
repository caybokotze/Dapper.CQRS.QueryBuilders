using System;

namespace Dapper.CQRS.Exceptions
{
    public class InvalidDatabaseTypeException : ArgumentException
    {
        public InvalidDatabaseTypeException() : base("The database you have selected is not valid, or is not supported yet.") { }
    }
}