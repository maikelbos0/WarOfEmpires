using System;

namespace WarOfEmpires.Utilities.Services {
    public static class EmailComparisonService {
        public static bool Equals(string email1, string email2) {
            return email1.Equals(email2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}