using PummaApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PummaApplication.Repo
{
    interface IRegister
    {
        RegisterResult RegisterNewUser(Register register);
        List<Register> GetAllUsers(int user_id);
        RegisterResult UpdateUser(Register register);
        RegisterResult DeleteUser(int user_id, int login_user_id);
    }
}
