// Project Models
using beltexam2.ViewModels;

namespace beltexam2.Models
{
    public class UserWrapper
    {
        public Login Login { get; set; }
        public User User { get; set; }
        public UserWrapper(User user, Login login)
        {
            Login = login;
            User = user;
        }
    }

}