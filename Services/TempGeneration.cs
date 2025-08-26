using FinalAttemptWPF.Model.Entity;
using System.Threading.Channels;

namespace FinalAttemptWPF.Services
{
    public class TempGeneration
    {
        private double Temp;
        private DateTime TimeCreated;
        private Random _rand = new Random();
        private SimulatedData SimData;
        public Channel<SimulatedData> channel1;
        public Channel<SimulatedData> channel2;

        public TempGeneration()
        {
            channel1 = Channel.CreateUnbounded<SimulatedData>();
            channel2 = Channel.CreateUnbounded<SimulatedData>();

        }

        public async Task TempGenerator(Device DeviceNum)
        {
            var Setter = new DataSetter();

            //Creates a random temp between 40 and 80
            Temp = _rand.NextDouble() * (80 - 40) + 40; // added +40 so range is 40–80
            Temp = Math.Round(Temp, 2);
            TimeCreated = DateTime.Now;

            //Creates an object for each temp
            SimData = await Setter.SetData(Temp, TimeCreated, DeviceNum);

            //Puts the object inside the channel
            if (DeviceNum == Device.Device1)
            {
                await channel1.Writer.WriteAsync(SimData);
            }
            else if (DeviceNum == Device.Device2) 
            {
                await channel2.Writer.WriteAsync(SimData);
            }
        }
    }
}
