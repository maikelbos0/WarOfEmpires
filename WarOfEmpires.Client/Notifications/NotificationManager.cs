namespace WarOfEmpires.Client.Notifications;

public class NotificationManager {
    private const int DefaultTimeoutInSeconds = 3;

    private readonly List<Notification> notifications = new();

    public Action? StateHasChanged { get; set; }

    public IReadOnlyCollection<Notification> Notifications => notifications.AsReadOnly();

    public async Task Notify(NotificationType type, string message, bool automaticallyDismiss) {
        var notification = new Notification(type, message);

        notifications.Add(notification);
        StateHasChanged?.Invoke();

        if (automaticallyDismiss) {
            await Task.Delay(DefaultTimeoutInSeconds * 1000);

            Dismiss(notification);
        }
    }

    public void Dismiss(Notification notification) {
        notifications.Remove(notification);
        StateHasChanged?.Invoke();
    }
}
