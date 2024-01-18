using MagicVilla_Utility;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class TokenService : ITokenService
    {

        public IHttpContextAccessor _httpContextAccessor;
        public string TokenClient { get ; set; }
        public string TokenValue { get; set; }

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            TokenValue = string.Empty;
            TokenValue= _httpContextAccessor.HttpContext.Session.GetString(SD.SessionTokenKeyName);
        }



        public Task<T> DoNothing<T>(int id)
        {
            throw new NotImplementedException();
        }
    }
}
