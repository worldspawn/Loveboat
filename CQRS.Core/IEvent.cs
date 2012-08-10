using System;
using CQRS.Core.ViewModel;

namespace CQRS.Core
{
    public interface IEvent : IMessage
    {
        //Guid Id { get; set; }
        Guid? SourceId { get; set; }
        Guid AggregateId { get; set; }
        int Version { get; set; }

    }

    public class Event : IEvent
    {
        public Guid? SourceId
        {
            get;
            set;
        }

        public Guid AggregateId { get; set; }
        public int Version { get; set; }
    }

    public class Message : IPersistable
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public IEvent Payload { get; set; }
        public DateTime Inserted { get; set; }

        Guid IPersistable.Id
        {
            get { return AggregateId; }
            set { AggregateId = value; }
        }
    }

    public class Sequence : IPersistable
    {
        public Guid AggregateId { get; set; }
        public int MinVersion { get; set; }
        public int MaxVersion { get; set; }
        public int Messages { get; set; }
        public DateTime Cleared { get; set; }
        
        Guid IPersistable.Id
        {
            get { return AggregateId; }
            set { AggregateId = value; }
        }
    }
}