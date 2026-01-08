namespace E_commerce.Models
{
    internal class Order
    {
        private static int _orderCounter = 1;

        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public string Name { get; private set; }
        public double Price { get; private set; }
        public int Quantity { get; private set; }

        public double TotalPrice => Price * Quantity;

        public Order(int productId, string name, double price, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero.");

            if (price <= 0)
                throw new Exception("Price must be greater than zero.");

            OrderId = _orderCounter++;
            ProductId = productId;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public void IncreaseQuantity() { Quantity++; }

    }
}
