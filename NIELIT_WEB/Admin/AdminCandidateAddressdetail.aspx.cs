using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
public partial class Admin_AdminCandidateAddressdetail : BasePage
{
    string strMessage = string.Empty;
    EConnectContext context;
    String currentRoleName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Request.QueryString.ToString());
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleName = (string)Session["RoleName"];
            if (currentRoleName == "Technical Support Query")
            {
                //Response.Write("Sorry! You don't have rights  to view this page");
                btnSave.Visible = false;
                //Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView(); 
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
    protected void ShowEditMode()
    {

        try
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Address Detail:Update", "", ""));
            BreadCrumb1.Render();
            context = new EConnectContext();
            Int64 candidateID = Convert.ToInt64(Request.QueryString["key1"]);
            Int64 addressID = Convert.ToInt64(Request.QueryString["Key"]);
            var student = (from  cad in context.Addresses 
                           where cad.ID == addressID
                           select cad).FirstOrDefault();
            btnMode.Visible = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Update Address Detail";
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            MyAddress.FillState();
            if(student.StateID.HasValue)
              MyAddress.StateID =  Convert.ToInt64(student.StateID);
            MyAddress.AddressTypeID = student.AddressTypeID;
            if(student.DistrictID.HasValue)
              MyAddress.DistrictID = Convert.ToInt64(student.DistrictID);
            if (!string.IsNullOrEmpty(student.AddressLine1))
               MyAddress.Address1 = student.AddressLine1;
            if (!string.IsNullOrEmpty(student.AddressLine2))
              MyAddress.Address2 = student.AddressLine2;
            if(!string.IsNullOrEmpty(student.AddressLine3))
              MyAddress.Address3 = student.AddressLine3;
            MyAddress.Pin = student.PinCode.HasValue ? student.PinCode.ToString() : "";
            if (!string.IsNullOrEmpty(student.CityName))
               MyAddress.CityName = student.CityName;

            Int64 maxAddressID = (from cad in context.Addresses
                                where cad.CandidateID == candidateID && cad.AddressTypeID == student.AddressTypeID
                                orderby cad.EffectiveDateFrom descending
                                select cad.ID).FirstOrDefault();
            if (maxAddressID != addressID)
            {
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            Int64 candidateID = Convert.ToInt64(Request.QueryString["key1"]);
            string addType = enmAddressType.CorrespondenceAddress.ToString();
            Int32 addTypeid = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            using (EConnectContext context = new EConnectContext())
            {
                
                    var student = (from  cad in context.Addresses 
                                   where cad.CandidateID == candidateID 
                                   orderby cad.EffectiveDateFrom descending
                                   select new
                                   {
                                       ID = cad.ID,
                                       address = ((!string.IsNullOrEmpty(cad.AddressLine1))? cad.AddressLine1 : "") + ((!string.IsNullOrEmpty(cad.AddressLine2))? cad.AddressLine2 : "") + ((!string.IsNullOrEmpty(cad.AddressLine3))?cad.AddressLine3:""),
                                       addresstype = cad.AddressType.Name,
                                       addressTypeID=cad.AddressTypeID,
                                       candidateID = candidateID,
                                       city = cad.CityName,
                                       state = cad.State.Name,
                                       effdate = cad.EffectiveDateFrom
                                   });
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        student = student.Where((s => s.addresstype.ToUpper().Contains(searchString)));
                    }
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "ID":
                                if (sortOrder == "DESC")
                                    student = student.OrderByDescending(s => s.ID);
                                else
                                    student = student.OrderBy(s => s.ID);
                                break;
                            case "address":
                                if (sortOrder == "DESC")
                                    student = student.OrderByDescending(s => s.address);
                                else
                                    student = student.OrderBy(s => s.address);
                                break;
                            case "city":
                                if (sortOrder == "DESC")
                                    student = student.OrderByDescending(s => s.city);
                                else
                                    student = student.OrderBy(s => s.city);
                                break;
                            case "state":
                                if (sortOrder == "DESC")
                                    student = student.OrderByDescending(s => s.state);
                                else
                                    student = student.OrderBy(s => s.state);
                                break;
                            case "addresstype":
                                if (sortOrder == "DESC")
                                    student = student.OrderByDescending(s => s.addresstype);
                                else
                                    student = student.OrderBy(s => s.addresstype);
                                break;
                            case "effdate":
                                if (sortOrder == "DESC")
                                    student = student.OrderByDescending(s => s.effdate);
                                else
                                    student = student.OrderBy(s => s.effdate);
                                break;
                            default:
                                student = student.OrderBy(s => s.ID);
                                break;
                        }
                    }
                    PagingBar1.Bind(student, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Address Detail", "Admin/AdminCandidateAddressdetail.aspx?key1=" + Request.QueryString["key1"], ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Address Detail", "Admin/AdminCandidateAddressdetail.aspx?" + Request.QueryString.ToString(), ""));
                    }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Address";
            //Label10.Visible = true;
        }
        else
        {
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidateAddressdetail.aspx?key1=" + Request.QueryString["key1"]));
            }
            else
            {
                Response.Redirect("AdminCandidateAddressdetail.aspx", true);
            }
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            Int32 appid = Convert.ToInt32(Request.QueryString["key1"]);
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
           
                Int64 AddressID = Convert.ToInt32(Request.QueryString["Key"]);
                var student = (from  cad in context.Addresses 
                                where (cad.ID == AddressID && cad.CandidateID == appid)
                                select cad).FirstOrDefault();
                if (string.IsNullOrEmpty(MyAddress.Address1))
                {
                    throw new Exception("Please enter Address Line 1");
                }
                else
                {
                    student.AddressLine1 = MyAddress.Address1.ToUpper();
                }
                if (string.IsNullOrEmpty(MyAddress.Address2))
                {
                    throw new Exception("Please enter Address Line 2");
                }
                else
                {
                    student.AddressLine2 = MyAddress.Address2.ToUpper();
                }
                student.AddressLine3 = MyAddress.Address3.ToUpper();
                if (string.IsNullOrEmpty(MyAddress.Pin))
                {
                    throw new Exception("Please enter PinCode Number");
                }
                else if (!IsNumeric(MyAddress.Pin))
                {
                    throw new Exception("Please enter valid PinCode Number");
                }
                else
                {
                    student.PinCode = Convert.ToInt32(MyAddress.Pin);
                }
                if (string.IsNullOrEmpty(MyAddress.CityName))
                {
                    throw new Exception("Please enter City Name");
                }
                else
                {
                    student.CityName = MyAddress.CityName.ToUpper();
                }
                if (MyAddress.StateID.ToString() == "0")
                {
                    throw new Exception("Please Select State Name");
                }
                else
                {
                    student.StateID = Convert.ToInt64(MyAddress.StateID.ToString());
                }
                if (MyAddress.DistrictID.ToString() == "0")
                {
                    throw new Exception("Please Select District Name");
                }
                else
                {
                    student.DistrictID = Convert.ToInt64(MyAddress.DistrictID.ToString());
                }
                student.CreatedByID = Convert.ToInt32(Session["UserID"]);
                student.EffectiveDateFrom = DateTime.Now;
                context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                strMessage = "Record updated.";
                ShowAlert(strMessage,true);

                Address_Update_temp();
            }
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidateAddressdetail.aspx?key1="+ Request.QueryString["key1"] + "&msg="+strMessage));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }

    public void Address_Update_temp()//this function use of temporary teble insert recored for online to offline updation purpose.
    {
        
        //////////string ipaddress;
        //////////ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //////////if (ipaddress == "" || ipaddress == null)
        //////////    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
        //////////string clientMachineName;
        //////////clientMachineName = (System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName);

        //if (MyAddress.AddressTypeID == 1)
        //{
           

        //}
            string addType = enmAddressType.CorrespondenceAddress.ToString();
            Int32 addTypeid = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 appid = Convert.ToInt32(Request.QueryString["key1"]);
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    Int64 AddressID = Convert.ToInt32(Request.QueryString["Key"]);
                    var student1 = (from cad in context.Addresses
                                    where (cad.ID == AddressID && cad.CandidateID == appid)
                                    select cad).FirstOrDefault();

                    AddressUpdateTemp add1 = new EConnect.AddressUpdateTemp();

                    add1.ID = Convert.ToInt64(Request.QueryString["Key"]);
                    add1.Address_Type_ID = Convert.ToInt32(MyAddress.AddressTypeID);
                    add1.Address1 = MyAddress.Address1.ToUpper();
                    add1.Address2 = MyAddress.Address2.ToUpper();
                    add1.Address3 = MyAddress.Address3.ToUpper();
                    if (add1.Address3=="" )
                    {
                        add1.Address3 = ", ";
                    }
                    add1.Country_ID = Convert.ToInt64(student1.CountryID);
                    add1.State_ID = Convert.ToInt64(MyAddress.StateID.ToString());
                    add1.District_ID = Convert.ToInt64(MyAddress.DistrictID.ToString());
                    add1.City_Name = MyAddress.CityName.ToUpper();
                    add1.Pin_Code = Convert.ToInt32(MyAddress.Pin);
                    add1.Candidate_ID = Convert.ToInt32(Request.QueryString["key1"]);
                    add1.Effective_From_Date = DateTime.Now;
                    add1.Created_On = Convert.ToDateTime(student1.CreatedOn);
                    add1.Created_By = Convert.ToInt32(Session["UserID"]);
                    add1.IsVerified = Convert.ToBoolean(student1.IsVerified);
                    add1.Verified_By = Convert.ToInt32(student1.VerifiedByID);
                    add1.State_Code = " ";
                    add1.Update_DateTime = DateTime.Now;
                    add1.Client_IPAddress = "TEST"; //ipaddress.ToString();
                    add1.Client_UserId = "";
                    add1.Client_HostName = "TEST"; // clientMachineName.ToString();
                    context.AddressUpdateTemps.Add(add1);
                    context.SaveChanges();
                    //ShowAlert("Record Inserted Successfully");
                }
            };
        //}
        //else
        //{

        //    string addType = enmAddressType.CorrespondenceAddress.ToString();
        //    Int32 addTypeid = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
        //    using (EConnectContext context = new EConnectContext())
        //    {
        //        Int32 appid = Convert.ToInt32(Request.QueryString["key1"]);
        //        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
        //        {
        //            Int64 AddressID = Convert.ToInt32(Request.QueryString["Key"]);
        //            var student1 = (from cad in context.Addresses
        //                            where (cad.ID == AddressID && cad.CandidateID == appid)
        //                            select cad).FirstOrDefault();

        //            AddressUpdateTemp add1 = new EConnect.AddressUpdateTemp();
        //    //string addType = enmAddressType.PermanentAddress.ToString();
        //    //Int32 addTypeid = Convert.ToInt32(enmAddressType.PermanentAddress);
        //    //using (EConnectContext context = new EConnectContext())
        //    //{
        //    //    Int32 appid = Convert.ToInt32(Request.QueryString["key1"]);
        //    //    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
        //    //    {
        //    //        Int64 AddressID = Convert.ToInt32(Request.QueryString["Key"]);
        //    //        var student1 = (from cad in context.Addresses
        //    //                        where (cad.ID == AddressID && cad.CandidateID == appid)
        //    //                        select cad).FirstOrDefault();

        //    //        AddressUpdateTemp add1 = new EConnect.AddressUpdateTemp();

        //            add1.ID = Convert.ToInt64(Request.QueryString["Key"]);
        //            add1.Address_Type_ID = a;// 2;// Convert.ToInt32(enmAddressType.PermanentAddress);
        //            add1.Address1 = MyAddress.Address1.ToUpper();
        //            add1.Address2 = MyAddress.Address2.ToUpper();
        //            add1.Address3 = MyAddress.Address3.ToUpper();
        //            add1.Country_ID = Convert.ToInt64(student1.CountryID);
        //            add1.State_ID = Convert.ToInt64(MyAddress.StateID.ToString());
        //            add1.District_ID = Convert.ToInt64(MyAddress.DistrictID.ToString());
        //            add1.City_Name = MyAddress.CityName.ToUpper();
        //            add1.Pin_Code = Convert.ToInt32(MyAddress.Pin);
        //            add1.Candidate_ID = Convert.ToInt32(Request.QueryString["key1"]);
        //            add1.Effective_From_Date = DateTime.Now;
        //            add1.Created_On = Convert.ToDateTime(student1.CreatedOn);
        //            add1.Created_By = Convert.ToInt32(Session["UserID"]);
        //            add1.IsVerified = Convert.ToBoolean(student1.IsVerified);
        //            add1.Verified_By = Convert.ToInt32(student1.VerifiedByID);
        //            add1.State_Code = " ";
        //            add1.Update_DateTime = DateTime.Now;
        //            add1.Client_IPAddress = "177"; //ipaddress.ToString();
        //            add1.Client_UserId = "";
        //            add1.Client_HostName = "177"; // clientMachineName.ToString();
        //            context.AddressUpdateTemps.Add(add1);
        //            context.SaveChanges();
        //            //add1.Client_IPAddress = "TEST";// ipaddress;
        //            //add1.Client_UserId = "";
        //            //add1.Client_HostName = "TEST";// clientMachineName;
        //            //context.AddressUpdateTemps.Add(add1);
        //            //context.SaveChanges();
        //            //ShowAlert("Record Inserted Successfully");
        //        }
        //    };
        //}
    }     


    protected void AllyFilter(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        //try
        //{
        //    //ddlSearchUserType.SelectedValue = "0";
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hyperlink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var users = from s in context.AddressTypes
                        select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.OrderBy(s => s.Name);
            foreach (var user in users)
            {
                items.Add(user.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCandidateAddressdetail.aspx?key1=" + Request.QueryString["key1"]));
    }
   
}