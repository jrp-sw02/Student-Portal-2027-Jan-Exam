using DocumentFormat.OpenXml.Office.Word;
using EConnect.NIELIT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data ;
using System.Data.SqlClient ;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;


/// <summary>
/// Summary description for utility
/// </summary>
public class utility
{
    public utility()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static DataSet executeProcedure(string vProcName, parameters vParameters)
    {
        DataSet ds = new DataSet();
        try
        {
            string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlDataAdapter da = new SqlDataAdapter();

            using (SqlConnection con = new SqlConnection(CS))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = vProcName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.Connection = con;

                    if (vParameters.count != 0)
                    {

                        PropertyInfo[] properties = typeof(parameters).GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.GetValue(vParameters) != null && property.Name != "count" && property.Name != "TypeId")
                            {
                                Type propertyType = property.PropertyType;
                                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    propertyType = propertyType.GetGenericArguments()[0];
                                }

                                string dataType = "";
                                string Stringvalue = "";
                                Int64 intValue = 0;
                                switch (propertyType.Name)
                                {
                                    case "Int64":
                                    case "Int32":
                                        dataType = "SqlDbType.BigInt";
                                        intValue = Convert.ToInt64(property.GetValue(vParameters));
                                        break;
                                }
                                string vName = "@" + property.Name;
                                string x = property.GetValue(vParameters).ToString();
                                cmd.Parameters.Add(vName, dataType).Value = intValue;
                            }
                        }

                    }

                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
            }

        }
        catch (Exception ex)
        {
        }
        finally
        {

        }
        return ds;
    }


    public static int CheckCandEmailMobile(
        string pMobileno,
        string pEmailID,
        int pExamCycleID,
        int pCourseType,
        int? projectId)
    {
        using (SqlConnection con = new SqlConnection(
               ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("checkCandEmailMobile", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@pMobileno", SqlDbType.VarChar, 20)
                    .Value = string.IsNullOrEmpty(pMobileno) ? "-99" : pMobileno;

                cmd.Parameters.Add("@pEmailID", SqlDbType.VarChar, 200)
                    .Value = string.IsNullOrEmpty(pEmailID) ? "-99" : pEmailID;

                cmd.Parameters.Add("@pExamCycleID", SqlDbType.Int).Value = pExamCycleID;
                cmd.Parameters.Add("@pCourseType", SqlDbType.Int).Value = pCourseType;

                if (projectId == null)
                    cmd.Parameters.Add("@projectID", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@projectID", SqlDbType.Int).Value = projectId;

                con.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);  // 0/1/2/3 etc.
            }
        }
    }


    public static int checkprojectduplicate(string vname, string vfathername, string vgender ,DateTime vDOB, Int64 vcourseID, Int64 vprojectID, String vnumber, Int32 isHO)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("checkprojectduplicate", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 20).Value =  vname;
                    cmd.Parameters.Add("@FatherName", SqlDbType.VarChar, 200).Value =  vfathername;
                    cmd.Parameters.Add("@Gender", SqlDbType.VarChar, 20).Value = vgender;
                    cmd.Parameters.Add("@DOB", SqlDbType.DateTime).Value = vDOB;
                    cmd.Parameters.Add("@CourseID", SqlDbType.BigInt).Value = vcourseID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.BigInt).Value = vprojectID;
                    cmd.Parameters.Add("@Number", SqlDbType.VarChar, 100).Value = vnumber;
                    cmd.Parameters.Add("@isHO", SqlDbType.Int).Value = isHO;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

}