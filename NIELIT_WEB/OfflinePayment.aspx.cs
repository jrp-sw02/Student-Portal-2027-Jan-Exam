using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class OfflinePayment : BasePage
{
    
    //MerchantID|CustomerID|NA|TxnAmount|NA|NA|NA|CurrencyType|NA|TypeField1|SecurityID|NA|NA|TypeField2|AdditionalInfo1|AdditionalInfo2|AdditionalInfo3|AdditionalInfo4| AdditionalInfo5|NA|NA|RU
    //ABCD|123456789012|NA|100.00|NA|NA|NA|INR|NA|R|abcd|NA|NA|F|2375613|XYZ|NA|NA|NA|NA|NA|http://www.domain.com/response.jsp

    //Name:                                                     SERVICE _ID
    //Recruitment                                               RECR01
    //Online Registration                                       REGN01
    //Online Examination                                        EXAM01
    //Online Accreditation                                      ACCR01


    protected String requestURL = "https://payments.billdesk.com/MercOnline/ValidationRequestController";
    protected String msg = "";
    String merchantID = "XNIELITEFT";
    String SecurityID = "xnieliteft";
    String ServiceID = "";
    String checkSumKey = "ggrLJ5I34HDeXGTF6BbOUniIgHnfcJdv";
    protected String returnURL = "https://student.nielit.gov.in/OfflinePaymentResponse.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["DID"] == null)
                {
                    Response.Write("Invalid request parameters");
                    Response.End();
                }
                else
                {
                    Int64 demandNoteID = Convert.ToInt64(Request.QueryString["DID"]);
                    using (EConnectContext context = new EConnectContext())
                    {
                        //Int32 AppCount = 0;
                        DemandNote demandNote = context.DemandNotes.Find(demandNoteID);
                        if (demandNote != null)
                        {
                            if (demandNote.enmPaymentStatus == enmPaymentStatus.Pending)
                            {
                                #region comment
                                //int ApplicationtypeId = demandNote.ApplicationTypeID;
                                //switch (ApplicationtypeId)
                                //{
                                //    case 1: //Course Registration 
                                //        ServiceID = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID).FirstOrDefault().Course.RegistrationServiceID;
                                //        break;
                                //    case 2: //Certificate Exam
                                //        ServiceID = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID).FirstOrDefault().Course.ExaminationServiceID;
                                //        break;
                                //    case 3: //Course Exam
                                //        ServiceID = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID).FirstOrDefault().Course.ExaminationServiceID;
                                //        break;
                                //    case 4: //Module Certificate
                                //        ServiceID = context.ModuleCertificateRequests.Where(s => s.DemandNoteID == demandNote.ID).FirstOrDefault().Course.CertificateServiceID;
                                //        break;
                                //    default: //Otherwise
                                //        throw new Exception("ServiceID not Found");
                                //} 
                                #endregion

                                #region 
                                if (demandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                    ServiceID = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID).FirstOrDefault().Course.RegistrationServiceID;
                                }
                                else if (demandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                {
                                    ServiceID = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID).FirstOrDefault().Course.ExaminationServiceID;
                                }
                                else if (demandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                {
                                    ServiceID = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID).FirstOrDefault().Course.ExaminationServiceID;
                                }
                                else if (demandNote.enmApplicationType == enmApplicationType.ModuleCertificateRequest)
                                {
                                    ServiceID = context.ModuleCertificateRequests.Where(s => s.DemandNoteID == demandNote.ID).FirstOrDefault().Course.CertificateServiceID;
                                }
                                else { throw new Exception("ServiceID not Found"); } 
                                #endregion

                                //if (AppCount > 0)
                                //{                        

                                //save transaction details before initiating the request.
                                OnlineTransaction objTransaction = new OnlineTransaction();
                                objTransaction.RequestDate = DateTime.Now;
                                objTransaction.DemandNoteID = Convert.ToInt64(demandNoteID);
                                objTransaction.Amount = Convert.ToDecimal(demandNote.Amount);
                                objTransaction.RequestParameters = msg;
                                context.OnlineTransaction.Add(objTransaction);
                                context.SaveChanges();
                                //ABCD|123456789012|NA|100.00|NA|NA|NA|INR|NA|R|abcd|NA|NA|F|2375613|XYZ|NA|NA|NA|NA|NA|http://www.domain.com/response.jsp
                                //msg = merchantID + "|" + objTransaction.ID.ToString() + "|NA|" + demandNote.Amount.ToString("F") + "|NA|NA|NA|INR|NA|R|" + SecurityID + "|NA|NA|F|" + objTransaction.ID.ToString() + "|" + ServiceID + "|NA|NA|NA|NA|NA|" + returnURL ;
                                //msg = merchantID.ToUpper() + "|" + objTransaction.ID.ToString() + "|NA|" + demandNote.Amount.ToString("F") + "|NA|NA|NA|INR|NA|R|" + SecurityID.ToLower() + "|NA|NA|F|" + objTransaction.DemandNoteID.ToString() + "|" + ServiceID + "|NA|NA|NA|NA|NA|" + returnURL;
                                msg = merchantID.ToUpper() + "|" + objTransaction.ID.ToString() + "|NA|" + demandNote.Amount.ToString("F") + "|NA|NA|NA|INR|DIRECT|R|" + SecurityID.ToLower() + "|NA|NA|F|" + objTransaction.DemandNoteID.ToString() + "|" + ServiceID + "|NA|NA|NA|NA|NA|" + returnURL;
                                SHASample dataprg = new SHASample();
                                String hash = String.Empty;
                                hash = dataprg.GetHMACSHA256(msg ,checkSumKey);
                                msg = msg + "|" + hash.ToUpper();
                                String msg1 = msg;
                                if (msg1.Length > 300)
                                    msg1 = msg1.Substring(0, 299).Trim();
                                objTransaction.RequestParameters = msg1;
                                context.Entry(objTransaction).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                            else
                                throw new Exception("Demand note number not found / Invalid demand note number / Demand note already paid.");
                        }
                        else
                            throw new Exception("Demand note number not found / Invalid demand note number.");
                        //}
                        //else
                        //{
                        //    throw new Exception("You have attempted to edit the application. Kindly re-submit to make payment.");
                        //}
                    };
                }
            }
        }
        catch (Exception ex)
        {

            Response.Write("Transaction can not be performed. Reason: " + "<br>Invalid request parameters./" + ex.Message + "<br>" + GeInvalidRequestMessage("Goto Home Page", "MainPage.aspx"));

            Response.End();
        }
    }
    public class SHASample
    {
        public SHASample() { }


        public string GetHMACSHA256(string text, string key)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(text);

            HMACSHA256 hashString = new HMACSHA256(keybyt);
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

    }
}