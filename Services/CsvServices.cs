using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using migration.Models;

namespace migration.Services
{
    public class CsvService
    {
        public List<InventoryItem> LoadFromFile(string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,    // Don't validate headers
                    MissingFieldFound = null,  // Ignore missing fields
                    PrepareHeaderForMatch = args => args.Header.ToLower()  // Make headers case-insensitive
                };

                using var reader = new StringReader(File.ReadAllText(filePath));
                using var csv = new CsvReader(reader, config);

                // Register custom mapping for price column
                csv.Context.RegisterClassMap<InventoryItemMap>();

                return csv.GetRecords<InventoryItem>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading CSV: {ex.Message}");
            }
        }
    }

    // Custom converter to handle dollar signs in prices
    public class PriceConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
                return 0m;

            // Remove dollar sign and any spaces
            text = text.Replace("$", "").Replace(" ", "");

            if (decimal.TryParse(text, out decimal result))
                return result;

            return 0m; // Default value if conversion fails
        }
    }

    // Custom mapping class to handle CSV column differences
    public class InventoryItemMap : ClassMap<InventoryItem>
    {
        public InventoryItemMap()
        {
            Map(m => m.ItemName).Name("ItemName");
            Map(m => m.Quantity).Name("Quantity");
            Map(m => m.Price).Name("price").TypeConverter<PriceConverter>();  // Use custom converter
            Map(m => m.Category).Name("Category");
        }
    }
}