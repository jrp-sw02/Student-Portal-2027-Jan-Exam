using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.HRMS;
using System.Web.Security;
using EConnect.Utils.Common;
using EConnect.NIELIT;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Entity;
using System.Data.Common;
using System.Transactions;

public partial class Common_TransferRecordsFilter : BasePage
{
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Transfer Candidate Report", "Common/TransferRecordsFilter.aspx", ""));
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!IsPostBack)
            {
                bindCourse();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void bindCourse()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == 1
                              select new { ValueField = s.ID, TextField = s.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddllowercourse, courses, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            ddllowercourse.SelectedValue = "0";
            ddluppercourse.SelectedValue = "0";
            txtDateFrom.Text = "";
            txtToDate.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddllowercourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 lowercourseid = Convert.ToInt32(ddllowercourse.SelectedValue);
            if(lowercourseid !=0)
            {
                using (var context = new EConnectContext())
                {
                    ListItem lst = new ListItem("--Select One--", "0");
                    var courses = from s in context.Courses
                                  where s.CourseCategoryID == 1 && s.LowerCourseID >= lowercourseid
                                  select new { ValueField = s.ID, TextField = s.Name };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddluppercourse, courses, lst);
                }
            }
            if (lowercourseid == 0)
            {
                ListItem lst2 = new ListItem("--Select One--", "0");
                ddluppercourse.Items.Clear();
                ddluppercourse.Items.Insert(0, lst2);
                txtDateFrom.Text = "";
                txtToDate.Text = "";
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}