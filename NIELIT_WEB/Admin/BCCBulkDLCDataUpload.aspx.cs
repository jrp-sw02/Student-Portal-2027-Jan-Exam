using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using System.Web.Security;
using System.Data.OleDb;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;
using EConnect.NIELIT;

public partial class Admin_BCCBulkDLCDataUpload : BasePage
{
    String strMessage = string.Empty;
    Table tbl = new Table();
    enmLanguage pageLanguage = enmLanguage.English;
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {


        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");

            if (!Page.IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                }
                else
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("CSC Bulk DLC Data Upload", "Admin/BCCBulkDLCDataUpload.aspx", ""));
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";

                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lblMsgs.Text = "";
        lblMsgs.Visible = false;
        trdata.Visible = false;
        divReportData.Visible = false;
        RInvalidRecords.Visible = true;
        BindGrid();
    }

    public string UpdateInstituteDetails(string insd)
    {
        int i = 0;
        string MsgConfirmation = "NA";
        try
        {
            var json_serializer = new JavaScriptSerializer();
            var table = JsonConvert.DeserializeObject<DataTable>(insd);

            if (table != null && table.Rows.Count > 0)
            {
                string message = string.Empty;
                SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
                DataTable dt = table;

                SqlBulkCopy objbulk = new SqlBulkCopy(con1);
                objbulk.DestinationTableName = "dbo.CSCBulkInstituteUploadBCCTemp";

                objbulk.ColumnMappings.Add(dt.Columns[0].ColumnName.ToString(), "Id");
                objbulk.ColumnMappings.Add(dt.Columns[1].ColumnName.ToString(), "Name");

                objbulk.ColumnMappings.Add(dt.Columns[2].ColumnName.ToString(), "CENTRE_NAME");

                objbulk.ColumnMappings.Add(dt.Columns[3].ColumnName.ToString(), "ADDRESS1");
                objbulk.ColumnMappings.Add(dt.Columns[4].ColumnName.ToString(), "ADDRESS2");

                objbulk.ColumnMappings.Add(dt.Columns[5].ColumnName.ToString(), "DISTRICT");
                objbulk.ColumnMappings.Add(dt.Columns[6].ColumnName.ToString(), "CITY");

                objbulk.ColumnMappings.Add(dt.Columns[7].ColumnName.ToString(), "STATE_NAME");
                objbulk.ColumnMappings.Add(dt.Columns[8].ColumnName.ToString(), "PINCODE");

                objbulk.ColumnMappings.Add(dt.Columns[9].ColumnName.ToString(), "STDCODE");
                objbulk.ColumnMappings.Add(dt.Columns[10].ColumnName.ToString(), "PHONE");

                objbulk.ColumnMappings.Add(dt.Columns[11].ColumnName.ToString(), "MOBILE");
                objbulk.ColumnMappings.Add(dt.Columns[12].ColumnName.ToString(), "FAX");

                objbulk.ColumnMappings.Add(dt.Columns[13].ColumnName.ToString(), "EMAIL");
                objbulk.ColumnMappings.Add(dt.Columns[14].ColumnName.ToString(), "Number_of_PC");

                objbulk.ColumnMappings.Add(dt.Columns[15].ColumnName.ToString(), "Connectivity");
                objbulk.ColumnMappings.Add(dt.Columns[16].ColumnName.ToString(), "CSC_ID_OPERATOR");

                objbulk.ColumnMappings.Add(dt.Columns[17].ColumnName.ToString(), "OMT_ID_OPERATOR");
                objbulk.ColumnMappings.Add(dt.Columns[18].ColumnName.ToString(), "OPERATOR_ROLL_NO");

                objbulk.ColumnMappings.Add(dt.Columns[19].ColumnName.ToString(), "OPERATOR_GRADE");
                objbulk.ColumnMappings.Add(dt.Columns[20].ColumnName.ToString(), "OPERATOR_NAME");

                objbulk.ColumnMappings.Add(dt.Columns[21].ColumnName.ToString(), "OPERATOR_QUALIFICATIONS");
                objbulk.ColumnMappings.Add(dt.Columns[22].ColumnName.ToString(), "OPERATOR_MOBILE");

                objbulk.ColumnMappings.Add(dt.Columns[23].ColumnName.ToString(), "OPERATOR_EMAIL");
                objbulk.ColumnMappings.Add(dt.Columns[24].ColumnName.ToString(), "UTR_TRNSACTION_NO");

                objbulk.ColumnMappings.Add(dt.Columns[25].ColumnName.ToString(), "BANK_NAME");
                objbulk.ColumnMappings.Add(dt.Columns[26].ColumnName.ToString(), "TRANSACTION_DATE");

                objbulk.ColumnMappings.Add(dt.Columns[27].ColumnName.ToString(), "AMOUNT_Incl_GST");
                objbulk.ColumnMappings.Add(dt.Columns[28].ColumnName.ToString(), "VerifyStatus");

                objbulk.ColumnMappings.Add(dt.Columns[29].ColumnName.ToString(), "VerificationDate");
                objbulk.ColumnMappings.Add(dt.Columns[30].ColumnName.ToString(), "VerifiedBy");

                objbulk.ColumnMappings.Add(dt.Columns[31].ColumnName.ToString(), "Accrediation_Number");

                con1.Open();
                objbulk.WriteToServer(dt);
                con1.Close();

                MsgConfirmation = "Record uploaded successfully !!";
                return MsgConfirmation;
            }
            return MsgConfirmation;
        }

        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // context.Dispose();
        }

        return MsgConfirmation;
    }

    protected void btnUploadFile_Click(object sender, EventArgs e)
    {
        trdata.Visible = false;
        divReportData.Visible = false;
        GridView1.Visible = false;
        RInvalidRecords.Visible = false;
        if (txtdate.Text == "")
        {
            ShowAlert("Please enter the Date !!");
            lblMsgs.Text = "Please enter the Date !!";
            lblMsgs.ForeColor = System.Drawing.Color.Red;
            lblMsgs.Visible = true;
            return;
        }
        if (!IsDate(txtdate.Text))
        {
            txtdate.Text = "";
            lblMsgs.Text = "Invalid Date !!";
            lblMsgs.ForeColor = System.Drawing.Color.Red;
            lblMsgs.Visible = true;
            return;
        }

        lblMsgs.Text = "";
        lblMsgs.Visible = false;
        CSCBulkUploadService objServices = new CSCBulkUploadService();
        string entryDate = txtdate.Text.ToString();
        Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
        Dictionary<string, dynamic> result1 = new Dictionary<string, dynamic>();
        result = CSCBulkUploadService.CSCBulkServiceGetData(entryDate);
        if (result.Count > 0)
        {
            var resresult = result.ElementAt(0).Value;
            if (resresult == "Success")
            {
                var JsonResult = result.ElementAt(1).Value;
                int length = JsonResult.Count;
                var jsonstring = new JavaScriptSerializer().Serialize(JsonResult);
                string Msg = UpdateInstituteDetails(jsonstring);
                if (Msg != "NA")
                {
                    string result11 = CSCBulkUploadService.CSCBulkServiceUpdateData(jsonstring);
                    lblMsgs.Text = Msg+resresult;
                    lblMsgs.ForeColor = System.Drawing.Color.Green;
                    lblMsgs.Visible = true;
                }
                else
                {
                    lblMsgs.Text = "Data has already been uploaded !!";
                    lblMsgs.ForeColor = System.Drawing.Color.Red;
                    lblMsgs.Visible = true;
                }

            }
            else if (resresult == "Fail")
            {
                lblMsgs.Text = "Record not uploaded.!!";
                lblMsgs.ForeColor = System.Drawing.Color.Red;
                lblMsgs.Visible = true;
            }
            else
            {
            }

        }
        else
        {
            lblMsgs.Text = "Record not uploaded.!!";
            lblMsgs.ForeColor = System.Drawing.Color.Red;
            lblMsgs.Visible = true;
        }


    }

    protected void btnFinalized_Click(object sender, EventArgs e)
    {
        try
        {
            trdata.Visible = false;
            divReportData.Visible = false;
            string message = string.Empty, UserCreationErrorMSG = string.Empty, ID = string.Empty, Name = string.Empty, CITY = string.Empty, STATE_NAME = string.Empty, DISTRICT = string.Empty;
            string AccrNo = "0", EMAIL = string.Empty;
            using (DataTable dt = GetRecord())
            {
                if (dt.Rows.Count > 0)
                {
                    int j;                   
                    for (j = 0; j < dt.Rows.Count; j++)
                    {
                        ID = dt.Rows[j].Field<string>("ID");
                        Name = dt.Rows[j].Field<string>("CENTRE_NAME");
                        CITY = dt.Rows[j].Field<string>("CITY");
                        STATE_NAME = dt.Rows[j].Field<string>("STATE_NAME");
                        AccrNo = dt.Rows[j].Field<string>("Accrediation_Number");
                        DISTRICT = dt.Rows[j].Field<string>("DISTRICT");
                        EMAIL = dt.Rows[j].Field<string>("EMAIL");

                        string StateId = GetID("select cast ( ID as varchar(20)) ID  from NIELIT.dbo.Location where Parent_ID=1 and Name=" + "'" + STATE_NAME + "'");
                        string DistricId = GetID("select cast ( ID as varchar(20)) ID from NIELIT.dbo.Location where Location_Type_ID not in (2) and  Name=" + "'" + DISTRICT + "'");

                        if (DistricId == "0")
                        {
                            UpdateRemarks(ID, AccrNo, "Not Valid District !");
                        }
                        string ExistsInstitute = GetID("select cast ( ID as varchar(20)) ID  from NIELIT.dbo.Institute where ID=" + Convert.ToInt64(AccrNo));

                        if (ExistsInstitute == "0")
                        {
                            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
                            {
                                SqlCommand cmd = new SqlCommand("InsertAccrNoCscBulkData", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = ID;
                                cmd.Parameters.Add("@StateId", SqlDbType.BigInt).Value = Convert.ToInt64(StateId);
                                cmd.Parameters.Add("@DistricId", SqlDbType.BigInt).Value = Convert.ToInt64(DistricId);
                                cmd.Parameters.Add("@AccrNo", SqlDbType.BigInt).Value = Convert.ToInt64(AccrNo);

                                con.Open();
                                int i = cmd.ExecuteNonQuery();
                                if (i >= 7)
                                {
                                    lblMsgs.Text = "";
                                    lblMsgs.Text = "Finalized Done !!";
                                    lblMsgs.ForeColor = System.Drawing.Color.Green;
                                    lblMsgs.Visible = true;
                                    // For user creation Start on 21 March 2023
                                    //Create User and  Email Send
                                    #region  //Create User and Email Send
                                    int userType = 4;
                                    Int64 INSTID = Convert.ToInt64(AccrNo);
                                    using (EConnectContext context = new EConnectContext())
                                    {
                                        if (context.Users.Where(a => a.UserTypeID == userType && a.UserRefNumber == INSTID).Count() <= 0)
                                        {
                                            User user = new User();
                                            user.OrganizationID = 1;

                                            string accNumber = AccrNo.Trim();
                                            user.LoginID = accNumber;
                                            user.UserName = Name.ToUpper();
                                            user.Password = UserManager.ComputeSha256Hash(accNumber).ToUpper();
                                            user.UserTypeID = userType;
                                            user.UserRefNumber = Convert.ToInt64(accNumber);
                                            user.EmailID = EMAIL;
                                            user.PasswordExpiryDays = 0;
                                            user.LastPasswordChangedOn = DateTime.Now;
                                            user.FailedLoginAttempts = 0;
                                            user.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                            user.CreatedOn = DateTime.Now;
                                            user.HasLoginAccess = true;
                                            user.DefaultRoleID = Convert.ToInt32(enmRole.AdminInstitute);
                                            context.Users.Add(user);
                                            context.SaveChanges();

                                            String EmailMsg = "Dear " + Name.ToUpper() + ",  The userid and password for login to Student.nielit.gov.in is Userid : " + accNumber + "  and  Password:.  " + accNumber + ". Please change your password after first Login.";

                                            //sending Email Id
                                            if (EMAIL.Length > 0)
                                            {
                                                try
                                                {
                                                    EConnect.NIELIT.Email mail = new Email("Online LogIn:NIELIT", EmailMsg, EMAIL);
                                                    mail.Send();
                                                }
                                                catch { ShowAlert("Login Id and Password are sent on E-mail."); }
                                            }


                                        }

                                    }

                                    #endregion
                                    // For user creation End on 23 March 2023
                                }
                                else
                                {
                                    lblMsgs.Text = "";
                                    lblMsgs.Text = "Data not found !!";
                                    lblMsgs.ForeColor = System.Drawing.Color.Red;
                                    lblMsgs.Visible = true;
                                }
                                con.Close();
                            }
                        }
                        else
                        {
                            UpdateRemarks(ID, AccrNo, "AccrNo. Already Exists");
                        }
                    }
                }
                else
                {
                    lblMsgs.Text = "";
                    lblMsgs.Text = "No Data found !!";
                    lblMsgs.ForeColor = System.Drawing.Color.Red;
                    lblMsgs.Visible = true;
                }
                RInvalidRecords.Visible = false;
                grdRemarks.Visible = false;
                GridView1.Visible = false;
                BindGridView();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList duty = (DropDownList)gvr.FindControl("ddlDistrict");           
            Label DistricName = (Label)gvr.FindControl("Label2");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataSet GetDropDownData(string query)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlCommand cmd = new SqlCommand(query);
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataSet ds = new DataSet())
                {
                    sda.Fill(ds);
                    return ds;
                }
            }
        }
    }

    public DataTable GetRecord()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("select Id,CENTRE_NAME,Name,ADDRESS1,ADDRESS2,DISTRICT,CITY,STATE_NAME,PINCODE,STDCODE,PHONE,MOBILE,FAX,EMAIL,Accrediation_Number  FROM [NIELIT].[dbo].[CSCBulkInstituteUploadBCCTemp]   where  FinalizedDate is null and Finalized='N'", con))
                {
                    cmd.CommandType = CommandType.Text;
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

    public void UpdateRemarks(string Id, string AccrNo, string msg)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("update CSCBulkInstituteUploadBCCTemp set Remarks=" + "'" + msg + "'" + " , FinalizedDate=GETDATE() where Accrediation_Number =" + "'" + AccrNo + "'" + " and Id=" + "'" + Id + "'", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public string GetID(string query)
    {
        string result = "0";
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection Conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query, Conn))
            {
                Conn.Open();
                string getValue = (string)cmd.ExecuteScalar();
                if (getValue != null)
                {
                    result = getValue.ToString();
                }
                Conn.Close();
            }
        }
        return result;
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;

                if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
                {
                    href += "&key1=" + Request.QueryString["key1"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

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

    protected Boolean IsDate(String value)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(value);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public DataTable GetGridViewRecord()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("select Id,CENTRE_NAME,Name,ADDRESS1,ADDRESS2,DISTRICT,CITY,STATE_NAME,PINCODE,STDCODE,PHONE,MOBILE,FAX,EMAIL,cast( Accrediation_Number as bigint)Accrediation_Number,Remarks  FROM [NIELIT].[dbo].[CSCBulkInstituteUploadBCCTemp]   where Remarks is null and Finalized='Y'", con))
                {
                    cmd.CommandType = CommandType.Text;
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

    private void BindGrid()
    {
        RInvalidRecords.Visible = true;
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select Id RefNo, cast( Accrediation_Number as bigint)Accrediation_Number,CENTRE_NAME,DISTRICT,CITY,STATE_NAME,Remarks  FROM [NIELIT].[dbo].[CSCBulkInstituteUploadBCCTemp]   where Remarks is not null and Finalized='N'"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            grdRemarks.Visible = true;
                            GridView1.Visible = true;
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                        else
                        {                           
                            lblMsgss.Text = "Data not Found!!";
                            lblMsgss.Visible = true;
                            lblMsgss.ForeColor = System.Drawing.Color.Red;
                            grdRemarks.Visible = false;
                            GridView1.Visible = false;
                        }
                    }
                }
            }
        }
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        this.BindGrid();
    }

    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = GridView1.Rows[e.RowIndex];
        Int64 AccrNo = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        string DistrictName = Convert.ToString(row.Cells[4].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
        Int64 DistrictId = Convert.ToInt64(row.Cells[4].Controls.OfType<DropDownList>().FirstOrDefault().SelectedValue);
        Label Id = (Label)(GridView1.Rows[e.RowIndex].Cells[1].Controls[1]);
        Label InstName = (Label)(GridView1.Rows[e.RowIndex].Cells[2].Controls[1]);

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("UpdateDistrictCscBulkData", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar));
                cmd.Parameters["@id"].Value = Id.Text;
                cmd.Parameters.Add(new SqlParameter("@DistrictName", SqlDbType.VarChar));
                cmd.Parameters["@DistrictName"].Value = DistrictName;
                cmd.Parameters.Add(new SqlParameter("@InstName", SqlDbType.VarChar));
                cmd.Parameters["@InstName"].Value = InstName.Text;
                cmd.Parameters.Add(new SqlParameter("@AccrNo", SqlDbType.BigInt));
                cmd.Parameters["@AccrNo"].Value = AccrNo;
                cmd.Parameters.Add(new SqlParameter("@DistricId", SqlDbType.BigInt));
                cmd.Parameters["@DistricId"].Value = DistrictId;
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        GridView1.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        GridView1.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int customerId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(""))
            {
                cmd.Parameters.AddWithValue("@Id", customerId);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        this.BindGrid();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSerial = (Label)e.Row.FindControl("lblSerial");
            lblSerial.Text = ((GridView1.PageIndex * GridView1.PageSize) + e.Row.RowIndex + 1).ToString();
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlDistrict = (DropDownList)e.Row.FindControl("ddlDistricts");
                Label StateName = (Label)e.Row.FindControl("lbl_STATE_NAME");

                ddlDistrict.DataSource = GetDropDownData("select ID,CENTRE_NAME,Name  FROM [NIELIT].[dbo].[Location] where Location_Type_ID in (6,4 )and Parent_ID in (select ID  FROM [NIELIT].[dbo].[Location] where Name=" + "'" + StateName.Text.Trim() + "'" + ")");
                ddlDistrict.DataTextField = "Name";
                ddlDistrict.DataValueField = "ID";
                ddlDistrict.DataBind();

                //Add Default Item in the DropDownList
                ddlDistrict.Items.Insert(0, new ListItem("Please select"));
                ddlDistrict.Attributes.Add("style", "background-color:#FDEEF4;color:Black;font-weight:bold;");
            }
        }       
    }

    protected void BindGridView()
    {
        try
        {
            GridView1.Visible = false;
            using (DataTable dt = GetGridViewRecord())
            {
                if (dt.Rows.Count > 0)
                {                  
                    trdata.Visible = true;
                    divReportData.Visible = true;
                    var CentreBatchs = (from p in dt.AsEnumerable()
                                        select new
                                        {
                                            ID = p.Field<Int64>("Accrediation_Number"),
                                            AccreditationNumber = p.Field<Int64>("Accrediation_Number"),
                                            Name = p.Field<string>("CENTRE_NAME"),
                                            Remarks = p.Field<string>("Remarks"),
                                            ADDRESS = p.Field<string>("ADDRESS1"),
                                            DISTRICT = p.Field<string>("DISTRICT"),
                                            CITY = p.Field<string>("CITY"),
                                            STATE_NAME = p.Field<string>("STATE_NAME"),
                                            RefNo = p.Field<string>("Id")
                                        });
                    PagingBar1.Bind(CentreBatchs, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}