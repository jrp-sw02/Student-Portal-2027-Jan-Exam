using System;
using System.Data.Objects;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_Scholarship : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    txtflFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    if (!string.IsNullOrEmpty(Request.QueryString["Datefrom"]) && !string.IsNullOrEmpty(Request.QueryString["Dateto"]))
                    {
                        txtflFromDate.Text = Request.QueryString["Datefrom"];
                        txtToDate.Text = Request.QueryString["Dateto"];
                        BindGridView();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Aadhar & Bank Acc. Details:-" + txtflFromDate.Text + " To " + txtToDate.Text, "HO/Scholarship.aspx?Datefrom=" + Request.QueryString["datefrom"] + "&Dateto=" + Request.QueryString["dateto"], ""));
                        BreadCrumb1.Render();
                    }
                    else
                    {
                        BindGridView();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Aadhar & Bank Acc. Details :-" + txtflFromDate.Text + " To " + txtToDate.Text, "HO/Scholarship.aspx", ""));
                        BreadCrumb1.Render();
                    }
                }
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindGridView()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                DateTime fromdate = Convert.ToDateTime(txtflFromDate.Text);
                DateTime todate = Convert.ToDateTime(txtToDate.Text);
                var scholarship = from s in context.CandidateScholarships 
                                  where System.Data.Entity.DbFunctions.TruncateTime(s.Date) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate) && System.Data.Entity.DbFunctions.TruncateTime(s.Date) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                   select new
                                   {
                                       ID = s.ID,
                                       name = s.Candidate.Name,
                                       accno = s.AccountNumber,
                                       bname = s.BankName,
                                       Date = s.Date ,
                                       dob = s.Candidate.DateOfBirth,
                                       regno = context.RegistrationDetails.Where(t=>t.CandidateID == s.CandidateID).OrderByDescending(t=>t.CommencementFromDate).FirstOrDefault().RegistrationNo
                                   };
             
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "name":
                            if (sortOrder == "DESC")
                                scholarship = scholarship.OrderByDescending(s => s.name);
                            else
                                scholarship = scholarship.OrderBy(s => s.name);
                            break;
                        case "accno":
                            if (sortOrder == "DESC")
                                scholarship = scholarship.OrderByDescending(s => s.accno);
                            else
                                scholarship = scholarship.OrderBy(s => s.accno);
                            break;
                        case "bname":
                            if (sortOrder == "DESC")
                                scholarship = scholarship.OrderByDescending(s => s.bname);
                            else
                                scholarship = scholarship.OrderBy(s => s.bname);
                            break;
                        case "dob":
                            if (sortOrder == "DESC")
                                scholarship = scholarship.OrderByDescending(s => s.dob);
                            else
                                scholarship = scholarship.OrderBy(s => s.dob);
                            break;
                        case "regno":
                            if (sortOrder == "DESC")
                                scholarship = scholarship.OrderByDescending(s => s.regno);
                            else
                                scholarship = scholarship.OrderBy(s => s.regno);
                            break;
                        case "Date":
                            if (sortOrder == "DESC")
                                scholarship = scholarship.OrderByDescending(s => s.Date);
                            else
                                scholarship = scholarship.OrderBy(s => s.Date);
                            break;
                        default:
                            scholarship = scholarship.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(scholarship, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (gvMain.Rows.Count <= 0)
                {
                    lblError1.Visible = true;
                    lblError1.Text = "No record found. Please select filter criteria to  display Aadhar and Bank Account Details records";
                }
                else
                {
                    lblError1.Visible = false;
                    lblError1.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            if (hfActionID.Value != "")
            {
                Int64 recordID = Convert.ToInt64(hfActionID.Value.Split('$')[0]);
                var scholarship = context.CandidateScholarships.Where(s => s.ID == recordID).FirstOrDefault();
                LinkButton btnAction = (LinkButton)sender;
                if (btnAction.CommandName == "AH")
                {
                    lbaadhar.PostBackUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.AaadharFileID;
                    BindGridView();
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "IN")
                {
                    lbincome.PostBackUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.IncomeProofFileID;
                    BindGridView();
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "CT")
                {
                    lbcastcertificate.PostBackUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.CasteCertificateFileID;
                    BindGridView();
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "PH")
                {
                    lbphcertificate.PostBackUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.PhysicalHandicapFileID;
                    BindGridView();
                    hfActionID.Value = "";
                }
                else
                {
                    lbankaccount.PostBackUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholarship.BankAccountFileID;
                    BindGridView();
                    hfActionID.Value = "";
                }
                uPnlGrid.Update();
            }
        }
        catch (Exception ex)
        {
            hfActionID.Value = "";
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Aadhar & Bank Acc. Details:-" + txtflFromDate.Text + " To " + txtToDate.Text, "HO/Scholarship.aspx?Datefrom=" + Request.QueryString["datefrom"] + "&Dateto=" + Request.QueryString["dateto"], ""));
            BreadCrumb1.Render();
            upBread.Update();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
           
            txtflFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            Response.Redirect("Scholarship.aspx");
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
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h2.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h3.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h4.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                h5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h5.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                HyperLink h6 = (HyperLink)e.Row.Cells[6].Controls[0];
                h6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h6.NavigateUrl + "&datefrom=" + txtflFromDate.Text + "&dateto=" + txtToDate.Text);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
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
    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            btnCancel.Visible = true;
            lblHeading.Text = "Aadhar & Bank Acc. Candidate Details";
            using (EConnectContext context = new EConnectContext())
            {
                Int64 scholarshipId = Convert.ToInt64(Request.QueryString["Key"]);
                var scholar = (from s in context.CandidateScholarships
                               where s.ID == scholarshipId
                               select new
                               {
                                   name = s.Candidate.Name,
                                   dob = s.Candidate.DateOfBirth,
                                   candidateid = s.CandidateID,
                                   aacno = s.AccountNumber,
                                   bname = s.BankName,
                                   baddresss = s.BankAddress,
                                   ifscocde = s.IFSCCode,
                                   aadharcard = s.AadharNumber.HasValue ? s.AadharNumber : 0,
                                   aadharfileid = s.AaadharFileID.HasValue ? s.AaadharFileID : 0,
                                   icomeid = s.IncomeProofFileID.HasValue ? s.IncomeProofFileID : 0,
                                   phid = s.PhysicalHandicapFileID.HasValue ? s.PhysicalHandicapFileID : 0,
                                   cid = s.CasteCertificateFileID.HasValue ? s.CasteCertificateFileID : 0,
                                   bid = s.BankAccountFileID.HasValue ? s.BankAccountFileID : 0,
                                   anincome = s.IncomeCategory.Name,
                                   fname = s.Candidate.FatherName,
                                   mname  = s.Candidate.MotherName,
                                   gname = s.Candidate.GuardianName,
                                   gender = s.Candidate.Gender,
                                   isphysicallhandicp = s.IsHandicaped,
                                   catecategoryid = s.CasteCategory,
                                   acctype = s.TypeOfAccount
                               }).FirstOrDefault();

                tdName.InnerHtml = GetInitCap(scholar.name);
                tdgender.InnerHtml = scholar.gender;
                if (scholar.isphysicallhandicp.ToString() == "True")
                    tdphhandicap.InnerHtml = "Yes";
                else
                    tdphhandicap.InnerHtml = "No";
                tdcategory.InnerHtml = context.CastCategories.Where(s => s.ID == scholar.catecategoryid).FirstOrDefault().Name;
                tddob.InnerHtml = scholar.dob.ToString("dd-MMM-yyyy");
                tdincome.InnerHtml = scholar.anincome.ToString();
                if (scholar.aadharcard == 0)
                    tdaadharcard.InnerHtml = "NA";
                else
                    tdaadharcard.InnerHtml = scholar.aadharcard.Value.ToString();
                tdemail.InnerHtml = context.CandidateContactDetails.Where(t => t.CandidateID == scholar.candidateid).OrderByDescending(t => t.EffectiveFromDate).FirstOrDefault().EmailAddress;
                if (context.CandidateContactDetails.Where(t => t.CandidateID == scholar.candidateid).OrderByDescending(t => t.EffectiveFromDate).FirstOrDefault().MobileNumber.HasValue == true)
                    tdmobile.InnerHtml = context.CandidateContactDetails.Where(t => t.CandidateID == scholar.candidateid).OrderByDescending(t => t.EffectiveFromDate).FirstOrDefault().MobileNumber.Value.ToString();
                else
                    tdmobile.InnerHtml = "NA";


                if (string.IsNullOrEmpty(scholar.gname) == true && string.IsNullOrWhiteSpace(scholar.gname) == true)
                {
                    tdgname.Style.Add("display", "none");
                    tdhgame.Style.Add("display", "none");
                    if (string.IsNullOrEmpty(scholar.fname) == false && !string.IsNullOrWhiteSpace(scholar.fname))
                        tdfname.InnerHtml = GetInitCap(scholar.fname);
                    else
                        tdfname.InnerHtml = " ";
                    if (string.IsNullOrEmpty(scholar.mname) == false && string.IsNullOrWhiteSpace(scholar.mname) == false)
                        tdmname.InnerHtml = GetInitCap(scholar.mname);
                    else
                        tdmname.InnerHtml = " ";
                }
                else
                {
                    tdfname.Style.Add("display", "none");
                    tdhfame.Style.Add("display", "none");
                    tdhmame.Style.Add("display", "none");
                    tdmname.Style.Add("display", "none");
                    tdgname.InnerHtml = string.IsNullOrEmpty(scholar.gname) == false && string.IsNullOrWhiteSpace(scholar.gname) == false ? GetInitCap(scholar.gname) : " ";
                }

                //registration details
                var reg = context.RegistrationDetails.Where(s => s.CandidateID == scholar.candidateid).OrderByDescending(s => s.CommencementFromDate).FirstOrDefault();
                tdregdetails.InnerHtml = "Registration Number :-" + reg.RegistrationNo.ToString() + "<br/>" + " Registration Date:- " + reg.RegistrationDate.ToString("dd-MMM-yyyy") + "<br/>" + " Commencement Date:- " + reg.CommencementFromDate.ToString("dd-MMM-yyyy") + "<br/>" + " Valid Upto Date:- " + reg.ValidUptoDate.ToString("dd-MMM-yyyy") + "<br/>" + " Course :- " + GetInitCap(reg.Course.Name);
                if (reg.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                {
                    tdregdetails.InnerHtml += "<br/>" + " Institute Detail :- " + GetInitCap(reg.Institute.Name) + "(" + reg.Institute.AccreditationDetails.Where(c => c.CourseID == reg.CourseID).FirstOrDefault().AccreditationNumber.ToString() + ")";
                }

                //bank details
                tdbank.InnerHtml = "Bank Name :-" + scholar.bname.ToString() + "<br/>" + " Bank Address :- " + scholar.baddresss.ToString() + "<br/>" + " IFSC Code :- " + scholar.ifscocde.ToString() + "<br/>" + "Account Type:- " +  scholar.acctype.ToString() + "<br/>" +  "Account No:- " + scholar.aacno.ToString();

                if(scholar.aadharfileid != 0)
                {
                    HyperLink hlaadhar = new HyperLink();
                    hlaadhar.Text = "<img src='../images/download.jpg' style='border:none;'></img> Scanned Copy of Aadhar Card.";
                    hlaadhar.Style.Add("text-decoration", "none");
                    hlaadhar.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholar.aadharfileid;
                    tddoc.Controls.Add(hlaadhar);
                }
                if (scholar.icomeid != 0)
                {
                    HyperLink hlincome = new HyperLink();
                    if (scholar.aadharfileid != 0)
                        hlincome.Text = "<br/><img src='../images/download.jpg' style='border:none;'></img> Scanned Copy of Income Certificate of Parents.";
                    else
                        hlincome.Text = "<img src='../images/download.jpg'></img> Scanned Copy of Income Certificate of Parents.";
                    hlincome.Style.Add("text-decoration", "none");
                    hlincome.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholar.icomeid;
                    tddoc.Controls.Add(hlincome);
                }
                if (scholar.cid != 0)
                {
                    HyperLink hlcaste = new HyperLink();
                    hlcaste.Text = "<br/><img src='../images/download.jpg' style='border:none;'></img>  Scanned Copy of Cast Certificate.";
                    hlcaste.Style.Add("text-decoration", "none");
                    hlcaste.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholar.cid;
                    tddoc.Controls.Add(hlcaste);
                }

                if (scholar.bid != 0)
                {
                    HyperLink hlbank = new HyperLink();
                    hlbank.Text = "<br/><img src='../images/download.jpg' style='border:none;'></img>  Scanned Copy of Bank Account Pass Book. ";
                    hlbank.Style.Add("text-decoration", "none");
                    hlbank.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholar.bid;
                    tddoc.Controls.Add(hlbank);
                }

                if (scholar.phid != 0)
                {
                    HyperLink hlhandicap = new HyperLink();
                    hlhandicap.Text = "<br/><img src='../images/download.jpg' style='border:none;'></img>  Scanned Copy of Physically Handicapped Certificate.";
                    hlhandicap.Style.Add("text-decoration", "none");
                    hlhandicap.NavigateUrl = "../Handlers/UploadedFileHandler.ashx?ID=" + scholar.phid;
                    tddoc.Controls.Add(hlhandicap);
                }

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Candidate Details", "", ""));
                ViewState["LastModifiedOn"] = DateTime.Now; 
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // context.Dispose();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect("Scholarship.aspx?Datefrom=" + Request.QueryString["datefrom"] + "&Dateto=" + Request.QueryString["dateto"], true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}