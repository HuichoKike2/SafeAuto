using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeAuto.Common.Modelos
{
    public class DriverTripRpt
    {
        public Driver driver { get; set; }
        public List<Trip> lstTrips {get;set;}
    }
}
