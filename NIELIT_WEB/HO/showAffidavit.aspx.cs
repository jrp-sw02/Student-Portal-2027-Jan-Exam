using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

public partial class HO_showAffidavit : BasePage
{
    Int32 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            string prevPage = "";
            prevPage = Request.UrlReferrer.ToString();
            if (prevPage == null)
                return;
           
            lblError.Visible = false;
            entityID = Convert.ToInt32(Session["EntityID"]);
            BtnBack.Visible = false;
            if (!IsPostBack)
            {               
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Show Affidavit", "#", ""));
               
                if ( !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["num"])))
                {
                    
                    string  regNo = Convert.ToString(Request.QueryString["num"]);
                    string courseType = Convert.ToString(Request.QueryString["cat"]);
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                   
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration No. " + regNo.ToString(), "#", ""));
                    showData(regNo,courseType);
                   
                    
                    BtnBack.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    
    protected void showData( string regNo,string courseType)
    {
        try
        {
           
            using (EConnectContext context = new EConnectContext())
            {

                if (courseType == "1")
                {
                    var candidate = (from a in context.CourseRegistrationApplications
                                     join g in context.RegistrationDetails on a.CandidateID  equals g.CandidateID 
                                     where (
                                     g.RegistrationNo.ToString().Trim() == regNo.Trim()
//                                    && a.GuardianName != null
					)
                                     select a).FirstOrDefault();
                    if (candidate != null)
                    {
                        //Form of candidate belonging to institute but yet not verified
                        if (candidate.ApplicantTypeID == 2 && candidate.IsVerifiedByInstitute == false)
                        {
                            ShowAlert("Candidate not verified by institute");
                            return;
                        }


                        tabphoto.Visible = true;

                        byte[] bytes = (byte[])candidate.affidavitUpload;
                        //byte[] bytes = (byte[])candidate.affidavitUpload;

                        //Response.Clear();
                        //Response.ContentType = "application/pdf";
                        //Response.AddHeader("content-length", bytes.Length.ToString());
                        //Response.BinaryWrite(bytes);


                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment; filename=Affidavit.pdf");

                        Response.BinaryWrite(bytes);
                        // Response.End();
                        // Response.Flush();
                        // Response.Clear();

                        // ImgCandidateAffidavit.ImageUrl = "data:application/pdf;base64," + Convert.ToBase64String(bytes);
                        //   pdfViewer.InnerHtml = "<embed type=\"application/pdf\" style='width:100vw;height:calc(100vh - 20px);' src=\"data:application/pdf;base64," + Convert.ToBase64String(bytes, Base64FormattingOptions.None) + "\"/>";
                        //Document myDocument = new Document(PageSize.LETTER);
                        // PdfWriter.GetInstance(myDocument, new FileStream("D:\\mydocument.pdf", FileMode.Create));
                        // myDocument.Open();
                        // myDocument.Add(new Paragraph(Encoding.UTF8.GetString(bytes)));
                        // myDocument.Close();
                        //   PnlCandidate.Visible = true;



                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Sorry ! No such Record found who submitted Guardian Details.";
                        PnlCandidate.Visible = false;

                    }
                }

                if(courseType =="2")
                {
                        var candidate = (from a in context.CertificateExamApplications 
                                     
                                     where (
                                     a.RollNumber .ToString().Trim() == regNo.Trim()
                                    && a.GuardianName != null)
                                     select a).FirstOrDefault();

                    if (candidate != null)
                    {
                        //Form of candidate belonging to institute but yet not verified
                        if (candidate.ApplicantTypeID == 2 && candidate.IsVerifiedByInstitute == false)
                        {
                            ShowAlert("Candidate not verified by institute");
                            return;
                        }


                        tabphoto.Visible = true;

                     //   byte[] bytes = (byte[])candidate.affidavitUpload;
                        
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment; filename=Affidavit.pdf");

                    //    Response.BinaryWrite(bytes);
                        



                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Sorry ! No such Record found who submitted Guardian Details.";
                        PnlCandidate.Visible = false;

                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("AffidavitVerification.aspx?" + Request.QueryString.ToString());
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}