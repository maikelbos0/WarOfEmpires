﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace WarOfEmpires.Domain.Security {
    public class Password : ValueObject {
        private const int NewHashIterations = 1000;

        public byte[] Salt { get; private set; }
        public byte[] Hash { get; private set; }
        public int? HashIterations { get; private set; }

        protected Password() {
        }

        public Password(string password) : this() {
            Salt = GetNewSalt();
            HashIterations = NewHashIterations;
            Hash = GetHash(password);
        }

        private static byte[] GetNewSalt() {
            return RandomNumberGenerator.GetBytes(20);
        }

        private byte[] GetHash(string password) {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, Salt, HashIterations.Value);

            // Return 20 bytes because after that it repeats
            return pbkdf2.GetBytes(20);
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            if (Salt == null) {
                yield return 0;
            }
            else {
                yield return Salt.Length;
                foreach (var s in Salt) {
                    yield return s;
                }
            }

            if (Hash == null) {
                yield return 0;
            }
            else {
                yield return Hash.Length;
                foreach (var s in Hash) {
                    yield return s;
                }
            }

            yield return HashIterations;
        }

        public virtual bool Verify(string password) {
            if (Hash == null || Hash.Length == 0) {
                return false;
            }
            
            return Hash.SequenceEqual(GetHash(password));
        }
    }
}
