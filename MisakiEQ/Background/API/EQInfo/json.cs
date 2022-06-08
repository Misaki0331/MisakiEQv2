using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE1006 // 命名スタイル

namespace MisakiEQ.Background.API.EQInfo.JSON
{
    public class Hypocenter
    {
        public int depth { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double magnitude { get; set; }
        public string name { get; set; } = string.Empty;
    }

    public class Earthquake
    {
        public string domesticTsunami { get; set; } = string.Empty;
        public string foreignTsunami { get; set; } = string.Empty;
        public Hypocenter hypocenter { get; set; } = new();
        public int maxScale { get; set; }
        public string time { get; set; } = string.Empty;
    }

    public class Issue
    {
        public string correct { get; set; } = string.Empty;
        public string source { get; set; } = string.Empty;
        public string time { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
    }

    public class Point
    {
        public string addr { get; set; } = string.Empty;
        public bool isArea { get; set; }
        public string pref { get; set; } = string.Empty;
        public int scale { get; set; }
    }

    public class Area
    {
        public string grade { get; set; } = string.Empty;
        public bool immediate { get; set; } = false;
        public string name { get; set; } = string.Empty;
    }
    public class Root
    {
        public List<Area> areas { get; set; } = new();
        public bool? cancelled { get; set; }
        public int code { get; set; }
        public string created_at { get; set; } = string.Empty;
        public Earthquake? earthquake { get; set; } = new();
        public string id { get; set; } = string.Empty;
        public Issue issue { get; set; } = new();
        public List<Point> points { get; set; } = new();
        public string time { get; set; } = string.Empty;

        [JsonProperty("user-agent")]
        public string UserAgent { get; set; } = string.Empty;
        public string ver { get; set; } = string.Empty;
    }
}
