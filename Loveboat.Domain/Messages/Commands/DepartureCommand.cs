using System;
using System.ComponentModel.DataAnnotations;
using CQRS.Core;
using Loveboat.Domain.ViewModels;

namespace Loveboat.Domain.Messages.Commands
{
    public class DepartureCommand : ShipsViewModel, ICommand
    {
        [Required]
        public Guid DepartingShipId { get; set; }

    }
}