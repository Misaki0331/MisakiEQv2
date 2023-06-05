using ABI.Windows.Media.Protection.PlayReady;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MisakiEQ.Struct.jma.Area.Static.EarthquakePos;

namespace MisakiEQ.Struct.jma.Area.Static
{
    public class EarthquakePos {
        static EarthquakePos? instance = null;
        public static EarthquakePos Instance { get
            {
                instance ??= new EarthquakePos();
                return instance;
            } 
        } 
        private EarthquakePos()
        {
            ObservePoint = new();
            InfomationCites = new();
            ForecastLocal = new();
            var data = @static.Resource.ObservationPoint.Split(Environment.NewLine);
            foreach (var item in data)
            {
                if (string.IsNullOrEmpty(item)) continue;
                var columns = item.Split(',');
                ObservePoint.Add(new(int.Parse(columns[0]), int.Parse(columns[1]), int.Parse(columns[2]), columns[3], columns[4]));
            }
            data = @static.Resource.AreaInformationCity.Split(Environment.NewLine);
            foreach (var item in data)
            {
                if (string.IsNullOrEmpty(item)) continue;
                var columns = item.Split(',');
                InfomationCites.Add(new(int.Parse(columns[0]), int.Parse(columns[1]), columns[2], columns[3]));
            }
            
            data = @static.Resource.AreaForecastLocalE.Split(Environment.NewLine);
            foreach (var item in data)
            {
                if (string.IsNullOrEmpty(item)) continue;
                var columns = item.Split(',');
                ForecastLocal.Add(new(int.Parse(columns[0]), columns[1], columns[2],InfomationCites));
            }

        } 
        public readonly List<ObservationPoint> ObservePoint;
        public readonly List<AreaInfomationCity> InfomationCites;
        public readonly List<AreaForecastLocalE> ForecastLocal;
        #region クラス類
        public class ObservationPoint
        {
            internal ObservationPoint(int areaForecastLocalCode, int areaInfomationCityCode, int observationCode, string observationName, string observationCallName)
            {
                LocalCode = areaForecastLocalCode;
                CityCode = areaInfomationCityCode;
                ObservationCode = observationCode;
                Name = observationName;
                CallName = observationCallName;
            }

            public int LocalCode { get; private set; }
            public int CityCode { get; private set; }
            public int ObservationCode { get; private set; }
            public string Name { get; private set; }
            public string CallName { get; private set; }
            public AreaForecastLocalE? Local { get => Instance.GetForecastLocal(LocalCode); }
            public AreaInfomationCity? City { get => Instance.GetInfomationCity(CityCode); }
            public Common.Prefectures Prefectures
            {
                get
                {
                    return (Common.Prefectures)(ObservationCode / 100000);
                }
            }
        }
        public class AreaInfomationCity
        {
            internal AreaInfomationCity(int areaForecastLocalCode, int areaInfomationCityCode, string areaInfomationCityName, string areaInfomationCityCallName)
            {
                LocalCode = areaForecastLocalCode;
                CityCode = areaInfomationCityCode;
                Name = areaInfomationCityName;
                CallName = areaInfomationCityCallName;
            }

            public int LocalCode { get; private set; }
            public AreaForecastLocalE? Local { get => Instance.GetForecastLocal(LocalCode); }
            public int CityCode { get; private set; }
            public string Name { get; private set; }
            public string CallName { get; private set; }
            public Common.Prefectures Prefectures
            {
                get
                {
                    return (Common.Prefectures)(CityCode / 100000);
                }
            }
        }
        public class AreaForecastLocalE
        {
            internal AreaForecastLocalE(int areaForecastLocalCode, string areaForecastLocalName, string areaForecastLocalCallName, List<AreaInfomationCity> InfomationCites)
            {
                LocalCode = areaForecastLocalCode;
                Name = areaForecastLocalName;
                CallName = areaForecastLocalCallName;
                Cities = InfomationCites.FindAll(a=>a.LocalCode== LocalCode);
            }
            public int LocalCode { get; private set; }
            public string Name { get; private set; }
            public string CallName { get; private set; }
            public List<AreaInfomationCity> Cities { get; private set; }
        }
        #endregion
        #region コンバーター
        public ObservationPoint? GetObservation(int ID)
        {
            return ObservePoint.Find(a=>a.ObservationCode == ID);
        }
        public ObservationPoint? GetObservation(string name)
        {
            return ObservePoint.Find(a => string.Equals(a.Name, name));
        }
        public AreaInfomationCity? GetInfomationCity(int ID)
        {
            return InfomationCites.Find(a => a.CityCode == ID);
        }
        public AreaInfomationCity? GetInfomationCity(string name)
        {
            return InfomationCites.Find(a => string.Equals(a.Name, name));
        }
        public AreaForecastLocalE? GetForecastLocal(int ID)
        {
            return ForecastLocal.Find(a => a.LocalCode == ID);
        }
        public AreaForecastLocalE? GetForecastLocal(string name)
        {
            return ForecastLocal.Find(a => string.Equals(a.Name, name));
        }
        #endregion
        public string GetForecastLocalName(int ID)
        {
            var search = GetForecastLocal(ID);
            if (search == null) return "";
            return search.Name;
        }
        public string GetInfomationCityName(int ID)
        {
            var search = GetInfomationCity(ID);
            if (search == null) return "";
            return search.Name;
        }
    }
}
