using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class aboutCourse : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (Request.UrlReferrer == null)
            //    if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in") && (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "http://nielit.gov.in"))
            //    {
            //        Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            //        Response.End();
            //        return;
            //    }

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {                   
                    using (EConnectContext context = new EConnectContext())
                    {

                        Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
                       // Response.Redirect("~/WEB/BulkAdmitCard.aspx?" + Request.QueryString);
                       // Response.Redirect("~/Admin/Default.aspx?" + Request.QueryString);
                        //Response.Redirect("~/Admin/ChangeExamCentreChoice.aspx");
                        lblheading1.Text = currentCourse.CourseCategory.Name + ":- " + currentCourse.Name;
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Computer Courses:" + currentCourse.Name, "WEB/aboutCourse.aspx?" + Request.QueryString, ""));
                        lblHead.Text = "About" + " " + currentCourse.Name;

                        if (!string.IsNullOrEmpty(Request.QueryString["query"]))
                        {
                            if (Request.QueryString["query"].ToString() == "apply")
                            {
                                string candtype = "";
                                candtype = Request.QueryString["candtype"].ToString();

                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "window.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("RulesForOnlineRegistration.aspx?ID=" + Request.QueryString["id"].ToString() + "&query=" + Request.QueryString["query"].ToString() + "&candtype=" + candtype) + "';", true);
                                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "window.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("RulesforOnlineRegistrationOlevel.aspx?ID=" + Request.QueryString["id"].ToString() + "&query=" + Request.QueryString["query"].ToString() + "&candtype=" + candtype) + "';", true);
                            }
                            else if (Request.QueryString["query"].ToString() == "admit")
                            {
                              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "window.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("DownloadAdmitCard.aspx?" + Request.QueryString) + "';", true);
                                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "window.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("Default.aspx?" + Request.QueryString) + "';", true);
                                //Response.Redirect("~/Admin/Covid_Instructions_Ver2.aspx?" + Request.QueryString);
                            }
                            else if (Request.QueryString["query"].ToString() == "result")
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "window.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("Result.aspx?" + Request.QueryString) + "';", true);
                            }
                        }
                        else
                        {
                            RenderPage(currentCourse.enmCourseType);
                            showsidelink();
                            //Showdata();
                            ShowAboutUs();

                        }

                    };
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowAboutUs()
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            DateTime todayDate = DateTime.Now;
            Int32 DownloadableTypeID = Convert.ToInt32(enmDownloadableType.AboutCourse);
            using (EConnectContext context = new EConnectContext())
            {
                var objData = (from s in context.Downloadables
                               join c in context.UploadedFiles.AsNoTracking() on s.DownloadableFileID equals c.ID
                               where s.CourseID == courseID && s.DownloadableTypeID == DownloadableTypeID && s.EffectiveFromDate <= todayDate
                               orderby s.EffectiveFromDate descending
                               select new
                               {
                                   ID = s.ID,
                                   filename = c.BlobFile,
                                   fname = c.OriginalName,
                                   fileID = s.DownloadableFileID.Value,
                                   effectiveDate = s.EffectiveFromDate
                               }).FirstOrDefault();
                if (objData != null)
                {
                    ifrmAboutUs.Attributes.Add("src", "../Handlers/UploadedFileHandler.ashx?ID=" + objData.fileID);
                    ifrmAboutUs.Attributes.Add("onload", "autoResize(this);");
                }
                else
                {
                    ifrmAboutUs.Visible = false;
                }

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
        }

    }
    protected void RenderPage(enmCourseType CourseType)
    {
        try
        {
            if (CourseType == enmCourseType.CertificationCourse)
            {
                Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?" + Request.QueryString, "../images/Apply_Online.jpg"));
                Sidelink.Items.Add(new SideLinkItem("View Filled Application", "FilledForm.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/Get_Filled_Form.jpg"));
                Sidelink.Items.Add(new SideLinkItem("Check Application Status", "ApplicationStatus.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/View_Application_Status.jpg"));
                //Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/Print_Admit_Card.jpg"));
                Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/View_Result.jpg"));
                Sidelink.Items.Add(new SideLinkItem("Search Accredited Centre", "FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/View_Certificate_Status.jpg"));
                Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                Sidelink.Render();
            }
            else
            {
                Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?" + Request.QueryString, "../images/Apply_Online.jpg"));
                Sidelink.Items.Add(new SideLinkItem("View Filled Form", "FilledForm.aspx?" + Request.QueryString, "../images/Get_Filled_Form.jpg"));
                Sidelink.Items.Add(new SideLinkItem("Check Form Status", "ApplicationStatus.aspx?" + Request.QueryString, "../images/View_Application_Status.jpg"));
                Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?" + Request.QueryString, "../images/Print_Admit_Card.jpg"));
                Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?" + Request.QueryString, "../images/View_Result.jpg"));
                Sidelink.Items.Add(new SideLinkItem("Search Centre", "FrmAccredetedCentre.aspx?" + Request.QueryString, "../images/View_Certificate_Status.jpg"));
                Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                Sidelink.Render();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void showsidelink()
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            Sidelink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
            using (EConnectContext context = new EConnectContext())
            {
                var crs = context.Courses.Find(courseID);
                Int32 ccatId = crs.CourseCategoryID;

                var dl = from d in context.Downloadables
                         where d.CourseID == courseID
                         select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName };
                var d2 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == ccatId
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(dl);
                var d3 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == null
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(d2);

                foreach (var dnbl in d3.Distinct())
                {
                    Sidelink1.Items.Add(new SideLinkItem(dnbl.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + dnbl.FileID.ToString(), "", "_blank"));
                }

                Sidelink1.Render();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void Showdata()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                StringBuilder sb = new StringBuilder();
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
                var d1 = (from c in context.CourseRevisions
                          where c.CourseID == courseID
                          select new { CourseName = c.Name }).FirstOrDefault();

                sb.Append("<div class='summary_block'>");
                sb.Append("<span align='left'>About " + d1.CourseName + "</span>");
                sb.Append("<p style='width: 98%; text-align: justify;'>");
                sb.Append("</p></div>");
                pdescription.InnerHtml = sb.ToString();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}