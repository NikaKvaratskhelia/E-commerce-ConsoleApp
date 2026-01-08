using E_commerce.Models;

namespace E_commerce.Services.UserServices
{
    internal interface IUserServices
    {
        public void Register();
        public User Login();
        public User Logout();
        public void ShowProfile();
        public void UpdateProfile();
        public void UpdateBalance();
    }
}
