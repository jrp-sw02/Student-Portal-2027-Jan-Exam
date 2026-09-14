using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_MISBulkUploadStudents : BasePage
{
    Int32 loginUserNo = 0;
    String strMessage = string.Empty;
    Int64 entityID = 0;
    UserType loginUserType;
    Int32 currentRoleId = 0;
    Int32 UserRefNumber = 0;
    Int32 UserTypeid = 0;
    Int32 lnkID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    List<SqlParameter> paramList = new List<SqlParameter>();
    ArrayList TempDataTable = new ArrayList();
    ArrayList CheckBoxArray = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            loginUserNo = Convert.ToInt32(Session["UserID"]);

            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");

            entityID = Convert.ToInt64(Session["EntityID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);


            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt32(Session["EntityID"]);
            UserTypeid = Convert.ToInt32(Session["UserType"]);

           
            if (ViewState["CheckBoxArray"] != null)
            {
                CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
            }
            else
            {
                CheckBoxArray = new ArrayList();
            }

            if (ViewState["TempDataTable"] != null)
            {
                TempDataTable = (ArrayList)ViewState["TempDataTable"];
            }
            else
            {
                TempDataTable = new ArrayList();
            }

            ViewState["CheckBoxArray"] = CheckBoxArray;
            ViewState["TempDataTable"] = TempDataTable;


            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();

                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();

                    UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {

                        if (UserTypeid == 10) // Project NIELIT Centre
                        {
                            ListItem lst = new ListItem("--Select One--", "0");
                            var Center = from t in context1.NielitCentres
                                         where t.ID == UserRefNumber
                                         orderby (t.Name)
                                         select new { ValueField = t.ID, TextField = t.Name };

                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst);

                            var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);

                            if (lnkID != 0)
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                RdoAffInstOrNonAffInst.SelectedValue = "2";
                                // ddlSubcentreName.Enabled = false;
                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                RdoAffInstOrNonAffInst.SelectedValue = "2";
                                ddlCenter.SelectedValue = NielitCentreId.ToString();
                                ddlCenter.Enabled = false;
                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                            }
                        }

                    }

                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("MIS Bulk Data Upload", "Admin/MISBulkDataUpload.aspx?reset=1", ""));

                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    /*Backend Validations Start */
    protected bool isSelected(DropDownList Dropdown)
    {
        try
        {
            if (Dropdown.SelectedValue == "0")
            {
                Dropdown.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isNumber(TextBox txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Text.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Text.Trim()))
                {
                    txtBox.Text = "";
                    txtBox.Focus();
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidDob(TextBox txtBox)
    {
        try
        {
            DateTime today = DateTime.Now;
            DateTime inputDate = Convert.ToDateTime(txtBox.Text);

            if (inputDate > today.AddYears(-10))
            {
                return false;  // too young
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
    protected bool isValidForm()
    {

        if (!isSelected(ddlCenter))
        {
            lblerror.Visible = true;
            lblerror.Text = " Please select School";

            return false;
        }

        return true;
    }
    /* Backend Validations End */
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            using (NIELITMISContext ctx = new NIELITMISContext())
            {
                var batch = ctx.NielitCentreBatchs.Find(Convert.ToInt64(ddlBatch.SelectedValue));

                if (batch != null)
                {
                    lblBatchStart.Text = "Start Date : " + batch.startDate.ToString("dd/MMM/yyyy");
                    lblBatchEnd.Text = "End Date : " + batch.endDate.ToString("dd/MMM/yyyy");


                }

            }

            ddlCourse.Enabled = false;
            ddlBatch.Enabled = false;
            btnClick.Enabled = false;
            btnClick.Visible = false;

            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.InnerException.ToString(), true);
            lblerror.Visible = true;
            lblerror.Text = ex.InnerException.ToString();
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            ddlCourse.SelectedIndex = 0;
            ddlBatch.SelectedIndex = 0;


            ddlCourse.Enabled = true;
            ddlBatch.Enabled = true;
            btnClick.Enabled = true;
            btnClick.Visible = true;


            gvMain.DataSource = null;
            gvMain.DataBind();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;

            PagingBar1.Visible = false;
            gvMain.Visible = false;

            ViewState["CheckBoxArray"] = null;
            ViewState["TempDataTable"] = null;

            lblMessage.Visible = false;
            lblerror.Visible = false;

            lblBatchEnd.Text = "";
            lblBatchStart.Text = "";

            savebuttondiv.Visible = false;

            gvMain2.DataSource = null;
            lblsuccesscnt.Text = "0"; lblfailurecnt.Text = "0";
            Save_Record_section.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.InnerException.ToString(), true);
            lblerror.Visible = true;
            lblerror.Text = ex.InnerException.ToString();

        }


    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime currentdate = DateTime.Now;
            getGridcheckedvalues();

            Guid v_guid = Guid.NewGuid();

            string guid = v_guid.ToString();


            Int32 successCount = 0;
            Int32 failureCount = 0;

            if (ViewState["CheckBoxArray"] != null && ((ArrayList)ViewState["CheckBoxArray"]).Count > 0)
            {
                ArrayList TempDataTable1 = (ArrayList)ViewState["TempDataTable"];
                foreach (string cc in TempDataTable1)
                {
                    string IDCutfrmIndex = cc;
                    Int64 projectID = Convert.ToInt64(IDCutfrmIndex.Split('/')[0]);
                    Int64 regnNo = Convert.ToInt64(IDCutfrmIndex.Split('/')[1]);
                    Int64 Cast_Category_ID = Convert.ToInt64(IDCutfrmIndex.Split('/')[2]);
                    Int32 District_ID = Convert.ToInt32(IDCutfrmIndex.Split('/')[3]);


                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
                    {
                        connection.Open();

                        string failureReason = "";

                        if (projectID == 2 && Cast_Category_ID != 2 && Cast_Category_ID != 3)
                        {
                            failureReason = "Only Project for SC/ST Cand.";
                        }

                        if (projectID == 1 && checkAspirationalDistrict(District_ID) == 0)
                        {
                            failureReason += " Only Project for Aspirational District Cand";
                        }


                        if (failureReason != "")
                        {
                            string sql = @"INSERT INTO dbo.BulkUploadMIS_Log
                        (
                            Registration_No,
                            CourseID,
                            ProjectID,
                            Status,
                            Reason,
                            EnterBy,
                            EnterDate,
                            guid
                        )
                        VALUES
                        (
                            @regnNo,
                            @courseID,
                            @projectId,
                            'FAILED',
                            @reason,
                            @enterBy,
                            @enterDate,
                            @guid
                        )";

                            using (var command = new SqlCommand(sql, connection))
                            {
                                command.CommandType = CommandType.Text;

                                command.Parameters.AddWithValue("@regnNo", regnNo);
                                command.Parameters.AddWithValue("@courseID", ddlCourse.SelectedValue);
                                command.Parameters.AddWithValue("@projectId", projectID);
                                command.Parameters.AddWithValue("@reason", failureReason);
                                command.Parameters.AddWithValue("@enterBy", loginUserNo);
                                command.Parameters.AddWithValue("@enterDate", currentdate);
                                command.Parameters.AddWithValue("@guid", guid);
                                command.ExecuteNonQuery();

                                failureCount += 1;

                                continue;
                            }
                        }


                        using (var command = new SqlCommand("saveDataForBulkUploadMIS", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@regnNo", regnNo);
                            command.Parameters.AddWithValue("@centreID", ddlCenter.SelectedValue);
                            command.Parameters.AddWithValue("@projectId", projectID);
                            command.Parameters.AddWithValue("@courseID", ddlCourse.SelectedValue);
                            command.Parameters.AddWithValue("@batchID", ddlBatch.SelectedValue);
                            command.Parameters.AddWithValue("@enterBy", loginUserNo);
                            command.Parameters.AddWithValue("@enterDate", currentdate);
                            command.Parameters.AddWithValue("@guid", guid);
                            // output parameters
                            SqlParameter successParam = new SqlParameter("@successCount", SqlDbType.Int);
                            successParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(successParam);

                            SqlParameter failureParam = new SqlParameter("@failureCount", SqlDbType.Int);
                            failureParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(failureParam);


                            command.ExecuteNonQuery();


                            successCount += Convert.ToInt32(command.Parameters["@successCount"].Value);
                            failureCount += Convert.ToInt32(command.Parameters["@failureCount"].Value);
                        }
                    }

                }
                lblsuccesscnt.Visible = true; lblfailurecnt.Visible = true;
                lblsuccesscnt.Text = "Success Records : " + successCount.ToString() + " ";
                lblfailurecnt.Text = "Failure Records : " + failureCount.ToString() + " ";

                ViewState["CheckBoxArray"] = null;
                ViewState["TempDataTable"] = null;
                TempDataTable = null;
                CheckBoxArray = null;

                BindGridView2(TempDataTable1, guid);
                Save_Record_section.Visible = true;
            }
            else
            {
                ShowAlert("Zero Records Selected. Please Tick Records and Click Save.");
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.InnerException.ToString(), true);
            lblerror.Visible = true;
            lblerror.Text = ex.InnerException.ToString();
        }
    }
    protected void BindGridView()
    {
        try
        {
            // Basic validation (don’t skip this — your current code assumes everything is valid)
            if (string.IsNullOrEmpty(ddlBatch.SelectedValue) ||
                string.IsNullOrEmpty(ddlCenter.SelectedValue))
            {
                lblerror.Text = "Please select required fields.";
                lblerror.Visible = true;
                return;
            }

            if (ViewState["CheckBoxArray"] != null)
            {
                CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
            }
            else
            {
                CheckBoxArray = new ArrayList();
            }

            if (ViewState["TempDataTable"] != null)
            {
                TempDataTable = (ArrayList)ViewState["TempDataTable"];
            }
            else
            {
                TempDataTable = new ArrayList();
            }

            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("getDataForBulkUploadMIS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Clear();

                    command.Parameters.Add("@pBatchId", SqlDbType.BigInt).Value = Convert.ToInt64(ddlBatch.SelectedValue);
                    command.Parameters.Add("@pCentreID", SqlDbType.BigInt).Value = Convert.ToInt64(ddlCenter.SelectedValue);
                    command.Parameters.Add("@pSubCentreId", SqlDbType.BigInt).Value = 0;
                    command.Parameters.Add("@pEnterBy", SqlDbType.BigInt).Value = Convert.ToInt64(loginUserNo);

                    SqlParameter outParam = new SqlParameter("@pResult", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(outParam);

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }

                    string resultMsg = Convert.ToString(outParam.Value);
                }
            }

            // checkbox code end

            // UI binding section
            if (dt.Rows.Count > 0)
            {

                gvMain.Visible = true;
                PagingBar1.Visible = true;
                uPnlGrid.Update();
                PagingBar1.Visible = true;
                PagingBar1.Bind(dt, ref gvMain);
                uPnlNavigation.Update();
                lblerror.Visible = false;
                savebuttondiv.Visible = true;
            }
            else
            {
                gvMain.Visible = false;
                PagingBar1.Visible = false;

                lblerror.Text = "No record found.";
                lblerror.Visible = true;
            }

            //uPnlGrid.Update();
            uPnlNavigation.Update();


            // checkbox code start


            //int CheckBoxIndex;
            //bool CheckAllWasChecked = false;

            //CheckBox chkAll = (CheckBox)gvMain.HeaderRow.Cells[0].FindControl("chkAll");
            //string checkAllIndex = "chkAll-" + gvMain.PageIndex;
            //if (chkAll.Checked)
            //{
            //    if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
            //    {
            //        CheckBoxArray.Add(checkAllIndex);
            //    }
            //}
            //else
            //{
            //    if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
            //    {
            //        CheckBoxArray.Remove(checkAllIndex);
            //        CheckAllWasChecked = true;
            //    }
            //}


            //for (int i = 0; i < gvMain.Rows.Count; i++)
            //{
            //    if (gvMain.Rows[i].RowType == DataControlRowType.DataRow)
            //    {
            //        CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("chkInstitutes");
            //        Label lblID = (Label)gvMain.Rows[i].Cells[0].FindControl("lblID");
            //        Label lblIDACN = (Label)gvMain.Rows[i].Cells[0].FindControl("lblIDACN");
            //        // CheckBoxIndex = Convert.ToInt32(lblID.Text); 
            //        CheckBoxIndex = gvMain.PageSize * PagingBar1.CurrentPageIndex + (i + 1);
            //        if (chk.Checked)
            //        {
            //            if (CheckBoxArray.IndexOf(CheckBoxIndex) == -1 && !CheckAllWasChecked)
            //            {
            //                CheckBoxArray.Add(CheckBoxIndex);
            //                //TempDataTable.Add(Convert.ToInt64(lblID.Text));                               
            //                TempDataTable.Add(Convert.ToString(lblID.Text) + "/" + Convert.ToString(lblIDACN.Text));
            //            }
            //        }
            //        else
            //        {
            //            //if (TempDataTable.Contains(Convert.ToInt64(lblID.Text)))
            //            if (TempDataTable.Contains(Convert.ToString(lblID.Text) + "/" + Convert.ToString(lblIDACN.Text)))
            //            {
            //                //TempDataTable.Remove(Convert.ToInt64(lblID.Text));                               
            //                TempDataTable.Remove(Convert.ToString(lblID.Text) + "/" + Convert.ToString(lblIDACN.Text));
            //            }
            //            if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1 || CheckAllWasChecked)
            //            {
            //                CheckBoxArray.Remove(CheckBoxIndex);
            //            }
            //        }
            //    }
            //}


            //ViewState["CheckBoxArray"] = CheckBoxArray;
            //ViewState["TempDataTable"] = TempDataTable;
        }

        catch (Exception ex)
        {
            // Don’t expose raw exception in production
            lblerror.Text = "An error occurred while fetching data.";
            lblerror.Visible = true;

            // Log ex somewhere (file/db)
        }
    }
    protected void BindGridView2(ArrayList dt, string guid)
    {
        try
        {

            using (SqlConnection connection = new SqlConnection(
              ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {

                string reg_nos = "";

                foreach (string cc in dt)
                {
                    reg_nos = reg_nos + " " + Convert.ToInt64(cc.Split('/')[1]).ToString();
                }


                reg_nos = reg_nos.Trim().Replace(" ", ",");

                string sql = @"select Registration_No, 
ISNULL(np.projectName, 'NO PROJECT') as projectName, 
status, 
reason 
from BulkUploadMIS_Log bu
left join nielitmis.dbo.NielitProjects np on np.Id = projectID
where bu.guid = @guid";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {


                    command.Parameters.AddWithValue("@guid", guid);

                    DataTable datatable = new DataTable();

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(datatable);
                    }


                    // UI binding section
                    if (datatable.Rows.Count > 0)
                    {
                        gvMain2.Visible = true;
                        gvMain2.DataSource = datatable;
                        gvMain2.DataBind();

                    }
                    else
                    {
                        DataTable dt_empty = new DataTable();
                        lblfailurecnt.Text = "0";
                        lblsuccesscnt.Text = "0";
                        gvMain2.Visible = false;
                        gvMain2.DataSource = dt_empty;
                        gvMain2.DataBind();
                    }


                }
            }

            // checkbox code end



        }
        catch (Exception ex)
        {
            DataTable dt_empty = new DataTable();
            gvMain2.Visible = false;
            gvMain2.DataSource = dt_empty;
            gvMain2.DataBind();
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();


                DropDownList ddlProjectId = (DropDownList)e.Row.FindControl("ddlProjectId");
                using (NIELITMISContext context = new NIELITMISContext())
                {

                    ddlProjectId.Items.Clear();
                    Int64 cID = Convert.ToInt64(ddlCourse.SelectedValue);
                    ListItem lst = new ListItem("--No Project--", "0");

                    string dataName = returnDataName(cID);
                    // if (cID.ToString().Length > 3)
                    if (dataName == "NIELITMIS")
                    {
                        var Proc = from t in context.NielitProjectss
                                   join k in context.NielitProjCoursess on t.ID equals k.projID
                                   join d in context.NielitCourseDurations on k.courseID equals d.ID
                                   where d.ID == cID
                                   // && k.IsActive
                                   orderby (t.ProjectName)
                                   select new { ValueField = t.ID, TextField = t.ProjectName };

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjectId, Proc, lst);
                    }
                    else
                    {
                        var Proc = from t in context.NielitProjectss
                                   join k in context.NielitProjCoursess on t.ID equals k.projID
                                   join d in context.CourseFeetypes on k.courseID equals d.courseID
                                   where d.courseID == cID
                                   // && k.IsActive
                                   //  && d.isActive
                                   orderby (t.ProjectName)
                                   select new { ValueField = t.ID, TextField = t.ProjectName };

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjectId, Proc.Distinct(), lst);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvMain2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillddlcentreName()
    {
        try
        {
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                ListItem lst1 = new ListItem("--Select One--", "99");
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);


                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                    txtInstitute.Text = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                    ddlCenter.ClearSelection();
                    if (UserTypeid == 11)
                    {
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.NonAffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                    else
                    {
                        var centreName = from s in context.AffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                         select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                        //Changed above
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.AffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.instituteID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindCourses(Int32 CentreId, int CentreType)
    {
        // 2- Centre, 0--Non Aff Instt 1--Aff Instt

        using (NIELITMISContext context = new NIELITMISContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (DataTable dt = GetBatchCourseRecord(CentreId, CentreType))
            {
                //    //var Course = from p in context.NielitCentreCourses
                //    //             join k in context.NielitCourseDurations on p.ID equals k.courseID
                //    //             where p.IsVerified == true && k.isVerified==true
                //    //             orderby (p.Name)
                //    //             select new { ValueField = k.ID, TextField = p.Name+ " ( " + k.courseDurationDays +" Days )" };
                //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, Course, lst);

                if (dt.Rows.Count > 0)
                {
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "Name";
                    ddlCourse.DataValueField = "ID";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }
        }
    }
    protected void BindBatch()
    {
        try
        {
            User objUser;
            using (EConnectContext context = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                using (NIELITMISContext context1 = new NIELITMISContext())
                {
                    DateTime checkDate = System.DateTime.Today.AddDays(-7);
                    if (UserTypeid == 10)
                    {
                        var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                        Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            ddlCenter.Enabled = false;
                            ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true
                                              && (p.startDate >= checkDate && p.endDate >= System.DateTime.Now)
                                            && p.subCentreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.BatchCode + "(" + p.Name + ")" };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                        }
                        else
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            ddlCenter.Enabled = false;
                            ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true
                                             && (p.startDate >= checkDate && p.endDate >= System.DateTime.Now)
                                            && p.centreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.BatchCode + "(" + p.Name + ")" };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem lst = new ListItem("--Select One--", "0");
        Int64 NelitCentreLinkId = 0;
        Int64 NielitCentrelinkedToCentreId = 0;
        Int64 subcentreid = 0;
        string Seleted = "";
        User objUser;

        ddlBatch.ClearSelection();
        ddlCourse.ClearSelection();

        ddlBatch.Items.Clear();
        ddlCourse.Items.Clear();
        try
        {
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Seleted = RdoAffInstOrNonAffInst.SelectedValue;
                    subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
                    if (UserTypeid == 10)
                    {
                        var instituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeid == 11)
                    {
                        var instituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }

                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId1 = Convert.ToInt32(institutesName.ID);

                    }

                    if (Seleted == "2")
                        BindCourses(Convert.ToInt32(NielitCentreId), Convert.ToInt16(Seleted));
                    else
                        BindCourses(Convert.ToInt32(subcentreid), Convert.ToInt16(Seleted));

                    ddlCourse.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error:" + ex.Message);
        }

    }
    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 NelitCentreLinkId = 0;
        Int64 subcentreid = 0;
        Int64 Courseid = 0;



        ddlBatch.ClearSelection();

        ddlBatch.Items.Clear();


        try
        {
            subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            Courseid = Convert.ToInt64(ddlCourse.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                //   DateTime checkDate = System.DateTime.Today.AddDays(-7);
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid
                                    //  && (DbFunctions.TruncateTime(s.startDate) >= DbFunctions.TruncateTime(checkDate)
                                    // && DbFunctions.TruncateTime(s.endDate) >= DbFunctions.TruncateTime(System.DateTime.Today))
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.BatchCode + "(" + s.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);

                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid //&& (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                                                              //   && (DbFunctions.TruncateTime(s.startDate) >= DbFunctions.TruncateTime(checkDate)
                                                                              //&& DbFunctions.TruncateTime(s.endDate) >= DbFunctions.TruncateTime(System.DateTime.Today))
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.BatchCode + "(" + s.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);


                    }
                }
                else
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid
                                    //  && (DbFunctions.TruncateTime(s.startDate) >= DbFunctions.TruncateTime(checkDate)	
                                    //&& DbFunctions.TruncateTime(s.endDate) >= DbFunctions.TruncateTime(System.DateTime.Today))
                                    //(s.startDate <= System.DateTime.Now && 
                                    // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.BatchCode + "(" + s.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);

                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid
                                    //  && (DbFunctions.TruncateTime(s.startDate) >= DbFunctions.TruncateTime(checkDate)  
                                    //	&& DbFunctions.TruncateTime(s.endDate) >= DbFunctions.TruncateTime(System.DateTime.Now))
                                    //(s.startDate <= System.DateTime.Now && 

                                    // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.BatchCode + "(" + s.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);


                    }
                }
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCourse.ClearSelection();
            ddlBatch.ClearSelection();
            ddlCenter.Items.Clear();
            ddlCourse.Items.Clear();
            ddlBatch.Items.Clear();
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context2 = new NIELITMISContext())
                {
                    if (UserTypeid == 10)
                    {
                        var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeid == 11)
                    {
                        var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (lnkID != 0)
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "99");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlCenter.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                ddlCenter.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                if (UserTypeid == 11)//Non AffInstitutes by user refNumber
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                    //ddlCenter.Enabled = false;
                                    var NonAfflcentre = (from p in context.NonAffInstitutes
                                                         where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                         select p).FirstOrDefault();
                                    ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                                }
                                else //NonAffInstitutes for Nielit Centres
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                    ddlCenter.Enabled = true;
                                }
                                if (UserTypeid == 4)//AffInstitutes by user refNumber
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                                     select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                    //ddlCenter.Enabled = false;
                                    var Afflcentre = (from p in context.AffInstitutes
                                                      where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                                      select p).FirstOrDefault();
                                    ddlCenter.SelectedValue = Afflcentre.instituteID.ToString();
                                }
                                else // AffInstitutes for Nielit Centres
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                    ddlCenter.Enabled = true;
                                }
                            }
                            else
                            {
                                ddlCenter.Items.Add(new ListItem("--Select One--", "99"));
                                ddlCenter.SelectedValue = "99";
                                //ddlCenter.Enabled = false;
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
                            ListItem lst1 = new ListItem("--Select One--", "99");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlCenter.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                ddlCenter.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                ddlCenter.ClearSelection();
                                ddlCenter.Items.Clear();
                                var centreName = from s in context.NonAffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                ddlCenter.Enabled = true;
                            }

                            else
                            {
                                ddlCenter.ClearSelection();

                                var Center = from t in context.NielitCentres
                                             where t.ID == UserRefNumber
                                             orderby (t.Name)
                                             select new { ValueField = t.ID, TextField = t.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center.Distinct(), lst1);

                                ddlCenter.SelectedValue = NielitCentreId.ToString();
                                ddlCenter.Enabled = false;
                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
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
    protected string returnDataName(Int64 vCourseID)
    {
        //START -- CODE Added on 16 Nov 2022 by DEEP NARAYAN  NIELITMIS 
        Int32 courseCatId = 0;
        string checkDbExistsData = "NA";
        if (vCourseID != 0)
        {
            // string Query1 = "select courseID from NIELITMIS.dbo.NielitCourseDuration where id=" + vCourseID;
            //  Int32 courseId = GetCourseCategoryID(Query1);

            // string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=" + courseId;
            //November_2024
            string Query1 = "select courseID from NIELITMIS.dbo.NielitCourseDuration where id= @vCourseID ";
            paramList.Add(new SqlParameter("@vCourseID", vCourseID));

            Int32 courseId = GetCourseCategoryID(Query1);

            //string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=" + courseId;

            //November_2024
            paramList.Clear();
            string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=@courseId ";
            paramList.Add(new SqlParameter("@courseId", courseId));
            courseCatId = GetCourseCategoryID(Query2);
            checkDbExistsData = "NIELITMIS";

            if (courseCatId == 0)
            {
                checkDbExistsData = "NIELIT";
            }
        }
        return (checkDbExistsData);
        //END -- CODE Added on 16 Nov 2022 by DEEP NARAYAN  
    }
    public int GetCourseCategoryID(string myQuery)
    {
        Int32 result = 0;
        try
        {
            // string result = "0";          
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            SqlCommand cmd = new SqlCommand(myQuery, conn);
            //November_2024
            cmd.Parameters.AddRange(paramList.ToArray());

            conn.Open();
            var CourseCategoryId = cmd.ExecuteScalar();
            if (CourseCategoryId != null)
            {
                result = Convert.ToInt32(CourseCategoryId.ToString());
            }
            conn.Close();
            return result;
        }
        catch (Exception exx)
        {
            return result;
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {

            //getGridcheckedvalues();

            gvMain.PageIndex = NewPageIndex;
            //PagingBar1.CurrentPageIndex = NewPageIndex;

            BindGridView();


            ViewState["CheckBoxArray"] = null;
            ViewState["TempDataTable"] = null;


            // irrelevant now, we are not maintaing view state across page sessions
            //if (ViewState["CheckBoxArray"] != null)
            //{
            //    CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
            //    string checkAllIndex = "chkAll-" + gvMain.PageIndex;

            //    if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
            //    {
            //        CheckBox chkAll = (CheckBox)gvMain.HeaderRow.Cells[0].FindControl("chkAll");
            //        if (chkAll != null)
            //            chkAll.Checked = true;
            //    }

            //    for (int i = 0; i < gvMain.Rows.Count; i++)
            //    {
            //        if (gvMain.Rows[i].RowType == DataControlRowType.DataRow)
            //        {
            //            Label lblID = (Label)gvMain.Rows[i].Cells[0].FindControl("lblID");
            //            if (lblID == null) continue;

            //            Int64 CurrentID = Convert.ToInt64(lblID.Text);

            //            if (CheckBoxArray.Contains(CurrentID))          // ← Changed to use ID
            //            {
            //                CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("chkInstitutes");
            //                if (chk != null)
            //                    chk.Checked = true;
            //            }
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected DataTable GetBatchCourseRecord(Int32 CentreId, int CentreType)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBatchCourseRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt).Value = CentreId;
                    cmd.Parameters.Add("@pCentreType", SqlDbType.Int).Value = CentreType;
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
    //protected void getGridcheckedvalues()
    //{
    //   // ArrayList CheckBoxArray, TempDataTable;

    //    if (ViewState["CheckBoxArray"] != null)
    //    {
    //        CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
    //    }
    //    else
    //    {
    //        CheckBoxArray = new ArrayList();
    //    }

    //    if (ViewState["TempDataTable"] != null)
    //    {
    //        TempDataTable = (ArrayList)ViewState["TempDataTable"];
    //    }
    //    else
    //    {
    //        TempDataTable = new ArrayList();
    //    }


    //    int CheckBoxIndex;
    //    bool CheckAllWasChecked = false;


    //    CheckBox chkAll = (CheckBox)gvMain.HeaderRow.Cells[0].FindControl("chkAll");


    //    string checkAllIndex = "chkAll-" + gvMain.PageIndex;


    //    if (chkAll.Checked)
    //    {
    //        if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
    //        {
    //            CheckBoxArray.Add(checkAllIndex);
    //        }
    //    }
    //    else
    //    {
    //        if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
    //        {
    //            CheckBoxArray.Remove(checkAllIndex);
    //            CheckAllWasChecked = true;
    //        }
    //    }
    //    for (int i = 0; i < gvMain.Rows.Count; i++)
    //    {



    //        if (gvMain.Rows[i].RowType == DataControlRowType.DataRow)
    //        {
    //            CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("chkInstitutes");
    //            DropDownList ddlProjectId = (DropDownList)gvMain.Rows[i].Cells[0].FindControl("ddlProjectId");
    //            Label lblregnno = (Label)gvMain.Rows[i].Cells[0].FindControl("lblregnno");
    //            string casteCategory = Convert.ToString(gvMain.DataKeys[i]["Cast_Category_ID"]);

    //            string cor_districtID = Convert.ToString(gvMain.DataKeys[i]["Cor_District_ID"]);

    //            // CheckBoxIndex = Convert.ToInt32(lblID.Text); 
    //            CheckBoxIndex = gvMain.PageSize * PagingBar1.CurrentPageIndex + (i + 1);
    //            if (chk.Checked)
    //            {
    //                if (CheckBoxArray.IndexOf(CheckBoxIndex) == -1 && !CheckAllWasChecked)
    //                {
    //                    CheckBoxArray.Add(CheckBoxIndex);
    //                    //TempDataTable.Add(Convert.ToInt64(lblID.Text));                               
    //                    TempDataTable.Add(Convert.ToString(ddlProjectId.Text) + "/" + Convert.ToString(lblregnno.Text) + "/" + casteCategory + "/" + cor_districtID);

    //                }
    //            }
    //            else
    //            {
    //                //if (TempDataTable.Contains(Convert.ToInt64(lblID.Text)))
    //                if (TempDataTable.Contains(Convert.ToString(ddlProjectId.Text) + "/" + Convert.ToString(lblregnno.Text) + "/" + casteCategory + "/" + cor_districtID))
    //                {
    //                    //TempDataTable.Remove(Convert.ToInt64(lblID.Text));                               
    //                    TempDataTable.Remove(Convert.ToString(ddlProjectId.Text) + "/" + Convert.ToString(lblregnno.Text) + "/" + casteCategory + "/" + cor_districtID);
    //                }
    //                if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1 || CheckAllWasChecked)
    //                {
    //                    CheckBoxArray.Remove(CheckBoxIndex);
    //                }
    //            }
    //        }
    //    }
    //}

    protected void getGridcheckedvalues()
    {

        if (gvMain.Rows.Count == 0)
            return;

        if (ViewState["CheckBoxArray"] != null)
        {
            CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
        }
        else
        {
            CheckBoxArray = new ArrayList();
        }

        if (ViewState["TempDataTable"] != null)
        {
            TempDataTable = (ArrayList)ViewState["TempDataTable"];
        }
        else
        {
            TempDataTable = new ArrayList();
        }

        bool CheckAllWasChecked = false;
        CheckBox chkAll = (CheckBox)gvMain.HeaderRow.Cells[0].FindControl("chkAll");
        string checkAllIndex = "chkAll-" + gvMain.PageIndex;

        if (chkAll.Checked)
        {
            if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
            {
                CheckBoxArray.Add(checkAllIndex);
            }
        }
        else
        {
            if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
            {
                CheckBoxArray.Remove(checkAllIndex);
                CheckAllWasChecked = true;
            }
        }

        for (int i = 0; i < gvMain.Rows.Count; i++)
        {
            if (gvMain.Rows[i].RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("chkInstitutes");
                DropDownList ddlProjectId = (DropDownList)gvMain.Rows[i].Cells[0].FindControl("ddlProjectId");
                Label lblregnno = (Label)gvMain.Rows[i].Cells[0].FindControl("lblregnno");
                Label lblID = (Label)gvMain.Rows[i].Cells[0].FindControl("lblID");   // ← Added

                string casteCategory = Convert.ToString(gvMain.DataKeys[i]["Cast_Category_ID"]);
                string cor_districtID = Convert.ToString(gvMain.DataKeys[i]["Cor_District_ID"]);

                Int64 CurrentID = Convert.ToInt64(lblID.Text);   // ← Use ID instead of page index

                if (chk.Checked)
                {
                    if (CheckBoxArray.IndexOf(CurrentID) == -1 && !CheckAllWasChecked)
                    {
                        CheckBoxArray.Add(CurrentID);                    // ← Changed: Store ID
                        TempDataTable.Add(Convert.ToString(ddlProjectId.Text) + "/" +
                                          Convert.ToString(lblregnno.Text) + "/" +
                                          casteCategory + "/" + cor_districtID);
                    }
                }
                else
                {
                    if (TempDataTable.Contains(Convert.ToString(ddlProjectId.Text) + "/" +
                                               Convert.ToString(lblregnno.Text) + "/" +
                                               casteCategory + "/" + cor_districtID))
                    {
                        TempDataTable.Remove(Convert.ToString(ddlProjectId.Text) + "/" +
                                             Convert.ToString(lblregnno.Text) + "/" +
                                             casteCategory + "/" + cor_districtID);
                    }

                    if (CheckBoxArray.IndexOf(CurrentID) != -1 || CheckAllWasChecked)   // ← Changed
                    {
                        CheckBoxArray.Remove(CurrentID);                                 // ← Changed
                    }
                }
            }
        }

        ViewState["CheckBoxArray"] = CheckBoxArray;
        ViewState["TempDataTable"] = TempDataTable;
    }
    int checkAspirationalDistrict(Int32 pDistrictID)
    {

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString);
        try
        {
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "GetAspirationalDistrict";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            conn.Open();
            cmd.Parameters.Add("@DistrictId", SqlDbType.Int);
            cmd.Parameters["@DistrictId"].Value = pDistrictID;
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string districtID = dr["DistrictsExists"].ToString();
                if (districtID != null)
                {
                    if (pDistrictID.ToString() != districtID.ToString())
                    {

                        conn.Close();
                        dr.Dispose();
                        cmd.Dispose();
                        return 0;
                    }
                    else
                    {
                        conn.Close();
                        dr.Dispose();
                        cmd.Dispose();
                        return 1;
                    }
                }
                else
                {

                    conn.Close();
                    dr.Dispose();
                    cmd.Dispose();
                    return 0;
                }
            }
            else
            {

                conn.Close();
                dr.Dispose();
                cmd.Dispose();
                return 0;
            }

        }
        catch (Exception ex)
        {
            ShowAlert("Error, Contact Administrator");
            conn.Close();
            return 0;
        }
    }

}