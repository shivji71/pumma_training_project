using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PummaApplication
{
    public class Program
    {
       public static void Main(String[] args)
        {
            

            /*
            SqlConnection con = null;
            try
            {
                // Creating Connection  
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DEMO_TRAINING_1"].ToString());
                // writing sql query  
                SqlCommand cm = new SqlCommand("create table loginTable(id int not null, login varchar(100), password varchar(50))", con);
                con.Open();
                cm.ExecuteNonQuery();
                Console.WriteLine("Table created Successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("OOPs, something went wrong." + e);
            } 
            finally
            {

                con.Close();
            }
            */
        }

        public void Sd() {
            Console.WriteLine("Table created Successfully");
        }
    }
}