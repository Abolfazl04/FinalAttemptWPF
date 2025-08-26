using FinalAttemptWPF.Model.Entity;

namespace FinalAttemptWPF.Services
{
    internal class DataSetter
    {
        private SimulatedData SimData = new SimulatedData();
        private List<SimulatedData> DeviceOneList = new List<SimulatedData>();
        private List<SimulatedData> DeviceTwoList = new List<SimulatedData>();
        public async Task<SimulatedData> SetData(double _Temp, DateTime _CreatedTime, Device _DeviceNum)
        {
            SimData.Temp = _Temp;
            SimData.CreatedTime = _CreatedTime;
            SimData.DeviceNum = _DeviceNum;
            if (SimData.DeviceNum == Device.Device1)
            {
                DeviceOneList.Add(SimData);
            }
            if (SimData.DeviceNum == Device.Device2)
            {
                DeviceTwoList.Add(SimData);
            }
            return SimData;
        }
    }
}
