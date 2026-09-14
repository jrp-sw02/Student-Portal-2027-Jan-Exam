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
using System.Data.Entity.Core.Metadata.Edm;

public partial class HO_NielitCentreStudentCompletion : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserTypeId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Student Batch", "HO/Rpt/NielitCentreStudentBatchFilter.aspx", ""));
           
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
              //  Response.Write("Sorry! You don't have rights  to view this page");
               // Response.End();
            //}
            if (!IsPostBack)
            {
              User objUser;
              using (EConnectContext context = new EConnectContext())
              {
                  objUser = new EConnect.URM.User();

                  User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                  using (NIELITMISContext context1 = new NIELITMISContext())
                  {

                      if (UserTypeId == 10)
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
                      else if (UserTypeId == 11)
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
                      else if (UserTypeId == 4)
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
                              NielitCentreIdFilter = NelitCentreLinkId;
                          }
                          RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                          FillddlSubcentreName();
                          RdoAffInstOrNonAffInst.Items.RemoveAt(1);
                      }
                  }
                  FillCourseCategory();
               
              }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
        protected void FillCourseCategory()
    {
        try
        {
            using (DataTable dt = FillCourseCategoryNIELITMISCourseCatNielitCourseCatRecord())
            {
                if (dt.Rows.Count > 0)
                {                  
                    ddlcoursecategory.DataSource = dt;                  
                    ddlcoursecategory.DataTextField = "Name";
                    ddlcoursecategory.DataValueField = "ID";
                    ddlcoursecategory.DataBind();
                    ddlcoursecategory.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }
         
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
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
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            grdStudent.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);

             Int32 CourseType = coursecatID.ToString().Length;
             if (CourseType <= 2)
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

             }
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
            Int32 nielitcentreID =  Convert.ToInt32(NIELITCentreId.Value);
            Int32 courseID = Convert.ToInt32(ddlcourseName.SelectedValue);
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
             Int32 CourseType = coursecatID.ToString().Length;
             ddlbatchname.Items.Clear();
             ddlbatchname.Items.Insert(0, "--Select--");
             if (CourseType <= 2)
             {
                 if (courseID != 0)
                 {
                     using (NIELITMISContext context = new NIELITMISContext())
                     {
                         ListItem lst = new ListItem("--Select One--", "0");
                         var CourseList = from b in context.NielitCentreBatchs
                                          where b.CourseDurationID == courseID && b.centreID == nielitcentreID || b.subCentreID == nielitcentreID
                                          orderby (b.Name)
                                          select new { ValueField = b.ID, TextField = b.Name };
                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, CourseList.Distinct(), lst);
                     };
                 }
             }
             else
             {
                 if (courseID != 0)
                 {
                     using (NIELITMISContext context = new NIELITMISContext())
                     {
                         ListItem lst = new ListItem("--Select One--", "0");
                         var CourseList = from b in context.NielitCentreBatchs
                                          join d in context.NielitCourseDurations on b.CourseDurationID equals d.ID
                                          where d.courseID == courseID && b.centreID == nielitcentreID || b.subCentreID == nielitcentreID
                                          orderby (b.Name)
                                          select new { ValueField = b.ID, TextField = b.Name };
                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, CourseList.Distinct(), lst);
                     };
                 }

             }

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
    
      
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try 
        {
            ddlcoursecategory.SelectedIndex = 0;
            ddlcourseName.SelectedIndex = 0;
            ddlbatchname.SelectedIndex = 0;
            grdStudent.DataSource = null;
            grdStudent.DataBind();
            grdStudent.Visible = false;
        
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }



    protected void btnView_Click(object sender, EventArgs e)
    {
        BindGridView();
        }
    protected void BindGridView()
    {
        string CS = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(CS);
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "getBatchStudentData";
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
            cmd.Parameters.Add("@pSubCentreID", SqlDbType.BigInt);
            cmd.Parameters.Add("@pCourseID", SqlDbType.BigInt);
            cmd.Parameters.Add("@pBatchID", SqlDbType.BigInt);

            cmd.Parameters["@pCentreID"].Value = NIELITCentreId.Value;
            cmd.Parameters["@pSubCentreID"].Value = ddlSubcentreName.SelectedValue;
            cmd.Parameters["@pCourseID"].Value = ddlcourseName.SelectedValue;
            cmd.Parameters["@pBatchID"].Value = ddlbatchname.SelectedValue;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            grdStudent.DataSource = ds;
            grdStudent.DataBind();
            grdStudent.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }
    protected void grdStudent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox cbCompleted = (CheckBox)e.Row.FindControl("cbCompleted");
            CheckBox cbCertified = (CheckBox)e.Row.FindControl("cbCertIssued");
            CheckBox cbDropOut = (CheckBox)e.Row.FindControl("cbDropOut");
            TextBox txtCertIssueDate = (TextBox)e.Row.FindControl("txtCertIssueDate");
            if (cbDropOut.Checked )
            {
                cbCompleted.Enabled = false;
                cbCertified.Enabled = false;
                txtCertIssueDate.Enabled = false;
                return;
            }
            
            if (cbCompleted.Checked && !cbCertified.Checked)
            {
                cbCompleted.Enabled = true;
                cbCertified.Enabled = true;
                txtCertIssueDate.Enabled = true;
                return;
            }
            if (cbCertified.Checked)
            {
                cbCertified.Enabled = true;
                txtCertIssueDate.Enabled = true;
                return;
            }


        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string numberError = "";
        string compError = "";
        foreach(GridViewRow dr in grdStudent.Rows)
        {
            CheckBox cbCompleted = (CheckBox)dr.FindControl("cbCompleted");
            CheckBox cbCertified = (CheckBox)dr.FindControl("cbCertIssued");
            CheckBox cbDropOut = (CheckBox)dr.FindControl("cbDropOut");
            TextBox txtCertIssueDate = (TextBox)dr.FindControl("txtCertIssueDate");
            if(cbCompleted .Enabled||cbCertified.Enabled||cbDropOut.Enabled )
            {
                if ((!cbCompleted.Checked) && ( cbCertified.Checked))
                {
                    //ShowAlert("Dropout cannot complete course or get certificate-" + dr.Cells[0].Text );
                    compError = compError + dr.Cells[0].Text + ", ";
                    continue;
                }
                else
                {
                    if ((cbDropOut.Checked && cbCompleted.Checked) || (cbDropOut.Checked && cbCertified.Checked))
                    {
                        //ShowAlert("Dropout cannot complete course or get certificate-" + dr.Cells[0].Text );
                        numberError = numberError + dr.Cells[0].Text + ", ";
                        continue;
                    }
                    else
                    {
                        string number = dr.Cells[0].Text;
                        int whetherCompleted = Convert.ToInt32(cbCompleted.Checked);
                        int whetherCertified = Convert.ToInt32(cbCertified.Checked);
                        string certIssueDate = txtCertIssueDate.Text;
                        if (txtCertIssueDate.Text == "")
                            certIssueDate = "0";
                        int whetherdropout = Convert.ToInt32(cbDropOut.Checked);
                        saveRecord(number, whetherCompleted, whetherCertified, certIssueDate, whetherdropout);
                    }
                }
            }                
        }
        if (numberError != "")
            ShowAlert("Dropout cannot complete course or get certificate, Error numbers are-" + numberError);
        else
            ShowAlert("Status updated");
    }

    protected void saveRecord(string pNumber,int whetherCompleted, int whetherCertified, string certIssueDate, int whetherdropout)
    {
        string CS = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(CS);
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "saveBatchStudentData";
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
            cmd.Parameters.Add("@pSubCentreID", SqlDbType.BigInt);
            cmd.Parameters.Add("@pCourseID", SqlDbType.BigInt);
            cmd.Parameters.Add("@pBatchID", SqlDbType.BigInt);
            cmd.Parameters.Add("@pNumber", SqlDbType.VarChar ,100);
            cmd.Parameters.Add("@pWhetherCompleted",SqlDbType.Int);
            cmd.Parameters.Add("@pWhetherCertified", SqlDbType.Int);
            cmd.Parameters.Add("@pCertIssuedate", SqlDbType.VarChar,20) ;
            cmd.Parameters.Add("@pWhetherDropOut",SqlDbType.Int) ;


            cmd.Parameters["@pCentreID"].Value = NIELITCentreId.Value;
            cmd.Parameters["@pSubCentreID"].Value = ddlSubcentreName.SelectedValue;
            cmd.Parameters["@pCourseID"].Value = ddlcourseName.SelectedValue;
            cmd.Parameters["@pBatchID"].Value = ddlbatchname.SelectedValue;
            cmd.Parameters["@pNumber"].Value=pNumber;
            cmd.Parameters["@pWhetherCompleted"].Value = whetherCompleted;
            cmd.Parameters["@pWhetherCertified"].Value = whetherCertified;
            cmd.Parameters["@pCertIssuedate"].Value = certIssueDate;
            cmd.Parameters["@pWhetherDropOut"].Value = whetherdropout;

            cmd.ExecuteNonQuery();
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            con.Close();
        }

    }                         
    protected void btnResetOptions_Click(object sender, EventArgs e)
    {
        btnView_Click(sender, e);

    }
}