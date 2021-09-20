using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal static class ReferenceEntityExtensions {
        private readonly static EnumFormatter _formatter = new EnumFormatter();

        public static IEnumerable<TReferenceEntity> GetValues<TEnum, TReferenceEntity>()
            where TEnum : Enum
            where TReferenceEntity : BaseReferenceEntity<TEnum>, new() {

            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(value => new TReferenceEntity() {
                    Id = value,
                    Name = _formatter.ToString(value)
                });
        }
    }
}
