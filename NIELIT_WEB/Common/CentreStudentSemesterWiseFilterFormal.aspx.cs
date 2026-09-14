using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Collections.Generic;
using System.Web.UI;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;

public partial class Common_CentreStudentSemesterWiseFilterFormal : BasePage
    {//CentreStudentSemesterWiseFilterFormal   //Common_NielitCentreStudentBatchFilter
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;//, IdNielitCentre=0;
    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserTypeId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Student Semester Formal", "Common/CentreStudentSemesterWiseFilterFormal.aspx", ""));
           
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);      

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                //Response.Write("Sorry! You don't have rights  to view this page");
                //Response.End();
            }

            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

            if (!IsPostBack)
            {
              User objUser;
              using (EConnectContext context = new EConnectContext())
              {
                  objUser = new EConnect.URM.User();

                  User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                  using (NIELITMISContext context1 = new NIELITMISContext())
                  {
                      if (UserTypeId == 6)   //For Ho only 
                      {
                          rdoRow.Visible = false;
                          rdoRow1.Visible = false;
                          rdoRow2.Visible = false;
                          ddlSubcentreName.Enabled = true;
                          Label5.Text = "Centre Name";
                          ListItem lst1 = new ListItem("--Select One--", "0");
                          var centreName1 = from s in context1.NielitCentres
                                            select new { ValueField = s.ID, TextField = s.Name };

                          if (centreName1 != null)
                          {
                              var centreName = centreName1.OrderBy(i => i.ValueField);
                              EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                              ddlSubcentreName.Enabled = true;
                          }
                      }

                 else if (UserTypeId == 10)   //10Project NIELIT Centres
                      {
                          var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                          Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                          NielitCentreIdFilter = NielitCentreId;
                          HNonAfflAfflInst.Value = "99";
                          NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                          if (NielitCentrelinkedToCentreId != 0)
                          {
                              NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                              txtInstitute.Text = intitutesName.Name;
                              Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                              NielitCentreIdFilter = NelitCentreLinkId;
                              NIELITCentreId.Value = NelitCentreLinkId.ToString();
                              RdoAffInstOrNonAffInst.SelectedValue = "2";
                              ddlSubcentreName.Enabled = false;
                          }
                          else
                          {
                              NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                              txtInstitute.Text = intitutesName.Name;
                              Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                              NielitCentreIdFilter = NelitCentreLinkId;
                              NIELITCentreId.Value = NelitCentreLinkId.ToString();
                              RdoAffInstOrNonAffInst.SelectedValue = "2";
                              ddlSubcentreName.Enabled = false;
                          }
                      }
                  else if (UserTypeId == 11)   //Non Affiliated Institute
                      {
                          var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber); //HNonAfflAfflInst
                          NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                          HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                          NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                          NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                          if (institutesName != null)
                          {
                              txtInstitute.Text = institutesName.Name;
                              Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                              NIELITCentreId.Value = NelitCentreLinkId.ToString();
                              NielitCentreIdFilter = NelitCentreLinkId;
                          }
                          RdoAffInstOrNonAffInst.Items.RemoveAt(0);
                          FillddlSubcentreName();
                          RdoAffInstOrNonAffInst.Items.RemoveAt(1);
                      }
                  else if (UserTypeId == 4)     //Institute
                      {
                          var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                          NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                          HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                          NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                          NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                          if (institutesName != null)
                          {
                              txtInstitute.Text = institutesName.Name;
                              Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                              NIELITCentreId.Value = NelitCentreLinkId.ToString();
                              NielitCentreIdFilter = NelitCentreLinkId;   //5011
                          }
                          RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                          FillddlSubcentreName();
                          RdoAffInstOrNonAffInst.Items.RemoveAt(1);
                      }
                  }
                  FillCourseCategory();
                  bindCastCategory();
              }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    #region vCode
    protected void btnReset_Click(object sender, EventArgs e)
        {
        try
            {
            Response.Redirect("~/common/CentreStudentSemesterWiseFilterFormal.aspx");

            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    protected void bindCastCategory()
        {
        try
            {
            using (var context = new EConnectContext())
                {
                ListItem lst = new ListItem("--All--", "0");

                var castcategory = from p in context.CastCategories
                                   orderby (p.DisplayOrder)
                                   select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);

                };
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    
    protected void FillCourseCategory()
    {
        try
        {
        using (NIELITMISContext context = new NIELITMISContext())
            {

            ListItem lst1 = new ListItem("--Select One--", "0");

            var cCat = from c in context.NielitCentreCourseCategorys
                       where c.Name == "Formal Courses"
                       select new { ValueField = c.ID, TextField = c.Name };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, cCat, lst1);
            }
            //using (DataTable dt = FillCourseCategoryNIELITMISCourseCatNielitCourseCatRecord())
            //{
            //    if (dt.Rows.Count > 0)
            //    {                  
            //        ddlcoursecategory.DataSource = dt;                  
            //        ddlcoursecategory.DataTextField = "Name";
            //        ddlcoursecategory.DataValueField = "ID";
            //        ddlcoursecategory.DataBind();
            //        ddlcoursecategory.Items.Insert(0, new ListItem("--Select One--", "0"));
            //    }
            //}
         
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void FillCourseName()
        {
        try
            {
            //User objUser;
            //loginUserNo = Convert.ToInt32(Session["UserID"]);
            //using (EConnectContext contextNielit = new EConnectContext())
            //    {
            //    objUser = new EConnect.URM.User();
            //    User loginUser = contextNielit.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            //    Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            //    }
            Int32 nielitcentreID = 0;
                if (UserTypeId == 6)   //For Ho only  
                {
                     nielitcentreID = Convert.ToInt32(ddlSubcentreName.SelectedValue);
                    NIELITCentreId.Value = ddlSubcentreName.SelectedValue;
                }
                else
                {
                     nielitcentreID = Convert.ToInt32(NIELITCentreId.Value);
                }
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            if (coursecatID != 0)
                {
                using (NIELITMISContext context = new NIELITMISContext())
                    {
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var courseName = from c in context.NielitCentreCourses
                                     join d in context.NielitCourseDurations on c.ID equals d.courseID
                                     join b in context.NielitCentreBatchs on d.ID equals b.CourseDurationID
                                     join s in context.SemesterMaster on b.ID equals s.BatchId //b.CourseDurationID equals s.CourseId
                                     join sd in context.SemesterDetail on b.ID equals sd.batchID
                                     where c.ID == d.courseID && d.ID == b.CourseDurationID && b.CourseDurationID == s.CourseId
                                     && s.CourseId == sd.CourseDurationID && b.ID == s.BatchId && s.BatchId == sd.batchID
                                     && c.CourseCategoryID == coursecatID
                                     //&& c.CourseCategoryID == coursecatID 
					&& (b.centreID == nielitcentreID || b.subCentreID == nielitcentreID)
                                     //orderby (c.Name)
                                     //NielitCentreIdFilter
                                     select new { ValueField = d.ID, TextField = c.Name + "(" + d.courseDurationDays + " Day)" + d.courseDurationHrs + " Hrs" };
                    if (courseName != null)
                        {
                        //var courseName1 = courseName.Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, courseName.Distinct(), lst1);
                        }
                    }
                }

            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    //
    protected void FillBatchName()
        {
        try
            {
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            Int32 courseID = Convert.ToInt32(ddlcourseName.SelectedValue);
            Int32 nielitcentreID = Convert.ToInt32(NIELITCentreId.Value);
            if (coursecatID != 0 && courseID != 0)
                {
                using (NIELITMISContext context = new NIELITMISContext())
                    {
                    ListItem lst = new ListItem("--Select One--", "0");
                    var batchName = from c in context.NielitCentreCourses
                                    join cc in context.NielitCentreCourseCategorys on c.CourseCategoryID equals cc.ID
                                    join d in context.NielitCourseDurations on c.ID equals d.courseID
                                    join b in context.NielitCentreBatchs on d.ID equals b.CourseDurationID
                                    join n in context.NielitCentres on b.centreID equals n.ID
                                    join s in context.SemesterMaster on b.ID equals s.BatchId
                                    join sd in context.SemesterDetail on b.ID equals sd.batchID
                                    where c.ID == d.courseID && d.ID == b.CourseDurationID && b.CourseDurationID == s.CourseId && s.CourseId == sd.CourseDurationID
                                    && b.ID == s.BatchId && s.BatchId == sd.batchID
                                    && cc.ID == coursecatID && d.ID == courseID && (n.ID == nielitcentreID  || b.subCentreID == nielitcentreID)
                                    orderby (b.Name)
                                    select new { ValueField = b.ID, TextField = b.Name };
                    if (batchName != null)
                        {
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, batchName.Distinct(), lst);
                        }
                    };
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    protected void FillSemesterNo()
        {
        try
            {
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            Int32 courseID = Convert.ToInt32(ddlcourseName.SelectedValue);
            Int32 batchID = Convert.ToInt32(ddlbatchname.SelectedValue);
            Int32 nielitcentreID = Convert.ToInt32(NIELITCentreId.Value);

            if (coursecatID != 0 && courseID != 0 && batchID !=0)
                {
                using (NIELITMISContext context = new NIELITMISContext())
                    {
                    ListItem lst = new ListItem("--Select One--", "0");
                    var semesterNo = from c in context.NielitCentreCourses
                                    join cc in context.NielitCentreCourseCategorys on c.CourseCategoryID equals cc.ID
                                    join d in context.NielitCourseDurations on c.ID equals d.courseID
                                    join b in context.NielitCentreBatchs on d.ID equals b.CourseDurationID
                                    join n in context.NielitCentres on b.centreID equals n.ID
                                    join s in context.SemesterMaster on b.ID equals s.BatchId
                                    join sd in context.SemesterDetail on b.ID equals sd.batchID
                                    where c.ID == d.courseID && d.ID == b.CourseDurationID && b.CourseDurationID == s.CourseId && s.CourseId == sd.CourseDurationID
                                    && b.ID == s.BatchId && s.BatchId == sd.batchID
                                    && cc.ID == coursecatID && d.ID == courseID && b.ID == batchID //&& n.ID == nielitcentreID  || b.subCentreID == nielitcentreID
                                    //orderby (b.Name)
                                     select new { ValueField = sd.semesterNo, TextField = sd.semesterNo };
                    if (semesterNo != null)
                        {
                        var semesterNo1 = semesterNo.ToList();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSemesterNo, semesterNo.Distinct(), lst);
                        }
                    };
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
        
    //

    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        try
            {
         
            ddlcourseName.Items.Clear();
            ddlcourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
            FillCourseName();
            ddlbatchname.Items.Clear();
            ddlbatchname.Items.Insert(0, "--Select--");
            ddlSemesterNo.Items.Clear();
            ddlSemesterNo.Items.Insert(0, "--Select--");
            #region 
            /*if (CourseType <= 2)
             {
                 if (coursecatID != 0)
                 {
                     using (DataTable dt = FillCourseNIELITMISCourseNielitCourseRecord())
                     {
                         if (dt.Rows.Count > 0)
                         {
                             ddlcourseName.Items.Clear();
                             //ddlcourseName.Items.Insert(0, "--Select--");
                             ddlcourseName.DataSource = dt;
                             ddlcourseName.DataTextField = "Name";
                             ddlcourseName.DataValueField = "ID";
                             ddlcourseName.DataBind();
                             ddlcourseName.Items.Insert(0, new ListItem("--Select--", "0"));
                         }
                         else
                         {
                             ddlcourseName.Items.Clear();
                             ddlcourseName.Items.Insert(0, "--Select--");
                         }
                     }
                 }
             }
             else
             {
                 if (coursecatID != 0)
                 {
                     using (NIELITMISContext context = new NIELITMISContext())
                     {
                         ListItem lst = new ListItem("--Select One--", "0");
                         var CourseList = from p in context.NielitCentreCourses 
                                          join d  in context.NielitCourseDurations on p.ID equals d.courseID
                                          join b in context.NielitCentreBatchs on d.ID equals b.CourseDurationID
                                          where p.CourseCategoryID == coursecatID
                                          orderby (p.Name)
                                          select new { ValueField = p.ID, TextField = p.Name };
                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList.Distinct(), lst);
                     };
                 }
             }*/
            #endregion
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    protected void ddlcourseName_SelectedIndexChanged(object sender, EventArgs e)
        {
        try
            {
            ddlbatchname.Items.Clear();
            ddlbatchname.Items.Insert(0, "--Select--");
            ddlSemesterNo.Items.Clear();
            ddlSemesterNo.Items.Insert(0, "--Select--");
            FillBatchName();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    protected void ddlbatchname_SelectedIndexChanged(object sender, EventArgs e)
        {
        try
            {
            //ddlbatchname.Items.Clear();
            //ddlbatchname.Items.Insert(0, "--Select--");
            ddlSemesterNo.Items.Clear();
            ddlSemesterNo.Items.Insert(0, "--Select--");
            FillSemesterNo();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    protected void ddlSemesterNo_SelectedIndexChanged(object sender, EventArgs e)
        {
        //try
        //    {
        //    ddlbatchname.Items.Clear();
        //    ddlbatchname.Items.Insert(0, "--Select--");
        //    ddlSemesterNo.Items.Clear();
        //    ddlSemesterNo.Items.Insert(0, "--Select--");
        //    FillSemesterNo();
        //    }
        //catch (Exception ex)
        //    {
        //    ShowAlert(ex.Message);
        //    }
        }
    #endregion vCode


    #region other
    public DataTable FillCourseCategoryNIELITMISCourseCatNielitCourseCatRecord()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCourseCategoryNIELITMISCourseCatNielitCourseCatRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    public DataTable FillCourseNIELITMISCourseNielitCourseRecord()
    {
        Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetFillCourseNIELITMISCourseNielitCourseRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CourseCat", SqlDbType.Int));
                    cmd.Parameters["@CourseCat"].Value = coursecatID;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void FillddlSubcentreName()
        {
        try
            {
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
                {
                objUser = new EConnect.URM.User();
                ListItem lst1 = new ListItem("--Select One--", "0");
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context = new NIELITMISContext())
                    {
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                    txtInstitute.Text = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                    //lblCentreId.Text = NelitCentreLinkId.ToString();
                    ddlSubcentreName.ClearSelection();
                    if (UserTypeId == 11)
                        {
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                        ddlSubcentreName.Enabled = false;
                        var NonAfflcentre = (from p in context.NonAffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                        }
                    else
                        {
                        var centreName = from s in context.AffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                        ddlSubcentreName.Enabled = false;
                        var NonAfflcentre = (from p in context.AffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                        }
                    }
                }
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message, true);
            }
        }
    
    protected void ddlWhProjectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Int32 nielitcentreID = Convert.ToInt32(NIELITCentreId.Value);
            //Int32 nielitSubcentreID = Convert.ToInt32(ddlSubcentreName.SelectedValue);
            //Int32 courseID = Convert.ToInt32(ddlcourseName.SelectedValue);
            //Int32 BatchID = Convert.ToInt32(ddlbatchname.SelectedValue);
            //Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            //Int32 CourseType = coursecatID.ToString().Length;
            //Int32 WhProjectStudent = Convert.ToInt32(ddlWhProjectStudent.SelectedValue);
            //ddlProjectName.Items.Clear();
            ////ddlProjectName.Items.Insert(0, "--All--");
            //ddlProjectName.Items.Insert(0, new ListItem("--All--", "0"));
            //    if (WhProjectStudent != 0 && WhProjectStudent!=2)
            //    {
            //        if (nielitSubcentreID != 0)
            //        {
            //            using (NIELITMISContext context = new NIELITMISContext())
            //            {
            //                ListItem lst = new ListItem("--All--", "0");
            //                var ProjectList = from b in context.NielitCentreStudent
            //                                  join p in context.NielitProjectss on b.projectId equals p.ID
            //                                  where b.batch_ID == BatchID &&  b.InstituteID == nielitSubcentreID
            //                                  orderby (b.Name)
            //                                  select new { ValueField = p.ID, TextField = p.ProjectName };
            //                ProjectList = ProjectList.Distinct();
            //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjectName, ProjectList, lst);
            //            };
            //        }
            //        else
            //        {
            //            using (NIELITMISContext context = new NIELITMISContext())
            //            {
            //                ListItem lst = new ListItem("--All--", "0");
            //                var ProjectList = from b in context.NielitCentreStudent
            //                                  join p in context.NielitProjectss on b.projectId equals p.ID
            //                                  where b.batch_ID == BatchID && b.InstituteID == nielitcentreID 
            //                                  orderby (b.Name)
            //                                  select new { ValueField = b.ID, TextField = p.ProjectName };
            //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjectName, ProjectList.Distinct(), lst);
            //            };
            //        }
            //    }
                     

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    
    protected void ddlSubcentreName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    
    protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            User objUser;           
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context2 = new NIELITMISContext())
                {
                    //if (UserTypeId == 6 & RdoAffInstOrNonAffInst.SelectedValue == "2")
                    //{
                    //    ddlSubcentreName.Enabled = true;
                    //    Label5.Text = "Centre Name";
                    //}
                    //else if (UserTypeId == 6)
                    //{
                    //    ddlSubcentreName.Enabled = true;
                    //    Label5.Text = "Sub Centre Name";
                    //}

                    if (UserTypeId == 10)
                    {
                        var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeId == 11)
                    {
                        var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        NIELITCentreId.Value = NelitCentreLinkId.ToString();
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "0");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlSubcentreName.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                 ddlSubcentreName.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                if (UserTypeId == 11)//Non AffInstitutes by user refNumber
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                    ddlSubcentreName.Enabled = false;
                                    var NonAfflcentre = (from p in context.NonAffInstitutes
                                                         where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                         select p).FirstOrDefault();
                                    ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                                }
                                else //NonAffInstitutes for Nielit Centres
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                    ddlSubcentreName.Enabled = true;
                                }
                                if (UserTypeId == 4)//AffInstitutes by user refNumber
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                    ddlSubcentreName.Enabled = false;
                                    var Afflcentre = (from p in context.AffInstitutes
                                                      where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                      select p).FirstOrDefault();
                                    ddlSubcentreName.SelectedValue = Afflcentre.ID.ToString();
                                }
                                else // AffInstitutes for Nielit Centres
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                    ddlSubcentreName.Enabled = true;
                                }
                            }
                            else
                            {
                                ddlSubcentreName.Items.Add(new ListItem("--Select One--", "0"));
                                ddlSubcentreName.SelectedValue = "0";
                                ddlSubcentreName.Enabled = false;
                                //txtName.Text = "";
                                //txtBatchCode.Text = "";
                                //fillCourseWithDuration();
                            }
                        }
                    }
                    else
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "0");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlSubcentreName.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                ddlSubcentreName.Enabled = true;
                                //txtName.Text = "";
                                //txtBatchCode.Text = "";
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                ddlSubcentreName.ClearSelection();
                                var centreName = from s in context.NonAffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName, lst1);
                                ddlSubcentreName.Enabled = true;
                                //txtName.Text = "";
                                //txtBatchCode.Text = "";
                            }
                            else
                            {
                                ddlSubcentreName.Items.Add(new ListItem("--Select One--", "0"));
                                ddlSubcentreName.SelectedValue = "0";
                                ddlSubcentreName.Enabled = false;

                                NIELITCentreId.Value = NielitCentreId.ToString();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    
      //done
    
    
#endregion



}