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
        /// Сокрытие конструктора. Для созадния экземпляра нужно пользоваться статическими методами.
        /// </summary>
        protected ApiRequest() : base()
        {
        }
        
        [JsonProperty]
        public T Payload { get; private set; }

        public static ApiRequest<T> CreateRequest(T payload)
            => new ApiRequest<T>() {Payload = payload};
    }
}