namespace MagicVilla_Web.Models.Dto
{
    public class LoginResponseDTO
    {

        //     public UserDTO User { get; set; }
             public UserDTO aspnetUser { get; set; } //making this change because the object requires it. Change from "User" to aspnetUser.


        public string Token { get; set; }
        /*
         * """aspnetUser"": {
  ""id"": ""c3fcb060-a421-426c-9b40-5783c167f761"",
  ""userName"": ""fake@fake.com"",
  ""name"": ""mr. fake""
}"
"""role"": ""admin"""
"""token"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJjM2ZjYjA2MC1hNDIxLTQyNmMtOWI0MC01NzgzYzE2N2Y3NjEiLCJ1bmlxdWVfbmFtZSI6ImZha2VAZmFrZS5jb20iLCJyb2xlIjoiYWRtaW4iLCJuYmYiOjE3MDYyMjc5MzAsImV4cCI6MTcwNjgzMjczMCwiaWF0IjoxNzA2MjI3OTMwfQ.0jMp_HrMoxSDHsU138aytn1_Ok8J53VAycD3Brd_sIQ"""

         * */
    }
}
