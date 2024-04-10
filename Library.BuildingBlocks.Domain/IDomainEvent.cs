﻿namespace Library.BuildingBlocks.Domain;

public interface IDomainEvent
{
    Guid Id { get; }

    DateTime OccurredOn { get; set; }
}