﻿namespace Odering.Infrastructure.Processing.Outbox;

public class OutBoxMessageDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
}