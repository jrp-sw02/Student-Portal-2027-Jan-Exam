using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;


/// <summary>
/// Summary description for NationalAverage
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "https://student.nielit.gov.in/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class NationalAverage : System.Web.Services.WebService
{
    //public AuthUser User;

    public NationalAverage()
    {
    }


    [WebMethod]
    //[SoapHeader("User",Required=true)]
    public string HelloWorld()
    {
        //if (User != null)
        //{
        //    if (User.IsValid())
        //    {
                return "Hello World";
        //    }
        //    else
        //        return "Invalid User";
        //}
        //else
        //    return "Please Give User Details";

    }


    [WebMethod]
    //[SoapHeader("User",Required=true)]
    public void NationalAverageCourseWise(string input_course, int input_month, int input_year, string userName, string passWord)
    {
        //if (User!=null)
        //{
        string JSONresult = "{}";
        if (userName == "Nielit" && passWord == "nielit@321$")
        {

            try
            {
                string conn = System.Configuration.ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;



                SqlConnection objsqlconn = new SqlConnection(conn);
                //set stored procedure name
                objsqlconn.Open();

                string spName = @"dbo.[NationalAverageCourse]";


                SqlCommand cmd = new SqlCommand(spName, objsqlconn);


                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@course";
                param1.SqlDbType = SqlDbType.VarChar;
                param1.Value = input_course;


                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@exam_month";

                param2.SqlDbType = SqlDbType.Int;
                param2.Value = input_month;


                SqlParameter param3 = new SqlParameter();
                param3.ParameterName = "@exam_year";
                param3.SqlDbType = SqlDbType.Int;
                param3.Value = input_year;

                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);
                cmd.Parameters.Add(param3);

                DataTable dt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


                JSONresult = JsonConvert.SerializeObject(dt);

                //JavaScriptSerializer js = new JavaScriptSerializer();
                //string json = js.Serialize(CourseDtList);

                Context.Response.ContentType = "application/json";
                Context.Response.Write(JSONresult);

            }
            catch (Exception e)
            {
                //Console.Write(e.Message);
                //JSONresult= JsonConvert.SerializeObject("Error: " + e.Message);
            }
            //return JSONresult;
        }
        else
        {
            JSONresult = "Please Provide Correct User Details";

            //JavaScriptSerializer js = new JavaScriptSerializer();
            //string json = js.Serialize(CourseDtList);


            Context.Response.ContentType = "application/json";
            Context.Response.Write(JSONresult);
        }

        //}
        //else
        //    throw new SoapException("Unauthorized", SoapException.ClientFaultCode);
    }

    [WebMethod]
    //[SoapHeader("User", Required = true)]
    public void NationalAverageInstituteWise(int instituteID, string input_course, string userName, string passWord)
    {

        //if (User != null)
        //{
        string JSONresult = "{}";
        if (userName == "Nielit" && passWord == "nielit@321$")
        {

            try
            {
                string conn = System.Configuration.ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;



                SqlConnection objsqlconn = new SqlConnection(conn);
                //set stored procedure name
                objsqlconn.Open();

                string spName = @"dbo.[NationalAverageInst]";


                SqlCommand cmd = new SqlCommand(spName, objsqlconn);

                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@instId";
                param1.SqlDbType = SqlDbType.Int;
                param1.Value = instituteID;

                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@course";
                param2.SqlDbType = SqlDbType.VarChar;
                param2.Value = input_course;


                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);


                DataTable dt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);



                JSONresult = JsonConvert.SerializeObject(dt);

                //JavaScriptSerializer js = new JavaScriptSerializer();
                //string json = js.Serialize(InstDtList);

                Context.Response.ContentType = "application/json";
                Context.Response.Write(JSONresult);

            }
            catch (Exception e)
            {
                //Console.Write(e.Message);
                //JSONresult = JsonConvert.SerializeObject("Error: " + e.Message);
            }

            //return JSONresult;
        }
        else
        {
            JSONresult = "Please Provide Correct User Details";

            //JavaScriptSerializer js = new JavaScriptSerializer();
            //string json = js.Serialize(CourseDtList);

            Context.Response.ContentType = "application/json";
            Context.Response.Write(JSONresult);
        }
        //}
        //else
        //    throw new SoapException("Unauthorized", SoapException.ClientFaultCode);

    }

}
