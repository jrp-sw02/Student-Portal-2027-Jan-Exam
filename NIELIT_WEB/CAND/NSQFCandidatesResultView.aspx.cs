using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using System.Net;
using System.Web;
using System.Data.Objects;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using EConnect;
using EConnect.NIELIT;
using System.Data .SqlClient ;
using System.Configuration;
using System.Data;


public partial class NSQFCandidatesResultView : BasePage
{

    Int32 UserTypeId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

        if (!IsPostBack)
        {
            if (UserTypeId == 3)
            {
                NSQFResultView();
            }
            else if (UserTypeId == 2 ||  UserTypeId == 6 || UserTypeId == 1)
            {
                lblregno.Visible = true;
                txtRegNo.Visible = true;
                btnShowResult.Visible = true;
                PagingBar1.Visible = false;
                adminrow.Visible = true;
            }
        }
      
        BreadCrumb1.Render();
    }
    protected void btnShowResult_Click(object sender, EventArgs e)
    {
        if (IsValidForm())
        {
            NSQFResultView();
        }
    }
    protected bool IsValidForm()
    {
        try
        {
            if (!isBlankTextBox(txtRegNo))
                {                    
                    lblnsqfResult.Text = "Please enter the Registration Number."; 
                    lblnsqfResult.Visible = true;  
                    return false;
                }
            if (!VerifyNumericValue(txtRegNo.Text))
            {               
                lblnsqfResult.Text = "Please enter the Number only.";
                lblnsqfResult.Visible = true;
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isBlankTextBox(TextBox txtRegNo)
    {
        try
        {
            if (txtRegNo.Text.Length == 0)
            {                
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
    protected bool VerifyNumericValue(string txtRegNo)
    {
        Int32 numval;
        bool rslt = false;

        rslt = Int32.TryParse(txtRegNo, out numval);

        if (rslt == false)
        {
            return false;
        }
        else
            return true;
    }  
    protected void NSQFResultView()
    {     
        try
        {           
            using (DataTable dt = FillGridViewNSQFResultViewRecord())
            {
                if (dt.Rows.Count > 0)
                {
                    var CentreBatchs = (from p in dt.AsEnumerable()
                                        select new
                                        {
                                            Registration_no = p.Field<Int32>("Registration number"),
                                            NSQF_roll_no = p.Field<string>("Roll Number"),
                                            Module_Name = p.Field<string>("Module Name"),
                                            Result = p.Field<string>("Result"),
                                            Exam_Cycle = p.Field<string>("Exam Cycle"),
                                            Reslut_Declaration_Date = p.Field<string>("Result Declaration Date"),
                                           // Result_upload_date = p.Field<string>("Result_upload_date"),                                   
                                        });

                    PagingBar1.Bind(CentreBatchs, ref gvMain);
                    uPnlNavigation.Update();
                    PagingBar1.Visible = true;
                    gvMain.Visible = true;
                    lblnsqfResult.Visible = false;
                }
                else
                {
                    lblnsqfResult.Text = "NSQF Result not found.";
                    PagingBar1.Visible = false;
                    gvMain.Visible = false;
                    lblnsqfResult.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable FillGridViewNSQFResultViewRecord()
    {
        Int32 RegistrationNumber = 0;
        if (UserTypeId == 3)
        {
            string UserID;
            UserID = (Session["studentReg"]).ToString();
             RegistrationNumber = Convert.ToInt32(UserID);
            lblregno.Visible = false ;
            txtRegNo.Visible = false;
            btnShowResult.Visible = false;
        }
        else if (UserTypeId == 2 || UserTypeId == 6 || UserTypeId == 1)
        {
            adminrow.Visible = true;
            RegistrationNumber = Convert.ToInt32(txtRegNo.Text.Trim());
        }
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("Show_NSQF_Result", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PReg_no", SqlDbType.Int).Value = RegistrationNumber;
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

    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            NSQFResultView();
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
            NSQFResultView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString(); 
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }    
}