using WarOfEmpires.Utilities.Reflection;
using System;
using System.Linq;
using System.Reflection;
using Unity;

namespace WarOfEmpires.Utilities.Container {
    public sealed class InjectionRegistrar {
        private readonly IClassFinder _classFinder;

        public InjectionRegistrar(IClassFinder classFinder) {
            _classFinder = classFinder;
        }

        public void RegisterTypes(IUnityContainer container) {
            var mappedTypes = _classFinder.FindAllClasses()
                .Where(type => Attribute.IsDefined(type, typeof(InterfaceInjectableAttribute)))
                .Select(type => new {
                    Type = type,
                    Interface = type.GetInterfaces().SingleOrDefault(i => i.Name == $"I{type.Name}") ?? type.GetInterfaces().SingleOrDefault()
                })
                .Where(type => type.Interface != null);
            
            foreach (var mappedType in mappedTypes) {
                if (container.IsRegistered(mappedType.Interface)) {
                    throw new InvalidOperationException($"Type registration for {mappedType.Interface.FullName} failed; double type registration found.");
                }

                var decoratorTypes = mappedType.Type.GetCustomAttributes()
                    .Where(a => a.GetType().IsSubclassOf(typeof(DecoratorAttribute)))
                    .Select(a => (DecoratorAttribute)a)
                    .OrderByDescending(a => a.Order)
                    .Select(a => a.DecoratorType.MakeGenericType(mappedType.Interface.GetGenericArguments()))
                    .ToArray();
                
                if (!decoratorTypes.Any()) {
                    container.RegisterType(mappedType.Interface, mappedType.Type);
                }
                else {
                    if (decoratorTypes.Any(t => !t.GetInterfaces().Contains(mappedType.Interface))) {
                        throw new InvalidOperationException($"Type registration for {mappedType.Interface.FullName} failed; invalid decorator found.");
                    }

                    // If we have decorators, we chain the calls
#pragma warning disable IDE0039 // Use local function
                    Func<IUnityContainer, object> action = c => {
#pragma warning restore IDE0039 // Use local function
                        dynamic obj = c.Resolve(mappedType.Type);

                        foreach (var decoratorType in decoratorTypes) {
                            dynamic decoratorObj = c.Resolve(decoratorType);

                            decoratorObj.Handler = obj;
                            obj = decoratorObj;
                        }

                        return obj;
                    };

                    container.RegisterFactory(mappedType.Interface, action);
                }
            }
        }
    }
}