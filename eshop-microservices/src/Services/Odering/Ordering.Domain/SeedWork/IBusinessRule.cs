﻿namespace Ordering.Domain.SeedWork;

public interface IBusinessRule
{
    bool IsBroken();
    string Message { get; }
}
