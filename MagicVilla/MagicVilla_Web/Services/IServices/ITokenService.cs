using MagicVilla_Web.Models;
using System.Runtime.CompilerServices;
// not used at this time.  
namespace MagicVilla_Web.Services.IServices
{
    public interface ITokenService
    {

        
        public string TokenClient { get; set; }
        public string TokenValue { get; set; }



        Task<T> DoNothing<T>(int id);

    }
}
