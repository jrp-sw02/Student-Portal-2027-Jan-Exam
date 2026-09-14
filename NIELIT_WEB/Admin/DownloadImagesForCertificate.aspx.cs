using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.IO;
using Ionic.Zip;
using System.Data.Objects;
using System.Text;
using System.Web.UI;


public partial class DownloadImagesForCertificate : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
        lblNoRecord.Text = "";
        lblNoRecord.Visible = false;
              
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
            {
            if (!IsPostBack)
            {
                BindCourseCategory();
                ddlCourseCategry.SelectedValue = "6";
                BindDataDownloadedSequence();
              //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "Admin/DownloadImagesForCertificate.aspx", ""));
                 
                lblNoRecord.Text = "";
                lblNoRecord.Visible = false;
            }
            }
            else
            {
//                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "#", ""));
              //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "Admin/DownloadImagesForCertificate.aspx", ""));
                btnView.Visible = false;
                btnReset.Visible = false;
                Lblerror.Text = "You can not download candidate photographs.";
                Lblerror.Visible = true;
            }            
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }
    protected void BindCourseCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseCategories
                                 where p.ID==6
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };                               
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, CourseList, lst);
                //ddlCourseCategry.SelectedValue = "6";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //BreadCrumb1.Render();
            //lblNoRecord.Text = "";
            //int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            //using (var context = new EConnectContext())
            //{
            //    ListItem lst = new ListItem("--Select One--", "0");
            //    if (courseCatId > 0)
            //    {
                    
            //        var DataDownloadedSequence = from p in context.NSQFCertPhasePrintDetails
            //                                     join c in context.CourseMappingWithNSQFCoursecodes on p.course_code equals c.NSQFCourseCode
            //                                     join k in context.Courses on c.CourseID equals k.ID
            //                                     where k.CourseCategoryID == courseCatId && p.data_downloaded_sequence != null
            //                                     orderby (p.data_downloaded_sequence)
            //                                     select new { ValueField = p.data_downloaded_sequence, TextField = p.data_downloaded_sequence };

            //        DataDownloadedSequence= DataDownloadedSequence.Distinct().Take(20);
            //        DataDownloadedSequence = DataDownloadedSequence.Distinct().OrderByDescending(a=>a.TextField);
            //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlDataDownloadedSequence, DataDownloadedSequence, lst);
            //    }
            //    else
            //    {
            //        ddlDataDownloadedSequence.Items.Clear();
            //        ddlDataDownloadedSequence.Items.Insert(0, lst);
            //    }
            //    ddlDataDownloadedSequence.SelectedValue = "0";
            //    ddlDataDownloadedSequence_SelectedIndexChanged(ddlDataDownloadedSequence, EventArgs.Empty);
            //};

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ddlDataDownloadedSequence_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            lblNoRecord.Text = "";
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            int DataDownloadSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("-- All --", "0");
                if (courseCatId > 0 && DataDownloadSequenceId > 0)
                {
                    var courses = from p in context.NSQFCertPhasePrintDetails
                                  join c in context.CourseMappingWithNSQFCoursecodes on p.course_code equals c.NSQFCourseCode
                                  join k in context.Courses on c.CourseID equals k.ID
                                  where k.CourseCategoryID == courseCatId && p.data_downloaded_sequence != null
                                  && p.data_downloaded_sequence == DataDownloadSequenceId 
                                  select new { ValueField = c.CourseID, TextField = k.Name + " (" + c.NSQFCourseCode + ")" };
                    courses = courses.Distinct();                    
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
                }
                else
                {
                    ddlCourseName.Items.Clear();
                    ddlCourseName.Items.Insert(0, lst);
                }
                ddlCourseName.SelectedValue = "0";
                ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlPhaseNumber.Items.Clear();
            lblNoRecord.Text = "";
            Int32 cid =Convert.ToInt32( ddlCourseName.SelectedValue);
            int DataDownloadSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
            ListItem lst = new ListItem("-- All --", "0");
            using (EConnectContext context = new EConnectContext())
            {
                if (cid != null)
                {
                    var PhaseNumberList = from p in context.NSQFCertPhasePrintDetails
                                          join k in context.CourseMappingWithNSQFCoursecodes on p.course_code equals k.NSQFCourseCode
                                          where k.CourseID == cid && p.data_downloaded_sequence != null
                                          && p.data_downloaded_sequence == DataDownloadSequenceId
                                          select new { ValueField = p.phase, TextField = p.phase };
                    PhaseNumberList = PhaseNumberList.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlPhaseNumber, PhaseNumberList, lst);
                }
                else
                {
                    ddlPhaseNumber.Items.Clear();
                    ddlPhaseNumber.Items.Insert(0, lst);
                }
                ddlPhaseNumber.SelectedValue = "0";                
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
    protected void BindDataDownloadedSequence()
    {
        try
        {
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            BreadCrumb1.Render();
            lblNoRecord.Text = "";           
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseCatId > 0)
                {

                    var DataDownloadedSequence = from p in context.NSQFCertPhasePrintDetails
                                                 join c in context.CourseMappingWithNSQFCoursecodes on p.course_code equals c.NSQFCourseCode
                                                 join k in context.Courses on c.CourseID equals k.ID
                                                 where k.CourseCategoryID == courseCatId && p.data_downloaded_sequence != null
                                                 orderby (p.data_downloaded_sequence)
                                                 select new { ValueField = p.data_downloaded_sequence, TextField = p.data_downloaded_sequence };

                    //DataDownloadedSequence = DataDownloadedSequence.Distinct().Take(20);
                    DataDownloadedSequence = DataDownloadedSequence.Distinct().OrderByDescending(a => a.TextField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlDataDownloadedSequence, DataDownloadedSequence, lst);
                }
                else
                {
                    ddlDataDownloadedSequence.Items.Clear();
                    ddlDataDownloadedSequence.Items.Insert(0, lst);
                }
                ddlDataDownloadedSequence.SelectedValue = "0";
                ddlDataDownloadedSequence_SelectedIndexChanged(ddlDataDownloadedSequence, EventArgs.Empty);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }  
       
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlDataDownloadedSequence.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlPhaseNumber.SelectedValue = "0";
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
       
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            

            using (EConnectContext context = new EConnectContext())
            {
                var photoCount = 0;
                lblNoRecord.Text = "";
                int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
                int DataDownloadSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
                Int32 courseID = Convert.ToInt32( ddlCourseName.SelectedValue);                
                int PhaseNumber = Convert.ToInt32(ddlPhaseNumber.SelectedValue);                               
             
                string datetimeF = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                string dirnamedate = datetimeF.Replace("/", "").Replace(":", "").Replace(" ", "_");
              
                 String   directoryName = "NIELIT_" + DataDownloadSequenceId + "_" + dirnamedate;
                directoryName = directoryName.Replace("-", "").Replace("/", "_");
                string directoryPath = Server.MapPath("~/Download/" + directoryName);

                if (!Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName));
                }

                string photoName = "";

                if (DataDownloadSequenceId != 0) // Data Download Sequence Number  SELECTED by user 
                    {
                        var photos = (from a in context.CourseRegistrationApplications.AsNoTracking()
                                      join b in context.RegistrationDetails.AsNoTracking() on a.CandidateID equals b.CandidateID
                                      join c in context.NSQFCertPhasePrintDetails.AsNoTracking() on b.RegistrationNo equals c.registration_no
                                      join k in context.CourseMappingWithNSQFCoursecodes.AsNoTracking() on c.course_code equals k.NSQFCourseCode
                                      where c.data_downloaded_sequence == DataDownloadSequenceId && a.CourseID == b.CourseID && a.FinalSubmitted == true
                                      select new { a.Photo, b.RegistrationNo, k.CourseID, c.phase }).ToList();

                        if (courseID != 0) // course code selected by user
                        {
                            if (PhaseNumber != 0) // course code and phase number  selected by user
                            {
                                photos = (from a in context.CourseRegistrationApplications.AsNoTracking()
                                          join b in context.RegistrationDetails.AsNoTracking() on a.CandidateID equals b.CandidateID
                                          join c in context.NSQFCertPhasePrintDetails.AsNoTracking() on b.RegistrationNo equals c.registration_no
                                          join k in context.CourseMappingWithNSQFCoursecodes.AsNoTracking() on c.course_code equals k.NSQFCourseCode
                                          where c.data_downloaded_sequence == DataDownloadSequenceId && k.CourseID == courseID && a.CourseID == b.CourseID 
                                          && c.phase==PhaseNumber && a.FinalSubmitted == true
                                          select new { a.Photo, b.RegistrationNo, k.CourseID, c.phase }).ToList();
                            }
                            else // // course code selected  but phase number not selected by user
                            {
                                photos = (from a in context.CourseRegistrationApplications.AsNoTracking()
                                          join b in context.RegistrationDetails.AsNoTracking() on a.CandidateID equals b.CandidateID
                                          join c in context.NSQFCertPhasePrintDetails.AsNoTracking() on b.RegistrationNo equals c.registration_no
                                          join k in context.CourseMappingWithNSQFCoursecodes.AsNoTracking() on c.course_code equals k.NSQFCourseCode
                                          where c.data_downloaded_sequence == DataDownloadSequenceId && k.CourseID == courseID && a.CourseID == b.CourseID && a.FinalSubmitted == true
                                          select new { a.Photo, b.RegistrationNo, k.CourseID, c.phase }).ToList();
                            }

                        }                         
                        photoCount = photos.Count();
                        if (photoCount == 0)
                        {
                            Directory.Delete(directoryPath);
                        }
                        else
                        {
                            foreach (var photo in photos)
                            {
                                photoName = photo.RegistrationNo.ToString();
                                FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + ".jpg"));
                                //FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_Photo.jpeg"));
                                BinaryWriter bw = new BinaryWriter(fs);
                                bw.Write(photo.Photo);
                                bw.Flush();
                                fs.Flush();
                                bw.Close();
                                fs.Close();
                            }
                        }
                    }

                if (photoCount != 0)
                {
                    using (ZipFile zipFile = new ZipFile())
                    {
                        zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName));
                        Response.Clear();
                        zipFile.CompressionMethod = CompressionMethod.None;
                        zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment; filename=" + directoryName + ".zip");
                        zipFile.Save(Response.OutputStream);
                    };
                }
                else
                {
                    lblNoRecord.Text = "No Record Found";
                    lblNoRecord.Visible = true;
                }
            };
        }
        catch (Exception ex)
        {
            Response.Write(ex.InnerException.ToString() + "<br/>" + ex.Message.ToString() + "<br/> " + ex.InnerException.InnerException.Message.ToString() + "<br/>" + ex.InnerException.Message.ToString());
        }
    }
   
}