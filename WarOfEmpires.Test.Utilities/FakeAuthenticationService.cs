using System;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Services;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeAuthenticationService : IAuthenticationService {
        public bool IsAuthenticated {
            get {
                return Identity != null;
            }
        }
        public string Identity { get; set; }

        public string GetUserName() {
            return Identity;
        }

        public bool IsAdmin() {
            // Not used for now
            throw new NotImplementedException();
        }

        public CurrentAllianceRightsViewModel GetAllianceRights() {
            // Not used for now
            throw new NotImplementedException();
        }

        public void SignIn(string identity) {
            Identity = identity;
        }

        public void SignOut() {
            Identity = null;
        }
    }
}