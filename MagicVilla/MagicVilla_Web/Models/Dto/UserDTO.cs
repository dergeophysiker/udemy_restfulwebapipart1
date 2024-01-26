namespace MagicVilla_Web.Models.Dto
{
    public class UserDTO
    {
        public string Id { get; set; } //change from int because we are using aspnet identity and id is guid
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
      //  public string Role { get; set; }  not needed for aspnet identity
    }
}
