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
public partial class downloadImages : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
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
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
            {
                if (!IsPostBack)
                {
                    BindCourseCategory();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Candidate Photographs", "#", ""));
                }
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Candidate Photographs", "#", ""));
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
                var courses = from s in context.CourseCategories
                              join c in context.Courses
                                  on s.ID equals c.CourseCategoryID
                              //--where (c.CourseTypeID == courseTypeCertificateExam || s.ID == 6 || s.ID==1)
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);

                var rc = from s in context.RegionalCenters
                         select new { ValueField = s.ID, TextField = s.Name };
                ListItem lst1 = new ListItem("--All--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRc, rc.OrderBy(c => c.TextField), lst1);
                if (loginUserType == UserType.RegionalCenter)
                {
                    ddlRc.SelectedValue = entityID.ToString();
                    ddlRc.Enabled = false;
                }
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
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == courseCatId && s.ShowOnWeb == true
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            }
            ddlCourseName.SelectedValue = "0";
            ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
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
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
            };
            ddlAppType.SelectedValue = "0";
            ddlAppType_SelectedIndexChanged(ddlAppType, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            if (AppTypeId > 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    if (courseCatId == 2)
                    {
                        ListItem lst1 = new ListItem("--Select One--", "0");
                        var courses = from s in context.ExaminationCycles
                                      join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                      where s.CourseID == CourseId
                                      select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);
                    }
                    else
                    {

                        ListItem lst1 = new ListItem("--Select One--", "0");
                        var courses = from s in context.Exams
                                      join a in context.ExaminationCycles on s.ExaminationCycleID equals a.ID
                                      where a.CourseID == CourseId
                                      select new { ValueField = s.ExaminationCycleID, TextField = a.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);
                        
                       
                    }

                };
            }
            else
            {
                ddlExamCycle.Items.Clear();
                ddlExamCycle.Items.Insert(0, lst);
                ddlExamCycle.SelectedValue = "0";
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
        //            if (courseCatId == 6 || courseCatId == 1)
        //            {
        //                  //select distinct Exam_Year,[Examn_Cycle_ID]  from Exam where ID in (
        ////select Exam_ID  from Course_Exam_Application where Course_ID =115) and [Examn_Cycle_ID] = 99

        //                var courses = (from s in context.Exams
        //                               join c in context.CourseExamApplications
        //                                   on s.ID equals c.ExamID
        //                               where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
        //                               select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
        //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
        //                ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
        //            }
        //            else
        //            {
                        
        //                var courses = (from s in context.Exams
        //                               join c in context.CertificateExamApplications
        //                                   on s.ID equals c.ExamID
        //                               where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
        //                               select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
        //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
        //                ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
        //            }
                    if (courseCatId == 2)
                    {
                        //select distinct Exam_Year,[Examn_Cycle_ID]  from Exam where ID in (
                        //select Exam_ID  from Course_Exam_Application where Course_ID =115) and [Examn_Cycle_ID] = 99

                        var courses = (from s in context.Exams
                                       join c in context.CertificateExamApplications
                                           on s.ID equals c.ExamID
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                       select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
                        ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);


                        
                    }
                    else
                    {
                        var courses = (from s in context.Exams
                                       join c in context.CourseExamApplications
                                           on s.ID equals c.ExamID
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                       select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
                        ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
                      
                    }
                }
                else
                {
                    ddlExamYear.Items.Clear();
                    ddlExamYear.Items.Insert(0, lst);
                    ddlExamYear.SelectedValue = "0";
                    ddlExamYear_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    if (courseCatId == 2)
                    {
                        var courses = (from s in context.Exams
                                       join c in context.CertificateExamApplications
                                           on s.ID equals c.ExamID
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                       select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                        ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);  
                    }
                    else
                    {

                        var courses = (from s in context.Exams
                                       join c in context.CourseExamApplications
                                           on s.ID equals c.ExamID
                                       where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                       select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                        ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                        
                    }
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                    ddlExamName.SelectedValue = "0";
                    ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            ddlExamCycle.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlExamYear.SelectedValue = "0";
            ddlDownloadType.SelectedValue = "0";
            chkbatchlist.Items.Clear();
            chkbatchlist.DataBind();
            divbatches.Style.Remove("overflow");
            divbatches.Style.Remove("width");
            divbatches.Style.Remove("height");
            if ((loginUserType != UserType.RegionalCenter) && (loginUserType!=UserType.ExternalRegionalCenter))
            {
                ddlRc.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlDownloadType.SelectedIndex = 0;
            ddlDownloadType_SelectedIndexChanged(this, null);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Int32 batchID = Convert.ToInt32(ddlBatch.SelectedValue);
            List<Int32> Batches = new List<Int32>();
            foreach (ListItem chk in chkbatchlist.Items)
            {
                if (chk.Selected)
                    Batches.Add(Convert.ToInt32(chk.Value));
            }
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var applCount = (from p in context.BatchItems
                                 where Batches.Contains(p.BatchID)
                                 select p).Count();
                //lblTotal.Text = applCount.ToString();
            };
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
            //Int32 batchID = Convert.ToInt32(ddlBatch.SelectedValue);
            Int32 applType = Convert.ToInt32(ddlAppType.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 applTypeID = Convert.ToInt32(ddlAppType.SelectedValue);
                Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                Int32 completed = Convert.ToInt32(enmBatchStatus.Completed);
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                Int32 regionalCenterID = Convert.ToInt32(ddlRc.SelectedValue);
                Int32 paymentPendingID = Convert.ToInt32(enmPaymentStatus.Pending);
                //
                List<Int32> Batches = new List<Int32>();
                if (chkbatchlist.Items.Count > 0)
                {
                    foreach (ListItem chk in chkbatchlist.Items)
                    {
                        if (chk.Selected)
                            Batches.Add(Convert.ToInt32(chk.Value));
                    }
                    if (Batches.Count <= 0)
                    {
                        ShowAlert("Please select atleast one batch.", true);
                        return;
                    }

                }
                else
                {
                    if (ddlRc.SelectedValue != "0")
                    {
                        Batches = context.Batchs.Where(b => b.ApplicationTypeID == applTypeID && b.ExamID == examID && b.StatusID == completed && b.RegionalCenterID == regionalCenterID).Select(a => a.ID).ToList();
                    }
                    else
                    {
                        Batches = context.Batchs.Where(b => b.ApplicationTypeID == applTypeID && b.ExamID == examID && b.StatusID == completed).Select(a => a.ID).ToList();
                    }
                     
                    //if (Batches.Count <= 0)
                    //{
                    //    if (context.Exams.Find(examID).IsBatchProcessable)
                    //    {
                    //        ShowAlert("Data not available for this category.", true);
                    //        return;
                    //    }
                    //}
                }
                String NSQFdirectoryName = "";
                String directoryName = ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "") + "_" + context.Courses.Where(s => s.ID == courseID).FirstOrDefault().Code.ToUpper();
                if (ddlRc.SelectedValue != "0")
                    directoryName = ddlRc.SelectedItem.Text.Replace("/", "_") + "_" + directoryName + "_" + ddlNameFormat.SelectedItem;
                else
                    directoryName = "NIELIT_Regional_Centers_" + directoryName + "_" + ddlNameFormat.SelectedItem;
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName));
                string photoName = "";
                if (context.Exams.Find(examID).IsBatchProcessable)
                {
                    var photos = (from a in context.CertificateExamApplications.AsNoTracking()
                                  join b in context.BatchItems on a.BatchItemID equals b.ID
                                  where Batches.Contains(b.BatchID)
                                  select new { a.Photo, a.Number, a.RollNumber }).ToList();
                    foreach (var photo in photos)
                    {
                        if (ddlNameFormat.SelectedValue == "1")
                            photoName = photo.Number;
                        else
                        {
                            if (String.IsNullOrEmpty(photo.RollNumber))
                                photoName = photo.Number;
                            else
                                photoName = photo.RollNumber;
                        }
                        FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName +"/" + photoName + "_Photo.jpeg"));
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(photo.Photo);
                        bw.Flush();
                        fs.Flush();
                        bw.Close();
                        fs.Close();
                    }
                    ZippedFile(directoryName);
                }
                else
                {
                    if (Convert.ToInt32(ddlCourseCategry.SelectedValue) != 2)
                    {
                       
                        var pht1 = context.CourseExamApplications.AsNoTracking().Where(a => a.ExamID == examID && a.CourseID == courseID && a.FinalSubmitted == true && a.PaymentStatusID != paymentPendingID);
                        string str = "LR-P-";


                        if ((ddlRc.SelectedValue != "0" && ddlCourseCategry.SelectedValue == "6") || (ddlRc.SelectedValue == "0" && ddlCourseCategry.SelectedValue == "1"))
                        {

                           
                            var photos = (from a in pht1
                                          join b in context.UploadedFiles on (str + a.CandidateID.ToString().Trim()) equals b.Name
                                          where b.Name.StartsWith("LR-P-")
                                          select new
                                          {
                                              Number = a.RegistrationNumber,
                                              RollNumber = a.RollNumber,
                                              photo = b.BlobFile
                                          }).ToList();

                            foreach (var photo in photos)
                            {
                                if (ddlNameFormat.SelectedValue == "1")
                                    photoName = photo.Number.ToString();
                                else
                                {
                                    if (String.IsNullOrEmpty(photo.Number.ToString()))
                                        photoName = photo.Number.ToString();
                                    else
                                        photoName = photo.RollNumber.ToString();
                                }
                                FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_Photo.jpeg"));
                                BinaryWriter bw = new BinaryWriter(fs);
                                bw.Write(photo.photo);
                                bw.Flush();
                                fs.Flush();
                                bw.Close();
                                fs.Close();
                            }
                            ZippedFile(directoryName);
                        }
                    }

                    else
                    {
                      

                        var pht = context.CertificateExamApplications.AsNoTracking().Where(a => a.ExamID == examID && a.CourseID == courseID && a.FinalSubmitted == true && a.PaymentStatusID != paymentPendingID);

                        if (ddlRc.SelectedValue != "0")
                            pht = pht.Where(a => a.RegionalCenterID == regionalCenterID);

                        foreach (var photo in pht)
                        {
                            if (ddlNameFormat.SelectedValue == "1")
                                photoName = photo.Number;
                            else
                            {
                                if (String.IsNullOrEmpty(photo.RollNumber))
                                    photoName = photo.Number;
                                else
                                    photoName = photo.RollNumber;
                            }
                            FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + "_Photo.jpeg"));
                            BinaryWriter bw = new BinaryWriter(fs);
                            bw.Write(photo.Photo);
                            bw.Flush();
                            fs.Flush();
                            bw.Close();
                            fs.Close();
                        }
                        ZippedFile(directoryName);

                    }

                  };
            }
        }
        catch (Exception ex)
        {
            //ShowAlert(ex.Message, true);
            Response.Write(ex.InnerException.ToString() + "<br/>" + ex.Message.ToString() + "<br/> " + ex.InnerException.InnerException.Message.ToString() + "<br/>" + ex.InnerException.Message.ToString());
        }
    }
    protected void ddlDownloadType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (var context = new EConnectContext())
            {
                chkbatchlist.Items.Clear();
                chkbatchlist.DataBind();
                if (ddlDownloadType.SelectedIndex == 1)
                {
                    Int32 applType = Convert.ToInt32(ddlAppType.SelectedValue);
                    Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                    Int32 completed = Convert.ToInt32(enmBatchStatus.Completed);
                    ListItem lst = new ListItem("--Select One--", "0");
                    var batches = from b in context.Batchs
                                  where b.ApplicationTypeID == applType && b.ExamID == examID && b.StatusID == completed
                                  select new { ValueField = b.ID, TextField = b.Number, RegionalCenterID = b.RegionalCenterID };
                    if (ddlRc.SelectedValue != "0")
                    {
                        Int32 regionalCentreID = Convert.ToInt32(ddlRc.SelectedValue);
                        batches = batches.Where(b => b.RegionalCenterID == regionalCentreID);
                    }
                    EConnect.Utils.Common.ControlUtility.BindListObject(chkbatchlist, batches.ToList());
                    if (chkbatchlist.Items.Count <= 0)
                    {
                        ShowAlert("Batches does not exist in this category.", true);
                        ddlDownloadType.SelectedIndex = 0;
                        return;
                    }
                }
                else
                {
                    divbatches.Style.Remove("overflow");
                    divbatches.Style.Remove("width");
                    divbatches.Style.Remove("height");
                }
                chkbatchlist.Visible = (chkbatchlist.Items.Count > 0);
                if (chkbatchlist.Items.Count > 6)
                {
                    divbatches.Style.Add("overflow", "scroll");
                    divbatches.Style.Add("width", "750px");
                    divbatches.Style.Add("height", "90px");
                    chkbatchlist.RepeatColumns = 10;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public void ZippedFile(string directoryName)
    {
        string p = Server.MapPath("~/download/" + directoryName);
        if (Directory.Exists(p))
        {
            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName));
                Response.Clear();
                zipFile.CompressionMethod = CompressionMethod.None;
                zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "filename=" + directoryName + ".zip");
                zipFile.Save(Response.OutputStream);
            };
            
        }
        else
        {
            ShowAlert("Data not available for this category.", true);
            return;
        }
    }

   
}