using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {

                list= JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }

             return View(list);
        }

        //https://learn.microsoft.com/en-us/aspnet/mvc/overview/performance/using-asynchronous-methods-in-aspnet-mvc-4
        public async Task<IActionResult> CreateVilla()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
		{
			if (ModelState.IsValid)
            {
				var response = await _villaService.CreateAsync<APIResponse>(model);
				if (response != null && response.IsSuccess)
				{

					return RedirectToAction(nameof(IndexVilla));
				}

			}

			return View(model);
		}
		public async Task<IActionResult> UpdateVilla(int villaId)
		{
			var response = await _villaService.GetAsync<APIResponse>(villaId);
			if (response != null && response.IsSuccess)
			{

				VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
				return View(_mapper.Map<VillaUpdateDTO>(model));
			}
			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
		{
			if (ModelState.IsValid)
			{
				var response = await _villaService.UpdateAsync<APIResponse>(model);
				if (response != null && response.IsSuccess)
				{

					return RedirectToAction(nameof(IndexVilla));
				}

			}

			return View(model);
		}
        [HttpGet]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            // var response = await _villaService.DeleteAsync<APIResponse>(villaId); // delte immediately 1/2
            var response = await _villaService.GetAsync<APIResponse>(villaId);

            if (response != null && response.IsSuccess)
            {
                //SendAsync
                // return RedirectToAction(nameof(IndexVilla)); // delte immediately 2/2
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(model);

            }
            return NotFound();
        }


        // [HttpDelete]
        //[HttpDelete("int:{id}")])
        //[HttpDelete("{villaId:int}", Name = "DeleteVilla")]
        //[ValidateAntiForgeryToken]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            var response = await _villaService.DeleteAsync<APIResponse>(model.Id);
            if (response != null && response.IsSuccess)
            {
                //SendAsync
                return RedirectToAction(nameof(IndexVilla));
            }
            return NotFound();
        }


    }
}
