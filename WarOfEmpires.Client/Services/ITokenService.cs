using System;
using System.Threading.Tasks;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Client.Services;

public interface ITokenService {
    public Task<UserTokenModel?> AcquireNewTokens(UserTokenModel tokens);
}
