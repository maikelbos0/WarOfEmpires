using System;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WarOfEmpires.Client.Services;

public class TimerService : ITimerService {
    private readonly List<Timer> timers = new();

    public void ExecuteAfter(Func<Task> method, TimeSpan timeSpan) {
        var timer = new Timer(timeSpan.TotalMilliseconds) {
            AutoReset = false
        };
        timer.Elapsed += (_, _) => Execute(method, timer);
        timer.Start();
    }

    private void Execute(Func<Task> method, Timer timer) {
        _ = method();

        timers.Remove(timer);
    }
}