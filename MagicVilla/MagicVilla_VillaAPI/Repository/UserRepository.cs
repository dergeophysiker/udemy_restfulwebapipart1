using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public  bool IsUniqueUser(string username)
        {
            //could make this async

            var user =  _db.LocalUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {

        var  user = _db.LocalUsers.FirstOrDefault(u=>u.UserName == loginRequestDTO.UserName && u.Password==loginRequestDTO.Password);


            if (user == null)
            {
                return null;
            }

            //if user was found generate JWT Token



        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            LocalUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
                Name = registrationRequestDTO.Name,
                Password = registrationRequestDTO.Password,
                Role  = registrationRequestDTO.Role
            };

            _db.LocalUsers.Add(user);
           await _db.SaveChangesAsync();
            user.Password = "";
            return user;



        }
    }
}
