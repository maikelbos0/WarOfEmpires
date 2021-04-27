using System;

namespace WarOfEmpires.Domain.Events {
    public class ScheduledTask : AggregateRoot {
        public virtual TimeSpan Interval { get; set; }
        public virtual string EventType { get; protected set; }
        public virtual bool IsPaused { get; protected set; } = true;
        public virtual TaskExecutionMode ExecutionMode { get; set; }
        public virtual DateTime? LastExecutionDate { get; protected set; }
        public virtual DateTime? NextExecutionDate {
            get {
                if (LastExecutionDate == null) {
                    return null;
                }

                switch (ExecutionMode) {
                    case TaskExecutionMode.ExecuteOnce:
                        var timeSinceLastExecution = DateTime.UtcNow - LastExecutionDate.Value;

                        if (timeSinceLastExecution < Interval) {
                            return LastExecutionDate + Interval;
                        }
                        else {
                            return LastExecutionDate + new TimeSpan(timeSinceLastExecution.Ticks / Interval.Ticks * Interval.Ticks);
                        }
                    case TaskExecutionMode.ExecuteAllIntervals:
                        return LastExecutionDate + Interval;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public static ScheduledTask Create<TEvent>(int id, TimeSpan interval, TaskExecutionMode executionMode) where TEvent : IEvent, new() {
            return new ScheduledTask(id, interval, typeof(TEvent).AssemblyQualifiedName, executionMode);
        }

        protected ScheduledTask() {
        }

        protected ScheduledTask(int id, TimeSpan interval, string eventType, TaskExecutionMode executionMode) {
            Id = id;
            Interval = interval;
            EventType = eventType;
            ExecutionMode = executionMode;
        }

        public virtual void Pause() {
            IsPaused = true;
            LastExecutionDate = null;
        }

        public virtual void Unpause() {
            IsPaused = false;
            LastExecutionDate = DateTime.UtcNow.Date + new TimeSpan(DateTime.UtcNow.TimeOfDay.Ticks / Interval.Ticks * Interval.Ticks);
        }

        public virtual bool Execute() {
            if (IsPaused || NextExecutionDate > DateTime.UtcNow) {
                return false;
            }

            LastExecutionDate = NextExecutionDate;

            return true;
        }
    }
}