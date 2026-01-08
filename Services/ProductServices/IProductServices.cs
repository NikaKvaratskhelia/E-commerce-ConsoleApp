namespace E_commerce.Services.ProductServices
{
    internal interface IProductServices
    {
        public void CreateProduct();
        public void DeleteProduct();
        public void UpdateProduct();
        public void GetAllProducts();
        public void GetProductById();
        public void FilterProductsByCategory();

    }
}
