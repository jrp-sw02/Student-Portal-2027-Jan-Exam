using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json;

/// <summary>
/// Summary description for Graph
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "https://student.nielit.gov.in/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class Graph : System.Web.Services.WebService {

    string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    parameters vPara = new parameters();

    public class Enity
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
    //[WebMethod]
    //public List<Enity> RegisterCand()
    //{
    //    List<Enity> Register = new List<Enity>();
    //    DataSet ds = new DataSet();
    //    ds = utility.executeProcedure("RegisterCand", vPara);
    //    ds.Tables[0].TableName = "StudentsRegistered";

    //    if (ds != null)
    //    {
    //        if (ds.Tables.Count > 0)
    //        {
    //            if (ds.Tables["StudentsRegistered"].Rows.Count > 0)
    //            {
    //                foreach (DataRow dr in ds.Tables["StudentsRegistered"].Rows)
    //                {
    //                    Register.Add(new Enity { Name = dr["xName"].ToString(), Value = Convert.ToInt32(dr["value"]) });
    //                }
    //            }
    //        }
    //    }
    //    return Register;
    //}

    //[WebMethod]
    //public List<Enity> CourseCompleted()
    //{
    //    List<Enity> Course = new List<Enity>();
    //    DataSet ds = new DataSet();
    //    ds = utility.executeProcedure("CourseCompleted", vPara);
    //    ds.Tables[0].TableName = "CourseCompleted";

    //    if (ds != null)
    //    {
    //        if (ds.Tables.Count > 0)
    //        {
    //            if (ds.Tables["CourseCompleted"].Rows.Count > 0)
    //            {
    //                foreach (DataRow dr in ds.Tables["CourseCompleted"].Rows)
    //                {
    //                    Course.Add(new Enity { Name = dr["xName"].ToString(), Value = Convert.ToInt32(dr["value"]) });
    //                }
    //            }
    //        }
    //    }
    //    return Course;
    //}

    [WebMethod]
    public List<Enity> NielitAccrInst()
    {
        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("getNIELITACCREDITEDCentreCount", vPara);
        ds.Tables[0].TableName = "NielitAccrInst";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["NielitAccrInst"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["NielitAccrInst"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Name"].ToString(), Value = Convert.ToInt32(dr["Center"]) });
                    }
                }
            }
        }
        return Register;
    }


    [WebMethod]
    public List<Enity> NielitCentre()
    {
        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("getStateNIELITCentresCount", vPara);
        ds.Tables[0].TableName = "NielitCentre";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["NielitCentre"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["NielitCentre"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["xName"].ToString(), Value = Convert.ToInt32(dr["value"]) });
                    }
                }
            }
        }
        return Register;
    }


    [WebMethod]
    public List<Enity> DigiLocker()
    {
        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();

        vPara.count = 1;
        vPara.pGrand = 0;

        ds = utility.executeProcedure("getDigiLockerCounts", vPara);
        ds.Tables[0].TableName = "DigiLocker";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["DigiLocker"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["DigiLocker"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Lyear"].ToString(), Value = Convert.ToInt32(dr["CertCount"]) });
                    }
                }
            }
        }
        return Register;
    }


    [WebMethod]
    public List<Enity> NSQFCounts()
    {
        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();

        vPara.count = 1;
        vPara.pID = 1;
        vPara.pGrand = null;

        ds = utility.executeProcedure("[getCandidatesNSQF]", vPara);
        ds.Tables[0].TableName = "NSQFCounts";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["NSQFCounts"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["NSQFCounts"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Centre"].ToString(), Value = Convert.ToInt32(dr["Count"]) });
                    }
                }
            }
        }
        return Register;
    }
    [WebMethod]
    public List<Enity> getCandidatesNSQF()
    {
        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();

        vPara.count = 1;
        vPara.pID = 1;
        vPara.pGrand = null;

        ds = utility.executeProcedure("[getCandidatesNSQF]", vPara);
        ds.Tables[0].TableName = "NSQFCounts";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["NSQFCounts"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["NSQFCounts"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Centre"].ToString(), Value = Convert.ToInt32(dr["Count"]) });
                    }
                }
            }
        }
        return Register;
    }

    [WebMethod]
    public List<Enity> NSQFCentreCounts(int pCentre)
    {
        string x = string.Format("{0}", pCentre);

        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();

        vPara.count = 1;
        vPara.pID = Convert.ToInt64(pCentre);
        vPara.pGrand = null;
        //ds = utility.executeProcedure("[getStateNSQFCounts]", vPara);  getCandidatesNSQF
        ds = utility.executeProcedure("[getStateNSQFCounts]", vPara);
        ds.Tables[0].TableName = "NSQFCounts";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["NSQFCounts"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["NSQFCounts"].Rows)
                    {
                        // Register.Add(new Enity { Name = dr["Centre"].ToString(), Value = Convert.ToInt32(dr["Count"]) });
                        Register.Add(new Enity { Name = dr["Name"].ToString(), Value = Convert.ToInt32(dr["Registered"]) });
                    }
                }
            }
        }
        return Register;
    }

    [WebMethod]
    public List<Enity> EmergingTrends()
    {
        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();

        vPara.count = 0;
        vPara.pID = null;
        vPara.pGrand = null;

        ds = utility.executeProcedure("[getCandEmergingTrends]", vPara);
        ds.Tables[0].TableName = "TrendsCounts";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["TrendsCounts"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["TrendsCounts"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Course"].ToString(), Value = Convert.ToInt32(dr["Count"]) });
                    }
                }
            }
        }
        return Register;
    }

    [WebMethod]
    public List<Enity> PlacementDetails(Int64 pGrand)
    {


        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();

        vPara.count = 1;
        vPara.pID = null;
        vPara.pGrand = pGrand;


        ds = utility.executeProcedure("getPlacementCounts", vPara);
        ds.Tables[0].TableName = "PlacementCounts";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (pGrand == 0)
                {
                    if (ds.Tables["PlacementCounts"].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables["PlacementCounts"].Rows)
                        {
                            Register.Add(new Enity { Name = dr["pYear"].ToString(), Value = Convert.ToInt32(dr["CandCount"]) });
                        }
                    }
                }
                else
                {
                    if (ds.Tables["PlacementCounts"].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables["PlacementCounts"].Rows)
                        {
                            Register.Add(new Enity { Name = dr["Course"].ToString(), Value = Convert.ToInt32(dr["CandCount"]) });
                        }
                    }
                }
            }
        }
        return Register;
    }

    [WebMethod]
    public List<Enity> RegisterCand()
    {
        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("RegisterCand", vPara);
        ds.Tables[0].TableName = "StudentsRegistered";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["StudentsRegistered"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["StudentsRegistered"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["xName"].ToString(), Value = Convert.ToInt32(dr["value"]) });
                    }
                }
            }
        }
        return Register;
    }


    [WebMethod]
    public List<Enity> RegisterCandParam(char? pSC, char? pST, char? pOBC, char? pPH, int? pG, int? pFY, int? pTY, int? pFM, int? pTM)
    {

    
        int? pG1 = null;
        int? pFY1 = null;
        int? pTY1 = null;
        int? pFM1 = null;
        int? pTM1 = null;

       
        if (pG != null)
            pG1 = pG;
        if (pFY != null)
            pFY1 = pFY;
        if (pTY != null)
            pTY1 = pTY;
        if (pFM != null)
            pFM1 = pFM;
        if (pTM != null)
            pTM1 = pTM;


        vPara.count = 9;
 
        vPara.pSC = pSC;
        vPara.pST = pST;
        vPara.pOBC = pOBC;
        vPara.pPH = pPH;
        vPara.pGender = pG1;
        vPara.pYearFrom = pFY1;
        vPara.pYearTo = pTY1;
        vPara.pmonthFrom = pFM1;
        vPara.pmonthTo = pTM1;



        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("RegisterCand", vPara);
        ds.Tables[0].TableName = "StudentsRegistered";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["StudentsRegistered"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["StudentsRegistered"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["xName"].ToString(), Value = Convert.ToInt32(dr["value"]) });
                    }
                }
            }
        }
        return Register;
    }



    [WebMethod]
    public List<Enity> RegisterCandYearWise()
    {
        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("Nielit_Reg_CandList", vPara);
        ds.Tables[0].TableName = "StudentsRegistered";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["StudentsRegistered"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["StudentsRegistered"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Name"].ToString(), Value = Convert.ToInt32(dr["Registered"]) });
                    }
                }
            }
        }
        return Register;
    }


    [WebMethod]
    public List<Enity> CourseCompleted(char pSC, char pST, char pOBC, char pPH, int? pG, int? pFY, int? pTY, int? pFM, int? pTM)
    {

        char? pSC1 = null;
        char? pST1 = null;
        char? pOBC1 = null;
        char? pPH1 = null;
        int? pG1 = null;
        int? pFY1 = null;
        int? pTY1 = null;
        int? pFM1 = null;
        int? pTM1 = null;


        if (pSC == 'Y')
            pSC1 = 'Y';
        if (pST == 'Y')
            pST1 = 'Y';
        if (pOBC == 'Y')
            pOBC1 = 'Y';
        if (pPH == 'Y')
            pPH1 = 'Y';
        if (pG != null)
            pG1 = pG;
        if (pFY != null)
            pFY1 = pFY;
        if (pTY != null)
            pTY1 = pTY;
        if (pFM != null)
            pFM1 = pFM;
        if (pTM != null)
            pTM1 = pTM;



        vPara.count = 9;
        vPara.pSC = pSC1;
        vPara.pST = pST1;
        vPara.pOBC = pOBC1;
        vPara.pPH = pPH1;
        vPara.pGender = pG1;
        vPara.pYearFrom = pFY1;
        vPara.pYearTo = pTY1;
        vPara.pmonthFrom = pFM1;
        vPara.pmonthTo = pTM1;
    
        List<Enity> Course = new List<Enity>();
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("CourseCompleted", vPara);
        ds.Tables[0].TableName = "CourseCompleted";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["CourseCompleted"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["CourseCompleted"].Rows)
                    {
                        Course.Add(new Enity { Name = dr["xName"].ToString(), Value = Convert.ToInt32(dr["value"]) });
                    }
                }
            }
        }
        return Course;
    }


    [WebMethod]
    public List<Enity> skilledCandidates()
    {
        List<Enity> Course = new List<Enity>();
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("skilledCandidates", vPara);
        ds.Tables[0].TableName = "skilledCandidates";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["skilledCandidates"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["skilledCandidates"].Rows)
                    {
                        Course.Add(new Enity { Name = dr["xName"].ToString(), Value = Convert.ToInt32(dr["value"]) });
                    }
                }
            }
        }
        return Course;
    }


    [WebMethod]
    public List<Enity> StateWiseNSQFCounts(int pStateID, char pSC, char pST, char pOBC, char pPH, int? pG, int? pFY, int? pTY, int? pFM, int? pTM)
    {
        string x = string.Format("{0}", pStateID);
        char? pSC1 = null;
        char? pST1 = null;
        char? pOBC1 = null;
        char? pPH1 = null;
        int? pG1 = null;
        int? pFY1 = null;
        int? pTY1 = null;
        int? pFM1 = null;
        int? pTM1 = null;

        if (pSC == 'Y')
            pSC1 = 'Y';
        if (pST == 'Y')
            pST1 = 'Y';
        if (pOBC == 'Y')
            pOBC1 = 'Y';
        if (pPH == 'Y')
            pPH1 = 'Y';
        if (pG != null)
            pG1 = pG;
        if (pFY != null)
            pFY1 = pFY;
        if (pTY != null)
            pTY1 = pTY;
        if (pFM != null)
            pFM1 = pFM;
        if (pTM != null)
            pTM1 = pTM;

        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();

        vPara.count = 10;
        vPara.pStateID = Convert.ToInt64(pStateID);
        vPara.pSC = pSC1;
        vPara.pST = pST1;
        vPara.pOBC = pOBC1;
        vPara.pPH = pPH1;
        vPara.pGender = pG1;
        vPara.pYearFrom = pFY;
        vPara.pYearTo = pTY;
        vPara.pmonthFrom = pFM;
        vPara.pmonthTo = pTM;

        ds = utility.executeProcedure("getStateWiseNSQFCounts", vPara);
        ds.Tables[0].TableName = "StateWiseNSQFCounts";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["StateWiseNSQFCounts"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["StateWiseNSQFCounts"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Course"].ToString(), Value = Convert.ToInt32(dr["Total"]) });
                    }
                }
            }
        }
        return Register;
    }

    [WebMethod]
    public List<Enity> StateWiseNSQFCountsCertified(int pStateID, char pSC, char pST, char pOBC, char pPH, int? pG, int? pFY, int? pTY, int? pFM, int? pTM)
    {
        string x = string.Format("{0}", pStateID);
        char? pSC1 = null;
        char? pST1 = null;
        char? pOBC1 = null;
        char? pPH1 = null;
        int? pG1 = null;
        int? pFY1 = null;
        int? pTY1 = null;
        int? pFM1 = null;
        int? pTM1 = null;

        if (pSC == 'Y')
            pSC1 = 'Y';
        if (pST == 'Y')
            pST1 = 'Y';
        if (pOBC == 'Y')
            pOBC1 = 'Y';
        if (pPH == 'Y')
            pPH1 = 'Y';
        if (pG != null)
            pG1 = pG;
        if (pFY != null)
            pFY1 = pFY;
        if (pTY != null)
            pTY1 = pTY;
        if (pFM != null)
            pFM1 = pFM;
        if (pTM != null)
            pTM1 = pTM;



        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();

        vPara.count = 10;
        vPara.pStateID = Convert.ToInt64(pStateID);
        vPara.pSC = pSC1;
        vPara.pST = pST1;
        vPara.pOBC = pOBC1;
        vPara.pPH = pPH1;
        vPara.pGender = pG1;
        vPara.pYearFrom = pFY;
        vPara.pYearTo = pTY;
        vPara.pmonthFrom = pFM1;
        vPara.pmonthTo = pTM1;

        ds = utility.executeProcedure("getStateWiseNSQFCounts_Certified", vPara);
        ds.Tables[0].TableName = "StateWiseNSQFCountsCertified";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["StateWiseNSQFCountsCertified"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["StateWiseNSQFCountsCertified"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Course"].ToString(), Value = Convert.ToInt32(dr["Total"]) });
                    }
                }
            }
        }
        return Register;
    }

    [WebMethod]
    public List<Enity> getStateWiseCourseCategoryWiseNSQFCountsRegGrid(int pStateID, int pCourseCategoryID, char pSC, char pST, char pOBC, char pPH, int? pG, int? pFY, int? pTY, int? pFM, int? pTM)
    {
       
        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();

        char? pSC1 = null;
        char? pST1 = null;
        char? pOBC1 = null;
        char? pPH1 = null;
        int? pG1 = null;
        int? pFY1 = null;
        int? pTY1 = null;
        int? pFM1 = null;
        int? pTM1 = null;

        if (pSC == 'Y')
            pSC1 = 'Y';
        if (pST == 'Y')
            pST1 = 'Y';
        if (pOBC == 'Y')
            pOBC1 = 'Y';
        if (pPH == 'Y')
            pPH1 = 'Y';
        if (pG != null)
            pG1 = pG;
        if (pFY != null)
            pFY1 = pFY;
        if (pTY != null)
            pTY1 = pTY;
        if (pFM != null)
            pFM1 = pFM;
        if (pTM != null)
            pTM1 = pTM;


        vPara.count = 11;
        vPara.pStateID = Convert.ToInt64(pStateID);
        vPara.pCourseCategoryID = Convert.ToInt64(pCourseCategoryID);
        vPara.pSC = pSC1;
        vPara.pST = pST1;
        vPara.pOBC = pOBC1;
        vPara.pPH = pPH1;
        vPara.pGender = pG1;
        vPara.pYearFrom = pFY;
        vPara.pYearTo = pTY;
        vPara.pmonthFrom = pFM1;
        vPara.pmonthTo = pTM1;


        ds = utility.executeProcedure("getStateWiseCourseCategoryWiseNSQFCounts_Reg", vPara);
        ds.Tables[0].TableName = "StateWiseCourseCategoryWiseNSQFRegCounts";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["StateWiseCourseCategoryWiseNSQFRegCounts"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["StateWiseCourseCategoryWiseNSQFRegCounts"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Course_Name"].ToString(), Value = Convert.ToInt32(dr["Registered_Total"]) });
                    }
                }
            }
        }
        return Register;
    }

    [WebMethod]
    public List<Enity> getStateWiseCourseCategoryWiseNSQFCountsCertGrid(int pStateID, int pCourseCategoryID, char pSC, char pST, char pOBC, char pPH, int? pG, int? pFY, int? pTY, int? pFM, int? pTM)
    {
        

        List<Enity> Register = new List<Enity>();
        DataSet ds = new DataSet();



        char? pSC1 = null;
        char? pST1 = null;
        char? pOBC1 = null;
        char? pPH1 = null;
        int? pG1 = null;
        int? pFY1 = null;
        int? pTY1 = null;
        int? pFM1 = null;
        int? pTM1 = null;

        if (pSC == 'Y')
            pSC1 = 'Y';
        if (pST == 'Y')
            pST1 = 'Y';
        if (pOBC == 'Y')
            pOBC1 = 'Y';
        if (pPH == 'Y')
            pPH1 = 'Y';
        if (pG != null)
            pG1 = pG;
        if (pFY != null)
            pFY1 = pFY;
        if (pTY != null)
            pTY1 = pTY;
        if (pFM != null)
            pFM1 = pFM;
        if (pTM != null)
            pTM1 = pTM;
      

        vPara.count = 11;
        vPara.pStateID = Convert.ToInt64(pStateID);
        vPara.pCourseCategoryID = Convert.ToInt64(pCourseCategoryID);
        vPara.pSC = pSC1;
        vPara.pST = pST1;
        vPara.pOBC = pOBC1;
        vPara.pPH = pPH1;
        vPara.pGender = pG1;
        vPara.pYearFrom = pFY;
        vPara.pYearTo = pTY;
        vPara.pmonthFrom = pFM1;
        vPara.pmonthTo = pTM1;


        ds = utility.executeProcedure("getStateWiseCourseCategoryWiseNSQFCounts_cert", vPara);
        ds.Tables[0].TableName = "StateWiseCourseCategoryWiseNSQFCertGrid";

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables["StateWiseCourseCategoryWiseNSQFCertGrid"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["StateWiseCourseCategoryWiseNSQFCertGrid"].Rows)
                    {
                        Register.Add(new Enity { Name = dr["Course_Name"].ToString(), Value = Convert.ToInt32(dr["Certified_Total"]) });
                    }
                }
            }
        }
        return Register;
    }

}
