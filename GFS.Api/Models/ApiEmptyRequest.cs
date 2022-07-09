using System;
using Newtonsoft.Json;

namespace GFS.Api.Models
{
    /// <summary>
    /// Wrapper for empty api request
    /// </summary>
    public class ApiEmptyRequest
    {
        /// <summary>
        /// Сокрытие конструктора. Для созадния экземпляра нужно пользоваться статическими методами.
        /// </summary>
        protected ApiEmptyRequest()
        {
            TraceId = Guid.NewGuid();
        }

        [JsonProperty]
        public Guid TraceId{ get; private set; }
        
        public static ApiEmptyRequest CreateRequest()
            => new ApiEmptyRequest();
    }
}