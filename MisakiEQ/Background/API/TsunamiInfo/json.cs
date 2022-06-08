using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE1006 // 命名スタイル

namespace MisakiEQ.Background.API.TsunamiInfo.JSON
{
    public class Area
    {
        public string? grade { get; set; }
        public bool? immediate { get; set; }
        public string? name { get; set; }
    }

    public class Issue
    {
        public string? source { get; set; }
        public string? time { get; set; }
        public string? type { get; set; }
    }

    public class Root
    {
        public List<Area>? areas { get; set; }
        public bool? cancelled { get; set; }
        public int? code { get; set; }
        public string? created_at { get; set; }
        public string? id { get; set; }
        public Issue? issue { get; set; }
        public string? time { get; set; }

        [JsonProperty("user-agent")]
        public string? UserAgent { get; set; }
        public string? ver { get; set; }
    }
}
