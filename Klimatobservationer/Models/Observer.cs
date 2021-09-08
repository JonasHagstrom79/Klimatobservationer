using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimatobservationer
{
    public class Observer
    {
        public int ObserverId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
