﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaNumberUrl;
        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(clientFactory, httpContextAccessor)
        {
            _clientFactory = clientFactory;
            villaNumberUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

        }


        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)
        {

            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaNumberUrl + $"/api/{SD.TargetApiVersion}/villaNumberAPI"
           
            }) ;
        }

        public Task<T> DeleteAsync<T>(int id)
        {
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = villaNumberUrl + $"/api/{SD.TargetApiVersion}/VillaNumberAPI/" + id
                   
			});
		}

        public Task<T> GetAllAsync<T>()
        {
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = villaNumberUrl + $"/api/{SD.TargetApiVersion}/VillaNumberAPI"

            });
		}

        public Task<T> GetAsync<T>(int id)
        {
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = villaNumberUrl + $"/api/{SD.TargetApiVersion}/VillaNumberAPI/" + id
                
			});
		}

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
        {
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.PUT,
				Data = dto,
				Url = villaNumberUrl + $"/api/{SD.TargetApiVersion}/VillaNumberAPI/" + dto.VillaNo
                   
			});
		}
    }
}
