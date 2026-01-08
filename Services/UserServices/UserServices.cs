using E_commerce.Enums;
using E_commerce.Models;
using System.Text.Json;

namespace E_commerce.Services.UserServices
{
    internal class UserServices : IUserServices
    {
        public User? _currentUser = null;

        private static List<User> _users = new List<User>();
        public static string _path = "users.json";
        public static void LoadUsers()
        {
            string json = File.ReadAllText(_path);
            var users = JsonSerializer.Deserialize<List<User>>(json);

            if (users != null) _users = users;
        }
        public static void SaveUsers()
        {
            string json = JsonSerializer.Serialize(_users);
            File.WriteAllText(_path, json);
        }
        public User Login()
        {
            Console.Write("Enter User Email: ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email)) throw new Exception("Email cannot be empty.");

            Console.WriteLine();

            Console.Write("Enter User Password: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Password cannot be empty.");

            User user = _users.Find(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                _currentUser = user;
                _currentUser.IsActive = true;
                Console.WriteLine("Login Successful!");
                return _currentUser;
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
                return null;
            }
        }
        public User Logout()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("No user is currently logged in.");
                return null;
            }

            _currentUser.IsActive = false;
            _currentUser = null;
            Console.WriteLine("Logged out successfully!");
            return _currentUser;
        }
        public void Register()
        {
            Console.Write("Username: ");
            var username = Console.ReadLine()?.Trim();

            Console.Write("Email: ");
            var email = Console.ReadLine()?.Trim();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Registration failed: fields cannot be empty.");
                return;
            }

            if (_users.Any(u => u.Username == username))
            {
                Console.WriteLine("Registration failed: username already exists.");
                return;
            }

            if (_users.Any(u => u.Email == email))
            {
                Console.WriteLine("Registration failed: email already exists.");
                return;
            }

            if (password.Length < 6)
            {
                Console.WriteLine("Registration failed: password too short.");
                return;
            }

            User user = new User(
                username,
                password,
                email,
                Role.Customer,
                0
            );

            _users.Add(user);
            SaveUsers();
            Console.WriteLine("Registration successful.");
        }
        public void ShowProfile()
        {
            if (_currentUser == null)
                throw new Exception("No user is currently logged in.");

            Console.WriteLine(_currentUser);
        }
        public void UpdateBalance()
        {
            Console.Write("Enter amount to deposit: ");
            string input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                double balance = double.Parse(input);
            }
            else
            {
                throw new Exception("Amount cannot be empty.");
            }
        }
        public void UpdateProfile()
        {
            if (_currentUser == null)
                throw new Exception("No user is currently logged in.");

            Console.WriteLine(_currentUser);

            Console.Write("New username (leave empty to keep current): ");
            string newUsername = Console.ReadLine()?.Trim();

            Console.Write("New email (leave empty to keep current): ");
            string newEmail = Console.ReadLine()?.Trim();

            Console.Write("New password (leave empty to keep current): ");
            string newPassword = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newUsername))
            {
                if (_users.Any(u => u.Username == newUsername && u.UserId != _currentUser.UserId))
                {
                    Console.WriteLine("Update failed: username already in use.");
                    return;
                }

                _currentUser.Username = newUsername;
            }

            if (!string.IsNullOrWhiteSpace(newEmail))
            {
                if (_users.Any(u => u.Email == newEmail && u.UserId != _currentUser.UserId))
                {
                    Console.WriteLine("Update failed: email already in use.");
                    return;
                }

                _currentUser.Email = newEmail;
            }

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword.Length < 6)
                {
                    Console.WriteLine("Update failed: password too short.");
                    return;
                }

                _currentUser.Password = newPassword;
            }

            SaveUsers();
            Console.WriteLine("Profile updated successfully.");
        }
    }
}
