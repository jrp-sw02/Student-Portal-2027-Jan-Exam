using DocumentFormat.OpenXml.Spreadsheet;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_UpdationRequestDocVerification : BasePage
{
    String strMessage = "";
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}

            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            btnMode.Visible = false;
            if (!Page.IsPostBack)
            {
                BindGridView();
                //BindCourse();
                //if (Session["CourseID"] != null)
                //{
                //    ddlCourse.SelectedValue = Session["CourseID"].ToString();
                //}

                //if (Session["Annexure"] != null)
                //{
                //    ddlAnnexure.SelectedValue = Session["Annexure"].ToString();
                //}
                BreadCrumb2.AddNewBreadCrumbItem(new BreadCrumbItem("Document Verification for Candidate Updation Request", "HO/UpdationRequestDocVerification.aspx", ""));

                if (!string.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    LoadVerificationDetails();
                    LoadVerifiedDetails();
                    SetFinalSubmitVisibility();
                }
                else if (Request.QueryString["Back"] == "1")
                {
                    BindGridView();
                }
                else
                {
                    lblHeading.Text = "Document Verification for Candidate Updation Request";
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());

                if (Session["FlashMessage"] != null)
                {
                    ShowAlert(Session["FlashMessage"].ToString());
                    Session.Remove("FlashMessage"); // clear after showing
                }
            }

            BreadCrumb2.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    //private void BindCourse()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            ListItem lst = new ListItem("--Select Course--", "0");

    //            var course = context.Courses
    //                                 .Where(c => c.CourseCategoryID == 1)
    //                                 .OrderBy(c => c.ID)
    //                                 .Select(c => new
    //                                 {
    //                                     ValueField = c.ID,
    //                                     TextField = c.Name
    //                                 });

    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, course, lst);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert("Error loading courses: " + ex.Message, true);
    //    }
    //}


    protected void BindGridView()
    {
        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            // int courseID = Convert.ToInt32(ddlCourse.SelectedValue);
            // int AnnexureNumber = Convert.ToInt32(ddlAnnexure.SelectedValue);
            string sortOrder = ViewState["SortOrder"] != null ? ViewState["SortOrder"].ToString() : "ASC";
            string sortField = ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "requestNo";

            // default sorting safeguard
            if (string.IsNullOrEmpty(sortField)) sortField = "requestNo";
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "ASC";

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("UpdationRequestDocument_display", con)) // Using stored procedure
            {
                cmd.CommandType = CommandType.StoredProcedure;
               // cmd.Parameters.AddWithValue("@CourseID", courseID);
                // cmd.Parameters.AddWithValue("@Annx_nmber", AnnexureNumber);
                cmd.Parameters.AddWithValue("@EntityID", Convert.ToInt64(Session["EntityID"]));

                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            // Apply sorting in memory
            string sortExpression = sortField + " " + sortOrder;
            dt.DefaultView.Sort = sortExpression;
            dt = dt.DefaultView.ToTable();

            // Add Serial Number Column if missing
            if (!dt.Columns.Contains("SNo"))
            {
                dt.Columns.Add("SNo", typeof(int));
            }

            int pageSize = PagingBar1.CurrentPageSize;
            int pageIndex = PagingBar1.CurrentPageIndex;

            DataTable dtPaged;

            if (pageSize == 0) // "All" selected
            {
                dtPaged = dt.Copy(); // take all rows
                for (int i = 0; i < dtPaged.Rows.Count; i++)
                {
                    dtPaged.Rows[i]["SNo"] = i + 1;
                }
            }
            else
            {
                int startRow = pageIndex * pageSize;
                int endRow = startRow + pageSize;

                dtPaged = dt.Clone();
                for (int i = startRow; i < endRow && i < dt.Rows.Count; i++)
                {
                    DataRow newRow = dtPaged.NewRow();
                    newRow.ItemArray = dt.Rows[i].ItemArray;
                    newRow["SNo"] = i + 1;
                    dtPaged.Rows.Add(newRow);
                }
            }

            PagingBar1.Bind(dt, ref gvMain);
            gvMain.DataSource = dtPaged;
            gvMain.DataBind();

            uPnlGrid.Update();
            uPnlNavigation.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void LoadVerificationDetails()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            btnFinalSubmit.Text = "Final Submit";
            lblHeading.Text = "Document Verification for Candidate Updation Request";
            DataTable dt = new DataTable();
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("GetUpdateRequestVerificationDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestNo", Convert.ToInt32(Request.QueryString["Key"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                lblCandidateID.Text = dt.Rows[0]["candID"].ToString();
                lblRequestNo.Text = dt.Rows[0]["requestNo"].ToString();
                lblAnnexureNumber.Text = dt.Rows[0]["annx_number"].ToString();
                ViewState["CandidateID"] = dt.Rows[0]["candID"];
                if (Convert.ToInt32(dt.Rows[0]["requestStatusID"]) == (int)enmCandidateUpdateRequestStatus.KeptinAbeyance)
                {
                    ShowAlert("This record has been kept on hold.");
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "This record has been kept on hold.";
                }
                gvDocuments.DataSource = dt;
                gvDocuments.DataBind();
            }
            BreadCrumb2.AddNewBreadCrumbItem(new BreadCrumbItem("Request Number - " + lblRequestNo.Text, "#", ""));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    private void LoadVerifiedDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("GetverifiedRecordForUpdatioRequest", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestNo", Convert.ToInt32(Request.QueryString["Key"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            gvVerifiedDocuments.DataSource = dt;
            gvVerifiedDocuments.DataBind();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 App_Status_Verified = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationVerified);
            Int32 App_Status_Rejected = Convert.ToInt32(enmCandidateUpdateRequestStatus.ApplicationRejected);
            Int32 App_Status_keptinHold = Convert.ToInt32(enmCandidateUpdateRequestStatus.KeptinAbeyance);
            if (e.CommandName == "Verify")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                long entityID = Convert.ToInt64(Session["EntityID"]);

                string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(@"
                                                         UPDATE candUpdateRequest
                                                         SET docsVerified = 1,
                                                             docsVerifiedOn = GETDATE(),
                                                             docsVerifiedBy = @EntityID,
                                                             requestStatusID = @StatusID,
                                                         remarks = @Remarks
                                                         WHERE ID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.Add("@EntityID", SqlDbType.BigInt).Value = entityID;
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = App_Status_Verified;
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = "Document Accepted";
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadVerifiedDetails();
                LoadVerificationDetails();
            }

            if (e.CommandName == "Reject")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                long entityID = Convert.ToInt64(Session["EntityID"]);

                GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;
                TextBox txtRemark = (TextBox)row.FindControl("txtRemark");

                string remark = txtRemark.Text.Trim();

                if (string.IsNullOrWhiteSpace(remark))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                        "alert('Please enter rejection remark.');", true);
                    return;
                }

                string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(@"
                                                         UPDATE candUpdateRequest
                                                         SET docsVerified = 0,
                                                             docsVerifiedOn = NULL,
                                                             docsVerifiedBy = NULL,
                                                             requestStatusID = @StatusID,
                                                             remarks = @Remarks
                                                         WHERE ID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = remark;
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = App_Status_Rejected;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    long candidateID = Convert.ToInt64(lblCandidateID.Text);
                    long requestNumber = Convert.ToInt64(lblRequestNo.Text);
                    SendRegistrationUpdationRejectionMail(candidateID, requestNumber);
                }

                LoadVerifiedDetails();
                LoadVerificationDetails();
            }

				if (e.CommandName == "Hold")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                long entityID = Convert.ToInt64(Session["EntityID"]);

                GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;
                TextBox txtRemark = (TextBox)row.FindControl("txtRemark");

                string remark = txtRemark.Text.Trim();

                if (string.IsNullOrWhiteSpace(remark))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                        "alert('Please enter hold remark.');", true);
                    return;
                }

                string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(@"
                                                         UPDATE candUpdateRequest
                                                         SET docsVerified = NULL,
                                                             docsVerifiedOn = NULL,
                                                             docsVerifiedBy = NULL,
                                                             requestStatusID = @StatusID,
                                                             remarks = @Remarks
                                                         WHERE ID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = remark;
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = App_Status_keptinHold;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    long candidateID = Convert.ToInt64(lblCandidateID.Text);
                    long requestNumber = Convert.ToInt64(lblRequestNo.Text);
                    SendRegistrationUpdationRejectionMail(candidateID, requestNumber);
                }

                LoadVerifiedDetails();
                LoadVerificationDetails();
            }           
            
             SetFinalSubmitVisibility();


        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    #region[PageSorting]
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;

            // Default to ASC if SortOrder is null
            if (ViewState["SortOrder"] == null || ViewState["SortOrder"].ToString() == "DESC")
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
    #endregion

    #region[PageIndexChange]
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
    #endregion

    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{

            //    ShowAlert("Sorry! You don't have rights to add new record.", true);
            //    return;
            //}
            BreadCrumb2.Render();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            //Change the heading text as required
            lblHeading.Text = "Document Verification Form";

            BreadCrumb2.AddNewBreadCrumbItem(new BreadCrumbItem("Document Verification", "#", ""));
        }
        else
        {
            Response.Redirect("UpdationRequestDocVerification.aspx", true);
        }
    }


     //protected void btnView_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Session["CourseID"] = ddlCourse.SelectedValue;
    //        Session["Annexure"] = ddlAnnexure.SelectedValue;
    //        BindGridView();
    //        divGrid.Visible = true;
    //        divNavigation.Visible = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}

    //protected void btnReset_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ddlCourse.SelectedIndex = 0;
    //        ddlAnnexure.SelectedIndex = 0;

    //        gvMain.DataSource = null;
    //        gvMain.DataBind();

    //        divGrid.Visible = false;
    //        divNavigation.Visible = false;



    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    // Set serial number considering paging
            //    e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) +
            //        (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

            //    // Action image binding
            //    Image imgAction = (Image)e.Row.FindControl("imgAction");
            //    if (imgAction != null)
            //        imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

            //    // Checkbox binding
            //    CheckBox chk = (CheckBox)e.Row.FindControl("chk");
            //    if (chk != null)
            //        chk.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

            //    object effToObj = DataBinder.Eval(e.Row.DataItem, "EndDate");
            //    if (effToObj != null && effToObj != DBNull.Value)
            //    {
            //        DateTime effectiveTo;
            //        if (DateTime.TryParse(effToObj.ToString(), out effectiveTo))
            //        {


            //            if (effectiveTo.Date < DateTime.Now.Date)
            //            {
            //                // Mark expired rows red
            //                //e.Row.ForeColor = System.Drawing.Color.Red;
            //                int effToColumnIndex = 4; // replace with actual index
            //                e.Row.Cells[effToColumnIndex].ForeColor = System.Drawing.Color.Gray;

            //                // Disable Edit button/link (if it exists)
            //                LinkButton lnkEdit = e.Row.FindControl("lnkEdit") as LinkButton;
            //                if (lnkEdit != null)
            //                {
            //                    lnkEdit.Enabled = true;
            //                    lnkEdit.ForeColor = System.Drawing.Color.Gray;
            //                    lnkEdit.OnClientClick = "alert('This record has expired and cannot be Updated.'); return false;";
            //                }

            //                //Optional: disable selection click if using row command
            //                e.Row.Attributes["onclick"] = "alert('This record has expired and cannot be Updated.'); return false;";
            //            }

            //        }


            //    }

            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("UpdationRequestDocVerification.aspx?Back=1");
    }

    protected void btnFinalSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("updationRequestProcess", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RequestNo", Convert.ToInt64(Request.QueryString["Key"]));
                cmd.Parameters.AddWithValue("@candidateID", Convert.ToInt64(ViewState["CandidateID"]));

                con.Open();

                result = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (result == 1)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "Rejected",
                    "alert('Entire request has been rejected.');window.location='UpdationRequestDocVerification.aspx';",
                    true);

                return;
            }

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Success",
                "alert('Candidate details updated successfully.');window.location='UpdationRequestDocVerification.aspx';",
                true);

            LoadVerifiedDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

   //protected void BtnKeepOnHold_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Int32 App_Status_keptinHold = Convert.ToInt32(enmCandidateUpdateRequestStatus.KeptinAbeyance);
    //        long requestNo = Convert.ToInt64(Request.QueryString["Key"]);
    //        long entityID = Convert.ToInt64(Session["EntityID"]);

    //        string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    //        using (SqlConnection con = new SqlConnection(connStr))
    //        using (SqlCommand cmd = new SqlCommand(@"
    //        UPDATE candUpdateRequest
    //        SET requestStatusID = @StatusID,
    //        remarks = 'Request kept on hold',
    //        docsVerified = 0,
    //        docsVerifiedOn = NULL,
    //        docsVerifiedBy = NULL
    //        WHERE requestNo = @RequestNo", con))
    //        {
    //            cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = App_Status_keptinHold;
    //            cmd.Parameters.Add("@RequestNo", SqlDbType.BigInt).Value = requestNo;
    //            //cmd.Parameters.Add("@EntityID", SqlDbType.BigInt).Value = entityID;
    //            con.Open();
    //            cmd.ExecuteNonQuery();
    //        }

    //        Session["FlashMessage"] = "Request has been kept on hold.";
    //        Response.Redirect("UpdationRequestDocVerification.aspx");
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}
    private void SetFinalSubmitVisibility()
    {
        string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(@"
        SELECT COUNT(*)
        FROM candUpdateRequest
        WHERE RequestNo = @RequestNo and requestFinalised = 1
          AND requestStatusID not in (13,14);", con))
        {
            cmd.Parameters.AddWithValue("@RequestNo", Convert.ToInt64(Request.QueryString["Key"]));

            con.Open();

            int pending = Convert.ToInt32(cmd.ExecuteScalar());
            if (pending == 0)
                btnFinalSubmit.Visible = true;
            else
                btnFinalSubmit.Visible = false;
        }
    }

    private void SendRegistrationUpdationRejectionMail(long candidateID, long requestNumber)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                bool allowSendingEmail = false;

                var evnt = context.NotificationEvent.Find(
                    Convert.ToInt32(enmNotificationEvents.AftersubmittingtheRegistrationUpdationCorrectionform));

                if (evnt != null && evnt.SendEmail)
                    allowSendingEmail = true;

                if (!allowSendingEmail)
                    return;

                var candidate = (from c in context.Candidates
                                 join cd in context.CandidateContactDetails
                                    on c.ID equals cd.CandidateID
                                 join r in context.candidateUpdateRequest
                                    on c.ID equals r.candID
                                    join u in context.UpdateRegnMasters
                                    on r.updateFieldID equals u.ID
                                 where c.ID == candidateID
                                       && r.requestNo == requestNumber && r.requestFinalised == true
                                 select new
                                 {
                                     Name = c.Salutation + " " + c.Name,
                                     Email = cd.EmailAddress,
                                     RequestNo = r.requestNo,
                                     FieldName = u.fieldName,
                                     Remark = r.remarks,
                                     RequestDate = r.enterdate
                                 }).FirstOrDefault();

                if (candidate != null && !string.IsNullOrWhiteSpace(candidate.Email))
                {
                    string msg =
                        "Dear " + GetInitCap(candidate.Name) + ",<br/><br/>" +
                        "One of the documents submitted as part of your Registration Updation Request has been rejected." +
                        "<br/><br/>" +
                        "<b>Request Number :</b> " + candidate.RequestNo +
                        "<br/><b>Rejected Field :</b> " + candidate.FieldName +
                        "<br/><b>Remark :</b> " + candidate.Remark +
                        "<br/><br/>Kindly review the remark, make the necessary corrections, and submit the request again." +
                        "<br/><br/>Regards,<br/>NIELIT";

                    EConnect.NIELIT.Email mail =
                        new EConnect.NIELIT.Email(
                            "Registration Updation Request - Document Rejected : NIELIT",
                            msg,
                            candidate.Email);

                    mail.Send();
                }
            }
        }
        catch
        {
            // Ignore email failures.
        }
    }
}