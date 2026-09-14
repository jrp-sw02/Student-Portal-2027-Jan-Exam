using System;
using System.Web;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SelectMenu
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "https://student.nielit.gov.in/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class dropdowns : System.Web.Services.WebService
{
    private const int CacheTime = 300;	// seconds for caching webmethods in seconds (here it is 5 minutes)
    public dropdowns()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod(CacheDuration = CacheTime, Description = "Returns the list course categories")]
    public AjaxControlToolkit.CascadingDropDownNameValue[] GetCourseCategoryList(string knownCategoryValues, string category)
    {
        DataSet myDataset = new DataSet();
        myDataset = ClassJKS.JKS_GetCourseCategoryList();
        List<AjaxControlToolkit.CascadingDropDownNameValue>
           cascadingValues = new
           List<AjaxControlToolkit.CascadingDropDownNameValue>();
        foreach (DataRow dRow in myDataset.Tables[0].Rows)
        {
            string CategoryCode = dRow["ID"].ToString();
            string CategoryName = dRow["Name"].ToString();
            cascadingValues.Add(new AjaxControlToolkit.CascadingDropDownNameValue(CategoryName, CategoryCode));
        }
        myDataset.Clear();
        myDataset.Dispose();
        return cascadingValues.ToArray();
    }

    [WebMethod]
    public AjaxControlToolkit.CascadingDropDownNameValue[] GetCoursesListForCategory(string knownCategoryValues, string category)
    {
        StringDictionary categoryValues = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
        int category_code = Convert.ToInt32(categoryValues["CourseCategory"]);

        DataSet myDataset = new DataSet();
        myDataset = ClassJKS.JKS_GetCoursesListForCategory(category_code);

        List<AjaxControlToolkit.CascadingDropDownNameValue> cascadingValues = new List<AjaxControlToolkit.CascadingDropDownNameValue>();

        foreach (DataRow dRow in myDataset.Tables[0].Rows)
        {
            string c_code = dRow["ID"].ToString().Trim();
            string c_name = dRow["Name"].ToString().Trim();

            cascadingValues.Add(new AjaxControlToolkit.CascadingDropDownNameValue(c_name, c_code));
        }
        myDataset.Clear();
        myDataset.Dispose();
        return cascadingValues.ToArray();
    }
    
   
}

