using System.Collections.Generic;
using WarOfEmpires.Domain.Markets;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class MerchandiseTypeEntity : BaseReferenceEntity<MerchandiseType> {
        public ICollection<Merchandise> Merchandise { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}