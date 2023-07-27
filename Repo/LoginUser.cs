using System;
using System.Collections.Generic;
using PummaApplication.Models;
using PummaApplication.DBConnection;
using System.Data.SqlClient;
using System.Data;

namespace PummaApplication.Repo
{
    public class LoginUser : ILoginUser
    {

        public LoginResult SignInUser(Login user)
        {

            //Database COnnection
            DBConnect db = new DBConnect();
            SqlConnection con = db.GetSqlConnection();
            SqlCommand cmd = db.GetSqlCommand("sp_login_user_details", con);

            DataTable dt = null;
            List<LoginResult> loginDetails = new List<LoginResult>();
            LoginResult cobj = new LoginResult();

            try
            {
                db.SetSqlLoginParameters(user, cmd);
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                string savedStatus = cmd.Parameters["@v_saved"].Value.ToString();
                if (savedStatus == "Invalid")
                {
                    cobj.status = false;
                    cobj.message = "Invalid Username OR Paswword";
                    return cobj;
                }
                else
                {
                    cobj.status = true;
                    cobj.message = savedStatus;
                }

                foreach (DataRow row in dt.Rows)
                {
                    cobj.lud_user_id = Convert.ToInt32(row["lud_user_id"]);
                    cobj.lud_user_firstname = row["lud_user_firstname"].ToString();
                    cobj.lud_user_lastname = row["lud_user_lastname"].ToString();
                    cobj.lud_user_street_add1 = row["Address Line"].ToString();
                    cobj.lud_user_city = row["lud_user_city"].ToString();
                    cobj.lud_user_state = row["lud_user_state"].ToString();
                    cobj.lud_user_zipcode = row["lud_user_zipcode"].ToString();
                    cobj.lud_user_primary_phoneno = row["lud_user_primary_phoneno"].ToString();
                    cobj.lud_user_primary_alteno = row["lud_user_primary_alteno"].ToString();
                    cobj.lud_user_faxno = row["lud_user_faxno"].ToString();
                    cobj.lud_user_email = row["lud_user_email"].ToString();
                }
                return cobj;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                return cobj;
            }
            finally
            {
                con.Close();
            }
        }
        
    }
}