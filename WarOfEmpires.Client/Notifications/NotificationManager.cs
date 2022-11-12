using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WarOfEmpires.Client.Notifications;

public sealed class NotificationManager {
    private const int TimeoutInSeconds = 3;

    private readonly List<Notification> notifications = new();

    public Action? StateHasChanged { get; set; }

    public IReadOnlyCollection<Notification> Notifications => notifications.AsReadOnly();

    public void Notify(NotificationType type, string message) {
        var notification = new Notification(type, message);

        notifications.Add(notification);
        StateHasChanged?.Invoke();
        _ = DismissAfterTimeout(notification);
    }

    private async Task DismissAfterTimeout(Notification notification) {
        await Task.Delay(TimeoutInSeconds * 1000);

        Dismiss(notification);
    }

    public void Dismiss(Notification notification) {
        notifications.Remove(notification);
        StateHasChanged?.Invoke();
    }
}
