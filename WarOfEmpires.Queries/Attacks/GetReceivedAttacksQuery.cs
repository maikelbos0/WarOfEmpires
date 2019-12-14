﻿using WarOfEmpires.Models.Attacks;

namespace WarOfEmpires.Queries.Attacks {
    public sealed class GetReceivedAttacksQuery : IQuery<ReceivedAttackViewModel> {
        public string Email { get; }

        public GetReceivedAttacksQuery(string email) {
            Email = email;
        }
    }
}