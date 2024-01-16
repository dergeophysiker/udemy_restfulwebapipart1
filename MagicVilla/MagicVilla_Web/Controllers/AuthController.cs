using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;

using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicVilla_Web.Controllers
{
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
                LoginResponseDTO loggedinLoginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));
                HttpContext.Session.SetString(SD.SessionTokenKeyName, loggedinLoginResponseDTO.Token);
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

            return View();
            
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();

        }


    }
}
