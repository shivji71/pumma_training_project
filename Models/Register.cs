using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PummaApplication.Models
{
    public class Register
    {
        public int lud_user_id { get; set; }
        public string lud_user_firstname { get; set; }
        public string lud_user_lastname { get; set; }
        public string lud_user_street_add1 { get; set; }
        public string lud_user_street_add2 { get; set; }
        public string lud_user_zipcode { get; set; }
        public string lud_user_city { get; set; }
        public string lud_user_state { get; set; }
        public string lud_user_primary_phoneno { get; set; }
        public string lud_user_primary_alteno { get; set; }
        public string lud_user_faxno { get; set; }
        public string lud_user_email { get; set; }
        public string lud_user_username { get; set; }
        public string lud_user_password { get; set; }
        public string lud_user_re_password { get; set; }
        public int login_user_id { get; set; }
        public int lud_activelab { get; set; }
        public List<Register> Get_Users_List { get; internal set; }
        public RegisterResult result { get; set; }
    }
    public class RegisterResult
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}