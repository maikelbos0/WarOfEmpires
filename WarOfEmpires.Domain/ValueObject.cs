using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain {
    public abstract class ValueObject {
        /// <summary>
        /// An enumerable of items to compare the value object to another
        /// </summary>
        /// <returns>An enumerable of the items that must be equal</returns>
        /// <remarks>Care must be made to only return only value objects as equality components to prevent it using only reference equality</remarks>
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null)) {
                return false;
            }

            if (GetType() != obj.GetType())
                return false;

            return GetEqualityComponents().SequenceEqual(((ValueObject)obj).GetEqualityComponents());
        }

        public override int GetHashCode() {
            return GetEqualityComponents().Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
        }

        public static bool operator ==(ValueObject a, ValueObject b) {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b) {
            return !(a == b);
        }
    }
}