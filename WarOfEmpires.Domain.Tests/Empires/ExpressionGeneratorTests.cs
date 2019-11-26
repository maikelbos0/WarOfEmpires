using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Tests.Empires {
    [TestClass]
    public sealed class ExpressionGeneratorTests {
        [TestMethod]
        public void ExpressionGenerator_Generates_Value_From_Default_Constant() {
            var generator = new ExpressionGenerator<string>("Default");

            generator.Get(5).Should().Be("Default");
        }

        [TestMethod]
        public void ExpressionGenerator_Generates_Value_From_Constant() {
            var generator = new ExpressionGenerator<string>("Default");

            generator.Add(4, "Medium");
            generator.Add(8, "Large");

            generator.Get(4).Should().Be("Medium");
        }

        [TestMethod]
        public void ExpressionGenerator_Generates_Value_From_Default_GeneratorFunction() {
            var generator = new ExpressionGenerator<string>((int level, int levelOffset) => $"Level {level}, offset {levelOffset}");

            generator.Get(5).Should().Be("Level 5, offset 5");
        }

        [TestMethod]
        public void ExpressionGenerator_Generates_Value_From_GeneratorFunction() {
            var generator = new ExpressionGenerator<string>("Default");

            generator.Add(4, (int level, int levelOffset) => $"Level {level}, offset {levelOffset}");
            generator.Add(8, "New");

            generator.Get(5).Should().Be("Level 5, offset 1");
        }

        [TestMethod]
        public void ExpressionGenerator_Add_Throws_Exception_For_Negative_Level() {
            var generator = new ExpressionGenerator<string>("Default");

            Action action = () => generator.Add(-1, "Error");

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ExpressionGenerator_Get_Throws_Exception_For_Negative_Level() {
            var generator = new ExpressionGenerator<string>("Default");

            Action action = () => generator.Get(-1);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}