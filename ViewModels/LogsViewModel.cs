using CommunityToolkit.Mvvm.ComponentModel;
using FinalAttemptWPF.Model.Entity;
using FinalAttemptWPF.Services;
using System.Collections.ObjectModel;

namespace FinalAttemptWPF.ViewModels
{
    public class LogsViewModel : ObservableObject
    {
        public ObservableCollection<SimLog> SimLogs { get; set; } = new();

        public dbService _dbService = new dbService();
        public LogsViewModel()
        {
            LoadLogsAsync();      
        }
        private async Task LoadLogsAsync()
        {
            SimLogs = await _dbService.LoadData();
            Console.WriteLine("da");
        }
    }
}
