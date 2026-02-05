
namespace E_commerce.Services.MenuServices
{
    internal class MenuServices : IMenuServices
    {
        public void AdminMenu(UserServices.UserServices userService, ProductServices.ProductServices productService)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"====================================================================");
                Console.WriteLine($"|| Logged in as: {UserServices.UserServices._currentUser.Username}");
                Console.WriteLine($"|| Role: {UserServices.UserServices._currentUser.Role}");
                Console.WriteLine($"|| Balance: {UserServices.UserServices._currentUser.Balance}");
                Console.WriteLine($"====================================================================");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Show all products");
                Console.WriteLine("2. Show product by ID");
                Console.WriteLine("3. Filter products by category");
                Console.WriteLine("4. Show Profile");
                Console.WriteLine("5. Update Profile");
                Console.WriteLine("6. Logout");
                Console.WriteLine();
                Console.WriteLine("========Update Product List=======");
                Console.WriteLine("7. Add a product");
                Console.WriteLine("8. Delete a product");
                Console.WriteLine("9. Update a product");
                Console.WriteLine();
                Console.Write("Select an option: ");

                string? choice = Console.ReadLine()?.Trim();

                try
                {
                    if (string.IsNullOrEmpty(choice))
                    {
                        Console.WriteLine("Option cannot be empty");
                        continue;
                    }

                    if (choice == "1")
                    {
                        productService.GetAllProducts();
                    }
                    else if (choice == "2")
                    {
                        productService.GetProductById();
                    }
                    else if (choice == "3")
                    {
                        productService.FilterProductsByCategory();
                    }
                    else if (choice == "4")
                    {
                        userService.ShowProfile();
                    }
                    else if (choice == "5")
                    {
                        userService.UpdateProfile();
                    }
                    else if (choice == "6")
                    {
                        userService.Logout();
                        return;
                    }
                    else if (choice == "7")
                    {
                        productService.CreateProduct();
                    }
                    else if (choice == "8")
                    {
                        productService.DeleteProduct();
                    }
                    else if (choice == "9")
                    {
                        productService.UpdateProduct();
                    }
                    else
                    {
                        Console.WriteLine("Invalid option, try again.");
                    }



                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                Console.WriteLine();
            }
        }

        public void UserMenu(UserServices.UserServices userService, ProductServices.ProductServices productService)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"====================================================================");
                Console.WriteLine($"|| Logged in as: {UserServices.UserServices._currentUser.Username}");
                Console.WriteLine($"|| Role: {UserServices.UserServices._currentUser.Role}");
                Console.WriteLine($"|| Balance: {UserServices.UserServices._currentUser.Balance}");
                Console.WriteLine($"====================================================================");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Show all products");
                Console.WriteLine("2. Show product by ID");
                Console.WriteLine("3. Filter products by category");
                Console.WriteLine("4. Buy Product");
                Console.WriteLine("5. Show Profile");
                Console.WriteLine("6. Update Profile");
                Console.WriteLine("7. Update Balance");
                Console.WriteLine("8. Logout");
                Console.WriteLine();
                Console.Write("Select an option: ");

                string? choice = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(choice))
                {
                    Console.WriteLine("Option cannot be empty");
                    continue;
                }

                try
                {
                    if (choice == "1")
                    {
                        productService.GetAllProducts();
                    }
                    else if (choice == "2")
                    {
                        productService.GetProductById();
                    }
                    else if (choice == "3")
                    {
                        productService.FilterProductsByCategory();
                    }
                    else if (choice == "4")
                    {
                        productService.BuyProduct();
                    }
                    else if (choice == "5")
                    {
                        userService.ShowProfile();
                    }
                    else if (choice == "6")
                    {
                        userService.UpdateProfile();
                    }
                    else if (choice == "7")
                    {
                        userService.UpdateBalance();
                    }
                    else if (choice == "8")
                    {

                        userService.Logout();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option");
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }
    }
}
