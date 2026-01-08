using E_commerce.Enums;
using E_commerce.Models;
using E_commerce.Services.UserServices;
using System.Text.Json;

namespace E_commerce.Services.ProductServices
{
    internal class ProductServices : IProductServices
    {
        public static List<Product> _products = new List<Product>();

        public static string _path = "products.json";
        public static void LoadProducts()
        {
            if (!File.Exists(_path))
                return;

            string json = File.ReadAllText(_path);
            var products = JsonSerializer.Deserialize<List<Product>>(json);

            if (products == null || products.Count == 0)
                return;

            _products = products;

            int nextId = products.Max(p => p.ProductId) + 1;
            Product.SetNextId(nextId);
        }

        public static void SaveProducts()
        {
            string json = JsonSerializer.Serialize(_products);
            File.WriteAllText(_path, json);
        }

        public void BuyProduct()
        {
            Console.Clear();
            GetAllProducts();

            Console.WriteLine();
            Console.Write("Enter Product ID: ");
            int id = int.Parse(Console.ReadLine());

            var product = _products.FirstOrDefault(p => p.ProductId == id);

            if (product == null) throw new Exception($"Product with id of {id} does not exist");

            UserServices.UserServices.BuyProduct(product);
            SaveProducts();
            Console.Clear();
        }
        public void CreateProduct()
        {
            Console.Clear();
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Description: ");
            string description = Console.ReadLine();

            Console.Write("Price: ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Stock: ");
            int stock = int.Parse(Console.ReadLine());

            Console.Write("Category: ");
            string categoryInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(price.ToString()) ||
                string.IsNullOrWhiteSpace(stock.ToString()) ||
                !Enum.TryParse(categoryInput, out Enums.Category category)
                )
            {
                throw new Exception("Fields can not be empty!");
            }

            if (price <= 0) throw new Exception("Price must be greater than 0!");
            if (stock < 0) throw new Exception("Stock can not be negative!");

            Product product = new Product(name, description, price, stock, category);

            _products.Add(product);
            SaveProducts();
            Console.WriteLine("Product added successfully!");
            Console.Clear();
        }
        public void DeleteProduct()
        {
            Console.Clear();
            GetAllProducts();

            Console.Write("Enter Product Id to delete: ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) throw new Exception("Input cannot be empty.");

            var product = _products.FirstOrDefault(p => p.ProductId.ToString() == input);

            if (product == null) throw new Exception("Product not found.");
            _products.Remove(product);
            SaveProducts();
            Console.WriteLine("Product deleted Successfully!");
            Console.Clear();
        }
        public void FilterProductsByCategory()
        {
            Console.Clear();
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
            Console.Clear();
            foreach (var product in _products)
            {
                Console.WriteLine(product);
            }
        }

        public void GetProductById()
        {
            Console.Clear();
            Console.Write("Enter product id: ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) throw new Exception("Input cannot be empty.");

            var product = _products.FirstOrDefault(p => p.ProductId.ToString() == input);

            if (product == null) throw new Exception("Product not found.");

            Console.WriteLine(product);
        }

        public void UpdateProduct()
        {
            Console.Clear();
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
            string newPrice = Console.ReadLine();

            Console.Write("New stock (leave empty to keep current): ");
            string newStock = Console.ReadLine();


            if (!string.IsNullOrWhiteSpace(newName))
                product.Name = newName;

            if (!string.IsNullOrWhiteSpace(newDesc))
                product.Description = newDesc;

            if (!string.IsNullOrWhiteSpace(category) && Enum.TryParse(category, out Enums.Category newCat))
            {
                product.Category = newCat;
            }

            if (!string.IsNullOrWhiteSpace(newPrice))
                product.Price = double.Parse(newPrice);


            if (!string.IsNullOrWhiteSpace(newStock))
                product.Stock = int.Parse(newStock);

            Console.Clear();
        }
    }
}
