using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable IDE1006 // 命名スタイル

namespace MisakiEQ.Background.API.KyoshinAPI.WebService.Latest.JSON
{
    public class Result
    {
        public string status { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
    }

    public class Root
    {
        public Security security { get; set; } = new Security();
        public string latest_time { get; set; } = string.Empty;
        public string request_time { get; set; } = string.Empty;
        public Result result { get; set; } = new Result();
    }

    public class Security
    {
        public string realm { get; set; } = string.Empty;
        public string hash { get; set; } = string.Empty;
    }


}
