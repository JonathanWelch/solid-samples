namespace _2_OpenClosed.DataAccess
{
    public interface ICommandAndQueryExecutor
    {
        T Query<T>(IQuery<T> query);

        void Execute(ICommand command);
    }

    public interface ICommand
    {
        void Execute();
    }

    public interface IQuery<T>
    {
        T Run();
    }

    public sealed class CommandAndQueryExecutor : ICommandAndQueryExecutor
    {
        public T Query<T>(IQuery<T> query)
        {
            return query.Run();
        }

        public void Execute(ICommand command)
        {
            command.Execute();
        }
    }
}