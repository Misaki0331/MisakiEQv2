using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Lib.PrefecturesAPI
{
    public class AddressElement
    {
        public string Name { get; set; } = string.Empty;
        public string Kana { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class Country
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class Feature
    {
        public Geometry Geometry { get; set; } = new();
        public Property Property { get; set; } = new();
    }

    public class Geometry
    {
        public string Type { get; set; } = string.Empty;
        public string Coordinates { get; set; } = string.Empty;
    }

    public class Property
    {
        public Country Country { get; set; } = new();
        public string Address { get; set; } = string.Empty;
        public List<AddressElement> AddressElement { get; set; } = new();
    }

    public class ResultInfo
    {
        public int Count { get; set; } = 0;
        public int Total { get; set; } = 0;
        public int Start { get; set; } = 0;
        public double Latency { get; set; } = double.NaN;
        public int Status { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public string CompressType { get; set; } = string.Empty;
    }

    public class JSON
    {
        public ResultInfo ResultInfo { get; set; } = new();
        public List<Feature> Feature { get; set; } = new();
    }

}
