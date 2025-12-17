using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Helpers
{
    // This class contains additional helper methods that work with the inventory system
    public static class InventoryExtensions
    {
        // This class contains additional helper methods that work with the inventory system
        // Extension method to display product list in simple format
        public static void DisplaySimpleList(this List<Product> products)
        {
            if (products == null || products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine("\n--- Product List ---");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}: {product.Name} (${product.Price:F2}) - {product.StockQuantity} in stock");
            }
        }

        // Method to calculate average price of products
        public static decimal CalculateAveragePrice(this List<Product> products)
        {
            if (products == null || products.Count == 0)
                return 0;

            decimal total = 0;
            foreach (var product in products)
            {
                total += product.Price;
            }

            return total / products.Count;
        }

        // Method to find products in price range
        public static List<Product> FindProductsInPriceRange(
            this List<Product> products,
            decimal minPrice,
            decimal maxPrice)
        {
            List<Product> result = new List<Product>();

            foreach (var product in products)
            {
                if (product.Price >= minPrice && product.Price <= maxPrice)
                {
                    result.Add(product);
                }
            }

            return result;
        }
        // Static method to validate email (example of unrelated helper method)
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }



    }

    // Another helper class with static methods
    public static class ConsoleHelper
    {
        public static void PrintHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine($"  {title.ToUpper()}");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void PrintColoredMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Skip if Backspace or Enter
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {

                    password = password.Substring(0, (password.Length - 1));
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
    }

}

    
