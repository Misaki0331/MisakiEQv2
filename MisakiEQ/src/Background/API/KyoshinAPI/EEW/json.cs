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
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("is_auth")]
        public bool IsAuth { get; set; } = false;
    }

    public class Security
    {
        [JsonPropertyName("realm")]
        public string Realm { get; set; } = string.Empty;

        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;
    }

    public class Root
    {
        [JsonPropertyName("result")]
        public Result Result { get; set; } = new();

        [JsonPropertyName("report_time")]
        public string ReportTime { get; set; } = string.Empty;

        [JsonPropertyName("region_code")]
        public string RegionCode { get; set; } = string.Empty;

        [JsonPropertyName("request_time")]
        public string RequestTime { get; set; } = string.Empty;

        [JsonPropertyName("region_name")]
        public string RegionName { get; set; } = string.Empty;

        [JsonPropertyName("longitude")]
        public string Longitude { get; set; } = string.Empty;

        [JsonPropertyName("is_cancel")]
        public bool IsCancel { get; set; } = false;

        [JsonPropertyName("depth")]
        public string Depth { get; set; } = string.Empty;

        [JsonPropertyName("calcintensity")]
        public string Calcintensity { get; set; } = string.Empty;

        [JsonPropertyName("is_final")]
        public bool IsFinal { get; set; } = false;

        [JsonPropertyName("is_training")]
        public bool IsTraining { get; set; } = false;

        [JsonPropertyName("latitude")]
        public string Latitude { get; set; } = string.Empty;

        [JsonPropertyName("origin_time")]
        public string OriginTime { get; set; } = string.Empty;

        [JsonPropertyName("security")]
        public Security Security { get; set; } = new();

        [JsonPropertyName("magunitude")]
        public string Magunitude { get; set; } = string.Empty;

        [JsonPropertyName("report_num")]
        public string ReportNum { get; set; } = string.Empty;

        [JsonPropertyName("request_hypo_type")]
        public string RequestHypoType { get; set; } = string.Empty;

        [JsonPropertyName("report_id")]
        public string ReportId { get; set; } = string.Empty;

        [JsonPropertyName("alertflg")]
        public string? Alertflg { get; set; } = null;
    }

}