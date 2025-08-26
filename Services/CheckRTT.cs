using FinalAttemptWPF.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalAttemptWPF.Services
{
    public class CheckRTT
    {
        
        public async Task<int> CalculateRTT(SimulatedData CurrentSim)
        {
            int rtt = 0;
            rtt =( CurrentSim.MonitoredTime - CurrentSim.CreatedTime).Microseconds;
             return rtt;
        }
    }
}
