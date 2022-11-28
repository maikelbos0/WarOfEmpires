namespace WarOfEmpires.Domain.Security {
    public enum UserEventType : byte {
        LoggedIn = 1,
        FailedLogIn = 2,
        Registered = 3,
        Activated = 4,
        FailedActivation = 5,
        ActivationCodeSent = 6,
        LoggedOut = 7,
        PasswordChanged = 8,
        FailedPasswordChange = 9,
        PasswordResetRequested = 10,
        PasswordReset = 11,
        FailedPasswordReset = 12,
        Deactivated = 13,
        FailedDeactivation = 14,
        EmailChangeRequested = 15,
        FailedEmailChangeRequest = 16,
        EmailChanged = 17,
        FailedEmailChange = 18,
        FailedPasswordResetRequest = 19,
        RefreshTokenGenerated = 20,
        RefreshTokenRotated = 21,
        FailedRefreshTokenRotation = 22
    }
}