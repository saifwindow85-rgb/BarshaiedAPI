using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Seeds
{
    public static  class SeedMethods
    {
        public static List<Category> SeedCategories()
        {
            return new List<Category>
    {
        new Category { CategoryId = 1, Name = "Beverages", CreatedAt = DateTime.Now },
        new Category { CategoryId = 2, Name = "Snacks", CreatedAt = DateTime.Now },
        new Category { CategoryId = 3, Name = "Dairy", CreatedAt = DateTime.Now },
        new Category { CategoryId = 4, Name = "Fruits", CreatedAt = DateTime.Now }
    };
        }


        public static List<Product> SeedProducts()
        {
            return new List<Product>
    {
        new Product
        {
            ProductId = 1,
            ProductName = "Milk",
            Barcode = "111111111",
            CategoryId = 3,
            CostPrice = 1.50m,
            SellPrice = 2.00m,
            Quantity = 50,
            MinQuantity = 10,
            ExpiryDate = DateTime.Now.AddDays(7),
            CreatedAt = DateTime.Now
        },
        new Product
        {
            ProductId = 2,
            ProductName = "Orange Juice",
            Barcode = "222222222",
            CategoryId = 1,
            CostPrice = 2.00m,
            SellPrice = 3.50m,
            Quantity = 30,
            MinQuantity = 5,
            ExpiryDate = DateTime.Now.AddDays(10),
            CreatedAt = DateTime.Now
        },
        new Product
        {
            ProductId = 3,
            ProductName = "Chips",
            Barcode = "333333333",
            CategoryId = 2,
            CostPrice = 0.80m,
            SellPrice = 1.50m,
            Quantity = 100,
            MinQuantity = 20,
            CreatedAt = DateTime.Now
        },
        new Product
        {
            ProductId = 4,
            ProductName = "Apple",
            Barcode = null, 
            CategoryId = 4,
            CostPrice = 0.50m,
            SellPrice = 1.00m,
            Quantity = 200,
            MinQuantity = 50,
            CreatedAt = DateTime.Now
        },
        new Product
        {
            ProductId = 5,
            ProductName = "Unknown Product",
            Barcode = "444444444",
            CategoryId = null,
            CostPrice = 5.00m,
            SellPrice = 7.00m,
            Quantity = 10,
            MinQuantity = 2,
            CreatedAt = DateTime.Now
        }
    };
        }
        public static List<ShoppingListItem> SeedShoppingListItems()
        {
            return new List<ShoppingListItem>
    {
        new ShoppingListItem
        {
            ShoppingListItemId = 1,
            ProductId = 1,
            Quantity = 2,
            Notes = "For breakfast",
            IsPurchased = false,
            CreatedAt = DateTime.Now
        },
        new ShoppingListItem
        {
            ShoppingListItemId = 2,
            ProductId = 2,
            Quantity = 1,
            Notes = "Fresh juice",
            IsPurchased = true,
            CreatedAt = DateTime.Now
        },
        new ShoppingListItem
        {
            ShoppingListItemId = 3,
            ProductId = 3,
            Quantity = 5,
            Notes = null,
            IsPurchased = false,
            CreatedAt = DateTime.Now
        },
        new ShoppingListItem
        {
            ShoppingListItemId = 4,
            ProductId = 1,
            Quantity = 3,
            Notes = "Extra stock",
            IsPurchased = false,
            CreatedAt = DateTime.Now
        }
    };
        }
    }


}
