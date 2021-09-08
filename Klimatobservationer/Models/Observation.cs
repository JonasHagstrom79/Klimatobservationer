using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimatobservationer
{
    public class Observation
    {
        public int ObservationId { get; set; }

        public DateTime Date { get; set; }

        public int ObserverId { get; set; } 

        public int GeolocationId { get; set; }

        
        public override string ToString()
        {            
            if (GeolocationId == 1)
            {
                return $"Plats: Sverigeparken datum:{Date} GeoId:{GeolocationId} Id:{ObservationId}";
            }
            if (GeolocationId == 2)
            {
                return $"Plats: Norgeparken datum:{Date} GeoId:{GeolocationId} Id:{ObservationId}";
            }
            if (GeolocationId == 3)
            {
                return $"Plats: Finlandparken datum:{Date} GeoId:{GeolocationId} Id:{ObservationId}";
            }
            else return ToString();
        }

    }
}
