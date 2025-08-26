using System.Reflection.Metadata;
using System.Runtime.InteropServices;
namespace FinalAttemptWPF.Services
{
    public static class ConsoleManager
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeConsole();
    }  
}