namespace Demo.Utility
{
    using Newtonsoft.Json;

    public enum ApiResponseStatus
    {
        [JsonProperty("ok")]
        Ok,

        [JsonProperty("error")]
        Error
    }
}