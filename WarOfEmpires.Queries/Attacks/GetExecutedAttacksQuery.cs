﻿using System.Collections.Generic;
using WarOfEmpires.Models.Attacks;

namespace WarOfEmpires.Queries.Attacks {
    public sealed class GetExecutedAttacksQuery : IQuery<List<ExecutedAttackViewModel>> {
        public string Email { get; }

        public GetExecutedAttacksQuery(string email) {
            Email = email;
        }
    }
}