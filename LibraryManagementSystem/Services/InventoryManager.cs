using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services
{
    public class InventoryManager
    {
        // Data storage
        private List<Product> products;
        private int nextProductId;

        public InventoryManager()
        {
            products = new List<Product>();
            nextProductId = 1;

            // Add some sample data for testing
            SeedSampleData();
        }

        // Method to seed sample data
        private void SeedSampleData()
        {
            AddProduct("Laptop", 999.99m, 15);
            AddProduct("Wireless Mouse", 29.99m, 42);
            AddProduct("USB-C Cable", 19.99m, 3);
            AddProduct("Monitor", 249.99m, 8);
            AddProduct("Keyboard", 89.99m, 25);
        }

        // Core Operations

        // Add a new product
        public Product AddProduct(string name, decimal price, int quantity)
        {
            // Validate input
            if (!ValidateProductData(name, price, quantity))
            {
                return null;
            }

            // Create new product with auto-incremented ID
            Product newProduct = new Product(nextProductId, name, price, quantity);
            products.Add(newProduct);
            nextProductId++;

            return newProduct;
        }

        // Update product stock
        public bool UpdateStock(int productId, int quantityChange)
        {
            Product product = FindProductById(productId);

            if (product == null)
            {
                return false;
            }

            if (quantityChange == 0)
            {
                return false;
            }
            else if (quantityChange > 0)
            {
                return product.IncreaseStock(quantityChange);
            }
            else
            {
                // quantityChange is negative
                return product.DecreaseStock(Math.Abs(quantityChange));
            }
        }

        // Remove a product
        public bool RemoveProduct(int productId)
        {
            Product product = FindProductById(productId);

            if (product != null)
            {
                return products.Remove(product);
            }

            return false;
        }
        // Get all products
        public List<Product> GetAllProducts()
        {
            return products.OrderBy(p => p.Id).ToList();
        }

        // Get products with low stock (below threshold)
        public List<Product> GetLowStockProducts(int threshold = 5)
        {
            return products.Where(p => p.StockQuantity < threshold)
                          .OrderBy(p => p.StockQuantity)
                          .ToList();
        }

        // Find product by ID
        public Product FindProductById(int productId)
        {
            return products.FirstOrDefault(p => p.Id == productId);
        }

        // Find products by name (partial match)
        public List<Product> SearchProductsByName(string searchTerm)
        {
            return products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()))
                          .OrderBy(p => p.Id)
                          .ToList();
        }

        // Get total inventory value
        public decimal CalculateTotalInventoryValue()
        {
            return products.Sum(p => p.Price * p.StockQuantity);
        }


        // Validation methods
        private bool ValidateProductData(string name, decimal price, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
            {
                return false;
            }

            if (price <= 0 || price > 99999.99m)
            {
                return false;
            }

            if (quantity < 0 || quantity > 9999)
            {
                return false;
            }

            return true;
        }
        // Method outside class example: Extension method for formatting currency
        public static string FormatCurrency(decimal amount)
        {
            return $"${amount:F2}";
        }

    }
}
