using System;

namespace WarOfEmpires.Domain.Events {
    public class ScheduledTask : AggregateRoot {
        public TimeSpan Interval { get; protected set; }
        public string EventType { get; protected set; }
        public bool IsPaused { get; protected set; } = true;
        public DateTime? LastExecutionDate { get; protected set; }
        public DateTime? NextExecutionDate {
            get {
                return LastExecutionDate + Interval;
            }
        }

        public static ScheduledTask Create<TEvent>(TimeSpan interval) where TEvent : IEvent, new() {
            return new ScheduledTask(interval, typeof(TEvent).AssemblyQualifiedName);
        }

        protected ScheduledTask() {
        }

        protected ScheduledTask(TimeSpan interval, string eventType) {
            Interval = interval;
            EventType = eventType;
        }

        public void Pause() {
            IsPaused = true;
            LastExecutionDate = null;
        }

        public void Unpause() {
            IsPaused = false;
            LastExecutionDate = DateTime.UtcNow.Date;

            while (NextExecutionDate < DateTime.UtcNow) {
                LastExecutionDate = NextExecutionDate;
            }
        }

        public bool Execute() {
            if (IsPaused || NextExecutionDate > DateTime.UtcNow) {
                return false;
            }

            LastExecutionDate = NextExecutionDate;
            EventService.Service.Dispatch((IEvent)Activator.CreateInstance(Type.GetType(EventType)));

            return true;
        }
    }
}