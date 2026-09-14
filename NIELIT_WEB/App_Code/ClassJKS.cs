using System;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Web.Security;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

/// <summary>
/// Summary description for ClassJKS
/// </summary>
public class ClassJKS
{
	public ClassJKS()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static bool check_pdf_file(string str)
    {
        if (Regex.IsMatch(str.ToLower(), @"([a-zA-Z0-9\s_\\.\-:])+(.pdf)$"))
            return true;
        else
            return false;
    }
    public static bool check_upload_file_name(string str)
    {
        StringBuilder sb = new StringBuilder(str.Length);
        bool result = true;
        foreach (char c in str)
        {
            if (char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.' || char.IsWhiteSpace(c))
            {
                sb.Append(c);
            }
            else
            {
                result = false;
                break;
            }
        }
        return result;
    }
    public static SqlConnection get_con()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        return con;
    }
    public static SqlConnection get_con2()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        return con;
    }
    public static Boolean InsertUpdateData(SqlCommand cmd)
    {
        SqlConnection con = ClassJKS.get_con2();
        cmd.Connection = con;

        using (con)
        {
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception err)
            {
                return false;
                throw new ApplicationException("Data error - " + err.ToString());
            }
            finally
            {
                cmd.Dispose();
            }
        }
    }
    public static DataSet ReturnDataset(SqlCommand cmd)
    {
        SqlConnection con = ClassJKS.get_con2();
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        cmd.Connection = con;
        DataSet ds = new DataSet();
        using (con)
        {
            try
            {
                con.Open();
                sda.Fill(ds);
                return ds;
            }
            catch (Exception err)
            {
                throw new ApplicationException("Data error - " + err.ToString());
            }
            finally
            {
                cmd.Dispose();
                sda.Dispose();
                ds.Dispose();
            }
        }
    }
    public static DataSet JKS_GetCourseCategoryList()
    {
        SqlCommand cmd = new SqlCommand("JKS_GetCourseCategoryList");
        cmd.CommandType = CommandType.StoredProcedure;
        return ClassJKS.ReturnDataset(cmd);
    }
    public static DataSet JKS_GetCoursesListForCategory(int Course_Category_ID)
    {
        SqlCommand cmd = new SqlCommand("JKS_GetCoursesListForCategory");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@Course_Category_ID", SqlDbType.Int).Value = Course_Category_ID;
        return ClassJKS.ReturnDataset(cmd);
    }
    public static DataSet JKS_GetCourseIDFiles(int course_id)
    {
        SqlCommand cmd = new SqlCommand("JKS_GetCourseIDFiles");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@course_id", SqlDbType.Int).Value = course_id;
        return ClassJKS.ReturnDataset(cmd);
    }
}