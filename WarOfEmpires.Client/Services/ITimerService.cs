using System;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Services;

public interface ITimerService {
    void ExecuteAfter(Func<Task> method, TimeSpan timeSpan);
}
