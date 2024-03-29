﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

//https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-a-json-file-in-asp-net-core


namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService 
    {

        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;
        public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(clientFactory, httpContextAccessor)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
            
        }
        public Task<T> CreateAsync<T>(VillaCreateDTO dto)
        {

            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data=dto,
                Url= villaUrl+ $"/api/{SD.TargetApiVersion}/VillaAPI"


            });

        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + $"/api/{SD.TargetApiVersion}/VillaAPI/" + id
              
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.TargetApiVersion}/VillaAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + $"/api/{SD.TargetApiVersion}/VillaAPI/" + id

            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + $"/api/{SD.TargetApiVersion}/VillaAPI/" + dto.Id
            });
        }
    }
}
