namespace WarOfEmpires.Client.Notifications;

public class NotificationManager {

    private readonly List<Notification> notifications = new();
    private readonly Action stateHasChanged;

    public NotificationManager(Action stateHasChanged) {
        this.stateHasChanged = stateHasChanged;
    }

    public IReadOnlyCollection<Notification> Notifications => notifications.AsReadOnly();

    public async Task Notify(NotificationType type, string message, int? timeoutInSeconds = null) {
        var notification = new Notification(type, message);

        notifications.Add(notification);
        stateHasChanged();

        if (timeoutInSeconds.HasValue) {
            await Task.Delay(timeoutInSeconds.Value * 1000);

            Dismiss(notification);
        }
    }

    public void Dismiss(Notification notification) {
        notifications.Remove(notification);
        stateHasChanged();
    }
}
