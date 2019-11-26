using System;
using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain.Empires {
    public class ExpressionGenerator<TValue> {
        public delegate TValue GeneratorFunction(int level, int levelOffset);

        private readonly Dictionary<int, GeneratorFunction> _generatorSteps = new Dictionary<int, GeneratorFunction>();

        public ExpressionGenerator(TValue startValue) {
            Add(0, startValue);
        }

        public ExpressionGenerator(GeneratorFunction startGeneratorFunc) {
            Add(0, startGeneratorFunc);
        }

        public void Add(int fromLevel, TValue value) {
            Add(fromLevel, (int level, int levelOffset) => value);
        }

        public void Add(int fromLevel, GeneratorFunction generatorFunc) {
            if (fromLevel < 0) throw new ArgumentOutOfRangeException("fromLevel", "Negative values are not allowed");

            _generatorSteps[fromLevel] = generatorFunc;
        }

        public TValue Get(int level) {
            if (level < 0) throw new ArgumentOutOfRangeException("fromLevel", "Negative values are not allowed");

            var keyLevel = _generatorSteps.Keys.Where(l => l <= level).Max();
            var levelOffset = level - keyLevel;

            return _generatorSteps[keyLevel](level, levelOffset);
        }
    }
}