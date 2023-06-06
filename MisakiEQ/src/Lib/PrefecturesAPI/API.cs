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
                //https://map.yahooapis.jp/geoapi/V1/reverseGeoCoder?lat=35.7244&lon=139.568&output=json&appid=dj00aiZpPTN1Z0hiNjZJYmwzSiZzPWNvbnN1bWVyc2VjcmV0Jng9Zjg-
                var raw = await WebAPI.GetString($"https://map.yahooapis.jp/geoapi/V1/reverseGeoCoder?lat={lal.Lat:0.0000}&lon={lal.Lon:0.0000}&output=json&appid={Resources.API.API.YahooAPI}");
                var json = JsonConvert.DeserializeObject<PrefecturesAPI.JSON>(raw);
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
