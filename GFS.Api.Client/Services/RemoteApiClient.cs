using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GFS.Api.Client.Exceptions;
using GFS.Api.Enums;
using GFS.Api.Models;
using GFS.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GFS.Api.Client.Services
{
    public interface IRemoteApiClient
    {
        Task Call<TApiService>()
            where TApiService : ApiService;

        Task Call<TApiService, TRequestPayload>(TRequestPayload requestPayload)
            where TApiService : ApiServiceWithRequest<TRequestPayload>;

        Task<TResponsePayload> Call<TApiService, TResponsePayload>()
            where TApiService : ApiServiceWithResponse<TResponsePayload>;

        Task<TResponsePayload> Call<TApiService, TRequestPayload, TResponsePayload>(TRequestPayload requestPayload)
            where TApiService : ApiServiceWithRequestResponse<TRequestPayload, TResponsePayload>;
    }

    internal class RemoteApiClient : IRemoteApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public RemoteApiClient(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> {new StringEnumConverter()},
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public async Task Call<TApiService>()
            where TApiService : ApiService
        {
            var requestJson = JsonConvert.SerializeObject(ApiEmptyRequest.CreateRequest(), _jsonSerializerSettings);
            var httpResponse = await GetRemoteResponse<ApiEmptyRequest>(GetUri(typeof(TApiService)), requestJson);
            await ProcessResponseMessageWithoutResponseAsync(httpResponse);
        }

        public async Task Call<TApiService, TRequestPayload>(TRequestPayload requestPayload)
            where TApiService : ApiServiceWithRequest<TRequestPayload>
        {
            var requestJson = JsonConvert.SerializeObject(ApiRequest<TRequestPayload>.CreateRequest(requestPayload), _jsonSerializerSettings);
            var httpResponse = await GetRemoteResponse<TRequestPayload>(GetUri(typeof(TApiService)), requestJson);
            await ProcessResponseMessageWithoutResponseAsync(httpResponse);
        }

        public async Task<TResponsePayload> Call<TApiService, TResponsePayload>()
            where TApiService : ApiServiceWithResponse<TResponsePayload>
        {
            var requestJson = JsonConvert.SerializeObject(ApiEmptyRequest.CreateRequest(), _jsonSerializerSettings);
            var httpResponse = await GetRemoteResponse<ApiEmptyRequest>(GetUri(typeof(TApiService)), requestJson);
            return await ProcessResponseMessageWithResponseAsync<ApiResponse<TResponsePayload>, TResponsePayload>(httpResponse);
        }

        public async Task<TResponsePayload> Call<TApiService, TRequestPayload, TResponsePayload>(TRequestPayload requestPayload)
            where TApiService : ApiServiceWithRequestResponse<TRequestPayload, TResponsePayload>
        {
            var requestJson = JsonConvert.SerializeObject(ApiRequest<TRequestPayload>.CreateRequest(requestPayload), _jsonSerializerSettings);
            var httpResponse = await GetRemoteResponse<TRequestPayload>(GetUri(typeof(TApiService)), requestJson);
            return await ProcessResponseMessageWithResponseAsync<ApiResponse<TResponsePayload>, TResponsePayload>(httpResponse);
        }

        private Uri GetUri(Type apiServiceType)
        {
            var schemeAndHost = _configuration.GetSection($"RemoteApis:{apiServiceType.Namespace}").Value ?? throw new InvalidOperationException();

            var path = (apiServiceType.GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute)?.Template ?? throw new InvalidOperationException();
            path += $"/{ApiServiceParameters.PATH}";

            return new Uri(new Uri(schemeAndHost), path);
        }

        private async Task<HttpResponseMessage> GetRemoteResponse<TRequestPayload>(Uri requestUri, string requestJson)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            
            var httpRequest = new HttpRequestMessage();
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Method = HttpMethod.Post;
            httpRequest.RequestUri = requestUri;
            httpRequest.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            return await httpClient.SendAsync(httpRequest);
        }

        private async Task<TResponsePayload> ProcessResponseMessageWithResponseAsync<TResult, TResponsePayload>(HttpResponseMessage responseMessage)
            where TResult : ApiResponse<TResponsePayload>
        {
            var responsePayload = await GetResponsePayload<ApiResponse<TResponsePayload>>(responseMessage);

            if (responsePayload.IsSuccess)
                return responsePayload.Payload!;

            throw RemoteCallException.Create(responsePayload.Exception);
        }

        private async Task ProcessResponseMessageWithoutResponseAsync(HttpResponseMessage responseMessage)
        {
            var responsePayload = await GetResponsePayload<ApiEmptyResponse>(responseMessage);

            if (responsePayload.IsSuccess)
                return;

            throw RemoteCallException.Create(responsePayload.Exception);
        }

        private async Task<TResponsePayload> GetResponsePayload<TResponsePayload>(HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == HttpStatusCode.NoContent || responseMessage?.Content == null)
                throw new InvalidOperationException("Wrong response type - NoContent");

            var responseString = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.StatusCode != HttpStatusCode.OK)
                throw RemoteCallException.Create(ApiExceptionsTypeEnum.RemoteServerError, $"Status Code {responseMessage.StatusCode}, {responseString}");

            var responsePayload = JsonConvert.DeserializeObject<TResponsePayload>(responseString, _jsonSerializerSettings) ??
                                  throw new InvalidOperationException($"Response payload deserialization failed - {responseString}");

            return responsePayload;
        }
    }
}