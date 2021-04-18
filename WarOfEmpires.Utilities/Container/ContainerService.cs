using WarOfEmpires.Utilities.Reflection;
using System;
using System.Linq;
using System.Reflection;
using Unity;
using Unity.Lifetime;

namespace WarOfEmpires.Utilities.Container {
    [Obsolete]
    public sealed class ContainerService {        
        private readonly IClassFinder _classFinder;

        public ContainerService(IClassFinder classFinder) {
            _classFinder = classFinder;
        }

        public IUnityContainer GetContainer() {
            var container = new UnityContainer();
            var mappedTypes = _classFinder.FindAllClasses()
                .Where(type => Attribute.IsDefined(type, typeof(InterfaceInjectableAttribute)))
                .Select(type => new {
                    Type = type,
                    Interface = type.GetInterfaces().SingleOrDefault(i => i.Name == $"I{type.Name}") ?? type.GetInterfaces().SingleOrDefault()
                });
            
            foreach (var mappedType in mappedTypes) {
                if (mappedType.Interface == null) {
                    throw new InvalidOperationException($"Type registration for {mappedType.Type.FullName} failed; no valid interface found");
                }

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
                    container.RegisterType(mappedType.Interface, mappedType.Type, new PerResolveLifetimeManager());
                }
                else {
                    if (decoratorTypes.Any(t => !t.GetInterfaces().Contains(mappedType.Interface))) {
                        throw new InvalidOperationException($"Type registration for {mappedType.Interface.FullName} failed; invalid decorator found.");
                    }

                    // If we have decorators, we chain the calls
                    object action (IUnityContainer c) {
                        dynamic obj = c.Resolve(mappedType.Type);

                        foreach (var decoratorType in decoratorTypes) {
                            dynamic decoratorObj = c.Resolve(decoratorType);

                            decoratorObj.Handler = obj;
                            obj = decoratorObj;
                        }

                        return obj;
                    };

                    container.RegisterFactory(mappedType.Interface, action, new PerResolveLifetimeManager());
                }
            }

            return container;
        }
    }
}