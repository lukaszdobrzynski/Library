﻿namespace Library.Modules.Reservation.Infrastructure.InternalCommands
{
    internal class InternalCommandDto
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }
    }
}