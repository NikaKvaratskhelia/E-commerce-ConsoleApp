using E_commerce.Models;
using System.Text.Json;

namespace E_commerce.Services.ProductServices
{
    internal class ProductServices : IProductServices
    {
        public static List<Product> _products = new List<Product>();
        public static string _path = "products.json";
        public static void LoadProducts()
        {
            string json = File.ReadAllText(_path);
            var products = JsonSerializer.Deserialize<List<Product>>(json);

            if (products != null) _products = products;
        }
        public static void SaveProducts()
        {
            string json = JsonSerializer.Serialize(_products);
            File.WriteAllText(_path, json);
        }
        public void CreateProduct()
        {

        }
        public void DeleteProduct()
        {
            GetAllProducts();

            Console.Write("Enter Product Id to delete: ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) throw new Exception("Input cannot be empty.");

            var product = _products.FirstOrDefault(p => p.ProductId.ToString() == input);

            if (product == null) throw new Exception("Product not found.");
            _products.Remove(product);
            SaveProducts();
            Console.WriteLine("Product deleted Successfully!");
        }
        public void FilterProductsByCategory()
        {
            Console.WriteLine("Valid Categories: Phones, Laptops, Accessories, Tablets, Chargers");
            Console.Write("Enter category to filter with: ");

            string category = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(category) || !Enum.TryParse(category, out Enums.Category wanted))
                throw new Exception("Invalid Category, try again!");

            var wantedProducts = _products.Where(p => p.Category.Equals(wanted));

            if (wantedProducts.Count() == 0)
            {
                Console.WriteLine("No products in this category");
            }
            else
            {
                foreach (var item in wantedProducts)
                {
                    Console.WriteLine(item);
                }
            }
        }
        public void GetAllProducts()
        {
            foreach (var product in _products)
            {
                Console.WriteLine(product);
            }
        }

        public void GetProductById()
        {
            Console.Write("Enter product id: ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) throw new Exception("Input cannot be empty.");

            var product = _products.FirstOrDefault(p => p.ProductId.ToString() == input);

            if (product == null) throw new Exception("Product not found.");

            Console.WriteLine(product);
        }

        public void UpdateProduct()
        {
            Console.Write("Enter product id: ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) throw new Exception("Input cannot be empty.");

            var product = _products.FirstOrDefault(p => p.ProductId.ToString() == input);

            if (product == null) throw new Exception("Product not found.");

            Console.WriteLine(product);

            Console.Write("New name (leave empty to keep current): ");
            string newName = Console.ReadLine()?.Trim();

            Console.Write("New description (leave empty to keep current): ");
            string newDesc = Console.ReadLine()?.Trim();

            Console.WriteLine("Valid Categories: Phones, Laptops, Accessories, Tablets, Chargers");
            Console.Write("New category (leave empty to keep current): ");
            string category = Console.ReadLine();

            Console.Write("New price (leave empty to keep current): ");
            string price = Console.ReadLine();

            if (!Enum.TryParse(category, out Enums.Category newCat))
            {
                throw new Exception("Valid cateogries are: Phones, Laptops, Accessories, Tablets, Chargers");
            }

            if (!string.IsNullOrWhiteSpace(newName))
                product.Name = newName;

            if (!string.IsNullOrWhiteSpace(newDesc))
                product.Description = newDesc;

            if (!string.IsNullOrWhiteSpace(category) && Enum.TryParse(category, out Enums.Category newCategory))
                product.Category = newCategory;

            if (!string.IsNullOrWhiteSpace(price))
                product.Price = double.Parse(price);

        }
    }
}
