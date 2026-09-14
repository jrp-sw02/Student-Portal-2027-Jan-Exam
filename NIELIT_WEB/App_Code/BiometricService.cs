using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using RestSharp;
using Newtonsoft.Json;

using System.Data.Objects;
using System.Xml;

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
//Added 8 June 2020 for webservice modifications
using System.Security.Cryptography;
//


/// <summary>
/// Summary description for BiometricService
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "https://student.nielit.gov.in/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class BiometricService : System.Web.Services.WebService {

    public BiometricService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    [WebMethod]

    public string DBEntity(string name, string dob, string gender, string biometricDeviceResponse, string aadhaarNumber, Int64 id)
    {
        string encryptionKey = "bbdeaa53-deb9-45a1-b24f-8fbd28265113";
        try
        {
            if (biometricDeviceResponse.Length == 0)
                return "0";
           
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(biometricDeviceResponse);
            //biometricDeviceResponse = System.Convert.ToBase64String(plainTextBytes);
            //Commented 8 June 2020 for webservice modification
            //var body = new
            //{
            //    name = name,
            //    //dob = "27/11/1974",
            //    dob = dob,
            //    gender = gender,
            //    biometricDeviceResponse = biometricDeviceResponse,
            //    aadhaarNumber = aadhaarNumber

            //};
            //Added for Modification in biometric webservice
            string password = "Welcome@123";
          //  string txnReqID = id.ToString() + "_" + System.DateTime.Now.ToString();
            string txnReqID = id.ToString() + "-" + System.DateTime.Today.Year.ToString().Trim() + "-" + System.DateTime.Today.Month.ToString().Trim() + "-" + System.DateTime.Today.Day.ToString().Trim() + "-" + System.DateTime.Now.Hour.ToString().Trim() + "-" + System.DateTime.Now.Minute.ToString().Trim() + "-" + System.DateTime.Now.Second.ToString().Trim();
                //"650fc944-0c65-4eff-998c-333984f478d1";;
            string demoAuth = "y";
            string BioAuth = "y";
            //
            var obj = new
            {
                //Added on 8 June 2020 for web service call modifications
                password = password,
                txnRequestID = txnReqID,
                demoAuth = demoAuth,
                bioAuth = BioAuth,
                aadhaarNumber = aadhaarNumber,
                //
                name = name,
                //dob = "27/11/1974",
                dob = dob,
                gender = gender,
                biodata = biometricDeviceResponse,
                

            };

            var jsonString = JsonConvert.SerializeObject(obj);

            Console.WriteLine("********************Encryption Example******************");

            CryptoClass crypto = CryptoClass.Instance;

            string inputText = jsonString;
           // var jsonString = JsonConvert.SerializeObject(obj);
            //Added on 8 June 2020 for web service call modifications
           // CryptoClass cryptoClass = new CryptoClass();

           // string body1 =CryptoClass .EncryptText(JsonConvert.SerializeObject(body));
            // only use first 16 bytes of the key
            byte[] keybytes = Encoding.UTF8.GetBytes(encryptionKey);
            //byte[] truncatedkeybytes = new byte[16];
            //Array.Copy(keybytes, truncatedkeybytes, 16);
            // initialization vector is all 0's; no additonal initilization required
            byte[] iv = new byte[16];



            string ciphertext = CryptoClass.AESEncrypt(inputText, encryptionKey );
                //keybytes  , iv);
           // string hexciphertext = CryptoClass.ByteArrayToHexString(ciphertext);
                
               // System.Text.Encoding.UTF8.GetString(ciphertext );
                //CryptoClass.ByteArrayToHexString(ciphertext);

           // string Sid = "10001";

          //  var body2 = new
          //  {
          //      serviceId = Sid,
          //      data = hexciphertext 
          //  };

            //
            string json_data = ciphertext;
                //JsonConvert.SerializeObject(body2);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //Print the Json object
             //ShowAlert(json_data);
            //string result = GetJSON ("aaa","02/01/1961","F",finalResponse ,"999999999999");
            //var client = new RestClient("https://sp.epramaan.in:4003/nielitwebservice");
            string Sid = "10001";

             var body2 = new
              {
                  serviceId = Sid,
		 reqTxnId = txnReqID,
                  data = json_data 
             };

           //Added 3 Jan 2023
	// var client = new RestClient("https://sp.epramaan.in:4003");
//Added 2 Jan 2023 after test at local
		var client = new RestClient("https://authenticate.epramaan.gov.in");
		var request1 = new RestRequest("/authwebservice/requestauth/v2", Method.POST);
		//Commented 4 Mar 2023
//	 var request1 = new RestRequest("/nielitwebservice/requestauth", Method.POST);
           // Commented 3 Jan 2023 uncommented 12 Jan 2023
	//var client = new RestClient("https://department.epramaan.gov.in");
            //var client = new RestClient("https://sp.epramaan.in:4003");
            //var request1 = new RestRequest();
           // Commented 3 Jan 2023 uncommented 12 Jan 2023
	//var request1 = new RestRequest("/nielitauth/requestauth", Method.POST);



            //request1.Method = Method.POST;
            //    request1.RequestFormat = RestSharp .DataFormat .Json ;
            request1.AddHeader("Accept", "application/json");
            request1.AddHeader("ContentType", "application/json");
            request1.Parameters.Clear();
            //request1.AddParameter("application/json", json_data, ParameterType.RequestBody);
            request1.AddParameter("application/json",  JsonConvert.SerializeObject(body2), ParameterType.RequestBody);
            request1.Timeout = 500000;



            IRestResponse response1 = client.Execute(request1);
            response1.ContentType = "application/json";
            
            
            var url = response1.ResponseUri;
            // return(url.ToString ());
            var content = response1.Content; // raw content as string  
            //ShowAlert("content"+content);
            //var content = "";
            if (content == "serverError")
                return ("Cannot connect to server");
//Added for decryption 30 April 2024
            string decrypted = CryptoClass.Decrypt(content, encryptionKey);
            if (decrypted == "")
                return ("Result receiving for aadhaar failed");
            bioReturn biometricReturn = JsonConvert.DeserializeObject<bioReturn>(decrypted);
            /* Changed for epramaan change 30 Apr 2024 if (content == "")
                return ("Result receiving for aadhaar failed");
            bioReturn biometricReturn = JsonConvert.DeserializeObject<bioReturn>(content);*/
            //return ("status" + biometricReturn.status);
            bool statusId = false;
            string errorCode = "";
            if (biometricReturn.status.ToLower () == "true")
            {
                statusId = true;
                errorCode = "Null";
            }
            if (biometricReturn.status.ToLower () == "false")
            {
                statusId = false;
                errorCode = biometricReturn.errorCode;
            }

            //Change dfor epramaan changes 30 April 2024 int result = saveResponse(txnReqID, statusId, biometricReturn.transactionID, errorCode, id);
            int result = saveResponse(txnReqID, statusId, biometricReturn.transactionID, errorCode, id,biometricReturn .bioResponseCode );
		
             string msg = "";
            if (errorCode.ToString().StartsWith("REQ") || errorCode.ToString().StartsWith("100"))
                msg = "Invalid Name, Date of birth or Gender";
            if (errorCode.ToString().StartsWith("300"))
                msg = "Biometric data mismatch";
            if (errorCode.ToString().StartsWith("SYS"))
                errorCode = "System Error";
            if (errorCode.ToString().StartsWith("998"))
               msg = "Invalid Aadhaar number";


            return ("Verified: " + biometricReturn.status+" "+msg);

        }
        catch (Exception ex)
        {
            return ("Exception :-" + ex.Message);
        }


    }

//    [WebMethod]
    
//    public string DBEntityOriginal(string name,string dob,string gender,string biometricDeviceResponse,string aadhaarNumber,Int64  id)
//    {
//        try
//        {
//            if (biometricDeviceResponse.Length  == 0)
//                return "0";
//            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(biometricDeviceResponse);
//            //biometricDeviceResponse = System.Convert.ToBase64String(plainTextBytes);
//            //Commented 8 June 2020 for webservice modification
//            //var body = new
//            //{
//            //    name = name,
//            //    //dob = "27/11/1974",
//            //    dob = dob,
//            //    gender = gender,
//            //    biometricDeviceResponse = biometricDeviceResponse,
//            //    aadhaarNumber = aadhaarNumber
               
//            //};
//            //Added for Modification in biometric webservice
//            string password="Welcome@123";
//            string txnReqID=id.ToString ();
//            string demoAuth = "y";
//            string bioAuth = "y";
//            //
//            var body = new
//            {
//                //Added on 8 June 2020 for web service call modifications
//                password=password,
//                txnRequestID=txnReqID,
//                demoAuth=demoAuth,
//                BioAuth=bioAuth,

//                //
//                name = name,
//                //dob = "27/11/1974",
//                dob = dob,
//                gender = gender,
//                biometricDeviceResponse = biometricDeviceResponse,
//                aadhaarNumber = aadhaarNumber

//            };
//            //Added on 8 June 2020 for web service call modifications
//            string body1 = Encrypt(JsonConvert.SerializeObject(body));
//            string Sid = "10001";

//            var body2=new
//            {
//                serviceId=Sid,
//                   data=body1
//        };

//            //
//            string json_data = JsonConvert.SerializeObject(body);

//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 |  SecurityProtocolType.Tls;
//            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

//            //Print the Json object
//            // ShowAlert(json_data);
//            //            //string result = GetJSON ("aaa","02/01/1961","F",finalResponse ,"999999999999");
//            //var client = new RestClient("https://sp.epramaan.in:4003/nielitwebservice");
//            var client = new RestClient("https://sp.epramaan.in:4003");
//            //var request1 = new RestRequest();
//            var request1 = new RestRequest("/nielitwebservice/validate", Method.POST);


//            //request1.Method = Method.POST;
//            //    request1.RequestFormat = RestSharp .DataFormat .Json ;
//            request1.AddHeader("Accept", "application/json");

//            request1.Parameters.Clear();
//            request1.AddParameter("application/json", json_data, ParameterType.RequestBody);
//            request1.Timeout = 500000;

           

//            IRestResponse response1 = client.Execute(request1);
//            var url = response1.ResponseUri;
//            // return(url.ToString ());
//            var content = response1.Content; // raw content as string  
//            //ShowAlert("content"+content);
//            //var content = "";
//            if (content == "serverError")
//                return ("Cannot connect to server");
//            if (content == "")
//                return ("Result receiving for aadhaar failed");
//            bioReturn biometricReturn = JsonConvert.DeserializeObject<bioReturn>(content);
////return ("status" + biometricReturn.status);
//            bool statusId = false;
//            string errorCode = "";
//            if (biometricReturn.status == "True")
//            {
//                statusId = true;
//                errorCode = "Null";
//            }
//            if (biometricReturn.status == "False")
//            {
//                statusId = false;
//                errorCode = biometricReturn.errorCode;
//            }

//            int result = saveResponse(biometricReturn.epramaanid,statusId, biometricReturn.transactionID, errorCode,id);
//          return ("Verified: " + biometricReturn.status);

//        }
//        catch (Exception ex)
//        {
//            return ("Exception :-" + ex.Message);
//        }


//    }
     protected int saveResponse(string reqTransID,bool status, string transID, string errCode,Int64 id,string bioResponseCode)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                NielitCentreStudent student = context.NielitCentreStudent.Find(id);
                aadhaarResponse aadharResp = new aadhaarResponse();
                aadharResp.epramaanid =transID  ;
                aadharResp.applicationNo = student.Number;
                 aadharResp.courseID = student.CourseID;
                //Added for epramaan Changes 30 Apr 2024
                 if (bioResponseCode != null)
                     aadharResp.bioResponseCode = bioResponseCode;
                //
                //aadharResp.courseCategoryID = ;
                aadharResp.status = status;
                aadharResp.transactionID = reqTransID;
                student.AadharVerfied = false;
                if (errCode.ToLower () != "null")
                {
                    aadharResp.errorCode = errCode;
                    student.AadharVerfied = false;
                }
                else
                {
                    if (status)
                    {
                        aadharResp.Remarks = "Success";
                        student.AadharVerfied = true;
                        student.IsVerifiedByInstitute = true;
                        student.DateOfVerificationByInstitute = System.DateTime.Now;
                        
                    }
                }

                 //Added for epramaan changes 18 Apr 2024
                if (errCode.ToString().StartsWith("REQ001"))
                    aadharResp.Remarks = "Error From Server";

                if (errCode.ToString().StartsWith("REQ002"))
                    aadharResp.Remarks = "reqTxnId same as earlier, send changed one";

                if (errCode.ToString().StartsWith("REQ003") || errCode.ToString().StartsWith("REQ005"))
                    aadharResp.Remarks = "Encryption Algo incorrect";

                if (errCode.ToString().StartsWith("REQ006"))
                    aadharResp.Remarks = "Incorrect password";

                if (errCode.ToString().StartsWith("REQ004"))
                    aadharResp.Remarks = "Incorrect AES key";

                if (errCode.ToString().StartsWith("100"))
                    aadharResp.Remarks = "Invalid Name, Date of birth or Gender";
                if (errCode.ToString().StartsWith("300"))
                    aadharResp.Remarks = "Biometric data mismatch";
                if (errCode.ToString().StartsWith("330"))
                    aadharResp.Remarks = "Biometric locked by resident";
                if (errCode.ToString().StartsWith("998"))
                    aadharResp.Remarks = "Invalid Aadhaar number";
                if (errCode.ToString().StartsWith("996"))
                    aadharResp.Remarks = "Aadhaar number Cancelled";

               /* if (errCode.ToString().StartsWith("REQ") || errCode.ToString().StartsWith("100"))
                if (errCode.ToString().StartsWith("100"))
                    aadharResp.Remarks = "Invalid Name, Date of birth or Gender";
		        if (errCode.ToString().StartsWith("300"))
                    aadharResp.Remarks = "Biometric data mismatch";
                if (errCode.ToString().StartsWith("SYS"))
                    aadharResp.Remarks = "System Error";
                if (errCode.ToString().StartsWith("998"))
                    aadharResp.Remarks = "Invalid Aadhaar number";*/


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
        //Added 10 June 2020 for modifications
        public string reqTransactionID {get;set;}  
        //
        public string status { get; set; }
        public string transactionID { get; set; } // id from epramaan
        public string errorCode { get; set; }
        //In case 4 values returned 4th value to capture and store ??/ 8 June 2020 , yet to get call of clqarification from CDAC
  //Added for aadhaar change
        public string bioResponseCode { get; set; } 
    };

    //Added for Biometric Service modification 8 June 2020
    private string Encrypt(string clearText)
    {
       // string EncryptionKey = "MAKV2SPBNI99212";
        string EncryptionKey = "bbdeaa53-deb9-45a1-b24f-8fbd28265113";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    
    private string Decrypt(string cipherText)
    {
        //string EncryptionKey = "MAKV2SPBNI99212";
        string EncryptionKey = "bbdeaa53-deb9-45a1-b24f-8fbd28265113";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
    //

}
