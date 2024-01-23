using Asp.Versioning;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_VillaAPI.Controllers.v1
{

    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    // No [ApiController] then must check model state in the code, if apicontrollor is enalbed, then it will hit the actions after modelstate is valid

    public class VillaAPIController : ControllerBase
    {
        /* DEFAULT LOGGER private readonly ILogger<VillaAPIController> _logger;

         public VillaAPIController(ILogger<VillaAPIController> logger) {
             _logger = logger;
         }
         */
        private readonly ApplicationDbContext _db;
        private readonly ILogging _logger;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;
        protected APIResponse _response;

        //could remove applicationdbcontext
        public VillaAPIController(ILogging logger, ApplicationDbContext db, IMapper mapper, IVillaRepository dbVilla)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _dbVilla = dbVilla;
            _response = new APIResponse();

        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This gets all of the villas.
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ResponseCache(Duration =30)]
        [ResponseCache(CacheProfileName="Default30")]

        [HttpGet]

        public async Task<ActionResult<APIResponse>> GetVillas()
        {

            // DEFAULT LOGGER _logger.LogInformation("Getting all villas");
            _logger.Log("custom getting all villas", "");

            //https://www.linkedin.com/pulse/difference-between-ienumerable-ilist-list-iqueryable-pawan-verma/

            try
            {
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;

        }




        //return Ok(villaList); this is a list of Villa types which has not been mapped.

        //////////////////////////////////////////////////////////////////////


        //#################################################################### IGNORE
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("GetAllVillas")]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetAllVillas()
        {

            // DEFAULT LOGGER _logger.LogInformation("Getting all villas");
            _logger.Log("custom getting all villas", "");
            //   return Ok(VillaStore.villaList);

            //  IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

            //https://www.linkedin.com/pulse/difference-between-ienumerable-ilist-list-iqueryable-pawan-verma/

            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            //IEnumerable<Villa> villaList = await _dbVilla.GetAllSimple();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));

            //return Ok(villaList); this is a list of Villa types which has not been mapped.
        }
        //####################################################################


        //####################################################################
        [HttpGet("{id:int}", Name = "GetVilla")]
        // [ProducesResponseType(200,Type=typeof(VillaDTO))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
       // [ResponseCache(Duration = 30,Location =ResponseCacheLocation.None, NoStore =true)]
        [Authorize(Roles = "admin")]
        //public async Task<ActionResult<VillaDTO>> GetVilla(int id)


        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            if (id == 0)
            {
                //DEFAULT LOGGER    _logger.LogError("Get villa error with id" + id);
                _logger.Log("Custom get villa error with id" + id, "error");
                _response.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);
            }
            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id); // no database version
            //var villa =await _db.Villas.FirstOrDefaultAsync(u => u.Id == id); // no repository class version
            //// these are the IVillRepository calls
            var villa = await _dbVilla.GetAsync(u => u.Id == id);
            //var villa = await _dbVilla.GetOne($"u => u.Id == {id}"); //not a good approach because it would require building your own database access
            // return Redirect("Home/Contact");


            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;

                return NotFound(_response);
            }
            else
            {

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
        }//end
         //####################################################################

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            // he initially calls the argument villaDTO instead of createDTO which is confusing, he changes it later in the course
            // this never gets hit with [Apicontroller] is enabled
            if (!ModelState.IsValid) //necessary if apiControllor is not included or if needing custom state
            {
                return BadRequest(ModelState);
            }
            //if(VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null ) //no database version
            //            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null ) // no repository version
            if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("ErrorMessages", "villa already exists");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            //if(createDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //} //additional logic on checking create condition

            Villa model = _mapper.Map<Villa>(createDTO);
            /*Villa model = new()
            {
                Amenity = createDTO.Amenity,
                Details = createDTO.Details,
             //   Id = villaDTO.Id,
                ImageUrl = createDTO.ImageUrl,
                Name = createDTO.Name,
                Occupancy = createDTO.Occupancy,
                Rate = createDTO.Rate,
                Sqft = createDTO.Sqft
            };*/ //not needed if using mapper
            await _dbVilla.CreateAsync(model);
            //await _db.Villas.AddAsync(model); //not used when using repo 1/2
            // await _db.SaveChangesAsync();    //not used when using repo 2/2

            //   villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id+1; //non db version 1/3
            // VillaStore.villaList.Add(villaDTO); //non db version 2/3
            // return Ok(villaDTO); //non db version 3/3

            //            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO); old way without a separeate VillaCreateDTO

            _response.Result = _mapper.Map<VillaDTO>(model);
            _response.StatusCode = HttpStatusCode.Created;


            //return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
            return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);


        }
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //####################################################################
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "CUSTOM,admin")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            // var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id); //no database logic
            // var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id); //no repo logic
            var villa = await _dbVilla.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            // VillaStore.villaList.Remove(villa); //no database logic
            //_db.Villas.Remove(villa); //no repo logic
            // await _db.SaveChangesAsync(); //no repo logic
            await _dbVilla.RemoveAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        //####################################################################

        //////////////////////////////////////////////////////////////////////
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        //[HttpPut(Name = "UpdateVilla")]
        //could also remove int parameter and do it checking the list for the id then altering
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {

            // he initially calls the argument villaDTO instead of createDTO which is confusing because it differs from VillaDTO by a lowercase letter, he changes it later in the course
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
            /*  var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
              villa.Name = villaDTO.Name;
              villa.Sqft = villaDTO.Sqft;
              villa.Occupancy = villaDTO.Occupancy;
            */ // this is the non database version of this code

            Villa model = _mapper.Map<Villa>(updateDTO);
            /*Villa model = new()
            {
                Amenity = updateDTO.Amenity,
                Details = updateDTO.Details,
                Id = updateDTO.Id,
                ImageUrl = updateDTO.ImageUrl,
                Name = updateDTO.Name,
                Occupancy = updateDTO.Occupancy,
                Rate = updateDTO.Rate,
                Sqft = updateDTO.Sqft
            };*/ // only needed if not using mapper

            // _db.Villas.Update(model);// non-repo version 1/2
            // await _db.SaveChangesAsync(); // non-repo version 2/2
            await _dbVilla.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        //////////////////////////////////////////////////////////////////////

        //####################################################################
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            // var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id); // non-db non EF framework version
            // var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id); //non async EF version
            // var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);  // ALT1 non repo version
            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);
            VillaUpdateDTO villaDto = _mapper.Map<VillaUpdateDTO>(villa);
            // replace commented code below with mapper above
            /*VillaUpdateDTO villaDto = new()
            {
                Amenity = villa.Amenity,
                Details = villa.Details,
                Id = villa.Id,
                ImageUrl = villa.ImageUrl,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };*/
            if (villa == null)
            {
                return BadRequest();
            }
            // patchDTO.ApplyTo(villa, ModelState); on used for no eneity frameowrk version
            patchDTO.ApplyTo(villaDto, ModelState);
            Villa model = _mapper.Map<Villa>(villaDto);
            // replace commented code below with mapper above

            /*
            villa.Name = villaDto.Name;
            villa.Details = villaDto.Details;
            villa.Rate = villaDto.Rate;
            villa.Sqft = villaDto.Sqft;
            villa.Occupancy = villaDto.Occupancy;
            villa.ImageUrl = villaDto.ImageUrl;
            villa.Amenity = villaDto.Amenity;
            */ // needed if no mapper 1/2

            // _db.Villas.Update(villa); // needed if no mapper 2/2

            //_db.Villas.Update(model);  //non repo version 1/2
            //await _db.SaveChangesAsync(); //non repo version 2/2
            await _dbVilla.UpdateAsync(model);
            /*
                 Villa model = new()
                 {

                     Amenity = villaDTO.Amenity,
                     Details = villaDTO.Details,
                     Id = villaDTO.Id,
                     ImageUrl = villaDTO.ImageUrl,
                     Name = villaDTO.Name,
                     Occupancy = villaDTO.Occupancy,
                     Rate = villaDTO.Rate,
                     Sqft = villaDTO.Sqft
                 };

                 _db.Villas.Update(model);
                 _db.SaveChanges();
                 */  // possibly not needed, if used must use asNoTracking
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
        //####################################################################


        //####################################################################
        [HttpPatch("NoRepo/{id:int}", Name = "UpdatePartialVillaNoRepo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVillaNoRepo(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            // var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id); // non-db non EF framework version
            // var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id); //non async EF version
            // var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);  // ALT1 non repo version
            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);
            VillaUpdateDTO villaDto = _mapper.Map<VillaUpdateDTO>(villa);
            // replace commented code below with mapper above
            /*VillaUpdateDTO villaDto = new()
            {
                Amenity = villa.Amenity,
                Details = villa.Details,
                Id = villa.Id,
                ImageUrl = villa.ImageUrl,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };*/
            if (villa == null)
            {
                return BadRequest();
            }
            // patchDTO.ApplyTo(villa, ModelState); on used for no eneity frameowrk version
            patchDTO.ApplyTo(villaDto, ModelState);
            Villa model = _mapper.Map<Villa>(villaDto);
            // replace commented code below with mapper above

            /*
            villa.Name = villaDto.Name;
            villa.Details = villaDto.Details;
            villa.Rate = villaDto.Rate;
            villa.Sqft = villaDto.Sqft;
            villa.Occupancy = villaDto.Occupancy;
            villa.ImageUrl = villaDto.ImageUrl;
            villa.Amenity = villaDto.Amenity;
            */ // needed if no mapper 1/2

            // _db.Villas.Update(villa); // needed if no mapper 2/2

            //_db.Villas.Update(model);  //non repo version 1/2
            //await _db.SaveChangesAsync(); //non repo version 2/2
            await _dbVilla.UpdateAsync(model);
            /*
                 Villa model = new()
                 {

                     Amenity = villaDTO.Amenity,
                     Details = villaDTO.Details,
                     Id = villaDTO.Id,
                     ImageUrl = villaDTO.ImageUrl,
                     Name = villaDTO.Name,
                     Occupancy = villaDTO.Occupancy,
                     Rate = villaDTO.Rate,
                     Sqft = villaDTO.Sqft
                 };

                 _db.Villas.Update(model);
                 _db.SaveChanges();
                 */  // possibly not needed, if used must use asNoTracking
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok("Updated");
        }
        //####################################################################



    }
}
