using System;
using System.Data.Objects;
using System.Linq;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class ProtsahanPuraskarInstructions : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           // if (Request.UrlReferrer == null)
				if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in" ) && (Request.UrlReferrer == null  || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
            {
                Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                Response.End();
                return;
            }
            if (!Page.IsPostBack)
            {
                
                    Int32 courseid = 1;
                    
                    ShowDownloadables(courseid);
                   
               
            }
            base.ReWriteAction(this.Form);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ShowDownloadables(Int32 courseID)
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ccatId = context.Courses.Find(courseID).CourseCategoryID;

                var Downloadables = from s in context.Downloadables
                                    join c in context.UploadedFiles on s.DownloadableFileID equals c.ID
                                    where s.CourseCategoryID == ccatId
                                    && s.ID==120
                                    select new
                                    {
                                        ID = s.ID,
                                        fname = c.OriginalName,
                                        fileID = s.DownloadableFileID.Value,
                                        CourseId = s.CourseID,
                                        DownloadableTypeID = s.DownloadableTypeID,
                                        EffectiveFromDate = s.EffectiveFromDate,
                                        linkname = s.LinkName
                                    };
               var DownloadableFile = Downloadables.FirstOrDefault();
                ifrmAboutUs1.Attributes.Add("src", "../Handlers/UploadedFileHandler.ashx?ID=" + DownloadableFile.fileID);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    

    #region Events: Click
    protected void btnSave_Click(object sender, EventArgs e)
    {

//					     ShowAlert("Implementation of updates for Protsahan Puraskar is under process.Dates of opening of application form shall be intimated soon.", true);
//return;
        Response.Redirect("~/CAND/OnlinePuraskarApplicationForm.aspx", false);
    }
  
    #endregion
}