using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;

public partial class Admin_AffInstitute : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    NIELITMISContext context1;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 entityID = 0;
    ArrayList TempDataTable = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;


        try
        {
            ArrayList CheckBoxArray, TempDataTable;
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

            if (IsPostBack)
            {
                int CheckBoxIndex;
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
                        Label lblID = (Label)gvMain.Rows[i].Cells[0].FindControl("lblID");
                        Label lblIDACN = (Label)gvMain.Rows[i].Cells[0].FindControl("lblIDACN");
                        // CheckBoxIndex = Convert.ToInt32(lblID.Text); 
                        CheckBoxIndex = gvMain.PageSize * PagingBar1.CurrentPageIndex + (i + 1);
                        if (chk.Checked)
                        {
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) == -1 && !CheckAllWasChecked)
                            {
                                CheckBoxArray.Add(CheckBoxIndex);
                                //TempDataTable.Add(Convert.ToInt64(lblID.Text));                               
                                TempDataTable.Add(Convert.ToString(lblID.Text) + "/" + Convert.ToString(lblIDACN.Text));                               
                            }
                        }
                        else
                        {
                            //if (TempDataTable.Contains(Convert.ToInt64(lblID.Text)))
                            if (TempDataTable.Contains(Convert.ToString(lblID.Text) + "/" + Convert.ToString(lblIDACN.Text)))
                            {
                                //TempDataTable.Remove(Convert.ToInt64(lblID.Text));                               
                                TempDataTable.Remove(Convert.ToString(lblID.Text) + "/" + Convert.ToString(lblIDACN.Text));
                            }
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1 || CheckAllWasChecked)
                            {
                                CheckBoxArray.Remove(CheckBoxIndex);
                            }
                        }
                    }
                }
            }

            ViewState["CheckBoxArray"] = CheckBoxArray;
            ViewState["TempDataTable"] = TempDataTable;
         

            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            entityID = Convert.ToInt32(Session["EntityID"]);




            if (!Page.IsPostBack)
            {

                BindGridView();
                BindGridAffliated();
            }

            //    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            //    {
            //        //BindState();
            //        //Added 13 feb 2019
            //        //BindCityType();
            //        //ShowEditMode();
            //        BindGridView();
            //    }
            //    else
            //    {
            //        ViewState["SortField"] = "";
            //        ViewState["SortOrder"] = "";
            //        //FillFilter();
            //        //BindCity();
            //        //Added 13 feb 2019
            //        //BindCityType();
            //        //BindState();
            //        BindGridView();
            //        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Accredited", "Admin/AffInstitute.aspx", ""));
            //    }
            //    if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
            //        ShowAlert(Request.QueryString["msg"].ToString());


            //}
            //BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    protected void FillFilter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var statelist = from p in context.Locations
                                where p.LocationTypeID == 2
                                select new { ValueField = p.ID, TextField = p.Name };


                //var mylist = string.Concat(statelist,CourseList);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAccentre, statelist, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
            //txtInstituteID.Enabled = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Accredited Centres";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Accredited Centres", "#", ""));
        }
        else
        {
            //txtInstituteID.Enabled = false;
            Response.Redirect("AffInstitute.aspx", true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
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

    protected void AllyFilter(object sender, EventArgs e)
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
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlAccentre.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        NIELITMISContext context1 = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var centre = from s in context1.NonAffInstitutes
                         select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            centre = centre.OrderBy(s => s.Name).Distinct();
            foreach (var course in centre)
            {
                items.Add(course.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context1.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("AffInstitute.aspx", true);
    }

    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["CheckBoxArray"] != null)
            {
                ArrayList TempDataTable1 = (ArrayList)ViewState["TempDataTable"];
                //foreach (Int64 c in TempDataTable1)
                foreach (string cc in TempDataTable1)
                {
                    string IDCutfrmIndex = cc;
                   

                    String c = cc.Substring(0, cc.LastIndexOf("/") );

                    string AccrNumber = IDCutfrmIndex.Substring(IDCutfrmIndex.IndexOf("/")+1);
                 
                    Int64 InsID = Convert.ToInt64( c);

                    BreadCrumb1.Render();
                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {
                        AffInstitute objCentre;
                        NIELITMIS lnkMIS = new NIELITMIS();
                        //Int64 instituteid = 0; //Convert.ToInt64(txtInstituteID.Text);
                        Int32 lnkID = 0;
                        lnkID = entityID;
                        if (!context1.AffInstitutes.Any(s => s.ID == InsID))
                        {
                            context = new EConnectContext();
                            //var centre = (from s in context.Institutes
                            //              join p in context.AccreditationDetails on s.ID equals p.InstituteID
                            //              where s.ID == InsID && p.AccreditationStatusID  <= 4
                            //              select new
                            //              {
                            //                  ID = s.ID,
                            //                  Location = s.CityName.ToUpper(),//+ ", " + s.State.Name.ToUpper(),
                            //                  City1 = s.CityName,
                            //                  StateID = s.StateID,
                            //                  Name = s.Name,
                            //                  ContactPersonName = s.ContactPersonName,
                            //                  //MobileNumber = s.MobileNumber.HasValue ? s.MobileNumber : 0,
                            //                  AccreditationNumber = s.AccreditationDetails.Select(a => a.AccreditationNumber).ToList()//s.AccreditationDetails.Select(a => a.AccreditationNumber).Aggregate((a, x) => a + ", " + x)
                            //                   //AccreditationNumber = s.AccreditationDetails.Select(a => a.AccreditationNumber).FirstOrDefault() //s.AccreditationDetails.Select(a => a.AccreditationNumber).Aggregate((a, x) => a + ", " + x)
                            //              }).Distinct();


                           
                            var CountAccrNo = (from s in context.AccreditationDetails
                                               where s.InstituteID == InsID && s.AccreditationStatusID <= 4 && s.AccreditationNumber == AccrNumber
                                               select new
                                               {
                                                   ID = s.ID,
                                                   Courseid = s.CourseID,
                                                   AccreditationNumber = s.AccreditationNumber

                                               }).Distinct();

                            string accrno = "";

                            if (CountAccrNo.Count() >= 0)
                            {
                                foreach (var result in CountAccrNo)
                                {
                                    accrno = result.AccreditationNumber;

                                    var centre = (from s in context.Institutes
                                                  join p in context.AccreditationDetails on s.ID equals p.InstituteID
                                                  where s.ID == InsID && p.AccreditationStatusID <= 4 && p.AccreditationNumber == accrno
                                                  select new
                                                  {
                                                      ID = s.ID,
                                                      Location = s.CityName.ToUpper(),//+ ", " + s.State.Name.ToUpper(),
                                                      City1 = s.CityName,
                                                      StateID = s.StateID,
                                                      Name = s.Name,
                                                      ContactPersonName = s.ContactPersonName,
                                                      Courseid = p.CourseID,
                                                      AccreditationNumber = p.AccreditationNumber

                                                  }).Distinct();

                                    objCentre = new AffInstitute();

                                    foreach (var v in centre)
                                    {
                                        objCentre.instituteID = Convert.ToInt64(v.ID);
                                        objCentre.Name = v.Name;
                                        objCentre.Accr_No = v.AccreditationNumber;
                                        objCentre.linkedToCentre = lnkID;
                                        objCentre.enterDate = DateTime.Now;
                                        objCentre.enterBy = loginUserNo;

                                        context1.AffInstitutes.Add(objCentre);
                                        var RecordExists = (from s in context1.AffInstitutes
                                                            where s.instituteID == InsID && s.Accr_No == accrno
                                                            select new
                                                            {
                                                                ID = s.ID,
                                                                AccreditationNumber = s.Accr_No
                                                            }).Distinct();

                                        if (RecordExists.Count() == 0)
                                        {
                                            context1.SaveChanges();
                                        }

                                    }
                                }
                            }
                        }
                 

                    }
                }
            }
            strMessage = "New record saved.";
            Response.Redirect("AffInstitute.aspx?msg=" + strMessage, true);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    protected void BindGridView()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            lblError.Visible = false;
            context = new EConnectContext();

            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int64 stateID = 0;
            if (ddlAccentre.SelectedValue != "0")
                stateID = Convert.ToInt64(ddlAccentre.SelectedValue);

            DataTable DT = new DataTable();


            con.Open();

            using (SqlCommand Cmm = new SqlCommand("genInsData", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }

            PagingBar1.Bind(DT, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            con.Close();
            context.Dispose();
        }
    }

    protected void BindGridAffliated()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            lblError.Visible = false;
            context1 = new NIELITMISContext();

            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int64 stateID = 0;
            if (ddlAccentre.SelectedValue != "0")
                stateID = Convert.ToInt64(ddlAccentre.SelectedValue);

            var centre = (from s in context1.AffInstitutes
                          select new
                          {
                              ID = s.ID,
                              Name = s.Name,
                              AccreditationNumber = s.Accr_No
                          }).Distinct();

            DataTable DT = new DataTable();


            con.Open();
            SqlParameter param;
            using (SqlCommand Cmm = new SqlCommand("BindGridACC", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                param = new SqlParameter("@loginUserNo", loginUserNo);
                Cmm.Parameters.Add(param);
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);


                Sda.Fill(DT);
            }

            PagingBar2.Bind(DT, ref GdAfflated);
            uPnlGrid.Update();
            uPnlNavigation2.Update();
            if (GdAfflated.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            con.Close();
            context.Dispose();
        }
    }

    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
        }
    }

    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            int NewIndex = NewPageIndex;
            BindGridView();

            if (ViewState["CheckBoxArray"] != null)
            {
                ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                string checkAllIndex = "chkAll-" + gvMain.PageIndex;

                if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                {
                    CheckBox chkAll = (CheckBox)gvMain.HeaderRow.Cells[0].FindControl("chkAll");
                    chkAll.Checked = true;
                }
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {

                    if (gvMain.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                        {
                            CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("chkInstitutes");
                            chk.Checked = true;
                            gvMain.Rows[i].Attributes.Add("style", "background-color:aqua");
                        }
                        else
                        {

                            int CheckBoxIndex = gvMain.PageSize * (NewIndex) + (i + 1);
                            Label lblID = (Label)gvMain.Rows[i].Cells[0].FindControl("lblID");
                            Int64 ID = Convert.ToInt64(lblID.Text);
                            //if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1)
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1)
                            {
                                CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("chkInstitutes");
                                chk.Checked = true;
                                gvMain.Rows[i].Attributes.Add("style", "background-color:aqua");
                            }
                        }
                    }
                }
            }
            BindGridAffliated();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

}