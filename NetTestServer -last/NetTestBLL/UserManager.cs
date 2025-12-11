using NetTestDAL;
using NetTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTestBLL
{
    public class UserManager
    {
        public string login(UserClass user)
        {
            UserService userService = new UserService();
            return userService.login(user);
        }
    }
}
