using Google.Apis.Auth;
using Mahamma.Identity.Domain.User.Entity;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.Helper
{
    public interface IJWTHelper
    {
        public string GenerateNewJWT(User user);
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string provider, string idToken);
    }
}
