using E_commerce.Enums;

namespace E_commerce.Models
{
    internal class User
    {
        private static int _id = 1;
        public int UserId { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public double Balance { get; set; }
        public string CreatedAt { get; private set; }
        public bool IsActive { get; set; }
        public List<Order> Cart { get; set; } = new List<Order>();

        public User(string username, string password, string email, Role role, double balance)
        {
            DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow);
            string result = date.ToString("yyyy-MM-dd");

            UserId = _id++;
            Username = username;
            Password = password;
            Email = email;
            Role = role;
            Balance = balance;
            CreatedAt = result;
            IsActive = false;
        }

        public override string ToString()
        {
            return $" Username: {Username}\n Email: {Email}\n Role: {Role}\n Balance: {Balance}\n Active: {IsActive}\n";
        }

    }
}
