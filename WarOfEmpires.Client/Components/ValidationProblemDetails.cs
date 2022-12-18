using System.Collections.Generic;

namespace WarOfEmpires.Client.Components;

public sealed class ValidationProblemDetails {
    public IDictionary<string, string[]> Errors { get; }

    public ValidationProblemDetails(IDictionary<string, string[]> errors) {
        Errors = errors;
    }
}