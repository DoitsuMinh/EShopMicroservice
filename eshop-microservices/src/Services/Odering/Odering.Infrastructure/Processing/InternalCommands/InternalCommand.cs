﻿namespace Odering.Infrastructure.Processing.InternalCommands;

public class InternalCommand
{
    public Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Data { get; set; }
    public DateTime? ProcessedDate { get; set; }
}
