﻿using MediatR;

namespace Ordering.Application.Configuration.CQRS.Commands;

public interface ICommand : IRequest
{
    Guid Id { get; }
}

public interface ICommand<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
