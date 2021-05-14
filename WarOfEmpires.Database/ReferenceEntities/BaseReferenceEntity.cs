using System;

namespace WarOfEmpires.Database.ReferenceEntities {
    public abstract class BaseReferenceEntity<TEnum> where TEnum : Enum {
        public virtual TEnum Id { get; set; }
        public virtual string Name { get; set; }
    }
}