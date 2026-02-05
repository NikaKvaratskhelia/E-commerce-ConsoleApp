using E_commerce.Enums;
using E_commerce.Models;
using E_commerce.Validators;
using System.Text;
using System.Text.Json;

namespace E_commerce.Services.UserServices
{
    internal class UserServices : IUserServices
    {
        public static User? _currentUser = null;
        private static List<User> _users = new List<User>();
        public static string _path = "users.json";
        public static string _currentUserPath = "currentUser.json";
        private static string ReadPasswordMasked(string prompt = null)
        {
            if (!string.IsNullOrEmpty(prompt))
                Console.Write(prompt);

            var sb = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    sb.Append(keyInfo.KeyChar);
                    Console.Write('*');
                }
            }
            return sb.ToString();
        }
        public static void LoadUsers()
        {
            if (!File.Exists(_path))
                return;

            string json = File.ReadAllText(_path);
            if (string.IsNullOrWhiteSpace(json)) return;
            var users = JsonSerializer.Deserialize<List<User>>(json);

            if (users == null || users.Count == 0)
                return;

            _users = users;
        }

        public static void LoadCurrentUser()
        {
            if (!File.Exists(_currentUserPath))
                return;

            string json = File.ReadAllText(_currentUserPath);
            var user = JsonSerializer.Deserialize<User>(json);

            if (user == null)
                return;

            _currentUser = user;
        }
        public static void SaveCurrentUser()
        {
            string json = JsonSerializer.Serialize(_currentUser);
            File.WriteAllText(_currentUserPath, json);
        }
        public static void SaveUsers()
        {
            string json = JsonSerializer.Serialize(_users);
            File.WriteAllText(_path, json);
        }
        public User Login()
        {
            Console.Clear();
            Console.Write("Enter User Email: ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email)) throw new Exception("Email cannot be empty.");

            Console.WriteLine();

            string password = ReadPasswordMasked("Enter User Password: ");
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Password cannot be empty.");

            var user = _users.FirstOrDefault(u => u.Email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                _currentUser = user;
                _currentUser.IsActive = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Login Successful!");
                Console.ResetColor();
                SaveUsers();
                SaveCurrentUser();
                return _currentUser;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid email or password.");
                Console.ResetColor();
                return null;
            }
        }
        public User Logout()
        {
            Console.Clear();
            if (_currentUser == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No user is currently logged in.");
                Console.ResetColor();
                return null;
            }

            _currentUser.IsActive = false;
            _currentUser = null;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Logged out successfully!");
            Console.ResetColor();
            SaveUsers();
            SaveCurrentUser();
            return _currentUser;
        }
        public void Register()
        {
            Console.Clear();
            Console.Write("Username: ");
            var username = Console.ReadLine()?.Trim();

            Console.Write("Email: ");
            var email = Console.ReadLine()?.Trim();

            string password = ReadPasswordMasked("Enter User Password: ");

            if (_users.Any(u => u.Username == username))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Registration failed: username already exists.");
                Console.ResetColor();
                return;
            }

            if (_users.Any(u => u.Email == email))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Registration failed: email already exists.");
                Console.ResetColor();
                return;
            }

            User user = new User(
                username,
                password,
                email,
                Role.Customer,
                0
            );

            UserValidators validator = new UserValidators();
            var validationResult = validator.Validate(user);

            if (!validationResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Registration failed due to the following errors:");
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }
                Console.ResetColor();
                return;
            }

            if (validationResult.IsValid)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            _users.Add(user);
            SaveUsers();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registration successful.");
            Console.ResetColor();
        }
        public void ShowProfile()
        {
            Console.Clear();
            if (_currentUser == null)
                throw new Exception("No user is currently logged in.");

            Console.WriteLine("Current User Profile:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(_currentUser);
            Console.ResetColor();
            Console.ReadLine();
        }
        public void UpdateBalance()
        {
            Console.Clear();
            Console.Write("Enter amount to deposit: ");
            string input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                double balance = double.Parse(input);

                if (balance <= 0)
                    throw new Exception("Amount must be greater than zero.");

                _currentUser.Balance += balance;
                SaveUsers();


                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Balance updated successfully!");
                Console.Write($"New Balance: ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{_currentUser.Balance:C2}");
                Console.ResetColor();
            }
            else
            {
                throw new Exception("Amount cannot be empty.");
            }
        }
        public void UpdateProfile()
        {
            Console.Clear();
            if (_currentUser == null)
                throw new Exception("No user is currently logged in.");

            Console.WriteLine(_currentUser);

            Console.Write("New username (leave empty to keep current): ");
            string newUsername = Console.ReadLine()?.Trim();

            Console.Write("New email (leave empty to keep current): ");
            string newEmail = Console.ReadLine()?.Trim();

            string newPassword = ReadPasswordMasked("Enter new Password (leave blank to keep current): ");

            if (!string.IsNullOrWhiteSpace(newUsername))
            {
                if (_users.Any(u => u.Username == newUsername && u.UserId != _currentUser.UserId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Update failed: username already in use.");
                    Console.ResetColor();
                    return;
                }

                _currentUser.Username = newUsername;
            }

            if (!string.IsNullOrWhiteSpace(newEmail))
            {
                if (_users.Any(u => u.Email == newEmail && u.UserId != _currentUser.UserId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Update failed: email already in use.");
                    Console.ResetColor();
                    return;
                }

                _currentUser.Email = newEmail;
            }

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword.Length < 6)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Update failed: password too short.");
                    Console.ResetColor();
                    return;
                }

                _currentUser.Password = newPassword;
            }

            SaveUsers();
            Console.WriteLine("Profile updated successfully.");
        }

        public static void BuyProduct(Product item)
        {
            if (_currentUser == null)
                throw new Exception("No user is logged in.");

            if (item == null)
                throw new Exception("Product does not exist.");

            if (_currentUser.Role == Role.Admin)
                throw new Exception("Admins cannot buy products.");

            if (item.Stock <= 0)
                throw new Exception("Product is out of stock.");

            if (_currentUser.Balance < item.Price)
                throw new Exception("Insufficient balance.");

            item.Stock--;
            _currentUser.Balance -= item.Price;

            var productInCart = _currentUser.Cart.FirstOrDefault(p => p.ProductId == item.ProductId);

            if (productInCart != null)
            {
                productInCart.IncreaseQuantity();
            }
            else
            {
                _currentUser.Cart.Add(new Order(
                       item.ProductId,
                       item.Name,
                       item.Price,
                       1
                ));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Product bought successfully!");
            Console.ResetColor();
            SaveUsers();
        }
    }
}
