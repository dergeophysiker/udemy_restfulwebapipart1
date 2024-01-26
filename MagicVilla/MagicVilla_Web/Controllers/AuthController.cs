using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;

using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace MagicVilla_Web.Controllers
{
  //  [Authorize(Roles ="administrator")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO defaultLoginRequestDTO = new LoginRequestDTO();
            return View(defaultLoginRequestDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO postLoginRequestDTO)
        {
            APIResponse response =  await _authService.LoginAsync<APIResponse>(postLoginRequestDTO);

            if (response != null && response.IsSuccess)
            {
               // get response from login endpoint and deserialize, set session information and redirect
               
                Object obj = JsonConvert.DeserializeObject(Convert.ToString(response.Result));

                //deserialization is not case sensitive
                LoginResponseDTO loggedinLoginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));
                



                var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(loggedinLoginResponseDTO.Token);


                //https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-7.0

                //this ensures httpContext knows the user is "logged in"
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
               // identity.AddClaim(new Claim(ClaimTypes.Name, loggedinLoginResponseDTO.aspnetUser.Name)); //deprecated because you can use token
                
                
                //use token to get role and name claims instead of directly from the model
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));


                // identity.AddClaim(new Claim(ClaimTypes.Role, loggedinLoginResponseDTO.User.Role)); //can pass in array of roles and loop
                var principal = new ClaimsPrincipal(identity);
                // signs in users and adds claims that were configured
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString(SD.SessionTokenKeyName, loggedinLoginResponseDTO.Token);

                //checks token in storage
                Console.WriteLine(HttpContext.Session.TryGetValue(SD.SessionTokenKeyName, out byte[] outvalue));
                string value = System.Text.Encoding.UTF8.GetString(outvalue);

                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("Custom Error", response.ErrorMessages.FirstOrDefault());
                return View(postLoginRequestDTO);
            }



            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO obj)
        {
            APIResponse result= await _authService.RegisterAsync<APIResponse>(obj);

            if(result != null && result.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            // could add error handling


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {


            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionTokenKeyName, "");
            return RedirectToAction("Index", "Home");

            
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();

        }


    }
}
