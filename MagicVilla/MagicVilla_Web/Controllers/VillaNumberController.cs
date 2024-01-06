using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
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

            /* view data approach 
            var data = new SelectList(list, "Id", "Name");
            ViewData["datalist"] = data; */

            /* view model approach */
            VillaNumberCreateVM villaNumberVM = new();
            villaNumberVM.VillaList = list.Select(i => new SelectListItem { Text= i.Name, Value=i.Id.ToString() });

            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateDTO model)
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {

            // var selectedIds = Request.Form["datalist"];
                        var number = model.VillaNumber.VillaID;

               if (ModelState.IsValid)
               {
                 //  var responseObject = await _villaNumberService.CreateAsync<Object>(model);
                 //  var responseTest = await _villaNumberService.CreateAsync<APIResponse>(model);    
                   var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);

                    if (response != null && response.IsSuccess)
                   {

                       return RedirectToAction(nameof(IndexVillaNumber));
                   }
                    else
                {
                    if(response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
                    Console.WriteLine(response);

               }

            var responseSelect = await _villaService.GetAllAsync<APIResponse>();
            List<VillaDTO> list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(responseSelect.Result));
            model.VillaList = list.Select(i => new SelectListItem { Text = i.Name, Value = i.Id.ToString() });

            return View(model);
           //return RedirectToAction(nameof(CreateVillaNumber));
          //  return View(model);
        }
        public async Task<IActionResult> UpdateVillaNumber(int villaNumberId)
        {


            var select_response = await _villaService.GetAllAsync<APIResponse>();
            List<VillaDTO> list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(select_response.Result));
            var data = new SelectList(list, "Id", "Name");
            ViewData["datalist"] = data;

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


                var selectedIds = Request.Form["datalist"];
                var number = model.VillaID;

                var response = await _villaNumberService.UpdateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {

                    return RedirectToAction(nameof(IndexVillaNumber));
                }

            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteVillaNumber(int villaNumberId)
        {
            VillaNumberDeleteVM model = new();

            // var response = await _villaService.DeleteAsync<APIResponse>(villaId); // delte immediately 1/2
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNumberId);

            if (response != null && response.IsSuccess)
            {
                //SendAsync
                // return RedirectToAction(nameof(IndexVilla)); // delte immediately 2/2
                // VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                model.VillaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));

                return View(model);

            }
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo);
            if (response != null && response.IsSuccess)
            {
                //SendAsync
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            return NotFound();
        }





    }
}
