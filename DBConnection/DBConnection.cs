using PummaApplication.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PummaApplication.DBConnection
{
    public class DBConnect
    {

        public SqlConnection GetSqlConnection()
        {
            SqlConnection con = null;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DEMO_TRAINING_1"].ToString());
            //SqlCommand cmd = new SqlCommand(SPName, con);
            //cmd.CommandType = CommandType.StoredProcedure;
            return con;
        }
        public SqlCommand GetSqlCommand(string SPName, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }
        public void SetSqlLoginParameters(Login user, SqlCommand cmd)
        {
            //Encrypted the user password using AesEncrytion 

            string encryptedPassword = AesEncryption.Encrypt(user.lud_user_password, "chhailEncryptionKey");

            cmd.Parameters.AddWithValue("@user_name", user.lud_user_username);
            cmd.Parameters.AddWithValue("@password", encryptedPassword);
            SqlParameter outputParameter = new SqlParameter("@v_saved", SqlDbType.VarChar, 10);
            outputParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParameter);

        }
        public void SetSqlCreateNewUserParameters(Register register, SqlCommand cmd)
        {
            ////Encrypted the user password using AesEncryption
            string encryptedPassword = AesEncryption.Encrypt(register.lud_user_password, "chhailEncryptionKey");
            string encryptedRePassword = AesEncryption.Encrypt(register.lud_user_re_password, "chhailEncryptionKey");

            cmd.Parameters.AddWithValue("@user_username", register.lud_user_username);
            cmd.Parameters.AddWithValue("@user_password", encryptedPassword);
            cmd.Parameters.AddWithValue("@user_re_password", encryptedRePassword);
            cmd.Parameters.AddWithValue("@user_firstname", register.lud_user_firstname);
            cmd.Parameters.AddWithValue("@user_lastname", register.lud_user_lastname);
            cmd.Parameters.AddWithValue("@user_street_add1", register.lud_user_street_add1);
            cmd.Parameters.AddWithValue("@user_street_add2", register.lud_user_street_add2);
            cmd.Parameters.AddWithValue("@user_zipcode", register.lud_user_zipcode);
            cmd.Parameters.AddWithValue("@user_city", register.lud_user_city);
            cmd.Parameters.AddWithValue("@user_state", register.lud_user_state);
            cmd.Parameters.AddWithValue("@user_primary_phoneno", register.lud_user_primary_phoneno);
            cmd.Parameters.AddWithValue("@user_primary_alteno", register.lud_user_primary_alteno);
            cmd.Parameters.AddWithValue("@user_faxno", register.lud_user_faxno);
            cmd.Parameters.AddWithValue("@user_email", register.lud_user_email);
            SqlParameter outputParameter = new SqlParameter("@v_saved", SqlDbType.VarChar, 10);
            outputParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParameter);

        }

        public void SetSqlUpdateUserParameters(Register register, SqlCommand cmd)
        {
            ////Encrypted the user password using AesEncryption
            string encryptedPassword = AesEncryption.Encrypt(register.lud_user_password, "chhailEncryptionKey");
            string encryptedRePassword = AesEncryption.Encrypt(register.lud_user_re_password, "chhailEncryptionKey");

            cmd.Parameters.AddWithValue("@user_id", register.lud_user_id);
            cmd.Parameters.AddWithValue("@login_user_id", register.login_user_id);
            cmd.Parameters.AddWithValue("@user_username", register.lud_user_username);
            cmd.Parameters.AddWithValue("@user_firstname", register.lud_user_firstname);
            cmd.Parameters.AddWithValue("@user_lastname", register.lud_user_lastname);
            cmd.Parameters.AddWithValue("@user_street_add1", register.lud_user_street_add1);
            cmd.Parameters.AddWithValue("@user_street_add2", register.lud_user_street_add2);
            cmd.Parameters.AddWithValue("@user_zipcode", register.lud_user_zipcode);
            cmd.Parameters.AddWithValue("@user_city", register.lud_user_city);
            cmd.Parameters.AddWithValue("@user_state", register.lud_user_state);
            cmd.Parameters.AddWithValue("@user_primary_phoneno", register.lud_user_primary_phoneno);
            cmd.Parameters.AddWithValue("@user_primary_alteno", register.lud_user_primary_alteno);
            cmd.Parameters.AddWithValue("@user_faxno", register.lud_user_faxno);
            cmd.Parameters.AddWithValue("@user_email", register.lud_user_email);
            cmd.Parameters.AddWithValue("@user_password", encryptedPassword);
            cmd.Parameters.AddWithValue("@user_re_password", encryptedRePassword);
            SqlParameter outputParameter = new SqlParameter("@v_saved", SqlDbType.VarChar, 100);
            outputParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParameter);

        }
    }
}