using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.HRMS;
using System.Web.Security;
using EConnect.Utils.Common;
using EConnect.NIELIT;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Entity;
using System.Data.Common;
using System.IO;
using System.Transactions;
using System.Dynamic;

public partial class CAND_EditUploadPhoto : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            if (IsSessionAlive() == false)
                Response.Redirect("../Home.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
                lblError.Visible = false;
                MultiView1.ActiveViewIndex = 0;
                showInput();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void showInput()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 select new
                                 {
                                     Photo = a.PhotoFileID != null && a.PhotoFileID != 0 ? "PhotoExist" : "",
                                     Signature = a.SignatureFileID != null && a.SignatureFileID != 0 ? "SignatureExist" : "",
                                     Thumb = a.LeftThumbImpressionFileID != null && a.LeftThumbImpressionFileID != 0 ? "ThumbExist" : ""
                                 }).FirstOrDefault();

                if (candidate.Photo == "")
                    TrPhotoFile.Visible = true;
                else
                    TrPhotoFile.Visible = false;
                if (candidate.Signature == "")
                    TrSignatureFile.Visible = true;
                else
                    TrSignatureFile.Visible = false;
                if (candidate.Thumb == "")
                    TrLeftThumbFile.Visible = true;
                else
                    TrLeftThumbFile.Visible = false;
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public bool IsValidForm()
    {
        try
        {
            if (TrPhotoFile.Visible == true)
            {
                if (ImgUpload.FileName.ToString() == "")
                {
                    lblError.Visible = true;
                    lblError.Text = "Photo can not be left blank";
                    return false;
                }
                if (!isvalidFileExtension(ImgUpload))
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Photo .Only jpg, gif, jpeg ,png extensions are allowed.";
                    return false;
                }
                if (!isvalidFileSize(ImgUpload, 51200))
                {
                    lblError.Visible = true;
                    lblError.Text = "Photo File size should be of 50 KB or less.";
                    return false;
                }
            }
            if (TrSignatureFile.Visible == true)
            {
                if (ImgUploadSignature.FileName.ToString() == "")
                {
                    lblError.Visible = true;
                    lblError.Text = "Signature can not be left blank";
                    return false;
                }
                if (!isvalidFileExtension(ImgUploadSignature))
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Signature .Only jpg, gif, jpeg ,png extensions are allowed.";
                    return false;
                }
                if (!isvalidFileSize(ImgUploadSignature, 51200))
                {
                    lblError.Visible = true;
                    lblError.Text = "Signature File size should be of 50 KB or less.";
                    return false;
                }
            }
            if (TrLeftThumbFile.Visible == true)
            {
                if (ImgUploadThumb.FileName.ToString() == "")
                {
                    lblError.Visible = true;
                    lblError.Text = "Left Thumb Impression can not be left blank";
                    return false;
                }
                if (!isvalidFileExtension(ImgUploadThumb))
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid Left Thumb Impression File .Only jpg, gif, jpeg ,png extensions are allowed.";
                    return false;
                }
                if (!isvalidFileSize(ImgUploadThumb, 51200))
                {
                    lblError.Visible = true;
                    lblError.Text = "Left Thumb Impression File size should be of 50 KB or less.";
                    return false;
                }
            }
            lblError.Visible = false;
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BtnPreview_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {
                using (EConnectContext context = new EConnectContext())
                {


                    var candidate = (from a in context.Candidates
                                     where a.ID == entityID
                                     select a).FirstOrDefault();

                    Boolean isUpdated = false;
                    if (candidate.TempPhoto == null && candidate.PhotoFileID == null)
                    {
                        candidate.PhotoFileName = ImgUpload.FileName.ToString();
                        candidate.TempPhoto = ImgUpload.FileBytes;
                    }
                    else
                    {
                        isUpdated = true;
                    }

                    if (candidate.TempSignature == null && candidate.SignatureFileID == null)
                    {
                        candidate.SignatureFileName = ImgUploadSignature.FileName.ToString();
                        candidate.TempSignature = ImgUploadSignature.FileBytes;
                    }
                    else
                    {
                        isUpdated = true;
                    }
                    if (candidate.TempLeftThumb == null && candidate.LeftThumbImpressionFileID == null)
                    {
                        candidate.LeftThumbFileName = ImgUploadThumb.FileName.ToString();
                        candidate.TempLeftThumb = ImgUploadThumb.FileBytes;
                    }
                    else
                    {
                        isUpdated = true;

                    }
                    if (isUpdated == true)
                    {
                        lblError.Visible = true;
                        lblError.Text = "Already Updated..Photo Detail";
                        MultiView1.ActiveViewIndex = 0;
                    }
                    else
                    {

                        context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        if (TrPhotoFile.Visible == true)
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.TempPhoto);
                        if (TrSignatureFile.Visible == true)
                            imgSignature.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.TempSignature);
                        if (TrLeftThumbFile.Visible == true)
                            imgThumb.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.TempLeftThumb);
                        MultiView1.ActiveViewIndex = 1;
                    }

                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnGoBack_Click(object sender, EventArgs e)
    {

    }
    protected void LnkBtnGoBack_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 select a).FirstOrDefault();
                candidate.TempPhoto = null;
                candidate.PhotoFileName = null;
                candidate.TempSignature = null;
                candidate.SignatureFileName = null;
                candidate.TempLeftThumb = null;
                candidate.LeftThumbFileName = null;
                context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            };
            MultiView1.ActiveViewIndex = 0;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {

                    var candidate = (from a in context.Candidates
                                     where a.ID == entityID
                                     select a).FirstOrDefault();
                    //string str = str.Substring(0, str.LastIndexOf(".dat", StringComparison.OrdinalIgnoreCase));


                    //Save Candidate Photo.
                    if (candidate.PhotoFileID == null || candidate.PhotoFileID == 0)
                    {
                        UploadedFile objPhoto = new EConnect.NIELIT.UploadedFile();
                        objPhoto.Name = "LR-P-" + candidate.ID; //Photo File Name Legend Indicate Particular Candidate's Photo
                        objPhoto.OriginalName = candidate.PhotoFileName;
                        objPhoto.BlobFile = candidate.TempPhoto;
                        objPhoto.Extension = System.IO.Path.GetExtension(candidate.PhotoFileName);
                        objPhoto.UploadedOn = DateTime.Now;
                        context.UploadedFiles.Add(objPhoto);
                        context.SaveChanges();
                        candidate.PhotoFileID = objPhoto.ID;
                        candidate.TempPhoto = null;
                        candidate.PhotoFileName = null;
                    }
                    //Save Candidate Signature.
                    if (candidate.SignatureFileID == null || candidate.SignatureFileID == 0)
                    {
                        UploadedFile objSignature = new EConnect.NIELIT.UploadedFile();
                        objSignature.Name = "LR-S-" + candidate.ID;//Photo File Name Legend Indicate Particular Candidate's Signature
                        objSignature.OriginalName = candidate.SignatureFileName.ToString();
                        objSignature.BlobFile = candidate.TempSignature;
                        objSignature.Extension = System.IO.Path.GetExtension(candidate.SignatureFileName);
                        objSignature.UploadedOn = DateTime.Now;
                        context.UploadedFiles.Add(objSignature);
                        context.SaveChanges();
                        candidate.SignatureFileID = objSignature.ID;
                        candidate.TempSignature = null;
                        candidate.SignatureFileName = null;
                    }
                    //Save Candidate Left Thumb Impression.
                    if (candidate.LeftThumbImpressionFileID == null || candidate.LeftThumbImpressionFileID == 0)
                    {
                        UploadedFile objThumb = new EConnect.NIELIT.UploadedFile();
                        objThumb.Name = "LR-T-" + candidate.ID;//Photo File Name Legend Indicate Particular Candidate's Left Thumb Impression
                        objThumb.OriginalName = candidate.LeftThumbFileName;
                        objThumb.BlobFile = candidate.TempLeftThumb;
                        objThumb.Extension = System.IO.Path.GetExtension(candidate.LeftThumbFileName);
                        objThumb.UploadedOn = DateTime.Now;
                        context.UploadedFiles.Add(objThumb);
                        context.SaveChanges();
                        candidate.LeftThumbImpressionFileID = objThumb.ID;
                        candidate.TempLeftThumb = null;
                        candidate.LeftThumbFileName = null;
                    }

                    //Update Candidate Personal Data with PhotoFileID,SignatureFileID,LeftThumbImpressionFileID.

                    context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    scope.Complete(); // Transaction Process finally Complete                               
                    MultiView1.ActiveViewIndex = 2;
                };
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void LnkBtnBacktoProfile1_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../FrmDashBoard.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void LnkBtnBacktoProfil2_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../FrmDashBoard.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnCancelUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 select a).FirstOrDefault();
                candidate.TempPhoto = null;
                candidate.PhotoFileName = null;
                candidate.TempSignature = null;
                candidate.SignatureFileName = null;
                candidate.TempLeftThumb = null;
                candidate.LeftThumbFileName = null;
                context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            };
            MultiView1.ActiveViewIndex = 0;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnCancelPreview_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString.ToString()))
            {
                Response.Redirect("../Admin/AdminRegstud.aspx?" + Request.QueryString.ToString());
            }
            else
            {
                Response.Redirect("../FrmDashBoard.aspx");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}