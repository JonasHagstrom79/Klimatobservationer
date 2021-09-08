using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimatobservationer//.Measurement
{
    public class Measurement
    {
        public int MeasurementId { get; set; }

        public double? Value { get; set; } 

        public int ObservationId { get; set; }

        public int CategoryId { get; set; }

        public override string ToString()
        {
            if (CategoryId == 8)
            {
                return "Lufttemperatur:" + " " + Value.ToString() + " " + "Grader Celsius";
            }
            if (CategoryId == 10)
            {
                return "Fjällräv:" + " " + Value.ToString() + " " + "Stycken";
            }
            if (CategoryId == 9 )
            {
                return "Fjällbjörk:" + " " + Value.ToString() + " " + "Stycken";
            }
            if (CategoryId == 11)
            {
                return "Fjällripa:" + " " + Value.ToString() + "Stycken";
            }
            else return Value.ToString();
            
            
        }

    }
}
