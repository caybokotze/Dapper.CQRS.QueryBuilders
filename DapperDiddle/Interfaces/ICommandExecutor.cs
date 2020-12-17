﻿using System.Collections.Generic;

namespace DapperDiddle.Interfaces
{
    public interface ICommandExecutor
    {
        void Execute(Command command);

        T Execute<T>(Command<T> command);

        void Execute(IEnumerable<Command> commands);
    }
}