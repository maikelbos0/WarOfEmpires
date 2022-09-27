namespace WarOfEmpires.Client.Notifications;

public class NotificationManager {

    private readonly List<Notification> notifications = new();

    public Action? StateHasChanged { get; set; }

    public IReadOnlyCollection<Notification> Notifications => notifications.AsReadOnly();

    public async Task Notify(NotificationType type, string message, int? timeoutInSeconds = null) {
        var notification = new Notification(type, message);

        notifications.Add(notification);
        StateHasChanged?.Invoke();

        if (timeoutInSeconds.HasValue) {
            await Task.Delay(timeoutInSeconds.Value * 1000);

            Dismiss(notification);
        }
    }

    public void Dismiss(Notification notification) {
        notifications.Remove(notification);
        StateHasChanged?.Invoke();
    }
}
