using System;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using migration.Models;
using migration.Services;

namespace migration
{
    class Program
    {
        static List<InventoryItem> loadedData = new List<InventoryItem>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("=== Migration Tool ===");
                Console.WriteLine("1. Load CSV File");
                Console.WriteLine("2. Show Data Preview");
                Console.WriteLine("3. Generate HTML Report");
                Console.WriteLine("4. Exit");

                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();

                if(choice == "1")
                {
                    LoadCSVFile();
                }
                else if(choice == "2")
                {
                    ShowDataPreview();
                }
                else if(choice == "3")
                {
                    GenerateHTMLReport();
                }
                else if(choice == "4")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice! Please enter 1-4.");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void LoadCSVFile()
        {
            try
            {
                string filePath = "/Users/d.imtiaz/Desktop/sample_data.csv";
                Console.WriteLine("Loading file: " + filePath);

                var csvService = new CsvService();
                loadedData = csvService.LoadFromFile(filePath);

                Console.WriteLine($"Found {loadedData.Count} items:");
                Console.WriteLine("First 5 items:");

                for(int i = 0; i < Math.Min(5, loadedData.Count); i++)
                {
                    var item = loadedData[i];
                    Console.WriteLine($"{i + 1}. {item.ItemName} - Qty: {item.Quantity} - Price: ${item.Price} - Category: {item.Category}");
                }

                Console.WriteLine("CSV Loaded Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void ShowDataPreview()
        {
            if(loadedData.Count == 0)
            {
                Console.WriteLine("No Data Loaded!");
                return;
            }

            Console.WriteLine($"\n=== Data Preview ({loadedData.Count} total items) ===");
            Console.WriteLine("Item Name          | Qty | Price  | Category");
            Console.WriteLine("-------------------------------------------");

            foreach(var item in loadedData)
            {
                Console.WriteLine($"{item.ItemName,-18} | {item.Quantity,3} | ${item.Price,5:F2} | {item.Category}");
            }
        }

        static void GenerateHTMLReport()
        {
            if(loadedData.Count == 0)
            {
                Console.WriteLine("No Data Loaded! Please load CSV first.");
                return;
            }

            try
            {
                var templateService = new TemplateService();
                string htmlContent = templateService.GenerateReport(loadedData);

                string reportPath = "/Users/d.imtiaz/Desktop/inventory_report.html";
                File.WriteAllText(reportPath, htmlContent);

                Console.WriteLine("HTML report generated successfully!");
                Console.WriteLine($"File saved to: {reportPath}");
                Console.WriteLine("You can open this file in any web browser.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating report: " + ex.Message);
            }
        }
    }
}