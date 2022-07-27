using MisakiEQ.Struct;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib.PrefecturesAPI
{
    internal class API
    {
        public static async Task<PrefData> GetReverseGeo(Common.LAL lal)
        {
            try
            {
                var raw = await WebAPI.GetString($"https://revgeo-forecastcode.herokuapp.com/lat={lal.Lat:0.0000}+lon={lal.Lon:0.0000}");
                var json = JsonConvert.DeserializeObject<JSON>(raw);
                if (json == null) return new PrefData(Common.Prefectures.Unknown, string.Empty);
                var pref=Common.StringToPrefectures(json.prefname);
                string city = json.prefname + json.cityname;
                return new PrefData(pref, city);
            }catch(Exception ex)
            {
                Log.Instance.Error(ex); 
                return new PrefData(Common.Prefectures.Unknown, string.Empty);
            }
        }
    }

    public class PrefData
    {

        public Struct.Common.Prefectures Prefectures=Struct.Common.Prefectures.Unknown;
        public string Prefcity = string.Empty;

        public PrefData(Common.Prefectures prefectures, string prefcity)
        {
            Prefectures = prefectures;
            Prefcity = prefcity;
        }
    }
}
