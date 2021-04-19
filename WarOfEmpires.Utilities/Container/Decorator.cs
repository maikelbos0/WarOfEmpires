using System;

namespace WarOfEmpires.Utilities.Container {
    [Obsolete]
    public abstract class Decorator<THandler> {
        public THandler Handler { get; set; }
    }
}