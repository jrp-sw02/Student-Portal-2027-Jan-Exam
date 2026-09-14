using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Net;
using System.Text;

/// <summary>
/// Summary description for NIELITMIS
/// </summary>
public class NIELITMIS
{
	public NIELITMIS()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public Int32 LnktoCenter(Int32 LnkID)
    {
        Int32 LnktoCenter = 0;
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            DataTable DT = new DataTable();


            con.Open();

            using (SqlCommand Cmm = new SqlCommand("sp_LnktoCenter", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                Cmm.Parameters.AddWithValue("@LnktoCenter", LnkID);
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);

                LnktoCenter = Convert.ToInt32(DT.Rows[0]["ID"]);
            }



        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }

        return LnktoCenter;

    }
	 public DataTable DistrictsExists(Int32 DistrictId)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAspirationalDistrict", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@DistrictId", SqlDbType.Int));
                    cmd.Parameters["@DistrictId"].Value = DistrictId;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }
}