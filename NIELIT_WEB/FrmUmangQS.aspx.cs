using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT; 

public partial class FrmUmangQS : BasePage
{
    string applno ;
    Int32 servicetype = 0;
    Int32 servicefor = 0;
    string urls;
    string urlsPrac;
    Int64 applID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
            if ((!String.IsNullOrEmpty(Request.QueryString["appno"])) && (!String.IsNullOrEmpty(Request.QueryString["servicefor"])) && (!String.IsNullOrEmpty(Request.QueryString["servicetype"])))
            {
                //servicefor: 1-OABC; 2-DLC
                //servicetype : 1-AdmitCard; 2-View Filled form

                using (EConnectContext context = new EConnectContext())
                {
                    CourseExamApplication appl_oabc = new CourseExamApplication();
                    CertificateExamApplication appl_dlc = new CertificateExamApplication();
                    applno = Convert.ToString(Request.QueryString["appno"]);
                    servicefor = Convert.ToInt32(Request.QueryString["servicefor"]);
                    servicetype = Convert.ToInt32(Request.QueryString["servicetype"]);

                    if (servicefor == 1)//OABC
                    {
                        if (servicetype == 1)
                        {
                            applID = context.CourseExamApplications.Where(s => s.Number == applno && s.RollNumber != null && s.CourseCategoryID == 1).Select(s => s.ID).SingleOrDefault();
                        }
                        else if (servicetype == 2)
                        {
                            applID = context.CourseExamApplications.Where(s => s.Number == applno && s.CourseCategoryID == 1).Select(s => s.ID).SingleOrDefault();
                        }
                        if (applID != 0)
                        {
                            appl_oabc = context.CourseExamApplications.Find(applID);
                        }
                        else
                        {
                            Response.Write("Record not found");
                            Response.End();
                            return;
                        }
                    }
                    else if (servicefor == 2)//DLC
                    {
                        if (servicetype == 1)
                        {
                            applID = context.CertificateExamApplications.Where(s => s.Number == applno && s.RollNumber != null && s.CourseCategoryID == 2).Select(s => s.ID).SingleOrDefault();
                        }
                        else if (servicetype == 2)
                        {
                            applID = context.CertificateExamApplications.Where(s => s.Number == applno && s.CourseCategoryID == 2).Select(s => s.ID).SingleOrDefault();
                        }
                        if (applID != 0)
                        {
                            appl_dlc = context.CertificateExamApplications.Find(applID);
                        }
                        else
                        {
                            Response.Write("Record not found");
                            Response.End();
                            return;
                        }
                    }
                    //CourseExamApplication appl_oabc = context.CourseExamApplications.Find(applID);
                    //CertificateExamApplication appl_dlc = context.CertificateExamApplications.Find(applID); 
                    //http://14.139.53.83:9998/FrmUmangQS.aspx?appno=GOCCC3669115&servicefor=2&servicetype=2

                    //for OABC Admit Card
                    if (servicefor == 1 && servicetype == 1) 
                    {
                        if (appl_oabc.NumberOfTheoryModulesApplied != 0 && appl_oabc.NumberOfPracticalModulesApplied != 0)
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseAdmitCard.aspx?ID=" + appl_oabc.CourseID + "&Appid=" + appl_oabc.ID);
                            urlsPrac = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CoursePracticalAdmitCard.aspx?ID=" + appl_oabc.CourseID + "&Appid=" + appl_oabc.ID);
                        }
                        if (appl_oabc.NumberOfTheoryModulesApplied != 0)
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseAdmitCard.aspx?ID=" + appl_oabc.CourseID + "&Appid=" + appl_oabc.ID);
                        }
                        else if (appl_oabc.NumberOfPracticalModulesApplied != 0)
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CoursePracticalAdmitCard.aspx?ID=" + appl_oabc.CourseID + "&Appid=" + appl_oabc.ID);
                        }
                    }
                    //for OABC view filled form
                    else if (servicefor == 1 && servicetype == 2) 
                    {
                        urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/ExamFormPreview.aspx?ID=" + appl_oabc.CourseID + "&Appid=" + appl_oabc.ID + "&candtype=External");
                    }
                    //for DLC Admit Card
                    else if (servicefor == 2 && servicetype == 1) 
                    {
                        if ((appl_dlc.ExamID >= 4371 && appl_dlc.CourseID == 7) || (appl_dlc.ExamID >= 4383 && appl_dlc.CourseID == 5) || (appl_dlc.ExamID >= 4395 && appl_dlc.CourseID == 98) || (appl_dlc.ExamID >= 4407 && appl_dlc.CourseID == 99) || (appl_dlc.ExamID == 4431 && appl_dlc.CourseID == 174) || (appl_dlc.ExamID == 4455 && appl_dlc.CourseID == 175))
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion6.aspx?ID=" + appl_dlc.CourseID + "&Appid=" + appl_dlc.ID);
                        }
                        else if ((appl_dlc.ExamID >= 3778 && appl_dlc.ExamID < 4371 && appl_dlc.CourseID == 7) || (appl_dlc.ExamID >= 3790 && appl_dlc.ExamID < 4383 && appl_dlc.CourseID == 5) || (appl_dlc.ExamID >= 3802 && appl_dlc.ExamID < 4395 && appl_dlc.CourseID == 98) || (appl_dlc.ExamID >= 3814 && appl_dlc.ExamID < 4407 && appl_dlc.CourseID == 99))
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion5.aspx?ID=" + appl_dlc.CourseID + "&Appid=" + appl_dlc.ID);
                        }
                        else if ((appl_dlc.ExamID >= 2708 && appl_dlc.CourseID == 7) || (appl_dlc.ExamID >= 2696 && appl_dlc.CourseID == 5) || (appl_dlc.ExamID >= 2720 && appl_dlc.CourseID == 98) || (appl_dlc.ExamID >= 2732 && appl_dlc.CourseID == 99))
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion4.aspx?ID=" + appl_dlc.CourseID + "&Appid=" + appl_dlc.ID);
                        }
                        else if ((appl_dlc.ExamID >= 1944 && appl_dlc.CourseID == 7) || (appl_dlc.ExamID >= 1932 && appl_dlc.CourseID == 5) || (appl_dlc.ExamID >= 1968 && appl_dlc.CourseID == 99) || (appl_dlc.ExamID >= 1956 && appl_dlc.CourseID == 98) || (appl_dlc.ExamID >= 2640 && appl_dlc.CourseID == 101) || (appl_dlc.ExamID >= 2628 && appl_dlc.CourseID == 100))
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion3.aspx?ID=" + appl_dlc.CourseID + "&Appid=" + appl_dlc.ID);
                        }
                        else if ((appl_dlc.ExamID >= 1177 && appl_dlc.CourseID == 7) || (appl_dlc.ExamID >= 1189 && appl_dlc.CourseID == 5) || (appl_dlc.ExamID >= 1201 && appl_dlc.CourseID == 99) || (appl_dlc.ExamID >= 1213 && appl_dlc.CourseID == 98) || (appl_dlc.ExamID >= 1225 && appl_dlc.CourseID == 101) || (appl_dlc.ExamID >= 1237 && appl_dlc.CourseID == 100))
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion2.aspx?ID=" + appl_dlc.CourseID + "&Appid=" + appl_dlc.ID);
                        }
                        else if ((appl_dlc.ExamID >= 1004 && appl_dlc.ExamID < 1011) || (appl_dlc.ExamID >= 992 && appl_dlc.ExamID < 999) || (appl_dlc.ExamID >= 1089 && appl_dlc.ExamID < 1124) || appl_dlc.ExamID >= 1170)
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion1.aspx?ID=" + appl_dlc.CourseID + "&Appid=" + appl_dlc.ID);
                        }
                        else
                        {
                            urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCard.aspx?ID=" + appl_dlc.CourseID + "&Appid=" + appl_dlc.ID);
                        }
                    }
                    else if (servicefor == 2 && servicetype == 2) //for DLC view filled form
                    {
                        urls = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificatePreview.aspx?ID=" + appl_dlc.CourseID + "&Appid=" + appl_dlc.ID + "&candtype=External");
                    }
                    if (urls.Contains('?'))
                    {
                        if (appl_oabc.NumberOfTheoryModulesApplied != 0 && appl_oabc.NumberOfPracticalModulesApplied != 0 && servicefor == 1 && servicetype == 1)
                        {
                            Response.Write("theory=https://student.nielit.gov.in" + urls.Substring(urls.IndexOf('/')) + " " + "practical=https://student.nielit.gov.in" + urlsPrac.Substring(urlsPrac.IndexOf('/')));
                            Response.End();
                            return;
                        }
                        else if (appl_oabc.NumberOfTheoryModulesApplied != 0 && appl_oabc.NumberOfPracticalModulesApplied == 0 && servicefor == 1 && servicetype == 1)
                        {                           
                            Response.Write("theory=https://student.nielit.gov.in" + urls.Substring(urls.IndexOf('/')) + " " + "practical=NA");
                            Response.End();
                            return;
                        }
                        else if (appl_oabc.NumberOfTheoryModulesApplied == 0 && appl_oabc.NumberOfPracticalModulesApplied != 0 && servicefor == 1 && servicetype == 1)
                        {
                            Response.Write("theory=NA" + " " + "practical=https://student.nielit.gov.in" + urls.Substring(urls.IndexOf('/')));
                            Response.End();
                            return;
                        }                   
                        else
                        {
                            //Response.Write(urls.Substring(urls.IndexOf('?') + 4));
                            Response.Write("https://student.nielit.gov.in" + urls.Substring(urls.IndexOf('/')));
                            Response.End();
                            return;
                        }
                    }
                }
            
            }
            else
            {
                Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                Response.End();
                return;
            }            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            //Response.Write(ex.Message);                        
        }
    }
}

