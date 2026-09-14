using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Web.Security;
using EConnect.Utils.Common;
using System.Globalization;
using System.Text;

public partial class UserControl_DuplicateRecords : System.Web.UI.UserControl
{
    Int64 batchItemID = 0;
    public Int64 BatchItemID
    {
        set
        {
            batchItemID = value;
            ViewState["batchItemID"] = value;
        }
    }
    string applicationNo = "";
    public string ApplicationNo
    {
        set
        {
            applicationNo = value;
            ViewState["applicationNo"] = value;
        }
    }
    Int32 applicationTypeID = 0;
    public Int32 ApplicationTypeID
    {
        set
        {
            applicationTypeID = value;
            ViewState["applicationTypeID"] = value;
        }
    }
    public void BindRecords()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                BatchItem batchItem = new BatchItem();
                CertificateExamApplication cr = new CertificateExamApplication();
                CourseRegistrationApplication crs = new CourseRegistrationApplication();
                CourseExamApplication cea = new CourseExamApplication();
                if (applicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    if (batchItemID != 0 && batchItemID != null)
                    {
                        batchItem = context.BatchItems.Find(batchItemID);
                        crs = context.CourseRegistrationApplications.Find(batchItem.CourseRegistrationApplicationID);
                    }
                    else
                    {
                        crs = context.CourseRegistrationApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    }
                    if (crs != null)
                    {
                        if (String.IsNullOrEmpty(crs.GuardianName) || String.IsNullOrWhiteSpace(crs.GuardianName))
                        {
                           
                              var candidatelist = (from s in context.Candidates
                                                   where (
                                                    (s.Name.ToUpper() == crs.Name.ToUpper() && s.DateOfBirth == crs.DateOfBirth && s.Gender.ToUpper() == crs.Gender.ToUpper()) ||
                                                    (s.Name.ToUpper() == crs.Name.ToUpper() && s.FatherName.ToUpper() == crs.FatherName.ToUpper() && s.Gender.ToUpper() == crs.Gender.ToUpper())
                                                    )
                                                select s).ToList();
                              if (crs.CandidateID != 0 && crs.CandidateID != null)
                              {
                                  candidatelist = candidatelist.Where(s => s.ID != crs.CandidateID).ToList();
                              }

                            if (candidatelist.Count() > 0)
                            {
                                trmsg.Visible = false;
                                trrecords.Visible = true;
                                trrecords1.Visible = true;
                                foreach (var candidate in candidatelist)
                                {
                                    if (candidate != null)
                                    {
                                        var registration = (from c in context.RegistrationDetails
                                                            where c.CandidateID == candidate.ID
                                                            select new
                                                            {
                                                                regno = c.RegistrationNo,
                                                            }).FirstOrDefault();
                                        if(registration!=null)
                                            tdduplicaterecords.InnerHtml += "<a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/DuplicateCandidateList.aspx?candidateID=" + candidate.ID) + "' target='_blank'>" + CommonFunctions.GetInitCap(candidate.Name) + " ( " + registration.regno + " ) " + "</a>, ";
                                    }
                                }
                                tdduplicaterecords.InnerHtml = tdduplicaterecords.InnerHtml.Trim().Trim(',');
                            }
                            else
                            {
                                trrecords.Visible = false;
                                trrecords1.Visible = false;
                                trmsg.Visible = true;
                            }
                        }
                        else 
                        {

                            var candidatelist = (from s in context.Candidates
                                                 where (
                                                    (s.Name.ToUpper() == crs.Name.ToUpper() && s.DateOfBirth == crs.DateOfBirth && s.Gender.ToUpper() == crs.Gender.ToUpper()) ||
                                                    (s.Name.ToUpper() == crs.Name.ToUpper() && s.GuardianName.ToUpper() == crs.GuardianName.ToUpper() && s.Gender.ToUpper() == crs.Gender.ToUpper())
                                                     )
                                               select s).ToList();
                            if (crs.CandidateID != 0 && crs.CandidateID != null)
                            {
                                candidatelist = candidatelist.Where(s => s.ID != crs.CandidateID).ToList();
                            }

                            if (candidatelist.Count() > 0)
                            {
                                trmsg.Visible = false;
                                trrecords.Visible = true;
                                trrecords1.Visible = true;
                                foreach (var candidate in candidatelist)
                                {
                                    if (candidate != null)
                                    {
                                        var registration = (from c in context.RegistrationDetails
                                                            where c.CandidateID == candidate.ID
                                                            select new
                                                            {
                                                                regno = c.RegistrationNo,
                                                            }).FirstOrDefault();
                                        if (registration != null)
                                            tdduplicaterecords.InnerHtml += "<a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/DuplicateCandidateList.aspx?candidateID=" + candidate.ID) + "' target='_blank'>" + CommonFunctions.GetInitCap(candidate.Name) + " ( " + registration.regno + " ) " + "</a>, ";
                                    }
                                }
                                tdduplicaterecords.InnerHtml = tdduplicaterecords.InnerHtml.Trim().Trim(',');
                            }
                            else
                            {
                                trrecords.Visible = false;
                                trrecords1.Visible = false;
                                trmsg.Visible = true;
                            }
                        }
                    }
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}