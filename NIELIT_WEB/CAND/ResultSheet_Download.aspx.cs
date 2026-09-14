using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using System.Net;
using System.Web;
using System.Data.Objects;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using EConnect;
using EConnect.NIELIT;


public partial class CAND_ResultSheet_Download : BasePage
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;     

        if (!IsPostBack)
        {          
            FillCategories();
        }
      
        BreadCrumb1.Render();
    }     
    protected void FillCategories()
    {
        string UserID;
        UserID = (Session["studentReg"]).ToString();
        Int32 RegistrationNumber = Convert.ToInt32(UserID);       
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
               // ListItem lst = new ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var levelcode = from p in context.CourseExamApplications
                                join c in context.CourseRevisions on
                                p.CourseCategoryID equals c.CourseCategoryID
                                where p.CourseID == c.CourseID && p.RegistrationNumber == RegistrationNumber
                                select new { ValueField = c.CourseID, TextField = c.Code };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlLevel, levelcode.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlLevel_SelectedIndexChanged(object sender, System.EventArgs e)
    {
          ddlyear.Items.Clear();
          examyear();
    }
    protected void examyear()
    {
        string UserID;
        UserID = (Session["studentReg"]).ToString();
        Int32 RegistrationNumber = Convert.ToInt32(UserID);
        int courseid = Convert.ToInt32(ddlLevel.SelectedValue);
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
               // ListItem lst = new ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var ExamYear1 = from p in context.CourseExamApplications
                                join c in context.Exams on
                                p.CourseCategoryID equals c.CourseCategoryID
                                where p.RegistrationNumber == RegistrationNumber && p.ExamID == c.ID && c.CourseID == courseid && c.ExamYear >= 2015
                                select new { ValueField = c.ExamYear, TextField = c.ExamYear };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlyear, ExamYear1.Distinct(), lst);
            };
      
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlyear_SelectedIndexChanged(object sender, System.EventArgs e)
    {
       
        ddlmonth.Items.Clear();
        exammonth();
    }
    protected void exammonth()
    {
        string UserID;
        UserID = (Session["studentReg"]).ToString();
        Int32 RegistrationNumber = Convert.ToInt32(UserID);
        int courseid = Convert.ToInt32(ddlLevel.SelectedValue);
        int year=Convert.ToInt32(ddlyear.SelectedValue);
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //ListItem lst = new ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var ExamMonth1 = from p in context.CourseExamApplications
                                 join c in context.Exams on
                                 p.CourseCategoryID equals c.CourseCategoryID
                                 where p.RegistrationNumber == RegistrationNumber && p.ExamID == c.ID && c.CourseID == courseid && c.ExamYear == year
                                 select new { ValueField = c.ExamMonth, TextField = c.Name.Substring(0, 3).ToUpper() };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlmonth, ExamMonth1.Distinct(), lst);
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlmonth_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        LblResultSheetMessage.Text = " ";
        //ddlmonth.Items.Clear();
        //exammonth();
    }
    protected void Button1_Click(object sender, System.EventArgs e)
    {
        LblResultSheetMessage.Visible = false;
        string UserID;
        UserID = (Session["studentReg"]).ToString();
        try
        {            
            string month;
            string year;
            int MonthCode = Convert.ToInt32(ddlmonth.SelectedValue);
            
            // added  to forward July 2020 resultsheet to Jan 2021
            if (MonthCode == 7 && Convert.ToInt32(ddlyear.SelectedValue) == 2020)
            {
                month = "January";
                 year = "2021";
               
            }
            //

            else if  (MonthCode == 7)
            {
                month = "July";
                year = ddlyear.SelectedValue;
               
            }
            else
            {
                month = "January";
                year = ddlyear.SelectedValue;
            }            
            LblResultSheetMessage.Text = " ";
            
           // string spath = "E:/e-Docs/e-ResultSheet/" + ddlyear.SelectedValue + "/" + ddlmonth.SelectedValue + "/" + ddlLevel.SelectedItem.Text + "Level" + "/" + UserID.ToString() + "_signed.pdf";
            //http://nielit.gov.in/oabc_resultsheet/DigitalPDF/resultsheet/2017/January/O/999719_signed.pdf
            string spath = @"F:\e-Docs\e-ResultSheet\" + year + "//" + month + "//" + ddlLevel.SelectedItem.Text + "LeveL" + "//" + UserID.ToString() + "_signed.pdf";
            //string spath = "@//e-Docs/e-ResultSheet/" + ddlyear.SelectedValue + "/" + month + "/" + ddlLevel.SelectedItem.Text + "LeveL" + "/" + UserID.ToString() + "_signed.pdf";
           // string spath = "http://nielit.gov.in/oabc_resultsheet/DigitalPDF/resultsheet/" + ddlyear.SelectedValue + "/" + month + "/" + ddlLevel.SelectedItem.Text  + "/" + UserID.ToString() + "_signed.pdf";
            ////deep 17 april 2017
            //System.Net.WebClient net = new System.Net.WebClient();
            //string link = spath;
            //Response.ClearHeaders();
            //Response.Clear();
            //Response.Expires = 0;
            //Response.Buffer = true;
            //Response.AddHeader("Content-Disposition", "Attachment;FileName=" + Path.GetFileName(spath));
            //Response.ContentType = "APPLICATION/octet-stream";
            //Response.BinaryWrite(net.DownloadData(link));
            //Response.End();
            //Create a stream for the file
            //Stream stream = null;

            //This controls how many bytes to read at a time and send to the client
            //int bytesToRead = 100000;

            //// Buffer to read bytes in chunk size specified above
            //byte[] buffer = new Byte[bytesToRead];

            //// The number of bytes read
            //try
            //{
            //    //Create a WebRequest to get the file
            //    HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(spath);
            //    fileReq.Method = "Get";
            //    fileReq.Proxy = null;
            //    //Create a response for this request
            //    HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                         
            //    if (fileReq.ContentLength > 0)                
            //        fileResp.ContentLength = fileReq.ContentLength;
                
            //    //Get the Stream returned from the response
            //    stream = fileResp.GetResponseStream();

            //    // prepare the response to the client. resp is the client Response
            //    var resp = HttpContext.Current.Response;

            //    //Indicate the type of data being sent
            //    resp.ContentType = "application/octet-stream";

            //    //Name the file 
            //    resp.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(spath));
            //    resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

            //    int length;
            //    do
            //    {
            //        // Verify that the client is connected.
            //        if (resp.IsClientConnected)
            //        {
            //            // Read data into the buffer.
            //            length = stream.Read(buffer, 0, bytesToRead);

            //            // and write it out to the response's output stream
            //            resp.OutputStream.Write(buffer, 0, length);

            //            // Flush the data
            //            resp.Flush();

            //            //Clear the buffer
            //            buffer = new Byte[bytesToRead];
            //        }
            //        else
            //        {
            //            // cancel the download if client has disconnected
            //            length = -1;
            //        }
            //    } while (length > 0); //Repeat until no data is read
            //} 
            //finally
            //{
            //    if (stream != null)
            //    {
            //        //Close the input stream
            //        stream.Close();
            //    }
            //    else
            //    {
            //        LblResultSheetMessage.Visible = true;
            //        LblResultSheetMessage.Text = "Requested Result Sheet is not available.";
            //    }
            //}
            ////deep 17 end
            //string spath = "E:/e-Docs/e-ResultSheet/2015/Jul/OLevel/" + UserID.ToString() + "_signed.pdf";
            if (File.Exists(spath))
                //DownloadResultSheet();
            {
                Response.ContentType = "application/pdf";// ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(spath));
                Response.WriteFile(spath);
                Response.End();
            }
            else
            {
                LblResultSheetMessage.Visible = true;
                LblResultSheetMessage.Text = "Requested Result Sheet is not available.";
                //Response.Write("SORRY...! Your Result Sheet has not been Uploaded yet,Kindly Wait.....");

            }
        }       
        catch (Exception ex)
        {
           // ShowAlert(ex.Message);
        }   
    }
}