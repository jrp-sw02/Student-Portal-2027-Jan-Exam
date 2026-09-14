using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Web;

public partial class Admin_NSQFForm : System.Web.UI.Page
{
    Int32 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			ShowAlert("x");
           
           
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            ShowAlert(loginUserNo.ToString ());
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            string userNo="",datetime="";

            if (Session["UserID"] != null) //UserNo Actually
            {
                userNo= Session["UserID"].ToString();
				
                DateTime baseDate = new DateTime(1970, 1, 1);
                TimeSpan diff = DateTime.Now - baseDate;
              Int64  datetime1 =Convert.ToInt64 ((diff.TotalMilliseconds));
              datetime = datetime1.ToString();
            
			
            string appKey=ConfigurationManager.AppSettings["NSQF_KEY"].ToString();
			
            string hash = hashGenerator(userNo,currentRoleId.ToString () , datetime,appKey );
			
			ShowAlert(hash);
           // Session.Abandon();
            string site1 = "https://student.nielit.gov.in/nsqf/userAuthenticate.php?para1=" + userNo + "&para2=" + currentRoleId + "&para3=" + datetime + "&para4=" + hash;
            //string site1=EConnect.Utils.Security.QuertStringModule.Encrypt("http://172.16.60.52/nsqf/userAuthenticate.php?para1=" + userNo + "&para2=" + currentRoleId + "&para3=" + datetime + "&para4=" + hash);
            ShowAlert(site1);
            //var random = Random("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 15); //HmAVyuot03dbrVi--Added to web.config
            Response.AddHeader("Content-Type", "text/html");
            Response.Redirect("../nsqf/userAuthenticate.php?para1=399636&para2=22&para3=1582110650477&para4=7ec2abd6693d2e40a5ae372ad45b4fc9de5b294cf3b5368ae9fbce473afae6489add0dc2f0f0ad271751b87f74e181e179d54bad1d77e2b1f386d322a9ec8590", false);
            //Response.Redirect("http://localhost:34639/NIELIT_WEB/nsqf/userAuthenticate.php?para1=" + userNo + "&para2=" + currentRoleId + "&para3=" + datetime + "&para4=" + hash, false);
            //Server.Transfer("http://172.16.60.52/nsqf/userAuthenticate.php?para1=" + userNo + "&para2=" + currentRoleId + "&para3=" + datetime + "&para4=" + hash, false);
			}
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected string Random(string chars, int length = 15)
{
    var randomString = new StringBuilder();
    var random = new Random();

    for (int i = 0; i < length; i++)
        randomString.Append(chars[random.Next(chars.Length)]);

    return randomString.ToString();
}
    protected String hashGenerator(String UserNo,String roleID, String dateTime,string appKey)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(UserNo).Append(roleID).Append(dateTime).Append (appKey );
        byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
        HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
        byte[] sec_key = sha1.ComputeHash(genkey);
        StringBuilder sb1 = new StringBuilder();
        for (int i = 0; i < sec_key.Length; i++)
        {
            sb1.Append(sec_key[i].ToString("x2"));
        }
        return sb1.ToString();
    }
    protected void ShowAlert(string Message)
    {
        Page currentPage = (Page)HttpContext.Current.Handler;
        if (!((currentPage == null)))
        {
            string sMsg = "alert('" + Message.Replace("\"", "'").Replace(char.ConvertFromUtf32(10).ToString(), " ").Replace("'", "") + "');";
            currentPage.ClientScript.RegisterClientScriptBlock(currentPage.GetType(), "", sMsg, true);
        }
    }
    protected void ShowAlert(string Message, Boolean isAsynchronous)
    {
        Page currentPage = (Page)HttpContext.Current.Handler;
        if (!((currentPage == null)))
        {
            string sMsg = "alert('" + Message.Replace("\"", "'").Replace(char.ConvertFromUtf32(10).ToString(), " ").Replace("'", "") + "');";
            if (isAsynchronous)
                ScriptManager.RegisterClientScriptBlock(currentPage, currentPage.GetType(), "", sMsg, true);
            else
                currentPage.ClientScript.RegisterClientScriptBlock(currentPage.GetType(), "", sMsg, true);
        }
    }

}