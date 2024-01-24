namespace MagicVilla_VillaAPI.Models.Dto
{
    public class LoginResponseDTO
    {

       // public LocalUser User { get; set; } //commenting out for aspnet identity management
        public UserDTO AspnetUser { get; set; } //added for aspnet identity managemenet 
        public string Role { get; set; } // added for aspnet identity management
        public string Token { get; set; }   

    }
}
