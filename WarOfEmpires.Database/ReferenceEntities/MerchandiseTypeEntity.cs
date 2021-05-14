using System.Collections.Generic;
using WarOfEmpires.Domain.Markets;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class MerchandiseTypeEntity : BaseReferenceEntity<MerchandiseType> {
        public virtual ICollection<Merchandise> Merchandise { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}