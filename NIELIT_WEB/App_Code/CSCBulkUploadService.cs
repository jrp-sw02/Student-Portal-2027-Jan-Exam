using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
//using System.Data.OleDb;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;
using System.Web;
using RestSharp;


public class CSCBulkUploadService
{
	public CSCBulkUploadService()
	{
		
	}

    public static Dictionary<string, dynamic> CSCBulkServiceGetData(string entryDate)
    {
        // it is used for local host Link
       // var client = new RestClient("http://localhost:18447/CCC_Registration/CSCBulkDataUpload.asmx/GetCSCBulkUploadData");

        // It is used for Live Server Link
        var client = new RestClient("https://dlcaccr.nielit.gov.in/CSCBulkDataUpload.asmx/GetCSCBulkUploadData");
      
        client.Timeout = -1;
        var request = new RestRequest(Method.POST);
       request.AddParameter("entryDate", entryDate);
     
        IRestResponse response = client.Execute(request);
       string statusCodes= response.StatusCode.ToString();
        Console.WriteLine(response.Content);

        var ret = new Dictionary<string, dynamic>();
        var decoder = new System.Web.Script.Serialization.JavaScriptSerializer();
        if (statusCodes == "OK")
        {            
            ret = decoder.Deserialize<Dictionary<string, dynamic>>(response.Content);
            return ret;
        }

        return ret;
    }

    public static string  CSCBulkServiceUpdateData(string confirmation)
    {
        // it is used for local host Link
        //var client = new RestClient("http://localhost:18447/CCC_Registration/CSCBulkDataUpload.asmx/GetCSCBulkServiceUpdateData");

        // It is used for Live Server Link 
         var client = new RestClient("https://dlcaccr.nielit.gov.in/CSCBulkDataUpload.asmx/GetCSCBulkServiceUpdateData");

        client.Timeout = -1;
        var request = new RestRequest(Method.POST);
        request.AddParameter("Confirmation", confirmation);

        IRestResponse response = client.Execute(request);
        string statusCodes = response.StatusCode.ToString();
        Console.WriteLine(response.Content);
        return "OK";
    } 
}