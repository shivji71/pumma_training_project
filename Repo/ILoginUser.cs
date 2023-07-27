using PummaApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PummaApplication.Repo
{
    interface ILoginUser
    {
       LoginResult SignInUser(Login user);

    }
}
