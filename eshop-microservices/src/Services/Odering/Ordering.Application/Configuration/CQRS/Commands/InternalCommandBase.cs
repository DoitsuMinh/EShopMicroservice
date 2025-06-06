
namespace Ordering.Application.Configuration.CQRS.Commands;

public class InternalCommandBase : ICommand
{
    public Guid Id { get; }
    protected InternalCommandBase(Guid id)
    {
        Id = id;
    }
}

public class InternalCommandBase<TResult> : ICommand<TResult>
{
    public Guid Id { get; }
    protected InternalCommandBase(Guid id)
    {
        Id = id;
    }
    protected InternalCommandBase()
    {
        Id = Guid.NewGuid();
    }
}