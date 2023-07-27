using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PummaApplication.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace PummaApplication.Repo
{
    public class CityAndStateZip : ICityAndStateZip
    {



        public List<CitiesAndState> GetAllCitiesAndState(string zipcd)
        {
            SqlConnection con = null;
            DataTable dt = null;
            CitiesAndState custlist = new CitiesAndState();
            List<CitiesAndState> cityStateList = new List<CitiesAndState>();

            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DEMO_TRAINING_1"].ToString());
                SqlCommand cmd = new SqlCommand("sp_get_city_state_county_from_zipcode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@zipcd", zipcd);
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
                    CitiesAndState cobj = new CitiesAndState();
                    cobj.mscz_state_city_statename = row["mscz_state_city_statename"].ToString();
                    cobj.mscz_state_city_city = row["mscz_state_city_city"].ToString();
                    cityStateList.Add(cobj);
                }

                // Assign the populated list to the custlist object
                custlist.CitiesAndStateList = cityStateList;

                return cityStateList;
            }
            catch(Exception ex)
            {
                custlist.result.message = ex.Message;
                cityStateList.Add(custlist);
                return cityStateList;
            }
            finally
            {
                con.Close();
            }
        }

       
    }
}