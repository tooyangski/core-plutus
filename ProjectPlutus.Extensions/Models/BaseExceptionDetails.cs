using Newtonsoft.Json;

namespace ProjectPlutus.Extensions.Models
{
    public class BaseExceptionDetails
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DevExceptionDetails : BaseExceptionDetails
    {
        [JsonProperty("stackTrace", Order = 3)]
        public string Stacktrace { get; set; }
    }
}
