using System;
using System.Data;
using DapperDiddle.Interfaces;

namespace DapperDiddle
{
    public abstract class Command<T> : Command
    {
        public T Result { get; set; }
    }
    
    public abstract class Command : BaseSqlExecutor
    {
        public abstract void Execute();
    }
}