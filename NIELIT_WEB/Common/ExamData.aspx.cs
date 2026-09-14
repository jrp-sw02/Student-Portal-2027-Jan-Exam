using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data;
using System.IO;

public partial class Common_ExamData : BasePage
{
    UserType loginUserType;   
    Int64 entityID = 0;
    Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerror.Text = "";
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
                BindCourse();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Data Report", "#", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    protected void BindCourse()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                var courses = from s in context.Courses
                              where s.CourseTypeID == courseType
                              select new { ValueField = s.ID, TextField = s.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        try
        {
            
            BreadCrumb1.Render();
            Int32 courseid = 0;
            string lvl = "";
            string examdate = "";
            if (ddlCourseName.SelectedValue != "0")
            {
                courseid = Convert.ToInt32(ddlCourseName.SelectedValue);
            }
            examdate = txtexamdate.Text.Trim();
            using (EConnectContext context = new EConnectContext())
            { lvl = context.Courses.Where(s => s.ID == courseid).FirstOrDefault().Code.ToUpper(); }       

            //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            con.Open();
            DataTable dt = new DataTable();

            if (ddltablename.SelectedValue == "1")
            {

                string strQuery = " select * from e_fm e " +
                                  " where e.lvl = '" + lvl + "' and REPLACE(CONVERT(VARCHAR(11), e.exam_dt, 106), ' ', '-') = '" + examdate + "' order by e.batch , e.s_no asc ";

                string sheetname = ddlCourseName.SelectedItem.Text.Replace("/", "_") + "_" + txtexamdate.Text.Replace(" ", "").Replace(",", "") + "_exam_details";
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, null, CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }
                    GridView1.RenderControl(hw);

                    //style to format numbers to string

                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                    con.Close();
                }
                else
                {
                    ShowAlert("No record found.");
                    con.Close();
                }

            }
            else if (ddltablename.SelectedValue == "2")
            {
                string strQuery = " select * from e_fm_sub s " +
                                  " where s.lvl = '" + lvl + "' and REPLACE(CONVERT(VARCHAR(11), s.exam_dt, 106), ' ', '-') = '" + examdate + "' order by s.batch , s.s_no asc ";

                string sheetname = ddlCourseName.SelectedItem.Text.Replace("/", "_") + "_" + txtexamdate.Text.Replace(" ", "").Replace(",", "") + "_exam_sub_details";
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, null, CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }
                    GridView1.RenderControl(hw);

                    //style to format numbers to string

                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                    con.Close();
                }
                else
                {
                    ShowAlert("No record found.");
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            con.Close();
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddltablename.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            txtexamdate.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}