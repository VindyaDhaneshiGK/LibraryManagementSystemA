using LibraryManagementSystem.UI;

namespace LibraryManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Inventory Management System";

            // Create and run the console interface
            ConsoleInterface consoleInterface = new ConsoleInterface();
            consoleInterface.Run();
        }
    }
}
