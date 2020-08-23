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

        public interface ITest6 {
            ITest6b First { get; }
            ITest6b Second { get; }
        }

        [InterfaceInjectable]
        public class Test6 : ITest6 {
            public ITest6b First { get; }
            public ITest6b Second { get; }

            public Test6(ITest6b first, ITest6b second) {
                First = first;
                Second = second;
            }
        }

        public interface ITest6b { }

        [InterfaceInjectable]
        public class Test6b : ITest6b { }

        public sealed class TestDecorator7Attribute : DecoratorAttribute {
            public TestDecorator7Attribute() : base(typeof(TestDecorator7<>)) {
            }
        }

        public sealed class TestDecorator7<TValue> : Decorator<ITest7<TValue>>, ITest7<TValue> {
            public TValue GetFirstValue() {
                return Handler.GetFirstValue();
            }

            public TValue GetSecondValue() {
                return Handler.GetSecondValue();
            }
        }

        public interface ITest7<TValue> {
            TValue GetFirstValue();
            TValue GetSecondValue();
        }

        [InterfaceInjectable]
        [TestDecorator7]
        public class Test7 : ITest7<ITest7b> {
            private readonly ITest7b _first;
            private readonly ITest7b _second;

            public ITest7b GetFirstValue() {
                return _first;
            }

            public ITest7b GetSecondValue() {
                return _second;
            }

            public Test7(ITest7b first, ITest7b second) {
                _first = first;
                _second = second;
            }
        }

        public interface ITest7b { }

        [InterfaceInjectable]
        public class Test7b : ITest7b { }

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

        [TestMethod]
        public void ContainerService_Registers_Single_Object_Of_Type_For_Entire_Object_Graph() {
            var registrar = new ContainerService(GetClassFinderFor(typeof(Test6), typeof(Test6b)));
            var container = registrar.GetContainer();

            var result = container.Resolve<ITest6>();

            result.First.Should().Be(result.Second);
        }

        [TestMethod]
        public void ContainerService_Registers_Single_Object_Of_Type_For_Entire_Object_Graph_With_Decorators() {
            var registrar = new ContainerService(GetClassFinderFor(typeof(Test7), typeof(Test7b)));
            var container = registrar.GetContainer();

            var result = container.Resolve<ITest7<ITest7b>>();

            result.GetFirstValue().Should().Be(result.GetSecondValue());
        }
    }
}