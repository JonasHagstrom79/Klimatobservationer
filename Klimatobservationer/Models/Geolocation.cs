using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimatobservationer
{
    public class Geolocation
    {
        public int GeolocationId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int AreaId { get; set; }
    }
}
