using System;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using iTextSharp.text;

public partial class RulesForOnlineRegistrationOLevel : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /* if (Request.UrlReferrer == null)
             //	if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in" ) && (Request.UrlReferrer == null  || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "http://nielit.gov.in"))
             {
                 Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                 Response.End();
                 return;
             }
             if (!Page.IsPostBack)
             {
                 if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                 {*/
            Int32 courseID = 1;
            //Convert.ToInt32(Request.QueryString["id"]);
                    RenderPage(courseID);
            if(!Page .IsPostBack )
                    GenerateNewCaptchaImage();
                    ShowDownloadables(courseID);
                    //ShowAboutUs(courseID);
                    //ShowBrowserLink(courseID);
                    //ShowInstructions(courseID);
              //  }//
            //}
            base.ReWriteAction(this.Form);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    #region Private Methods
    protected void RenderPage(Int32 courseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(courseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    Label1.Text = "Online Registration Application Form For:-" + currentCourse.Name+ " (BSB Students)";
                    Lblctype.Text = currentCourse.Name + " " + "Registration ";
                    lblhdeccoursecode.Text = currentCourse.NameRegional;
                    lbldeccoursecode.Text = currentCourse.Code;
                    trnotification.Visible = true;
                    trsyllabus.Visible = false;
                    trinstructions.Visible = false;
                    spancourse.Visible = true;
                    spancertificate.Visible = false;
                }
                else
                {
                    Label1.Text = "Online Examination Application Form For:-" + currentCourse.Name;
                    Lblctype.Text = currentCourse.Code + " " + "Examination";
                    lblhdeccoursecode.Text = currentCourse.NameRegional;
                    lbldeccoursecode.Text = currentCourse.Code;
                    trnotification.Visible = false;
                    trsyllabus.Visible = true;
                    trinstructions.Visible = true;
                    //ShowSyllabus(courseID);
                    spancourse.Visible = false;
                    spancertificate.Visible = true;
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowDownloadables(Int32 courseID)
    {
        Int32 StepByStepProcessForOnlineApplicationForm = Convert.ToInt32(enmDownloadableType.StepByStepProcessForOnlineApplicationForm);
        Int32 Brochure = Convert.ToInt32(enmDownloadableType.Brochure);
        Int32 Syllabus = Convert.ToInt32(enmDownloadableType.Syllabus);
        Int32 Instruction = Convert.ToInt32(enmDownloadableType.Instructions);
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ccatId = context.Courses.Find(courseID).CourseCategoryID;

                var Downloadables = from s in context.Downloadables
                                    join c in context.UploadedFiles on s.DownloadableFileID equals c.ID
                                    where s.CourseCategoryID == ccatId
                                    select new
                                    {
                                        ID = s.ID,
                                        fname = c.OriginalName,
                                        fileID = s.DownloadableFileID.Value,
                                        CourseId = s.CourseID,
                                        DownloadableTypeID = s.DownloadableTypeID,
                                        EffectiveFromDate = s.EffectiveFromDate,
                                        linkname = s.LinkName,
                                    };
                #region About Us
                var AboutUs = Downloadables.Where(s => s.DownloadableTypeID == StepByStepProcessForOnlineApplicationForm && s.EffectiveFromDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).OrderByDescending(s => s.EffectiveFromDate).FirstOrDefault();
                if (AboutUs != null)
                {
                    ifrmAboutUs.Attributes.Add("src", "../Handlers/UploadedFileHandler.ashx?ID=" + AboutUs.fileID);
                }
                else
                {
                    ifrmAboutUs.Visible = false;
                }
                #endregion

                #region Brochure
                var Brochures = Downloadables.Where(s => s.DownloadableTypeID == Brochure && s.CourseId == courseID && s.EffectiveFromDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).OrderByDescending(s => s.EffectiveFromDate).FirstOrDefault();
                if (Brochures != null)
                {
                    link.HRef = "../Handlers/UploadedFileHandler.ashx?ID=" + Brochures.fileID;
                }
                else
                {
                    link.HRef = "";
                }
                #endregion

                #region Syllabus
                var Syllabuss = Downloadables.Where(s => s.DownloadableTypeID == Syllabus && s.CourseId == courseID && s.EffectiveFromDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).OrderByDescending(s => s.EffectiveFromDate).FirstOrDefault();
                if (Syllabuss != null)
                {
                    link1.HRef = "../Handlers/UploadedFileHandler.ashx?ID=" + Syllabuss.fileID;
                }
                else
                {
                    link1.HRef = "";
                }
                #endregion

                #region Instructions
                var Instructions = Downloadables.Where(s => s.DownloadableTypeID == Instruction && s.EffectiveFromDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).OrderByDescending(s => s.EffectiveFromDate).FirstOrDefault();
                if (Instructions != null)
                {
                    sptext.InnerText = Instructions.linkname.ToString();
                    link2.HRef = "../Handlers/UploadedFileHandler.ashx?ID=" + Instructions.fileID;
                }
                else
                {
                    link2.HRef = "";
                }
                #endregion
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //protected void ShowAboutUs(Int32 courseID)
    //{
    //    try
    //    {
    //        Int32 DownloadableTypeID = Convert.ToInt32(enmDownloadableType.StepByStepProcessForOnlineApplicationForm);
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Int32 ccatId = context.Courses.Find(courseID).CourseCategoryID;

    //            var objData = (from s in context.Downloadables
    //                           join c in context.UploadedFiles on s.DownloadableFileID equals c.ID
    //                           where s.CourseCategoryID == ccatId
    //                           && s.DownloadableTypeID == DownloadableTypeID
    //                           && System.Data.Entity.DbFunctions.TruncateTime(s.EffectiveFromDate) <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
    //                           orderby s.EffectiveFromDate descending
    //                           select new
    //                           {
    //                               ID = s.ID,
    //                               fname = c.OriginalName,
    //                               fileID = s.DownloadableFileID.Value
    //                           }).FirstOrDefault();
    //            if (objData != null)
    //            {
    //                ifrmAboutUs.Attributes.Add("src", "../Handlers/UploadedFileHandler.ashx?ID=" + objData.fileID);
    //            }
    //            else
    //            {
    //                ifrmAboutUs.Visible = false;
    //            }
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    //protected void ShowBrowserLink(Int32 courseID)
    //{
    //    try
    //    {
    //        Int32 DownloadableTypeID = Convert.ToInt32(enmDownloadableType.Brochure);
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Int32 ccatId = context.Courses.Find(courseID).CourseCategoryID;

    //            var objData = (from s in context.Downloadables
    //                           join c in context.UploadedFiles on s.DownloadableFileID equals c.ID
    //                           where s.CourseCategoryID == ccatId && s.CourseID == courseID
    //                           && s.DownloadableTypeID == DownloadableTypeID
    //                           && System.Data.Entity.DbFunctions.TruncateTime(s.EffectiveFromDate) <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
    //                           orderby s.EffectiveFromDate descending
    //                           select new
    //                           {
    //                               ID = s.ID,
    //                               fname = c.OriginalName,
    //                               fileID = s.DownloadableFileID.Value
    //                           }).FirstOrDefault();
    //            if (objData != null)
    //            {
    //                link.HRef = "../Handlers/UploadedFileHandler.ashx?ID=" + objData.fileID;
    //            }
    //            else
    //            {
    //                link.HRef = "";
    //            }

    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    //protected void ShowSyllabus(Int32 courseID)
    //{
    //    try
    //    {
    //        Int32 DownloadableTypeID = Convert.ToInt32(enmDownloadableType.Syllabus);
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Int32 ccatId = context.Courses.Find(courseID).CourseCategoryID;
    //            var objData = (from s in context.Downloadables
    //                           join c in context.UploadedFiles on s.DownloadableFileID equals c.ID
    //                           where s.CourseCategoryID == ccatId && s.CourseID == courseID && s.DownloadableTypeID == DownloadableTypeID && System.Data.Entity.DbFunctions.TruncateTime(s.EffectiveFromDate) <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
    //                           orderby s.EffectiveFromDate descending
    //                           select new
    //                           {
    //                               ID = s.ID,
    //                               fname = c.OriginalName,
    //                               fileID = s.DownloadableFileID.Value
    //                           }).FirstOrDefault();
    //            if (objData != null)
    //            {
    //                link1.HRef = "../Handlers/UploadedFileHandler.ashx?ID=" + objData.fileID;
    //            }
    //            else
    //            {
    //                link1.HRef = "";
    //            }
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    //protected void ShowInstructions(Int32 courseID)
    //{
    //    try
    //    {
    //        Int32 DownloadableTypeID = Convert.ToInt32(enmDownloadableType.Instructions);
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Int32 ccatId = context.Courses.Find(courseID).CourseCategoryID;

    //            var objData = (from s in context.Downloadables
    //                           join c in context.UploadedFiles on s.DownloadableFileID equals c.ID
    //                           where s.CourseCategoryID == ccatId && s.DownloadableTypeID == DownloadableTypeID && System.Data.Entity.DbFunctions.TruncateTime(s.EffectiveFromDate) <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
    //                           orderby s.EffectiveFromDate descending
    //                           select new
    //                           {
    //                               ID = s.ID,
    //                               fname = c.OriginalName,
    //                               linkname = s.LinkName,
    //                               fileID = s.DownloadableFileID.Value
    //                           }).FirstOrDefault();
    //            if (objData != null)
    //            {
    //                sptext.InnerText = objData.linkname.ToString();
    //                link2.HRef = "../Handlers/UploadedFileHandler.ashx?ID=" + objData.fileID;
    //            }
    //            else
    //            {
    //                link2.HRef = "";
    //            }

    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    #endregion
    private bool IsValidForm()
    {
        try
        {
            if (txtcode.Text != ViewState["CaptchCode"].ToString())
            {
                lblError.Visible = true;
                lblError.Text = "Invalid Captcha Code";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
            }
            return true;
        }
        catch (Exception ex)
        { throw ex; }
    }
    private void GenerateNewCaptchaImage()
    {
        try
        {
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Arial");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }

    }
    #region Events: Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
                if (IsValidForm())
            {
                Course currentCourse;
                Int32 coursecategoryID;
                using (EConnectContext context = new EConnectContext())
                {
                    currentCourse = context.Courses.Find(1);
                    //Convert.ToInt32(Request.QueryString["id"]));
                    coursecategoryID = currentCourse.CourseCategoryID;
                };
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    Session["isBSB"] = "1";
                    Response.Redirect("~/CAND/NielitRegistration.aspx?ID=1");
                }
                //+ Request.QueryString["id"], false); }
              //  else
              //  { Response.Redirect("~/CAND/CertificateRegistration.aspx?id=1" + "&CoursecategoryID=1" + "&candtype=1", false); }
            }
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
    
    }
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception) { }
    }
    protected void btnback_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["query"]))
        { Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("allcourses.aspx?query=" + Request.QueryString["query"]), false); }
        else
        { Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("aboutCourse.aspx?" + Request.QueryString), false); }
    }
    #endregion
}