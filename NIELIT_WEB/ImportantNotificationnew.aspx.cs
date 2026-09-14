using iTextSharp.text.html.simpleparser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ImportantNotificationnew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLatestEvents();
        }
    }

    public static string ToPlainText(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // 1. Strip all HTML tags (including <img>, <p>, <span>, <strong> …)
        string text = Regex.Replace(input, @"<[^>]+>", string.Empty);

        // 2. Decode HTML entities (&nbsp; → space, &amp; → &, etc.)
        text = HttpUtility.HtmlDecode(text);

        // 3. Collapse multiple whitespace characters into a single space and trim
        text = Regex.Replace(text, @"\s+", " ").Trim();

        return text;
    }


    protected string GetNotificationHtml(object title, object link)
    {
        string sTitle = Convert.ToString(title);
        string sLink = Convert.ToString(link);

        if (string.IsNullOrEmpty(sLink))
        {
            return "<span class='notif-text'>" + sTitle + "</span>";
        }

        return "<a class='notif-text-link' href='" + sLink + "' target='_blank'>" + sTitle + "</a>";
    }
    private void LoadLatestEvents()
    {
        string url = "https://www.nielit.gov.in/api/getOptions";

        var payload = new
        {
            getOptionsJson = new
            {
                rdid = "b9e5bcc2e4fd94ddfcf10b3c76e7ff7298b201409cef49909aac8bb6544c4e2d",
                comp_no = "",
                FormName = "LatestEvent",
                table_ID = 931,
                formupdateid = "1101",
                center = "HQ",
                language = "en"
            }
        };

        try
        {
            string jsonPayload =
                JsonConvert.SerializeObject(payload);

            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";

            request.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";

            request.Referer =
                "https://www.nielit.gov.in/form?formName=LatestEvent&t=latestNews&center=HQ";

            request.Headers.Add(
                "Origin",
                "https://www.nielit.gov.in");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var streamWriter =
                new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonPayload);
                streamWriter.Flush();
            }

            using (HttpWebResponse response =
                (HttpWebResponse)request.GetResponse())

            using (StreamReader reader =
                new StreamReader(response.GetResponseStream()))
            {
                string responseString =
                    reader.ReadToEnd();

                dynamic result =
                    JsonConvert.DeserializeObject(responseString);

                List<EventItem> eventsList =
                    new List<EventItem>();

                if (result.result != null &&
                    result.result.Count > 0)
                {
                    var dataArray = result.result[0];


                    foreach (var item in dataArray)
                    {
                        DateTime parsedDate;
                        DateTime.TryParse(
                            Convert.ToString(item.Creation_Date),
                            out parsedDate);

                        bool isNew =
                            parsedDate != DateTime.MinValue &&
                            parsedDate >= DateTime.Now.AddDays(-7);

                        string newIcon =
                            isNew ? "<img src='./images/new1.gif' alt='New' />" : "";

                        eventsList.Add(new EventItem
                        {          
                            title =
                                " 👉 " +
                                (item.Title.ToString() ?? "No Title" ) +
                                " " +
                                newIcon,

                            date =
                                parsedDate != DateTime.MinValue
                                    ? parsedDate.ToString("dd-MMMM-yyyy")
                                    : "",

           
                             link =item.Link ,


                            SortDate =
                                parsedDate,

                            studentportal =
                                item.studentPortal,
                        });
                    }
                }

                // sort newest first
                eventsList =
                    eventsList
                    .Where(x=> x.studentportal == 1)
                    .OrderByDescending(x => x.SortDate)
                    .ToList();

                // query string check
                bool showAll =
                    Request.QueryString["99"] != null;

                if (!showAll)
                {
                    if (eventsList.Count > 10)
                    {
                        rptEvents.DataSource = eventsList.Take(20);

                        oldnotifblock.Visible = true;

                        lnkOlderNotifications.NavigateUrl =
                            Request.Path + "?99=1";
                    }
                    else
                    {
                        rptEvents.DataSource =eventsList;
                        oldnotifblock.Visible = false;
                    }
                }
                else
                {
                    rptEvents.DataSource =
                        eventsList;
                    oldnotifblock.Visible = false;
                }

                rptEvents.DataBind();

                loadingImgNotif.Visible = false;
                btnreload.Visible = false;
                lblError.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "OOPS ! Unable to Load Notifications ";
            btnreload.Visible = true;

        }
    }

 
}


public class EventItem
{
    public string title { get; set; }
    public string date { get; set; }
    public string link { get; set; }
    public int studentportal { get; set; }
    public DateTime SortDate { get; set; }
}