using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;

        //used by ASPNET Identity
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager,
            IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;  // added for ASPNET Identity
            _roleManager = roleManager; // added for ASPNET Identity
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");

        }
        public bool IsUniqueUser(string username)
        {
            //could make this async

            // var user =  _db.LocalUsers.FirstOrDefault(x => x.UserName == username);
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);  // using ASPNET identity instead of home brew

            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {

            //var  user = _db.LocalUsers.FirstOrDefault(u=>u.UserName == loginRequestDTO.UserName && u.Password==loginRequestDTO.Password);

            // first retrieve user in identity
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName); //// added for ASPNET Identity
            // check password in idenity
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            //add check for isValid
            if (user == null || isValid == false)
            {
                LoginResponseDTO emptyloginResponseDTO = new LoginResponseDTO()
                {
                    Token = "",
                    //User = null
                    AspnetUser = null // added for aspnet identity
                };

                return emptyloginResponseDTO;
            }



            //get roles from identity
            var roles = await _userManager.GetRolesAsync(user);

            //if user was found generate JWT Token

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),

                 //   new Claim(ClaimTypes.Role, user.Role.ToString()),  //deprecated localuser homebrew
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()), //added for identity assume only 1 role per user else need for each loop to assign
                   
                    //add more claims here
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                //  User = user, 
                // initially need to add a mapping injection else get Error   CS0029  Cannot implicitly convert type 'MagicVilla_VillaAPI.Models.ApplicationUser' to 'MagicVilla_VillaAPI.Models.Dto.UserDTO' MagicVilla_VillaAPI C:\Users\cxrf\source\repos\ud\restapinnet7cg\MagicVilla\MagicVilla_VillaAPI\Repository\UserRepository.cs    99  Active
                AspnetUser = _mapper.Map<UserDTO>(user), // added for aspnet identity management
                Role = roles.FirstOrDefault()  // added for aspnet identity management
            };




            return loginResponseDTO;


        }

        // public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO) // new return type for aspnet identity
        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)

        {
            /*  //code pre aspnet identity mgmt
            LocalUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
                Name = registrationRequestDTO.Name,
                Password = registrationRequestDTO.Password,
                Role  = registrationRequestDTO.Role
            };
            */

            ApplicationUser registerUser = new()
            {
                UserName = registrationRequestDTO.UserName,
                Name = registrationRequestDTO.Name,
                Email = registrationRequestDTO.UserName,
                NormalizedEmail = registrationRequestDTO.UserName.ToUpper()
            };

            try
            {
                var result = await _userManager.CreateAsync(registerUser, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    bool adminRoleExists = await _roleManager.RoleExistsAsync("admin");

                    if (!adminRoleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("user"));
                        await _roleManager.CreateAsync(new IdentityRole("CUSTOM"));
                    }
                    await _userManager.AddToRoleAsync(registerUser, "admin");

                    var userToReturn = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == registrationRequestDTO.UserName);
                   // var userToReturn = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == registerUser.UserName);

                    /* //do not need if using automapper
                    return new UserDTO()
                    {
                        UserName = userToReturn.Result.UserName,
                        Name = userToReturn.Result.Name,
                        ID = userToReturn.Result.Id
                    };*/

                    UserDTO newUser = _mapper.Map<UserDTO>(userToReturn);
                    return newUser;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new UserDTO();
            }

            /* //code pre aspnet identity mgmt
            string hash = "blah blah";

            string HashedPass = await Task.Run(() => GenerateSecret.HashPassword(hash));

           //var HashedPass2 = await Task.Run((GenerateSecret.HashPassword(hash));

            bool validateTrue = GenerateSecret.ValidatePassword(hash, HashedPass);
            */

            /* //code pre aspnet identity mgmt
            _db.LocalUsers.Add(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;
               */

            return new UserDTO();

        }
    }
}
