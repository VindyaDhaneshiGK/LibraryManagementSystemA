using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.UI
{
    public class ConsoleInterface
    {
        private InventoryManager inventoryManager;
        private bool isRunning;

        public ConsoleInterface()
        {
            inventoryManager = new InventoryManager();
            isRunning = true;
        }
        // Main program loop
        public void Run()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===================================");
            Console.WriteLine("   INVENTORY MANAGEMENT SYSTEM");
            Console.WriteLine("===================================");
            Console.ResetColor();

            while (isRunning)
            {
                DisplayMainMenu();
                string choice = GetUserInput("Enter your choice (1-6): ");
                ProcessMenuChoice(choice);
            }
        }

        // Main menu display
        private void DisplayMainMenu()
        {
            Console.WriteLine("\n--- MAIN MENU ---");
            Console.WriteLine("1. Add New Product");
            Console.WriteLine("2. Update Product Stock");
            Console.WriteLine("3. View All Products");
            Console.WriteLine("4. View Low Stock Products");
            Console.WriteLine("5. Remove Product");
            Console.WriteLine("6. Exit Program");
            Console.WriteLine(new string('-', 30));
        }

        // Process menu selection
        private void ProcessMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    DisplayAddProductScreen();
                    break;
                case "2":
                    DisplayUpdateStockScreen();
                    break;
                case "3":
                    DisplayAllProductsScreen();
                    break;
                case "4":
                    DisplayLowStockProductsScreen();
                    break;
                case "5":
                    DisplayRemoveProductScreen();
                    break;
                case "6":
                    ExitProgram();
                    break;
                default:
                    DisplayError("Invalid choice. Please select 1-6.");
                    PressAnyKeyToContinue();
                    break;
            }
        }
        // Screen 1: Add new product
        private void DisplayAddProductScreen()
        {
            Console.Clear();
            Console.WriteLine("--- ADD NEW PRODUCT ---\n");

            string name = GetUserInput("Product Name: ");

            decimal price;
            while (!decimal.TryParse(GetUserInput("Price ($): "), out price) || price <= 0)
            {
                DisplayError("Invalid price. Please enter a positive number.");
            }
            int quantity;
            while (!int.TryParse(GetUserInput("Initial Stock Quantity: "), out quantity) || quantity < 0)
            {
                DisplayError("Invalid quantity. Please enter a non-negative number.");
            }

            Product newProduct = inventoryManager.AddProduct(name, price, quantity);

            if (newProduct != null)
            {
                DisplaySuccess($"Product added successfully!\n{newProduct}");
            }
            else
            {
                DisplayError("Failed to add product. Please check your inputs.");
            }

            PressAnyKeyToContinue();
        }
        // Screen 2: Update stock
        private void DisplayUpdateStockScreen()
        {
            Console.Clear();
            Console.WriteLine("--- UPDATE PRODUCT STOCK ---\n");

            // Show all products first
            DisplayAllProducts(false);

            int productId;
            while (!int.TryParse(GetUserInput("\nEnter Product ID to update: "), out productId))
            {
                DisplayError("Invalid ID. Please enter a number.");
            }

            Product product = inventoryManager.FindProductById(productId);

            if (product == null)
            {
                DisplayError($"Product with ID {productId} not found.");
                PressAnyKeyToContinue();
                return;
            }

            Console.WriteLine($"\nCurrent Product: {product.Name}");
            Console.WriteLine($"Current Stock: {product.StockQuantity}");

            int quantityChange;
            while (!int.TryParse(GetUserInput("Enter quantity change (+ for restock, - for sale): "), out quantityChange))
            {
                DisplayError("Invalid quantity. Please enter a number.");
            }

            bool success = inventoryManager.UpdateStock(productId, quantityChange);

            if (success)
            {
                product = inventoryManager.FindProductById(productId); // Refresh data
                DisplaySuccess($"Stock updated successfully!\nNew Stock: {product.StockQuantity}");
            }
            else
            {
                DisplayError($"Failed to update stock. Check if you're trying to remove more than available.");
            }

            PressAnyKeyToContinue();
        }
        // Screen 3: View all products
        private void DisplayAllProductsScreen()
        {
            Console.Clear();
            Console.WriteLine("--- ALL PRODUCTS ---\n");
            DisplayAllProducts(true);
            PressAnyKeyToContinue();
        }

        // Screen 4: View low stock products

        private void DisplayLowStockProductsScreen()
        {
            Console.Clear();
            Console.WriteLine("--- LOW STOCK PRODUCTS (Below 5 items) ---\n");

            var lowStockProducts = inventoryManager.GetLowStockProducts();

            if (lowStockProducts.Count == 0)
            {
                Console.WriteLine("No products with low stock. Good job!");
            }
            else
            {
                DisplayProductsTable(lowStockProducts);
                Console.WriteLine($"\n⚠️  {lowStockProducts.Count} product(s) need restocking!");
            }

            Console.WriteLine($"\nTotal Inventory Value: {InventoryManager.FormatCurrency(inventoryManager.CalculateTotalInventoryValue())}");
            PressAnyKeyToContinue();
        }

        // Screen 5: Remove product
        private void DisplayRemoveProductScreen()
        {
            Console.Clear();
            Console.WriteLine("--- REMOVE PRODUCT ---\n");

            DisplayAllProducts(false);

            int productId;
            while (!int.TryParse(GetUserInput("\nEnter Product ID to remove: "), out productId))
            {
                DisplayError("Invalid ID. Please enter a number.");
            }

            Product product = inventoryManager.FindProductById(productId);
            if (product == null)
            {
                DisplayError($"Product with ID {productId} not found.");
                PressAnyKeyToContinue();
                return;
            }

            Console.WriteLine($"\nProduct to remove: {product}");
            string confirm = GetUserInput("Are you sure? (yes/no): ").ToLower();

            if (confirm == "yes" || confirm == "y")
            {
                bool success = inventoryManager.RemoveProduct(productId);

                if (success)
                {
                    DisplaySuccess($"Product '{product.Name}' removed successfully!");
                }
                else
                {
                    DisplayError("Failed to remove product.");
                }
            }
            else
            {
                Console.WriteLine("Removal cancelled.");
            }

            PressAnyKeyToContinue();
        }

        // Helper method to display all products

        private void DisplayAllProducts(bool showTotalValue)
        {
            var allProducts = inventoryManager.GetAllProducts();

            if (allProducts.Count == 0)
            {
                Console.WriteLine("No products in inventory.");
                return;
            }

            DisplayProductsTable(allProducts);

            if (showTotalValue)
            {
                Console.WriteLine($"\nTotal Products: {allProducts.Count}");
                Console.WriteLine($"Total Inventory Value: {InventoryManager.FormatCurrency(inventoryManager.CalculateTotalInventoryValue())}");
            }
        }
        // Helper method to display products in a table format
        private void DisplayProductsTable(List<Product> products)
        {
            Console.WriteLine("┌─────┬──────────────────────┬──────────┬────────┐");
            Console.WriteLine("│ ID  │ Product Name         │ Price    │ Stock  │");
            Console.WriteLine("├─────┼──────────────────────┼──────────┼────────┤");

            foreach (var product in products)
            {
                Console.WriteLine(product.ToTableRow());
            }

            Console.WriteLine("└─────┴──────────────────────┴──────────┴────────┘");
        }
        // Exit program
        private void ExitProgram()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThank you for using the Inventory Management System!");
            Console.ResetColor();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            isRunning = false;
        }

        // Utility methods

        // Method to get user input with prompt
        private string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? "";
        }

        // Method to display success message
        private void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {message}");
            Console.ResetColor();
        }

        // Method to display error message
        private void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ Error: {message}");
            Console.ResetColor();
        }

        // Method to wait for user to press any key
        private void PressAnyKeyToContinue()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
