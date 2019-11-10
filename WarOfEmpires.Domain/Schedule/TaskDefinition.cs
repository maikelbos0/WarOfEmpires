using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Domain.Schedule {
    public class TaskDefinition : AggregateRoot {
        public TimeSpan Interval { get; protected set; }
        public Type EventType { get; protected set; }
        public ICollection<ScheduledTask> Tasks { get; protected set; } = new List<ScheduledTask>();

        public static TaskDefinition Create<TEvent>(TimeSpan interval) where TEvent : IEvent, new() {
            return new TaskDefinition(interval, typeof(TEvent));
        }

        protected TaskDefinition() {
        }

        protected TaskDefinition(TimeSpan interval, Type eventType) {
            Interval = interval;
            EventType = eventType;
        }

        public void GenerateTasks(DateTime startDate, DateTime endDate) {

        }
    }
}