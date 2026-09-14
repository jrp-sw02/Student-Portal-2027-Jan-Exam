using System;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
/// <summary> 
/// Summary description for BasePage 
/// </summary> 

public class BasePage : System.Web.UI.Page
{
	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		if (Session["MyTheme"] == null)
		{
			Session.Add("MyTheme", "Blue");
			Page.Theme = ((string)Session["MyTheme"]);
		}
		else
		{
			Page.Theme = ((string)Session["MyTheme"]);
		}
	}
	protected bool IsSessionAlive()
	{
		if (HttpContext.Current.Session["UserID"] == null)
			return false;
		else
			return true;
	}

	protected void ReWriteAction(System.Web.UI.HtmlControls.HtmlForm childPage)
	{
		//if (Context.Request.QueryString.AllKeys.Length > 0 && Context.Request.QueryString.AllKeys.Contains("?qs") == false)
		//    childPage.Action = EConnect.Utils.Security.QuertStringModule.Encrypt(Context.Request.Url.ToString());
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

	//Validation Function
	protected Boolean IsNumeric(String value)
	{
		Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
		return regex.IsMatch(value.Trim());
	}
	protected Boolean IsDate(String value)
	{
		try
		{
			DateTime dt = Convert.ToDateTime(value);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
	protected String GeInvalidRequestMessage(String redirectText, String redirectPage)
	{
		return "<B>Invalid Request Parameters</B><br><a href='" + redirectPage + "'>" + redirectText + "</a>";
	}
	protected string GetInitCap(string str)
	{
		try
		{
			return new CultureInfo("en").TextInfo.ToTitleCase(str.ToLower());
			//return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(str.ToLower());
			//return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length).ToLower();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
	protected bool isvalidFileExtension(FileUpload ImgUpload1)
	{
		String fileExtension = System.IO.Path.GetExtension(ImgUpload1.FileName).ToLower();
		String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
		for (int i = 0; i < allowedExtensions.Length; i++)
		{
			if (fileExtension == allowedExtensions[i])
				return true;
		}
		return false;
	}
	protected bool isvalidFileSize(FileUpload ImgUpload1, int allowedSize)
	{

		String path = Server.MapPath("~/UploadedFilesFolder/");
		if (ImgUpload1.HasFile)
		{
			HttpPostedFile postedfile = ImgUpload1.PostedFile;
			if (postedfile.ContentLength > allowedSize)
			{
				return false;

			}
			else
				return true;
		}
		return true;

	}
	protected bool IsValidEmailAddress(string emailAddress)
	{
		try
		{
			MailAddress m = new MailAddress(emailAddress, emailAddress);
			string MatchEmailPattern =
						@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
				 + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
							[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
				 + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
							[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
				 + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
			if (emailAddress != null)
				return Regex.IsMatch(emailAddress, MatchEmailPattern);
			else
				return false;
		}
		catch (FormatException)
		{
			return false;
		}
	}
	protected bool IsValidMobileNumber(string mobileNumber)
	{
		try
		{
			if (mobileNumber.Length == 10)
			{
				if (!mobileNumber.StartsWith("0"))
				{
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}
		catch (FormatException)
		{
			return false;
		}
	}
	    //-------------------Added By Vishal on 11.08.2021---------------------------------------------/
    protected bool isvalidFileSize1(FileUpload ImgUpload1, int allowedMinSize, int allowedMaxSize)
    {
        String path = Server.MapPath("~/UploadedFilesFolder/");
        if (ImgUpload1.HasFile)
        {
            HttpPostedFile postedfile = ImgUpload1.PostedFile;
            if (allowedMinSize <= postedfile.ContentLength && postedfile.ContentLength <= allowedMaxSize)
            {
                return true;
            }
            else
                return false;
        }
        return false;
    }

    protected bool isvalidFileExtension1(FileUpload ImgUpload1)
    {
        String fileExtension = System.IO.Path.GetExtension(ImgUpload1.FileName).ToLower();
        String[] allowedExtensions = { ".jpeg",".jpg" };
        for (int i = 0; i < allowedExtensions.Length; i++)
        {
            if (fileExtension == allowedExtensions[i])
                return true;
        }
        return false;
    }
    //-------------------Added By Vishal on 11.08.2021---------------------------------------------/
}



