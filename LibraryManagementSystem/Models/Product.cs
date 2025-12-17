using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public class Product
    {
        // Properties
        public int Id { get; private set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }


        // Constructor
        public Product(int id, string name, decimal price, int stockQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
        }

        // Method to increase stock
        public bool IncreaseStock(int amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            StockQuantity += amount;
            return true;
        }

        // Method to decrease stock
        public bool DecreaseStock(int amount)
        {
            if (amount <= 0 || amount > StockQuantity)
            {
                return false;
            }

            StockQuantity -= amount;
            return true;
        }

        // Override ToString for easy display
        public override string ToString()
        {
            return $"ID: {Id} | {Name} | Price: ${Price:F2} | Stock: {StockQuantity}";
        }

        // Format product for table display
        public string ToTableRow()
        {
            return $"│ {Id,3} │ {Name,-20} │ ${Price,8:F2} │ {StockQuantity,6} │";
        }


    }
}
