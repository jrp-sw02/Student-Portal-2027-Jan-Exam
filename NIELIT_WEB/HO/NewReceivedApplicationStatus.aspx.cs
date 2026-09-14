using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;

public partial class HO_NewReceivedApplicationStatus : BasePage
{
    EConnectContext context;

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["SortField"] = "";
        ViewState["SortOrder"] = "";
        BindGridView();
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //        //Encryption url of hypelink field
                //HyperLink hl = (HyperLink)e.Row.Cells[3].Controls[0];
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                ////        Image imgAction = (Image)e.Row.FindControl("imgAction");
                ////        imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                ////        CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                ////        imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            int userType = 0;
            //if (ddlSearchUserType.SelectedValue != "0")
            //    userType = Convert.ToInt32(ddlSearchUserType.SelectedValue);
            //string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            var application = from s in context.CertificateExamApplications
                              select new
                              {   ApplicationNumber = s.ID,
                                  ApplicationDate = s.ApplicationDate,
                                  CandidateName = s.Candidate.Name,
                                  FatherName = s.Candidate.FatherName,
                                  MotherName = s.Candidate.MotherName,
                                  DOB = s.Candidate.DateOfBirth
                              };

            //var application = context.CertificateExamApplications
            //                  .GroupBy(p => new
            //                  {
            //                      p.Course.Name,
            //                      p.ExamCycleID
            //                  })
            //                .Select(g => new
            //                {
            //                    Course = g.Key.Name,
            //                    ExamCycleID = g.Key.ExamCycleID,
            //                    NewRcvd = g.Distinct().Count()
            //                });

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    application = application.Where(s => s.Course.ToUpper().Contains(searchString));

            //}
            //if (userType != 0)
            //    users = users.Where(s => s.UserTypeID == userType);
            application = application.OrderBy(s => s.CandidateName);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "CandidateName":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.CandidateName);
                        else
                            application = application.OrderBy(s => s.CandidateName);
                        break;
                    case "ApplicationNumber":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.ApplicationNumber);
                        else
                            application = application.OrderBy(s => s.ApplicationNumber);
                        break;
                    case "ApplicationDate":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.ApplicationDate);
                        else
                            application = application.OrderBy(s => s.ApplicationDate);
                        break;
                    case "FatherName":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.FatherName);
                        else
                            application = application.OrderBy(s => s.FatherName);
                        break;
                    case "MotherName":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.MotherName);
                        else
                            application = application.OrderBy(s => s.MotherName);
                        break;
                    case "DOB":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.DOB);
                        else
                            application = application.OrderBy(s => s.DOB);
                        break;
                    default:
                        application = application.OrderBy(s => s.CandidateName);
                        break;
                }
            }
            PagingBar1.Bind(application, ref gvMain);
            //uPnlGrid.Update();
            //uPnlNavigation.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}