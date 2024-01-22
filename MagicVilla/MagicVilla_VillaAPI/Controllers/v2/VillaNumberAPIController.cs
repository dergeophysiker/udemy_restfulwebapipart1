using Asp.Versioning;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MagicVilla_VillaAPI.Controllers.v2
{
    //    [Route("api/[controller]")]
    // [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]

    [ApiController]
    // [ApiVersion("1.0")]
    [ApiVersion("2.0")]


    public class VillaNumberAPIController : ControllerBase
    {



        public VillaNumberAPIController()
        {

        }


        [HttpGet]
        // [MapToApiVersion("2.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


    }
}
