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
                var raw = await WebAPI.GetString($"https://map.yahooapis.jp/geoapi/V1/reverseGeoCoder?lat={lal.Lat:0.0000}&lon={lal.Lon:0.0000}&output=json&appid={Resources.API.API.YahooAPI}");
                var json = JsonConvert.DeserializeObject<JSON>(raw);
                if (json == null) return new PrefData(Common.Prefectures.Unknown, string.Empty);
                if (json.Feature.Count == 0)
                {
                    return new PrefData(Common.Prefectures.Unknown, string.Empty);
                }
                var pref = Common.StringToPrefectures(json.Feature[0].Property.AddressElement[0].Name);
                string city = json.Feature[0].Property.Address;
                return new PrefData(pref, city);
            }catch(Exception ex)
            {
                Log.Error(ex); 
                return new PrefData(Common.Prefectures.Unknown, string.Empty);
            }
        }
    }

    public class PrefData
    {

        public Common.Prefectures Prefectures = Common.Prefectures.Unknown;
        public string Prefcity = string.Empty;

        public PrefData(Common.Prefectures prefectures, string prefcity)
        {
            Prefectures = prefectures;
            Prefcity = prefcity;
        }
    }
}
