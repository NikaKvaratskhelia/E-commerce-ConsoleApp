using E_commerce.Enums;

namespace E_commerce.Models
{
    internal class Product
    {
        private static int _id = 1;
        public int ProductId { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public Category Category { get; set; }
        public string CreatedAt { get; private set; }

        public Product(string name, string description, double price, int stock, Category category)
        {
            DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow);
            string result = date.ToString("yyyy-MM-dd");

            ProductId = _id++;
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            Category = category;
            CreatedAt = result;
        }

        public override string ToString()
        {
            return $"ProductId: {ProductId}, Name: {Name}, Description: {Description}, Price: {Price}, Stock: {Stock}, Category: {Category}";
        }
    }
}
