using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class FrmModuleHistory : BasePage
{
    Int32 moduleID = 0;
    Int32 courseID = 0;
    UserType loginUserType;
    Int64 entityID = 0;
    string moduleName;
    string courseName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Home.aspx");
            courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            moduleID = Convert.ToInt32(Request.QueryString["ModuleID"]);
            loginUserType = (UserType)Session["UserType"];
            if (!String.IsNullOrEmpty(Request.QueryString["CandidateID"]))
                entityID = Convert.ToInt64(Request.QueryString["CandidateID"]);
            else
                entityID = Convert.ToInt64(Session["EntityID"]);
            using (EConnectContext context = new EConnectContext())
            {
                courseName = context.Courses.Find(courseID).Name;
                moduleName = context.Modules.Find(moduleID).ShortName;
                lblHeading.Text = "Module Details: " + moduleName + " (" + courseName + ")";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Module Details: " + moduleName, "CAND/FrmModuleHistory.aspx?" + Request.QueryString.ToString(), ""));
                BreadCrumb1.Render();
            };
            BindGridView();
            if (loginUserType == UserType.Candidate)
            {
                showsidelink();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindGridView()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var listOfAppearedModules = (from d in context.CourseExamApplicationDetails
                                             join m in context.Modules on d.ModuleID equals m.ID
                                             where d.CourseID == courseID && d.CandidateID == entityID && d.ModuleID == moduleID
                                             orderby d.Exam.ExamYear, d.Exam.ExamMonth
                                             select new
                                             {
                                                 CourseID = m.CourseID,
                                                 ID = m.ID,
                                                 name = m.Name,
                                                 Code = m.ShortName,
                                                 ModuleTypeID = m.ModuleTypeID,
                                                 exname = d.Exam.Name == null ? "NA" : d.Exam.Name,
                                                 MType = m.ModuleType.Name + (m.ModuleTypeID == 1 ? (m.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                                                 Result = d.Grade.Description == null ? "NA" : d.Grade.Description,
                                                 examID = d.ExamID==null?0: d.ExamID,
                                                 Grade = d.Grade.Code == null ? "NA" : d.Grade.Code 
                                             });
                gvMain.DataSource = listOfAppearedModules.ToList();
                gvMain.DataBind();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                if (!String.IsNullOrEmpty(Request.QueryString["CandidateID"]))
                    hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&CandidateID=" + Request.QueryString["CandidateID"]);
                else
                    hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = hl.NavigateUrl;

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = hl.NavigateUrl;

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = hl.NavigateUrl;

                //HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                //hl4.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void showsidelink()
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Sidelink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ccatId = (from c in context.Courses
                                where c.ID == courseID && c.ShowOnWeb == true
                                select new
                                {
                                    courcatID = c.CourseCategoryID
                                }).FirstOrDefault().courcatID;
                var dl = from d in context.Downloadables
                         where d.CourseID == courseID && d.ShowOnWeb == true
                         select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName };
                var d2 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == ccatId && d.ShowOnWeb == true
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(dl);
                var d3 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == null && d.ShowOnWeb == true
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
}