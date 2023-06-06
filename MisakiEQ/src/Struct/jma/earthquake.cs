using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MisakiEQ.Struct.jma.Json
{
    //Root myDeserializedClass = JsonSerializer.Deserialize<List<Root>>(myJsonResponse);
    public class City
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("maxi")]
        public string Maxi { get; set; } = string.Empty;
    }

    public class Int
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("maxi")]
        public string Maxi { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public List<City> City { get; set; } = new();
    }

    public class Root
    {
        [JsonPropertyName("ctt")]
        public string Ctt { get; set; } = string.Empty;

        [JsonPropertyName("eid")]
        public string Eid { get; set; } = string.Empty;

        [JsonPropertyName("rdt")]
        public DateTime Rdt { get; set; }= DateTime.MinValue;

        [JsonPropertyName("ttl")]
        public string Ttl { get; set; } = string.Empty;

        [JsonPropertyName("ift")]
        public string Ift { get; set; } = string.Empty;

        [JsonPropertyName("ser")]
        public object Ser { get; set; } = string.Empty;

        [JsonPropertyName("at")]
        public DateTime? At { get; set; } = DateTime.MinValue;

        [JsonPropertyName("anm")]
        public string Anm { get; set; } = string.Empty;

        [JsonPropertyName("acd")]
        public string Acd { get; set; } = string.Empty;

        [JsonPropertyName("cod")]
        public string Cod { get; set; } = string.Empty;

        [JsonPropertyName("mag")]
        public string Mag { get; set; } = string.Empty;

        [JsonPropertyName("maxi")]
        public string Maxi { get; set; } = string.Empty;

        [JsonPropertyName("int")]
        public List<Int> Int { get; set; } = new();

        [JsonPropertyName("json")]
        public string Json { get; set; } = string.Empty;

        [JsonPropertyName("en_ttl")]
        public string EnTtl { get; set; } = string.Empty;

        [JsonPropertyName("en_anm")]
        public string EnAnm { get; set; } = string.Empty;
    }


}
