﻿using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetWorkersQuery : IQuery<WorkersModel> {
        public string Email { get; }

        public GetWorkersQuery(string email) {
            Email = email;
        }
    }
}