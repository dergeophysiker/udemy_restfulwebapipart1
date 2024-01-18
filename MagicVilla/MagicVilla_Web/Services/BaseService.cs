using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {

        public IHttpClientFactory httpClient { get; set; }
        public APIResponse responseModel { get; set; }

        //public IHttpContextAccessor _httpContextAccessor;
        public ITokenService _tokenService { get; set; }

         public IHttpContextAccessor _httpContextAccessor;


        string sessionToken = "";

        public BaseService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
            this._httpContextAccessor = httpContextAccessor;
            //  this._tokenService = tokenService;
            
           
        }


        public void About([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            sessionToken = httpContextAccessor.HttpContext.Session.GetString(SD.SessionTokenKeyName);
          
        }




        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                    
                }

                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }

                HttpResponseMessage apiResponseSend = null;

                /* could possible add this in the constructor? */

                // begin access httpcontext and find and set token if available
               
                try
                {

                    //sessionToken = this._tokenService.TokenValue;
                    About(_httpContextAccessor);
                    Console.Write(sessionToken);

                    sessionToken = _httpContextAccessor.HttpContext.Session.GetString(SD.SessionTokenKeyName);
                }
           
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                


                if(!string.IsNullOrEmpty(sessionToken))
                {
                    apiRequest.Token = sessionToken;
                }
                //end access


                // begin add token to header

                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }
                //end add token to header

                apiResponseSend = await client.SendAsync(message);
                var apiContent = await apiResponseSend.Content.ReadAsStringAsync();

                try
                {
                    APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                   
                    if(apiResponseSend.StatusCode==System.Net.HttpStatusCode.BadRequest || apiResponseSend.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ApiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        ApiResponse.IsSuccess = false;

                        return (T)Convert.ChangeType(ApiResponse, typeof(T));

                       // var res = JsonConvert.SerializeObject(ApiResponse);
                       // var returnObj = JsonConvert.DeserializeObject<T>(res);
                       // return returnObj;
                    }

                }
                catch (Exception e)
                {

                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;

                }

                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;



            }

           
            catch (Exception ex)
            {

                var dto = new APIResponse
                {
                    ErrorMessages = new List<string>() { ex.Message },
                    IsSuccess = false
                };
                var res= JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }

        }
    }
}
