using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib.PrefecturesAPI
{
    public class JSON
    {
        public string forecastcode { get; set; } = string.Empty;
        public string prefname { get; set; } = string.Empty;
        public string cityname { get; set; } = string.Empty;
        public string centers { get; set; } = string.Empty;
        public string offices { get; set; } = string.Empty;
        public string class10s { get; set; } = string.Empty;
        public string class15s { get; set; } = string.Empty;
        public string class20s { get; set; } = string.Empty;
    }
}
