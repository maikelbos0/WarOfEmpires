using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Utilities.Linq {
    public static class EnumerableExtensions {
        public static Resources Sum(this IEnumerable<Resources> source) {
            return new Resources(
                source.Sum(r => r.Gold),
                source.Sum(r => r.Food),
                source.Sum(r => r.Wood),
                source.Sum(r => r.Stone),
                source.Sum(r => r.Ore)
            );
        }

        public static Resources Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Resources> selector) {
            return source.Select(selector).Sum();
        }
    }
}