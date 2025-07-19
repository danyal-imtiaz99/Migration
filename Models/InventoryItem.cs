namespace migration.Models
{
    public class InventoryItem
        {
            public string? ItemName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public string? Category { get; set; }

            public decimal TotalValue => Quantity * Price;
        }
}