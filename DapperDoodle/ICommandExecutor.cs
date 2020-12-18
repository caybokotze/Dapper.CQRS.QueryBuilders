using System.Collections.Generic;

namespace DapperDoodle
{
    public interface ICommandExecutor
    {
        void Execute(Command command);

        T Execute<T>(Command<T> command);

        void Execute(IEnumerable<Command> commands);
    }
}