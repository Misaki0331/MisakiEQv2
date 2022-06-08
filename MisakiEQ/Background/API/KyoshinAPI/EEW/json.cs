using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MisakiEQ.Background.API.KyoshinAPI.EEW.JSON
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Result
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("is_auth")]
        public bool? IsAuth { get; set; }
    }

    public class Security
    {
        [JsonPropertyName("realm")]
        public string? Realm { get; set; }

        [JsonPropertyName("hash")]
        public string? Hash { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("result")]
        public Result? Result { get; set; }

        [JsonPropertyName("report_time")]
        public string? ReportTime { get; set; }

        [JsonPropertyName("region_code")]
        public string? RegionCode { get; set; }

        [JsonPropertyName("request_time")]
        public string? RequestTime { get; set; }

        [JsonPropertyName("region_name")]
        public string? RegionName { get; set; }

        [JsonPropertyName("longitude")]
        public string? Longitude { get; set; }

        [JsonPropertyName("is_cancel")]
        public bool? IsCancel { get; set; }

        [JsonPropertyName("depth")]
        public string? Depth { get; set; }

        [JsonPropertyName("calcintensity")]
        public string? Calcintensity { get; set; }

        [JsonPropertyName("is_final")]
        public bool? IsFinal { get; set; }

        [JsonPropertyName("is_training")]
        public bool? IsTraining { get; set; }

        [JsonPropertyName("latitude")]
        public string? Latitude { get; set; }

        [JsonPropertyName("origin_time")]
        public string? OriginTime { get; set; }

        [JsonPropertyName("security")]
        public Security? Security { get; set; }

        [JsonPropertyName("magunitude")]
        public string? Magunitude { get; set; }

        [JsonPropertyName("report_num")]
        public string? ReportNum { get; set; }

        [JsonPropertyName("request_hypo_type")]
        public string? RequestHypoType { get; set; }

        [JsonPropertyName("report_id")]
        public string? ReportId { get; set; }

        [JsonPropertyName("alertflg")]
        public string? Alertflg { get; set; }
    }

}