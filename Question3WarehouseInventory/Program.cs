using System;
using System.Collections.Generic;

namespace Question3WarehouseInventory
{
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public string Brand { get; }
        public int WarrantyMonths { get; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }
    }

    public class GroceryItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }
    }

    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    public class InventoryRepository<T> where T : IInventoryItem
    {
        private readonly Dictionary<int, T> _items = new();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
                throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
            _items[item.Id] = item;
        }

        public T GetItemById(int id)
        {
            if (!_items.TryGetValue(id, out var item))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            return item;
        }

        public void RemoveItem(int id)
        {
            if (!_items.Remove(id))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
        }

        public List<T> GetAllItems() => new List<T>(_items.Values);

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
                throw new InvalidQuantityException("Quantity cannot be negative.");

            if (!_items.TryGetValue(id, out var item))
                throw new ItemNotFoundException($"Item with ID {id} not found.");

            item.Quantity = newQuantity;
        }
    }

    public class WareHouseManager
    {
        private readonly InventoryRepository<ElectronicItem> _electronics = new();
        private readonly InventoryRepository<GroceryItem> _groceries = new();

        public void SeedData()
        {
            try
            {
                _electronics.AddItem(new ElectronicItem(1, "Smartphone", 10, "BrandA", 24));
                _electronics.AddItem(new ElectronicItem(2, "Laptop", 5, "BrandB", 12));
                _electronics.AddItem(new ElectronicItem(3, "Headphones", 20, "BrandC", 6));

                _groceries.AddItem(new GroceryItem(101, "Rice", 50, DateTime.Now.AddMonths(12)));
                _groceries.AddItem(new GroceryItem(102, "Beans", 30, DateTime.Now.AddMonths(6)));
                _groceries.AddItem(new GroceryItem(103, "Milk", 15, DateTime.Now.AddDays(10)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }

        public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
        {
            var items = repo.GetAllItems();
            foreach (var i in items)
            {
                Console.WriteLine($"ID:{i.Id} Name:{i.Name} Quantity:{i.Quantity}");
            }
        }

        public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
        {
            try
            {
                var item = repo.GetItemById(id);
                repo.UpdateQuantity(id, item.Quantity + quantity);
                Console.WriteLine($"Increased stock for ID {id}. New quantity: {repo.GetItemById(id).Quantity}");
            }
            catch (DuplicateItemException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
        {
            try
            {
                repo.RemoveItem(id);
                Console.WriteLine($"Removed item with ID {id}");
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        public void RunDemo()
        {
            SeedData();

            Console.WriteLine("--- Groceries ---");
            PrintAllItems(_groceries);

            Console.WriteLine();
            Console.WriteLine("--- Electronics ---");
            PrintAllItems(_electronics);

            Console.WriteLine();
            Console.WriteLine("--- Testing error cases ---");

            // Duplicate add
            try
            {
                Console.WriteLine("Attempting to add duplicate electronic item with ID 1");
                _electronics.AddItem(new ElectronicItem(1, "Tablet", 7, "BrandD", 12));
            }
            catch (DuplicateItemException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Remove non-existent
            try
            {
                Console.WriteLine("Attempting to remove non-existent grocery ID 999");
                _groceries.RemoveItem(999);
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Negative quantity update
            try
            {
                Console.WriteLine("Attempting to set negative quantity for electronic ID 2");
                _electronics.UpdateQuantity(2, -5);
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var manager = new WareHouseManager();
            manager.RunDemo();
        }
    }
}
