using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class allCourses : BasePage
{
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
            { bindcoursecategory(); }
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
    }
    protected void bindcoursecategory()
    {
        try
        {
            Table tbl = new Table();
            int tdCount = 0;
            tbl.Width = Unit.Percentage(100);
            tbl.CellSpacing = 4;
            tbl.CellPadding = 0;
            tbl.BackColor = System.Drawing.Color.White;

            using (EConnectContext context = new EConnectContext())
            {

                //IQueryable<CourseCategory> ccat = context.CourseCategories.Where(s => s.courses.FirstOrDefault().ShowOnWeb == true);
                IQueryable<CourseCategory> ccat = context.CourseCategories.Where(s => s.IsActive == true && s.ID != 6);                
                TableRow tr = new TableRow();
                StringBuilder sb;

                foreach (CourseCategory ct in ccat)
                {//Added to hide category without course 24 Jan 2023
                    int x = (from s in context.Courses
                             where s.CourseCategoryID == ct.ID
                             && s.IsActive == true
                             && s.ShowOnWeb == true
                             select s).Count();
                    if(x==0)
                        continue;
                    //

                    if (tdCount == 0 || tdCount == 3)
                    {
                        tr = new TableRow();
                        tdCount = 0;
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
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                                {
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx?query=" + Request.QueryString["query"], ""));
                            }
                            foreach (Course c in ct.courses.Where(s => s.IsActive == true && s.ShowOnWeb == true))
                            {
                                sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&query=" + Request.QueryString["query"].ToString() + "&candtype=External") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
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
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                                {
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx?query=" + Request.QueryString["query"], ""));
                            }
                            foreach (Course c in ct.courses.Where(s => s.ShowOnWeb == true))
                            {
                                sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&query=" + Request.QueryString["query"].ToString() + "&candtype=External") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
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
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                                {
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx?query=" + Request.QueryString["query"], ""));
                            }
                            foreach (Course c in ct.courses.Where(s => s.ShowOnWeb == true))
                            {
                                sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&query=" + Request.QueryString["query"].ToString() + "&candtype=External") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Lblerror.Visible = false;
                        if (Request.UrlReferrer != null)
                        {
                            if (Request.UrlReferrer.ToString().ToLower().Contains("frmaccredetedcentre.aspx"))
                            {
                                BreadCrumb1.RemoveLastBreadCrumbItem();
                            }
                            else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                            {
                                BreadCrumb1.RemoveLastBreadCrumbItem();
                            }
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx", ""));
                        }
                        foreach (Course c in ct.courses.Where(s => s.IsActive == true && s.ShowOnWeb == true))
                        {
                            sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&candtype=External") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
                        }
                    }
                    sb.Append("</ul></div>");
                    td.Text = sb.ToString();
                    sb.Clear();
                    tr.Cells.Add(td);
                    tbl.Rows.Add(tr);
                }
                pnlCourses.Controls.Add(tbl);
		 ShowNSQFBlock();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
 protected void ShowNSQFBlock()
    {
        try
        {
            Panel pnl1 = new Panel();
            pnl1.GroupingText = "<b>OTHER NSQF COURSES</b>";
            this.Controls.Add(pnl1);
            Table tbl = new Table();
            int tdCount = 0;
            tbl.Width = Unit.Percentage(100);
            tbl.CellSpacing = 4;
            tbl.CellPadding = 0;
            tbl.BackColor = System.Drawing.Color.White;

            using (EConnectContext context = new EConnectContext())
            {
                NIELITMISContext context1 = new NIELITMISContext();
                //IQueryable<CourseCategory> ccat = context.CourseCategories.Where(s => s.courses.FirstOrDefault().ShowOnWeb == true);
                IQueryable<NielitTrgSpecialization> ccat = context1.NielitTrgSpecializations.Where(s => s.specializationCode.ToString().Length != 0).OrderBy (s=>s.specializationName );
                TableRow tr = new TableRow();
                StringBuilder sb;

                foreach (NielitTrgSpecialization ct in ccat)
                {
                    int x = (from s in context.Courses
                             join p in context.CourseLevelDurationss
                             on s.ID equals p.CourseID
                             where s.NIELITrgSplnID == ct.ID
                             && s.IsActive == true
                             && s.ShowOnWeb == true
                             && s.CourseCategoryID == 6
                             select s).Count();
                    if (x == 0)
                        continue;
                    //
                    if (tdCount == 0 || tdCount == 3)
                    {
                        tr = new TableRow();
                        tdCount = 0;
                    }
                    tdCount++;
                    TableCell td = new TableCell();
                    td.Width = Unit.Percentage(33);
                    td.Style.Add("padding", "5px 5px 5px 0");
                    sb = new StringBuilder();
                    sb.Append("<div class='Course_block' style='overflow:auto;overflow-x:hidden;'><div>" + ct.specializationName + "</div><ul>");


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
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                                {
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx?query=" + Request.QueryString["query"], ""));
                            }
                            var cNSQF = (from s in context.Courses
                                         join p in context.CourseLevelDurationss
                                         on s.ID equals p.CourseID
                                         orderby s.Name 
                                         where s.NIELITrgSplnID == ct.ID
                                          && s.CourseCategoryID == 6
					   && p.Effective_From_Date <= DateTime .Today && ( p.Effective_To_Date ==null || p.Effective_To_Date >= DateTime.Today )
                                         && s.IsActive == true
                                         && s.ShowOnWeb == true
                                         select s);
                            foreach (Course c in cNSQF)
                            {
                                sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&query=" + Request.QueryString["query"].ToString() + "&candtype=External") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
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
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                                {
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx?query=" + Request.QueryString["query"], ""));
                            }
                            var cNSQF = (from s in context.Courses
                                         join p in context.CourseLevelDurationss
                                         on s.ID equals p.CourseID
                                         orderby s.Name 
                                         where s.NIELITrgSplnID == ct.ID
                                          && s.CourseCategoryID == 6
					   && p.Effective_From_Date <= DateTime .Today && ( p.Effective_To_Date ==null || p.Effective_To_Date >= DateTime.Today )
                                         && s.IsActive == true
                                         && s.ShowOnWeb == true
                                         select s);
                            foreach (Course c in cNSQF)
                            {
                                sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&query=" + Request.QueryString["query"].ToString() + "&candtype=External") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
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
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                                {
                                    BreadCrumb1.RemoveLastBreadCrumbItem();
                                }
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx?query=" + Request.QueryString["query"], ""));
                            }
                            var cNSQF = (from s in context.Courses
                                         join p in context.CourseLevelDurationss
                                         on s.ID equals p.CourseID
                                         orderby s.Name 
                                         where s.NIELITrgSplnID == ct.ID
                                          && s.CourseCategoryID == 6
					  && p.Effective_From_Date <= DateTime .Today && ( p.Effective_To_Date ==null || p.Effective_To_Date >= DateTime.Today )
                                         && s.IsActive == true
                                         && s.ShowOnWeb == true
                                         select s);
                            foreach (Course c in cNSQF)
                            {
                                sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&query=" + Request.QueryString["query"].ToString() + "&candtype=External") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Lblerror.Visible = false;
                        if (Request.UrlReferrer != null)
                        {
                            if (Request.UrlReferrer.ToString().ToLower().Contains("frmaccredetedcentre.aspx"))
                            {
                                BreadCrumb1.RemoveLastBreadCrumbItem();
                            }
                            else if (Request.UrlReferrer.ToString().ToLower().Contains("abt_centers.aspx"))
                            {
                                BreadCrumb1.RemoveLastBreadCrumbItem();
                            }
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "WEB/allCourses.aspx", ""));
                        }
                        var cNSQF = (from s in context.Courses
                                     join p in context.CourseLevelDurationss
                                     on s.ID equals p.CourseID
                                     orderby s.Name 
                                     where s.NIELITrgSplnID == ct.ID
                                      && s.CourseCategoryID == 6
				          && p.Effective_From_Date <= DateTime .Today && ( p.Effective_To_Date ==null || p.Effective_To_Date >= DateTime.Today )
                                     && s.IsActive == true
                                     && s.ShowOnWeb == true
                                     select s);
                        foreach (Course c in cNSQF)
                        {
                            sb.Append("<li style='list-style-type: decimal;'><a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("aboutcourse.aspx?id=" + c.ID.ToString() + "&candtype=External") + "'>" + c.Name + " (" + c.Code + ")</a></li> ");
                        }
                    }
                    sb.Append("</ul></div>");
                    td.Text = sb.ToString();
                    sb.Clear();
                    tr.Cells.Add(td);
                    tbl.Rows.Add(tr);
                }
                pnl1.Controls.Add(tbl);

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }


    }

}

