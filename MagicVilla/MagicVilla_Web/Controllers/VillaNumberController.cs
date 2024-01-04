using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {

        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
        }


        public async  Task<IActionResult> IndexVillaNumber()
        {

            List<VillaNumberDTO> list = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess){
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
               
            }
            return View(list);
        }

        //////////////////////////////////////////////////
        ///

        public async Task<IActionResult> CreateVillaNumber()
        {

            var response = await _villaService.GetAllAsync<APIResponse>();
            List<VillaDTO>  list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            var data = new SelectList(list, "Id", "Name");
            ViewData["datalist"] = data;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateDTO model)
        {

                        var selectedIds = Request.Form["datalist"];
                        var number = model.VillaID;

               if (ModelState.IsValid)
               {
                 //  var responseObject = await _villaNumberService.CreateAsync<Object>(model);
                 //  var responseTest = await _villaNumberService.CreateAsync<APIResponse>(model);    
                   var response = await _villaNumberService.CreateAsync<APIResponse>(model);

                    if (response != null && response.IsSuccess)
                   {

                       return RedirectToAction(nameof(IndexVillaNumber));
                   }

                    Console.WriteLine(response);

               } 

           return RedirectToAction(nameof(CreateVillaNumber));
          //  return View(model);
        }
        public async Task<IActionResult> UpdateVillaNumber(int villaNumberId)
        {
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNumberId);
            if (response != null && response.IsSuccess)
            {

                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<VillaNumberUpdateDTO>(model));
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {

                    return RedirectToAction(nameof(IndexVillaNumber));
                }

            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteVilla(int villaNumberId)
        {
            // var response = await _villaService.DeleteAsync<APIResponse>(villaId); // delte immediately 1/2
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNumberId);

            if (response != null && response.IsSuccess)
            {
                //SendAsync
                // return RedirectToAction(nameof(IndexVilla)); // delte immediately 2/2
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                return View(model);

            }
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaNumberDTO model)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaID);
            if (response != null && response.IsSuccess)
            {
                //SendAsync
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            return NotFound();
        }





    }
}
