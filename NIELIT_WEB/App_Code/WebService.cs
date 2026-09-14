using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Configuration ;
using RestSharp;
using Newtonsoft.Json;

using System.Data.Objects;

using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections;

/// <summary>
/// Summary description for WebService
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "https://student.nielit.gov.in/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    public WebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string DBEntity(string body)
    {
        try{
             string json_data = JsonConvert.SerializeObject(body);

                //Print the Json object
                // ShowAlert(json_data);
                //string result = GetJSON ("Santosh Bhardwaj","27/11/1974","F",finalResponse ,"206927461266");
                //var client = new RestClient("https://sp.epramaan.in:4003/nielitwebservice");
                var client = new RestClient("https://sp.epramaan.in:4003");
                //var request1 = new RestRequest();
                var request1 = new RestRequest("/nielitwebservice/validate", Method.POST);
                

                //request1.Method = Method.POST;
                //    request1.RequestFormat = RestSharp .DataFormat .Json ;
                request1.AddHeader("Accept", "application/json");

                request1.Parameters.Clear();
                request1.AddParameter("application/json", json_data, ParameterType.RequestBody);
                request1.Timeout = 50000;
                IRestResponse  response1 = client.Execute(request1);
                var url = response1.ResponseUri;
               // return(url.ToString ());
                var content = response1.Content; // raw content as string  
                //ShowAlert("content"+content);
                //var content = "";
                bioReturn biometricReturn = JsonConvert.DeserializeObject<bioReturn>(content);
                return("status"+biometricReturn.status);
                bool statusId = false;
                string errorCode = "";
                if (biometricReturn.status == "True")
                {
                    statusId = true;
                    errorCode = "Null";
                }
                if (biometricReturn.status == "False")
                {
                    statusId = false;
                    errorCode = biometricReturn.errorCode;
                }

                int result = saveResponse(statusId, biometricReturn.transactionID, errorCode);

            
        }
        catch (Exception ex)
        {
            return("Exception :-" + ex.Message);
        }
        //string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        //SqlConnection con = new SqlConnection(CS);
        //string sql = "INSERT into Test (Name) VALUES (@name)";

        //SqlCommand cmd = new SqlCommand(sql, con);
        //con.Open();
        //cmd.Parameters.Add("@name", SqlDbType.VarChar, 30).Value = name;
        //cmd.ExecuteNonQuery();

        //con.Close();

    }
    protected int saveResponse(bool status, string transID, string errCode)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                aadhaarResponse aadharResp = new aadhaarResponse();
                aadharResp.applicationNo = "Test";
                aadharResp.courseCategoryID = 1;
                aadharResp.courseID = 1;
                aadharResp.status = status;
                aadharResp.transactionID = transID;
                if (errCode != "Null")
                    aadharResp.errorCode = errCode;
                else
                {
                    if (status)
                        aadharResp.Remarks = "Success";
                }

                if (errCode.ToString().StartsWith("REQ") || errCode.ToString().StartsWith("100"))
                    aadharResp.Remarks = "Invalid Name, Date of birth or Gender";
                if (errCode.ToString().StartsWith("SYS"))
                    aadharResp.Remarks = "System Error";
                if (errCode.ToString().StartsWith("998"))
                    aadharResp.Remarks = "Invalid Aadhaar number";


                aadharResp.enterByID = 99;
                aadharResp.enterDate = System.DateTime.Today;

                context.aadhaarResponses.Add(aadharResp);
                context.SaveChanges();
                return (1);
            }
        }
        catch (Exception ex)
        {
           // return(ex.Message);
            return (0);
        }
    }
    public class bioReturn
    {

        public string status { get; set; }
        public string transactionID { get; set; }
        public string errorCode { get; set; }



    };
    
}
