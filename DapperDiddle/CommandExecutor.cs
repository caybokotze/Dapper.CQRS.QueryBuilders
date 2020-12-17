using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DapperDiddle
{
    public class CommandExecutor : ICommandExecutor
    {
        public void Execute(Command command)
        {
            command.Execute();
        }

        public T Execute<T>(Command<T> command)
        {
            Execute((Command) command);
            return command.Result;
        }

        public void Execute(IEnumerable<Command> commands)
        {
            if (!(commands is Command[] commandArray))
                commandArray = commands.ToArray();
            
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            
            foreach (var t in commandArray)
            {
                t.Execute();
            }
        }
    }

    public interface ICommandExecutor
    {
        void Execute(Command command);

        T Execute<T>(Command<T> command);

        void Execute(IEnumerable<Command> commands);
    }
}