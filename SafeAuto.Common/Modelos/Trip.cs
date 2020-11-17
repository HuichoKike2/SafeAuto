using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeAuto.Common.Modelos
{
    public class Trip
    {
        public Driver driver { get; set; }
        public TimeSpan initialTime { get; set; }
        public TimeSpan finalTime { get; set; }
        public double milles { get; set; }
        public int minutes { get; set; }
        public int velocity { get; set; }
    }
}
