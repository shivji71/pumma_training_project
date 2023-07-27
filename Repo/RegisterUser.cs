using System;
using System.Collections.Generic;
using PummaApplication.Models;
using System.Data.SqlClient;
using System.Data;
using PummaApplication.DBConnection;
using System.Linq;

namespace PummaApplication.Repo
{
    public class RegisterUser : IRegister
    {
        DBConnect db = new DBConnect();

        public List<Register> GetAllUsers(int user_id)
        {
            //string[] fruits = { "apple", "mango", "orange", "passionfruit", "grape" };

            // Determine whether any string in the array is longer than "banana".
            //string longestName =
            //    fruits.Aggregate("banana",
            //                    (longest, next) =>
            //                        next.Length > longest.Length ? next : longest,
            //                    // Return the final result as an upper case string.
            //                    fruit => fruit.ToUpper());

            //string sentence = "the quick brown fox jumps over the lazy dog";

            //// Split the string into individual words.
            //string[] words = sentence.Split(' ');

            //// Prepend each word to the beginning of the
            //// new sentence to reverse the word order.
            //string reversed = words.Aggregate((workingSentence, next) =>
            //                                      next + " " + workingSentence);

            //Console.WriteLine(reversed);


            //Product[] products = { new Product { Name = "apple", Code = 9 },
            //           new Product { Name = "orange", Code = 4 },
            //           new Product { Name = "apple", Code = 9 },
            //           new Product { Name = "lemon", Code = 12 } };

            //// Exclude duplicates.

            //IEnumerable<Product> noduplicates =
            //    products.Distinct(new ProductComparer());

            //foreach (var product in noduplicates)
            //    System.Diagnostics.Debug.WriteLine(product.Name + " " + product.Code);


            //Database COnnection
            SqlConnection con = db.GetSqlConnection();
            SqlCommand cmd = db.GetSqlCommand("sp_get_user_details", con);
            DataTable dt = null;
            List<Register> AllUsersList = new List<Register>();
            Register allUsers = new Register();

            try
            {
                cmd.Parameters.AddWithValue("@user_id ", user_id);
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Data Table is Empty");
                    return null;
                }

                foreach (DataRow row in dt.Rows)
                {
                    Register register = new Register();
                    register.lud_user_id = Convert.ToInt32(row["lud_user_id"]);
                    register.lud_user_username = row["lud_user_username"].ToString();
                    register.lud_user_firstname = row["lud_user_firstname"].ToString();
                    register.lud_user_lastname = row["lud_user_lastname"].ToString();
                    register.lud_user_email = row["lud_user_email"].ToString();
                    register.lud_user_street_add1 = row["lud_user_street_add1"].ToString();
                    register.lud_user_street_add2 = row["lud_user_street_add2"].ToString();
                    register.lud_user_zipcode = row["lud_user_zipcode"].ToString();
                    register.lud_user_city = row["lud_user_city"].ToString();
                    register.lud_user_state = row["lud_user_state"].ToString();
                    register.lud_user_primary_phoneno = row["lud_user_primary_phoneno"].ToString();
                    register.lud_user_primary_alteno = row["lud_user_primary_alteno"].ToString();
                    register.lud_user_faxno = row["lud_user_faxno"].ToString();
                    if(row["lud_user_password"].ToString().Length > 20 ) {
                        //decrypt the encrypted password 
                        register.lud_user_password = AesEncryption.Decrypt(row["lud_user_password"].ToString(), "chhailEncryptionKey");
                        register.lud_user_re_password = AesEncryption.Decrypt(row["lud_user_re_password"].ToString(), "chhailEncryptionKey");
                    }
                    else
                    {
                        register.lud_user_password =  row["lud_user_password"].ToString();
                        register.lud_user_re_password = row["lud_user_re_password"].ToString();

                    }

                    register.lud_activelab = Convert.ToInt32(row["lud_activelab"]);
                    AllUsersList.Add(register);
                }


                // Assign the populated list to the custlist object
                allUsers.Get_Users_List = AllUsersList;

                return AllUsersList;
            }
            catch (Exception ex)
            {
                allUsers.result.message = ex.Message;
                AllUsersList.Add(allUsers);
                return AllUsersList;
            }
            finally
            {
                con.Close();
            }
        }

        public RegisterResult RegisterNewUser(Register register)
        {
            //Database COnnection
            SqlConnection con = db.GetSqlConnection();
            SqlCommand cmd = db.GetSqlCommand("sp_save_user_data", con);
            var result = new RegisterResult();

            try
            {

                db.SetSqlCreateNewUserParameters(register, cmd);
                con.Open();
                cmd.ExecuteNonQuery();

                string savedStatus = cmd.Parameters["@v_saved"].Value.ToString();
                if (savedStatus == "Yes")
                {
                    result.status = true;
                    result.message = "User Registered Succesfully";
                    System.Diagnostics.Debug.WriteLine(result.message);
                    return result;
                }
                else if (savedStatus == "INVALID")
                {
                    result.status = false;
                    result.message = "User Name Already Exist";
                    return result;
                }

                return result;

            }
            catch (SqlException ex)
            {
                result.status = false;
                result.message = ex.Message;
                System.Diagnostics.Debug.WriteLine(result.message);
                return result;
            }
            finally
            {
                con.Close();
            }
        }

        public RegisterResult UpdateUser(Register register)
        {
            //Database COnnection
            SqlConnection con = db.GetSqlConnection();
            SqlCommand cmd = db.GetSqlCommand("sp_update_user_data", con);
            var result = new RegisterResult();

            try
            {

                db.SetSqlUpdateUserParameters(register, cmd);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                string savedStatus = cmd.Parameters["@v_saved"].Value.ToString();

                result.status = true;
                result.message = savedStatus;
                System.Diagnostics.Debug.WriteLine(result.message);

                return result;
            }
            catch (SqlException ex)
            {
                result.status = false;
                result.message = ex.Message;
                return result;
            }
            finally
            {
                con.Close();
            }
        }

        public RegisterResult DeleteUser(int user_id, int deletedby)
        {
            //Database COnnection
            SqlConnection con = db.GetSqlConnection();
            SqlCommand cmd = db.GetSqlCommand("sp_delete_user_details", con);
            var result = new RegisterResult();

            try
            {

                cmd.Parameters.AddWithValue("@user_id", user_id);
                cmd.Parameters.AddWithValue("@deletedby", deletedby);
                SqlParameter outputParameter = new SqlParameter("@v_user_deleted", SqlDbType.VarChar, 100);
                outputParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParameter);
                con.Open();
                cmd.ExecuteNonQuery();

                string savedStatus = cmd.Parameters["@v_user_deleted"].Value.ToString();
                if (savedStatus.ToLower().Contains("delete"))
                {
                    result.status = true;
                    result.message = savedStatus;
                }
                else
                {
                    result.status = false;
                    result.message = savedStatus;
                }

                return result;
            }
            catch (SqlException ex)
            {
                result.status = false;
                result.message = ex.Message;
                System.Diagnostics.Debug.WriteLine(result.message);
                return result;
            }
            finally
            {
                con.Close();
            }
        }
    }


    //methods for remove duplicate data from array of object 
    public class Product
    {
        public string Name { get; set; }
        public int Code { get; set; }
    }

    // Custom comparer for the Product class
    class ProductComparer : IEqualityComparer<Product>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Product x, Product y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Code == y.Code && x.Name == y.Name;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Product product)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(product, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = product.Code.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName ^ hashProductCode;
        }
    }
}