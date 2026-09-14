using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class allCoursesExam : BasePage
{
    Table tbl = new Table();
    int tdCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //if (Request.UrlReferrer == null)
				//if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in" ) && (Request.UrlReferrer == null  || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
    //        {
    //            Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
    //            Response.End();
    //            return;
    //        }
            if (!Page.IsPostBack)
            {
                tbl.Width = Unit.Percentage(100);
                tbl.CellSpacing = 4;
                tbl.CellPadding = 0;
                tbl.BackColor = System.Drawing.Color.White;
                bindcoursecategory();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindcoursecategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                IQueryable<CourseCategory> ccat = context.Courses.Where(s => s.ShowOnWeb == false).Select(s => s.CourseCategory).Distinct();

                TableRow tr = new TableRow();
                StringBuilder sb;

                foreach (CourseCategory ct in ccat)
                {
                    if (tdCount == 0 || tdCount == 3)
                    {
                        tr = new TableRow(); tdCount = 0;
                    }

                    tdCount++;
                    TableCell td = new TableCell();
                    td.Width = Unit.Percentage(33);
                    td.Style.Add("padding", "5px 5px 5px 0");
                    sb = new StringBuilder();
                    sb.Append("<div class='Course_block' style='overflow:auto;overflow-x:hidden;'><div>" + ct.Name + "</div><ul>");

                    if (!string.IsNullOrEmpty((Request.QueryString["query"])))
                    {
                        #region Apply
                        if (Request.QueryString["query"].ToString() == "apply")
                        {
                            Lblerror.Visible = true;
                            Lblerror.Text = "Please select the certification/course for which you want to apply";
                            if (Request.UrlReferrer != null)
                            {
                                if (Request.UrlReferrer.ToString().ToLower().Contains("frmaccredetedcentre.aspx"))
                                {
                                    //BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                                {
                                    //BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx?query=" + Request.QueryString["query"].ToString(), ""));
                            }
                        }
                        #endregion

                        #region AdmitCard
                        else if (Request.QueryString["query"].ToString() == "admit")
                        {
                            Lblerror.Visible = true;
                            Lblerror.Text = "Please select the certification/course for which you want to download the admit card";
                            if (Request.UrlReferrer != null)
                            {
                                if (Request.UrlReferrer.ToString().ToLower().Contains("frmaccredetedcentre.aspx"))
                                {
                                    //BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                                {
                                    //BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx?query=" + Request.QueryString["query"].ToString(), ""));
                            }
                        }
                        #endregion

                        #region View Result
                        else if (Request.QueryString["query"].ToString() == "result")
                        {
                            Lblerror.Visible = true;
                            Lblerror.Text = "Please select the certification/course for which you want to view the result";
                            if (Request.UrlReferrer != null)
                            {
                                if (Request.UrlReferrer.ToString().ToLower().Contains("frmaccredetedcentre.aspx"))
                                {
                                    //BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                                {
                                    //BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx?query=" + Request.QueryString["query"].ToString(), ""));
                            }
                        }
                        #endregion

                        foreach (Course c in ct.courses.Where(s => s.IsActive == true && s.ShowOnWeb == false))
                        {
                            sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&query=" + Request.QueryString["query"].ToString() + "&candtype=Internal") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
                        }
                    }
                    else
                    {
                        Lblerror.Visible = false;
                        if (Request.UrlReferrer != null)
                        {
                            if (Request.UrlReferrer.ToString().ToLower().Contains("frmaccredetedcentre.aspx"))
                            {
                                //BreadCrumb1.RemoveLastBreadCrumbItem();
                            }
                            else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                            {
                                //BreadCrumb1.RemoveLastBreadCrumbItem();
                            }
                            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx", ""));
                        }
                        foreach (Course c in ct.courses.Where(s => s.IsActive == true && s.ShowOnWeb == false))
                        {
                            sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&candtype=Internal") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
                        }
                    }
                    sb.Append("</ul></div>");
                    td.Text = sb.ToString();
                    sb.Clear();
                    tr.Cells.Add(td);
                    tbl.Rows.Add(tr);
                }
                pnlCourses.Controls.Add(tbl);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}
