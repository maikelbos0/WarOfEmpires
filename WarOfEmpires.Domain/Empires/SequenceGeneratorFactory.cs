using System;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Empires {
    public static class SequenceGeneratorFactory {
        private static readonly int[] _seed = { 2, 3, 5, 8, 12 };

        public static ExpressionGenerator<Resources>.GeneratorFunction GetGeneratorFunction(Resources multiplier) {
            return (int level, int levelOffset) => GetValue(levelOffset) * multiplier;
        }

        public static ExpressionGenerator<string>.GeneratorFunction GetGeneratorFunction(Func<int, string> valueFunction) {
            return (int level, int levelOffset) => valueFunction(GetValue(levelOffset));
        }

        public static ExpressionGenerator<int>.GeneratorFunction GetGeneratorFunction(int multiplier) {
            return (int level, int levelOffset) => GetValue(levelOffset) * multiplier;
        }

        public static int GetValue(int levelOffset) {
            var exponent = (int)System.Math.Pow(10, levelOffset / _seed.Length);

            return exponent * _seed[levelOffset % _seed.Length];
        }
    }
}