﻿using System;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeBuilder {
        public IWarContext Context { get; }

        public FakeBuilder() {
            Context = new FakeWarContext();
        }

        internal FakeBuilder(IWarContext context) {
            Context = context;
        }

        public FakeAllianceBuilder CreateAlliance(int id, string code = "FS", string name = "Føroyskir Samgonga") {
            return new FakeAllianceBuilder(Context, id, code, name);
        }

        public FakePlayerBuilder CreatePlayer(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active) {
            return new FakePlayerBuilder(Context, id, email, displayName, rank, title, lastOnline, status);
        }
    }
}