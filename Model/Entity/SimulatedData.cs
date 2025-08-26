namespace FinalAttemptWPF.Model.Entity
{
    public class SimulatedData
    {

        public double Temp { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime MonitoredTime { get; set; }
        public Device DeviceNum { get; set; }
        public int RTT { get; set; }


    }
    public enum Device
    {
        Device1 = 1,
        Device2 = 2
    }
}

