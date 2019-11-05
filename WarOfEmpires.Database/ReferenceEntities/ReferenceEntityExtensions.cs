using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal static class ReferenceEntityExtensions {
        public static IEnumerable<TReferenceEntity> GetValues<TEnum, TReferenceEntity>()
            where TEnum : Enum
            where TReferenceEntity : BaseReferenceEntity<TEnum>, new() {

            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new TReferenceEntity() {
                    Id = e,
                    Name = Regex.Replace(e.ToString(), "(\\B[A-Z])", g => $" {g.Value.ToLower()}")
                });
        }
    }
}