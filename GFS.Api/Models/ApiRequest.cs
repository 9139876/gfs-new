using Newtonsoft.Json;

namespace GFS.Api.Models
{
    /// <summary>
    /// Wrapper for api request
    /// </summary>
    /// <typeparam name="T">Type of request model</typeparam>
    public class ApiRequest<T> : ApiEmptyRequest
    {
        /// <summary>
        /// Сокрытие конструктора. Для создания экземпляра нужно пользоваться статическими методами.
        /// </summary>
        protected ApiRequest()
        {
        }

        [JsonProperty] 
        public T Payload { get; private set; } = default!;

        public static ApiRequest<T> CreateRequest(T payload)
            => new() { Payload = payload };
    }
}