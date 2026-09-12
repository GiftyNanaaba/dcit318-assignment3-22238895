using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Question5InventoryRecords
{
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private readonly List<T> _log = new();
        private readonly string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item)
        {
            _log.Add(item);
        }

        public List<T> GetAll() => new List<T>(_log);

        public void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_log, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    _log.Clear();
                    return;
                }

                var json = File.ReadAllText(_filePath);
                var items = JsonSerializer.Deserialize<List<T>>(json);
                _log.Clear();
                if (items != null) _log.AddRange(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }
    }

    public class InventoryApp
    {
        private readonly InventoryLogger<InventoryItem> _logger;

        public InventoryApp(string filePath)
        {
            _logger = new InventoryLogger<InventoryItem>(filePath);
        }

        public void SeedSampleData()
        {
            _logger.Add(new InventoryItem(1, "Keyboard", 10, DateTime.Now));
            _logger.Add(new InventoryItem(2, "Mouse", 15, DateTime.Now));
            _logger.Add(new InventoryItem(3, "Monitor", 5, DateTime.Now));
        }

        public void SaveData() => _logger.SaveToFile();

        public void LoadData() => _logger.LoadFromFile();

        public void PrintAllItems()
        {
            foreach (var item in _logger.GetAll())
            {
                Console.WriteLine($"ID:{item.Id} Name:{item.Name} Quantity:{item.Quantity} DateAdded:{item.DateAdded}");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "inventory.json");
            var app = new InventoryApp(filePath);

            Console.WriteLine("Seeding sample data and saving to file...");
            app.SeedSampleData();
            app.SaveData();

            Console.WriteLine("Clearing in-memory data and loading from file...");
            // create a new app instance or reload
            var loader = new InventoryApp(filePath);
            loader.LoadData();
            loader.PrintAllItems();
        }
    }
}
