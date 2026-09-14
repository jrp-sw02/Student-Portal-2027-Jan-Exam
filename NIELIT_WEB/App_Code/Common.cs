using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using EConnect;
using EConnect.DAL;
using EConnect.HRMS;
using EConnect.NIELIT;

/// <summary>
/// Summary description for Common
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "https://student.nielit.gov.in/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService()]
public class Common : System.Web.Services.WebService
{
    EConnectContext context;

    [WebMethod(EnableSession = true)]
    [ScriptMethod()]
    public String[] GetAllDepartments(String prefixText, Int32 count)
    {
        try
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<String> items = new List<String>();
            context = new EConnectContext();
            var orgs = from p in context.Organizations
                       where (p.Name.ToUpper().StartsWith(prefixText.ToUpper()) || p.Name.ToUpper().Contains(prefixText.ToUpper()))
                       orderby p.Name ascending
                       select p;
            if (orgs.Count() == 0)
            {
                items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new Organization { ID = 0, Name = "Not match found" })));
            }
            else
            {
                foreach (Organization org in orgs)
                {
                    items.Add(AutoCompleteExtender.CreateAutoCompleteItem(org.Name, js.Serialize(org)));
                }
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally { context.Dispose(); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod()]
    public String[] GetStateNames(String prefixText, Int32 count, String contextKey)
    {
        try
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<String> items = new List<String>();
            context = new EConnectContext();
            Int32 typeID = Convert.ToInt32(EConnect.enmLocationType.State);
            Int64 countryID = Convert.ToInt64(contextKey);
            var states = from s in context.Locations
                         where ((s.Name.ToUpper().StartsWith(prefixText.ToUpper()) || s.Name.ToUpper().Contains(prefixText.ToUpper())) && s.ParentLocationID == countryID && s.LocationTypeID == typeID)
                         orderby s.Name ascending
                         select new { ID = s.ID, Name = s.Name };
            if (states.Count() == 0)
            {
                items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new Location { ID = 0, Name = "Not match found" })));
            }
            else
            {
                foreach (var state in states.Take(count))
                {
                    items.Add(AutoCompleteExtender.CreateAutoCompleteItem(state.Name, js.Serialize(state)));
                }
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally { context.Dispose(); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod()]
    public String[] GetCityNames(String prefixText, Int32 count, String contextKey)
    {
        try
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<String> items = new List<String>();
            context = new EConnectContext();
            Int32 typeID = Convert.ToInt32(EConnect.enmLocationType.City);
            Int64 stateID = Convert.ToInt64(contextKey);
            var cities = from s in context.Locations
                         where ((s.Name.ToUpper().StartsWith(prefixText.ToUpper()) || s.Name.ToUpper().Contains(prefixText.ToUpper())) && s.ParentLocationID == stateID && s.LocationTypeID == typeID)
                         orderby s.Name ascending
                         select new { ID = s.ID, Name = s.Name };
            if (cities.Count() == 0)
            {
                items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new Location { ID = 0, Name = "Not match found" })));
            }
            else
            {
                foreach (var city in cities.Take(count))
                {
                    items.Add(AutoCompleteExtender.CreateAutoCompleteItem(city.Name, js.Serialize(city)));
                }
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally { context.Dispose(); }
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod()]
    public String[] GetBankNames(String prefixText, Int32 count)
    {
        try
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<String> items = new List<String>();
            context = new EConnectContext();
            var bank = (from s in context.NEFTTransactions
                        where (s.TransactionBank.ToUpper().StartsWith(prefixText.ToUpper()))
                        orderby s.TransactionBank ascending
                        select new { IssuingBankName = s.TransactionBank.ToUpper() }).Distinct();
            if (bank.Count() == 0)
            {
                items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new DemandDraftTransaction { IssuingBankName = "Not match found" })));
            }
            else
            {
                foreach (var banks in bank.Take(count))
                {
                    items.Add(banks.IssuingBankName);
                }
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally { context.Dispose(); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod()]
    public String[] GetInstitutes(String prefixText, Int32 count, String contextKey)
    {
        try
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<String> items = new List<String>();
            context = new EConnectContext();
            Int32 CourseID = Convert.ToInt32(contextKey);
            if (CourseID != 0)
            {
                var institute1 = (from i in context.Institutes
                                  join d in context.AccreditationDetails on i.ID equals d.InstituteID
                                  where ((i.Name.ToUpper().StartsWith(prefixText.ToUpper())) || (d.AccreditationNumber.ToUpper().StartsWith(prefixText.ToUpper())))
                                  && d.CourseID == CourseID && d.CourseCategoryID == 1
                                  orderby i.Name
                                  select new { ID = i.ID, Name = d.AccreditationNumber + " - " + i.Name + "  ,  " + (!String.IsNullOrEmpty(i.CityName) ? i.CityName : "") + "  ,  " + i.State.Name }).ToList();

                if (institute1.Count() == 0)
                {
                    items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new Institute { ID = 0, Name = "Not match found" })));
                }
                else
                {
                    foreach (var inst1 in institute1.Take(count))
                    {
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem(inst1.Name, js.Serialize(inst1)));
                    }
                }
            }
            else
            {
                var institute = (from i in context.Institutes
                                 join d in context.AccreditationDetails on i.ID equals d.InstituteID
                                 where d.CourseCategoryID == 1 && (i.Name.ToUpper().StartsWith(prefixText.ToUpper())) || (d.AccreditationNumber.ToUpper().StartsWith(prefixText.ToUpper()))
                                 orderby i.Name
                                 select new { ID = i.ID, Name = d.AccreditationNumber + " - " + i.Name + "  ,  " + (!String.IsNullOrEmpty(i.CityName) ? i.CityName : "") + "  ,  " + i.State.Name }).Distinct().ToList();

                if (institute.Count() == 0)
                {
                    items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new Institute { ID = 0, Name = "Not match found" })));
                }
                else
                {
                    foreach (var inst in institute.Take(count))
                    {
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem(inst.Name, js.Serialize(inst)));
                    }
                }
            }

            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally { context.Dispose(); }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod()]
    public String[] GetBCCCCCInstitutes(String prefixText, Int32 count, String contextKey)
    {
        try
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<String> items = new List<String>();
            context = new EConnectContext();
            Int32 CourseID = Convert.ToInt32(contextKey);
            if (CourseID != 0)
            {
                var institute1 = (from i in context.Institutes
                                  join d in context.AccreditationDetails on i.ID equals d.InstituteID
                                  where ((i.Name.ToUpper().StartsWith(prefixText.ToUpper())) || (d.AccreditationNumber.ToUpper().StartsWith(prefixText.ToUpper())))
                                  && d.CourseID == CourseID && d.CourseCategoryID == 2
                                  orderby i.Name
                                  select new { ID = i.ID, Name = d.AccreditationNumber + " - " + i.Name + "  ,  " + (!String.IsNullOrEmpty(i.CityName) ? i.CityName : "") + "  ,  " + i.State.Name }).ToList();

                if (institute1.Count() == 0)
                {
                    items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new Institute { ID = 0, Name = "Not match found" })));
                }
                else
                {
                    foreach (var inst1 in institute1.Take(count))
                    {
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem(inst1.Name, js.Serialize(inst1)));
                    }
                }
            }
            else
            {
                var institute = (from i in context.Institutes
                                 join d in context.AccreditationDetails on i.ID equals d.InstituteID
                                 where d.CourseCategoryID == 2 && (i.Name.ToUpper().StartsWith(prefixText.ToUpper())) || (d.AccreditationNumber.ToUpper().StartsWith(prefixText.ToUpper()))
                                 orderby i.Name
                                 select new { ID = i.ID, Name = d.AccreditationNumber + " - " + i.Name + "  ,  " + (!String.IsNullOrEmpty(i.CityName) ? i.CityName : "") + "  ,  " + i.State.Name }).Distinct().ToList();

                if (institute.Count() == 0)
                {
                    items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new Institute { ID = 0, Name = "Not match found" })));
                }
                else
                {
                    foreach (var inst in institute.Take(count))
                    {
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem(inst.Name, js.Serialize(inst)));
                    }
                }
            }

            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally { context.Dispose(); }
    }

    [WebMethod()]
    [ScriptMethod()]
    public String[] GetACCInstitutes(String prefixText, Int32 count, String contextKey)
    {
        try
        {           
            List<String> items = new List<String>();
            List<String> item1 = new List<String>();
            context = new EConnectContext();
            Int32 CourseID = context.Courses.Where(s => s.Code == "ACC").FirstOrDefault().ID;
            if (CourseID != 0)
            {
                var instss = context.AccCourseCompletionDates.Select(s => s.InstituteId).Distinct();
                foreach (var inst1 in instss)
                {
                    item1.Add(inst1.ToString());
                }
                var InstituteList = item1.Where(s => s.StartsWith(prefixText));
               
                foreach (var inst1 in InstituteList.Take(count))
                {
                    items.Add(inst1.ToString());
                }              
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally { context.Dispose(); }

    }  

}
