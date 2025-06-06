namespace Ordering.Application.Configuration.CQRS.Commands;

public class CommandBase : ICommand
{
    public Guid Id { get; }
    public CommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected CommandBase(Guid id)
    {
        Id = id;
    }
}

public class CommandBase<TResult> : ICommand<TResult>
{
    public Guid Id { get; }
    public CommandBase()
    {
        Id = Guid.NewGuid();
    }
    protected CommandBase(Guid id)
    {
        Id = id;
    }
}
