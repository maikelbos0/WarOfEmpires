﻿namespace WarOfEmpires.Client.Notifications;

public sealed class Notification {
    public Notification(NotificationType type, string message) {
        Type = type;
        Message = message;
    }

    public NotificationType Type { get; }
    public string Message { get; }
}
