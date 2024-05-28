using System.Text.Json;
using MiioNet8.Commands;
using System.Text.Json.Serialization;

namespace MiioNet8.Responses
{
    internal abstract class GetPropertiesResponseBase<T> : BaseResponse
    {
        [JsonPropertyName("result")]
        public List<T> Result { get; set; } = new();
    }

    internal class GetRawPropertiesResponse : GetPropertiesResponseBase<JsonElement>
    {
    }

    internal class GetPropertiesResponse : GetPropertiesResponseBase<Property>
    {
    }
}