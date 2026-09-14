using System;
using System.Linq;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class CAND_CandidateFeedBack : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Home.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                ShowName();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Feedback/Suggestions", "", ""));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                Feedback feedback;
                feedback = new EConnect.NIELIT.Feedback();
                feedback.Date = DateTime.Now;
                feedback.UserID = Convert.ToInt32(entityID);
                feedback.UserTypeID = Convert.ToInt32(loginUserType);
                if (!string.IsNullOrEmpty(Txtfeedback.Text))
                    feedback.FeedbackDescription = Txtfeedback.Text;
                else
                    feedback.FeedbackDescription = null;
                if (!string.IsNullOrEmpty(TxtSuggestions.Text))
                    feedback.Suggestions = TxtSuggestions.Text;
                else
                    feedback.Suggestions = null;
                feedback.IsMarkedRead = false;
                context.Feedbacks.Add(feedback);
                context.SaveChanges();
                divfinal.Visible = true;
                divfilter.Visible = false;
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowName()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candidate = (from a in context.Candidates
                                 where a.ID == entityID
                                 select new
                                 {
                                     name = a.Name,
                                     salutation = a.Salutation
                                 }).FirstOrDefault();
                if (candidate != null)
                {
                    LblTitle.Text = "Dear, " + candidate.salutation + " " + GetInitCap(candidate.name) + "<br>Please give your feedback or suggestions about this application.";
                    LblWelcome.Text = "Dear, " + candidate.salutation + " " + GetInitCap(candidate.name);
                }
                else
                {
                    LblTitle.Text = "NA";
                    LblWelcome.Text = "NA";
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Txtfeedback.Text = "";
            TxtSuggestions.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect("../frmDashBoard.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}