using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MagicVilla_VillaAPI.Controllers
{
    //    [Route("api/[controller]")]
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {


        private readonly ApplicationDbContext _db;
        private readonly ILogging _logger;
        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;
        protected APIResponse _response;

        public VillaNumberAPIController(ILogging logger, ApplicationDbContext db, IMapper mapper, IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
            _dbVillaNumber = dbVillaNumber;
            this._response = new APIResponse();
            _dbVilla = dbVilla;

        }

        //####################################################################
        // GET: api/<VillaNumberAPIController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            // DEFAULT LOGGER _logger.LogInformation("Getting all villaNumberss");
            _logger.Log("custom getting all villaNumbers", "");
            //https://www.linkedin.com/pulse/difference-between-ienumerable-ilist-list-iqueryable-pawan-verma/
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties:"Villa");
               // IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync();

				_response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
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

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]


        //####################################################################
        // GET: api/<VillaNumberAPIController>
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            if (id == 0)
            {
                //DEFAULT LOGGER    _logger.LogError("Get villa error with id" + id);
                _logger.Log("Custom get villa error with id" + id, "error");
                _response.StatusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);
            }
              var villaNumber = await _dbVillaNumber.GetAsync((u => u.VillaNo == id), includeProperties: "Villa");

            if (villaNumber == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;

                return NotFound(_response);
            }
            else
            {

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
        }//end
         //####################################################################


        // POST api/<VillaNumberAPIController>
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<VillaDTO>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
             if (!ModelState.IsValid) //necessary if apiControllor is not included or if needing custom state
            {
                return BadRequest(ModelState);
            }
             if (await _dbVillaNumber.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
            {
                ModelState.AddModelError("customerror", "Villa Number already exists");
                return BadRequest(ModelState);
            }


            if (await _dbVilla.GetAsync(u => u.Id == createDTO.VillaID) == null)
            {
                //ModelState.AddModelError("customerror", "VillaID is INVALID!");
                // return BadRequest(ModelState);

                var errorMessage = "VillaNumber already exists.";
                ModelState.AddModelError("ErrorMessages", errorMessage);
                _response.Result = ModelState;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { errorMessage };
             
                return BadRequest(_response);

            }




            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            VillaNumber model = _mapper.Map<VillaNumber>(createDTO);

            await _dbVillaNumber.CreateAsync(model);
  
            _response.Result = _mapper.Map<VillaNumberDTO>(model);
            _response.StatusCode = HttpStatusCode.Created;


            return CreatedAtRoute("GetVillaNumber", new { id = model.VillaNo }, _response);


        }
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!



        // DELETE api/<VillaNumberAPIController>/5
        //####################################################################
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            // var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id); //no database logic
            // var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id); //no repo logic
            var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
            if (villaNumber == null)
            {
                return NotFound();
            }
            // VillaStore.villaList.Remove(villa); //no database logic
            //_db.Villas.Remove(villa); //no repo logic
            // await _db.SaveChangesAsync(); //no repo logic
            await _dbVillaNumber.RemoveAsync(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        //####################################################################

        //////////////////////////////////////////////////////////////////////
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {

            if (updateDTO == null || id != updateDTO.VillaNo)
            {
                return BadRequest("Either something is null or your Ids do not match");
            }

            if (await _dbVilla.GetAsync(u => u.Id == updateDTO.VillaID) == null)
            {
                ModelState.AddModelError("customerror", "VillaID is INVALID!");
                return BadRequest(ModelState);
            }

            VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);
            await _dbVillaNumber.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        //////////////////////////////////////////////////////////////////////

    }
}
