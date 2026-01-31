using E_commerce.Enums;
using E_commerce.Models;
using E_commerce.Services.MenuServices;
using E_commerce.Services.ProductServices;
using E_commerce.Services.UserServices;
using System.Drawing;

MenuServices menuServices = new MenuServices();
ProductServices productServices = new ProductServices();
UserServices userServices = new UserServices();

UserServices.LoadUsers();
UserServices.LoadCurrentUser();
ProductServices.LoadProducts();

while (true)
{
    while (UserServices._currentUser == null)
    {
        Console.WriteLine("==========Welcome==========");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Log In");
        Console.WriteLine("3. Exit");

        string key = Console.ReadLine();


        try
        {
            if (key == "1")
            {
                userServices.Register();
            }
            else if (key == "2")
            {
                var user = userServices.Login();
                if (user == null)
                {
                    Console.WriteLine("Login failed. Try again.");
                }
            }

            else if (key == "3")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid Key!");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }
    }


    if (UserServices._currentUser.Role == Role.Admin)
    {
        menuServices.AdminMenu(userServices, productServices);
    }
    else if (UserServices._currentUser.Role == Role.Customer)
    {
        menuServices.UserMenu(userServices, productServices);
    }
    else
    {
        throw new Exception("There is no menu for other roles!");
    }
}