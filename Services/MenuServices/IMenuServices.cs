namespace E_commerce.Services.MenuServices
{
    internal interface IMenuServices
    {
        public void UserMenu(UserServices.UserServices userService, ProductServices.ProductServices productService);
        public void AdminMenu(UserServices.UserServices userService, ProductServices.ProductServices productService);
    }
}
