﻿using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Repositories.Players {
    public interface IPlayerRepository {
        void Add(Player player);
        Player Get(string email);
        Player Get(int id);
        IEnumerable<Player> GetAll();
        void Update();
        IEnumerable<Caravan> GetCaravans(MerchandiseType merchandiseType);
        [Obsolete]
        void RemoveCaravan(Caravan caravan);
    }
}