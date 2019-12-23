namespace WarOfEmpires.Domain.Empires {
    public static class SequenceGeneratorFactory {
        private static readonly int[] _seed = { 2, 3, 5, 8, 13 };

        public static ExpressionGenerator<int>.GeneratorFunction GetGeneratorFunction(int multiplier) {
            return (int level, int levelOffset) => GetValue(level) * multiplier;
        }

        public static int GetValue(int level) {
            var exponent = (int)System.Math.Pow(10, level / _seed.Length);

            return exponent * _seed[level % _seed.Length];
        }
    }
}