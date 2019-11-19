using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using Unity;

namespace WarOfEmpires.Utilities.Tests.Container {
    [TestClass]
    public sealed class ContainerServiceTests {
        public interface ITest1 { }

        [InterfaceInjectable]
        public sealed class Test1 : ITest1 { }

        [InterfaceInjectable]
        public sealed class Test1b : ITest1 { }

        public interface ITest2<TValue> { }

        [InterfaceInjectable]
        public sealed class Test2 : ITest2<Test1> { }

        [InterfaceInjectable]
        public sealed class Test4 : ITest1, ITest2<string> { }

        public interface ITest3<TValue> {
            TValue GetValue();
        }

        [InterfaceInjectable]
        [TestDecorator1]
        public sealed class Test3a : ITest3<string> {
            public string GetValue() {
                return "Success";
            }
        }

        [InterfaceInjectable]
        [TestDecorator2]
        [TestDecorator1(Order = 1)]
        public sealed class Test3b : ITest3<int> {
            public int GetValue() {
                return 1;
            }
        }

        public sealed class TestDecorator1Attribute : DecoratorAttribute {
            public TestDecorator1Attribute() : base(typeof(TestDecorator1<>)) {
            }
        }

        public sealed class TestDecorator1<TValue> : Decorator<ITest3<TValue>>, ITest3<TValue> {
            public TValue GetValue() {
                return Handler.GetValue();
            }
        }

        public sealed class TestDecorator2Attribute : DecoratorAttribute {
            public TestDecorator2Attribute() : base(typeof(TestDecorator2<>)) {
            }
        }

        public sealed class TestDecorator2<TValue> : Decorator<ITest3<TValue>>, ITest3<TValue> {
            public TValue GetValue() {
                return Handler.GetValue();
            }
        }

        public interface ITest5<TValue> {
            TValue GetValue();
        }

        [InterfaceInjectable]
        [TestDecorator5]
        public sealed class Test5 : ITest5<string> {
            public string GetValue() {
                return "Success";
            }
        }

        public sealed class TestDecorator5Attribute : DecoratorAttribute {
            public TestDecorator5Attribute() : base(typeof(TestDecorator5<,>)) {
            }
        }

        public sealed class TestDecorator5<TValue, TExtra> : Decorator<ITest5<TValue>>, ITest5<TValue> {
            public TValue GetValue() {
                return Handler.GetValue();
            }
        }

        public IClassFinder GetClassFinderFor<T>() {
            return GetClassFinderFor(typeof(T));
        }

        public IClassFinder GetClassFinderFor(params Type[] types) {
            var classFinder = Substitute.For<IClassFinder>();

            classFinder.FindAllClasses().Returns(types);

            return classFinder;
        }


        [TestMethod]
        public void ContainerService_Registers_For_Matching_Interface() {
            var registrar = new ContainerService(GetClassFinderFor<Test1>());
            var container = registrar.GetContainer();

            container.Resolve<ITest1>().Should().BeOfType<Test1>();
        }

        [TestMethod]
        public void ContainerService_Registers_For_Single_Interface() {
            var registrar = new ContainerService(GetClassFinderFor<Test2>());
            var container = registrar.GetContainer();

            container.Resolve<ITest2<Test1>>().Should().BeOfType<Test2>();
        }

        [TestMethod]
        public void ContainerService_Throws_Exception_For_Multiple_Interfaces() {
            var registrar = new ContainerService(GetClassFinderFor<Test4>());

            Action action = () => {
                registrar.GetContainer();
            };

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void ContainerService_Throws_Exception_For_Multiple_Types_One_Interface() {
            var registrar = new ContainerService(GetClassFinderFor(typeof(Test1), typeof(Test1b)));

            Action action = () => {
                registrar.GetContainer();
            };

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void ContainerService_Registers_Decorators() {
            var registrar = new ContainerService(GetClassFinderFor<Test3a>());
            var container = registrar.GetContainer();

            var decorator = container.Resolve<ITest3<string>>();

            decorator.Should().BeOfType<TestDecorator1<string>>();
            ((TestDecorator1<string>)decorator).Handler.Should().BeOfType<Test3a>();
            decorator.GetValue().Should().Be("Success");
        }

        [TestMethod]
        public void ContainerService_Registers_Chained_Decorators() {
            var registrar = new ContainerService(GetClassFinderFor<Test3b>());
            var container = registrar.GetContainer();

            var decorator = container.Resolve<ITest3<int>>();

            decorator.Should().BeOfType<TestDecorator1<int>>();
            ((TestDecorator1<int>)decorator).Handler.Should().BeOfType<TestDecorator2<int>>();
            ((TestDecorator2<int>)((TestDecorator1<int>)decorator).Handler).Handler.Should().BeOfType<Test3b>();
            decorator.GetValue().Should().Be(1);
        }

        [TestMethod]
        public void ContainerService_Throw_Exception_For_Mismatched_Decorator() {
            var registrar = new ContainerService(GetClassFinderFor<Test5>());

            Action action = () => {
                registrar.GetContainer();
            };

            action.Should().Throw<ArgumentException>();
        }
    }
}