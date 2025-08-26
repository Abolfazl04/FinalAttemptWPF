using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinalAttemptWPF.Model.Entity;
using FinalAttemptWPF.Services;
using System.Windows;
using System.Windows.Threading;


namespace FinalAttemptWPF.ViewModels
{
    public partial class DevicesViewModel : ObservableObject
    {
        public TempGeneration tempGeneration = new TempGeneration();

        public dbService DbService = new dbService();

        public CheckRTT CheckRTT = new CheckRTT();


        public int SampleRate1 = 10;

        public int SampleRate2 = 10;

        public bool Power1 = false;

        public bool Power2 = false;

        private int TransferChannelCounter = 0;

        private DispatcherTimer? _batchTimer;

        private CancellationTokenSource CancelLogging;

        [ObservableProperty]
        public string? currentTemp1 = null;

        [ObservableProperty]
        public string? currentTemp2 = null;

        [ObservableProperty]
        public bool rTTAlert1 = false;

        [ObservableProperty]
        public bool rTTAlert2 = false;

        [ObservableProperty]
        public string? rTTValue1;

        [ObservableProperty]
        public string? rTTValue2;

        [ObservableProperty]
        public int rTT_Threshold1 = 50;

        [ObservableProperty]
        public int rTT_Threshold2 = 50;



        [RelayCommand(AllowConcurrentExecutions = true)]
        public async Task StartSim(Device DeviceNum)

        {
            StartBatchTimer();

            if (DeviceNum == Device.Device1)
            {
                Power1 = !Power1;

                await GetSimTemp(DeviceNum);
            }
            else if (DeviceNum == Device.Device2)
            {
                Power2 = !Power2;

                await GetSimTemp(DeviceNum);
            }


        }
        public async Task GetSimTemp(Device DeviceNum)
        {
            SimulatedData simulatedData;

            while (true)
            {
                await tempGeneration.TempGenerator((DeviceNum));

                if (DeviceNum == Device.Device1)
                {
                    if (Power1)
                    {
                        simulatedData = await tempGeneration.channel1.Reader.ReadAsync();
                        CurrentTemp1 = simulatedData.Temp.ToString() + "°C";
                        simulatedData.MonitoredTime = DateTime.Now;
                        simulatedData.RTT = await CheckRTT.CalculateRTT(simulatedData);
                        DbService.TransferToDB.Writer.TryWrite(simulatedData);
                        TransferChannelCounter++;

                        RTTValue1 = simulatedData.RTT.ToString("F2");
                        RTTValue1 = $"{RTTValue1} ms";
                        if (simulatedData.RTT > RTT_Threshold1)
                        {
                            RTTAlert1 = true;
                        }
                        else
                        {
                            RTTAlert1 = false;
                        }

                        if (TransferChannelCounter == 100)
                        {
                            await DbService.SaveBatchToDb();
                            TransferChannelCounter = 0;
                        }
                        await Task.Delay(16);

                    }
                    else break;
                }
                else if (DeviceNum == Device.Device2)
                {
                    if (Power2)
                    {
                        simulatedData = await tempGeneration.channel2.Reader.ReadAsync();
                        CurrentTemp2 = simulatedData.Temp.ToString() + "°C";
                        simulatedData.MonitoredTime = DateTime.Now;
                        simulatedData.RTT = await CheckRTT.CalculateRTT(simulatedData);
                        DbService.TransferToDB.Writer.TryWrite(simulatedData);
                        TransferChannelCounter++;


                        RTTValue2 = simulatedData.RTT.ToString("F2");
                        RTTValue2 = $"{RTTValue2} ms";
                        if (simulatedData.RTT > RTT_Threshold2)
                        {
                            RTTAlert2 = true;
                        }
                        else
                        {
                            RTTAlert2 = false;
                        }
                        await Task.Delay(16);

                    }
                    else break;
                }
                else
                {
                    MessageBox.Show("Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }
        }
        public void StartBatchTimer()
        {
            _batchTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200) // flush every 200ms
            };

            _batchTimer.Tick += async (s, e) =>
            {
                // Save whatever is in the channel for both devices
                await DbService.SaveBatchToDb();
            };

            _batchTimer.Start();
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        public async Task OpenCLI()
        {

            ConsoleManager.AllocConsole();
            Console.WriteLine("Console Opened");
            Console.WriteLine("For Commands list type : help");

            await CLICommands();

        }
        private async Task CLICommands()
        {
            int DeviceNum = 0;
            string input;
            await Task.Run(async () =>
            {
                while (true)
                {
                    input = Console.ReadLine();

                    if (input != null && input.ToLower() == "exit")
                    {
                        Console.WriteLine("Closing Console...");
                        await Task.Delay(1500);
                        ConsoleManager.FreeConsole();
                        break;
                    }
                    else if (input != null && input.ToLower() == "help")
                    {
                        Console.WriteLine("Available Commands:");
                        Console.WriteLine("1. set sr [device number]:[value] - Set the sample rate of [device number] to [value] (Default is 10)");
                        Console.WriteLine("2. log [device number] - Start logging for the specified device (1 or 2) ");
                        Console.WriteLine("3. exit - Close the console");
                    }

                    else if (input.ToLower().StartsWith("set sr"))
                    {

                        string paramPart = input.Substring(6).Trim();
                        var parts = paramPart.Split(':');
                        try
                        {
                            if (
                            int.TryParse(parts[0], out int val1) &&
                            int.TryParse(parts[1], out int val2))
                            {
                                if (val2 <= 60 && val2 >= 1)
                                {
                                    if (val1 == 1)
                                    {
                                        SampleRate1 = val2;
                                        Console.WriteLine($"Device 1 Sample rate updated to {SampleRate1}");

                                    }
                                    else if (val1 == 2)
                                    {
                                        SampleRate2 = val2;
                                        Console.WriteLine($"Device 2 Sample rate updated to {SampleRate2}");

                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Device Number. Please enter 1 or 2.");
                                        continue;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Sample Rate. Please enter a value between 1 and 60.");
                                    continue;
                                }


                            }
                        }
                        catch (Exception)
                        {

                            Console.WriteLine($" '{input}' is not a valid command");
                        }
                        
                        

                    }

                    else if (input.ToLower().StartsWith("log"))
                    {
                        var parts = input.Split(' ');
                        if (int.TryParse(parts.Last(), out int val))
                        {
                            if (val != 1 && val != 2)
                            {
                                Console.WriteLine("Invalid Device Number. Please enter 1 or 2.");
                                continue;
                            }
                            DeviceNum = val;
                            Console.WriteLine($"Device {DeviceNum} Logging : ");
                        }
                        else Console.WriteLine($" '{input}' is not a valid command");
                        await RunLog(DeviceNum);
                    }
                    else
                    {
                    Console.WriteLine($" '{input}' is not a valid command");
                    }
                }

            });
        }
        private async Task RunLog(int DeviceNum)
        {
            await Task.Run(async () =>

            {
                if (DeviceNum == 1)
                {
                    if (Power1 == false)
                    {
                        Console.WriteLine("Sim is not running");
                    }
                    else
                    {
                        while (true)
                        {
                            if (Power1 == false)
                            {
                                Console.WriteLine("Sim stopped");
                                break;
                            }

                            Console.WriteLine($"Device 1 => Temp : {CurrentTemp1} , RTT : {RTTValue1}");
                            await Task.Delay(1000 / SampleRate1);
                        }
                    }
                }
                else if (DeviceNum == 2)
                {
                    if (Power2 == false)
                    {
                        Console.WriteLine("Sim is not running");
                    }
                    else
                    {
                        while (true)
                        {
                            if (Power2 == false)
                            {
                                Console.WriteLine("Sim stopped");
                                break;
                            }

                            Console.WriteLine($"Device 2 => Temp : {CurrentTemp2} , RTT : {RTTValue2}");
                            await Task.Delay(1000 / SampleRate2);
                        }
                    }
                }


            });
        }
    }
}
