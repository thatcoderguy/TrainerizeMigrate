using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class Point
    {
        public int id { get; set; }
        public string date { get; set; }
        public double value { get; set; }
    }

    public class BodyWeightResponse
    {
        public string unit { get; set; }
        public string? goal { get; set; }
        public List<Point> points { get; set; }
    }
}
