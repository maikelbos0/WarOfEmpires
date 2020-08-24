using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.Repositories.Players {
    [InterfaceInjectable]
    public sealed class PlayerRepository : IPlayerRepository {
        private readonly IWarContext _context;

        public PlayerRepository(IWarContext context) {
            _context = context;
        }

        public Player Get(string email) {
            return _context.Players.Single(p => p.User.Status == UserStatus.Active && EmailComparisonService.Equals(p.User.Email, email));
        }

        public Player Get(int id) {
            return _context.Players.Single(p => p.User.Status == UserStatus.Active && p.Id == id);
        }

        public IEnumerable<Player> GetAll() {
            return _context.Players.Where(p => p.User.Status == UserStatus.Active).ToList();
        }

        public void Add(Player player) {
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public void Update() {
            _context.SaveChanges();
        }

        public IEnumerable<Caravan> GetCaravans(MerchandiseType merchandiseType) {
            return _context.Players
                .Where(p => p.User.Status == UserStatus.Active)
                .SelectMany(p => p.Caravans)
                .Where(c => c.Merchandise.Any(m => m.Type == merchandiseType && m.Quantity > 0))
                .Include(c => c.Merchandise)
                .Include(c => c.Player)
                .ToList();
        }

        public void RemoveCaravan(Caravan caravan) {
            caravan.Player.Caravans.Remove(caravan);
            _context.Remove(caravan);
            _context.SaveChanges();
        }

        [Obsolete]
        public Alliance GetAlliance(int id) {
            return _context.Alliances.Single(a => a.Id == id);
        }

        [Obsolete]
        public IEnumerable<Alliance> GetAllAlliances() {
            return _context.Alliances.ToList();
        }

        [Obsolete]
        public void AddAlliance(Alliance alliance) {
            _context.Alliances.Add(alliance);
            _context.SaveChanges();
        }

        [Obsolete]
        public void RemoveInvite(Invite invite) {
            invite.Alliance.Invites.Remove(invite);
            _context.Remove(invite);
            _context.SaveChanges();
        }
    }
}