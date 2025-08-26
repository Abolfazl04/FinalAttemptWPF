using FinalAttemptWPF.Model.Entity;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Channels;

namespace FinalAttemptWPF.Services
{
    public class dbService
    {
        private readonly string dbPath;
        private readonly string ExeFolder;
        public Channel<SimulatedData> TransferToDB = Channel.CreateUnbounded<SimulatedData>();
        public ObservableCollection<SimLog> SimLogs { get; set; } = new();

        public dbService()
        {
            ExeFolder = AppDomain.CurrentDomain.BaseDirectory;
            dbPath = Path.Combine(ExeFolder, "DevicesLogs.db");
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            string createTable = @"
                CREATE TABLE IF NOT EXISTS SimLogs (
                    Temp REAL NOT NULL,
                    CreatedTime TEXT NOT NULL,
                    MonitoredTime TEXT NOT NULL,
                    DeviceNum INTEGER NOT NULL,
                    RTT INTEGER NOT NULL

                )";
            using var cmd = new SqliteCommand(createTable, connection);
            cmd.ExecuteNonQuery();
        }
        
        public async Task SaveBatchToDb()
        {


            await using var connection = new SqliteConnection($"Data Source={dbPath}");
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();
            while (TransferToDB.Reader.TryRead(out var item))
            {
                await using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
            INSERT INTO SimLogs (Temp, CreatedTime, MonitoredTime, DeviceNum, RTT)
            VALUES (@temp, @created, @monitored, @device, @rtt)";
                cmd.Parameters.AddWithValue("@temp", item.Temp);
                cmd.Parameters.AddWithValue("@created", item.CreatedTime.ToString("o"));
                cmd.Parameters.AddWithValue("@monitored", item.MonitoredTime.ToString("o"));
                cmd.Parameters.AddWithValue("@device", (int)item.DeviceNum);
                cmd.Parameters.AddWithValue("@rtt", item.RTT);

                await cmd.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

        }

        public async Task<ObservableCollection<SimLog>> LoadData()
        {
            SimLogs.Clear();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
            int devnum;
            string query = "SELECT Temp, CreatedTime, MonitoredTime, DeviceNum, RTT FROM SimLogs ORDER BY CreatedTime DESC";

            using var cmd = new SqliteCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var log = new SimLog
                {
                    Temp = reader.GetDouble(0),
                    CreatedTime = reader.GetDateTime(1),
                    MonitoredTime = reader.GetDateTime(2),                   
                    DeviceNum = reader.GetInt32(3),
                    RTT = reader.GetInt32(4)
                };

                SimLogs.Add(log);
            }
            return SimLogs;
        }

    }
}
