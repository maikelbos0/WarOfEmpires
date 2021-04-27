using System;
using System.Linq;
using WarOfEmpires.Database.ReferenceEntities;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Utilities.Services;
using Empires = WarOfEmpires.Domain.Empires;
using Events = WarOfEmpires.Domain.Events;
using Security = WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Database {
    public sealed class Seeder {
        public void Seed(WarContext context) {
            AddScheduledTask<Empires.RecruitTaskTriggeredEvent>(context, new TimeSpan(1, 0, 0), Events.TaskExecutionMode.ExecuteAllIntervals);
            AddScheduledTask<Empires.TurnTaskTriggeredEvent>(context, new TimeSpan(0, 10, 0), Events.TaskExecutionMode.ExecuteAllIntervals);
            AddScheduledTask<Empires.BankTurnTaskTriggeredEvent>(context, new TimeSpan(4, 0, 0), Events.TaskExecutionMode.ExecuteAllIntervals);
            AddScheduledTask<Empires.UpdateRankTaskTriggeredEvent>(context, new TimeSpan(0, 2, 0), Events.TaskExecutionMode.ExecuteOnce);

            AddOrUpdateUser(context, "example@test.com", "I am example");
            AddOrUpdateUser(context, "anon@test.com", "Anon");
            AddOrUpdateUser(context, "you@test.com", "You");
            AddOrUpdateUser(context, "another@test.com", "Another");
            AddOrUpdateUser(context, "singsongkat@live.com", "Shira");

            for (var i = 0; i < 100; i++) {
                AddOrUpdateUser(context, $"user{i}@test.com", $"User {i}");
            }
        }

        private void AddOrUpdateUser(WarContext context, string email, string displayName) {
            var user = context.Users.SingleOrDefault(u => EmailComparisonService.Equals(u.Email, email));

            if (user == null) {
                user = new Security.User(email, new Random().Next().ToString());
                user.Activate();

                context.Users.Add(user);
            }

            context.SaveChanges();

            var player = context.Players.SingleOrDefault(p => p.Id == user.Id);

            if (player == null) {
                player = new Player(user.Id, displayName);
                context.Players.Add(player);
            }
            else {
                player.DisplayName = displayName;
            }

            context.SaveChanges();
        }

        private void AddScheduledTask<TEvent>(WarContext context, TimeSpan interval, Events.TaskExecutionMode executionMode) where TEvent : Events.IEvent, new() {
            var task = context.ScheduledTasks.SingleOrDefault(t => t.EventType == typeof(TEvent).AssemblyQualifiedName);

            if (task == null) {
                task = Events.ScheduledTask.Create<TEvent>(interval, executionMode);
                context.ScheduledTasks.Add(task);
            }
            else {
                task.Interval = interval;
                task.ExecutionMode = executionMode;
            }

            context.SaveChanges();
        }
    }
}
