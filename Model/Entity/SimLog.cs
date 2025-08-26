using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalAttemptWPF.Model.Entity
{
    public class SimLog
    { 
        public double Temp { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime MonitoredTime { get; set; }
        public int DeviceNum { get; set; }
        public int RTT { get; set; }
    }
}
