﻿using System;
using System.ComponentModel.DataAnnotations;
using CQRS.Core;
using Loveboat.Domain.ViewModels;

namespace Loveboat.Domain.Messages.Commands
{
    public class ArrivalCommand : ShipsViewModel, ICommand
    {
        [Required]
        public Guid ArrivingShipId { get; set; }

        [Required]
        public string ArrivalPort { get; set; }

    }
}