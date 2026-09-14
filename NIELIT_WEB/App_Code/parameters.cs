using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for parameters
/// </summary>
public class parameters :System.Attribute 
{
	public parameters()
	{
		//
		// TODO: Add constructor logic here
		//
        count = 0;
        pID = null ;
        pLinkedToCentre = null;
        pGrand = null;

	}
    public int count
    {
        get;
        set;
    }
    public Nullable <Int64> pGrand
    {
        get;
        set;
    }
    public Nullable<Int64> pID
    {
        get;
        set;
    }
    public Nullable<Int64> pLinkedToCentre
    {
        get;
        set;
    }
    public Nullable<Int64> pInstituteId
    {
        get;
        set;
    }
    public Nullable<Char> pSC
    {
        get;
        set;
    }
    public Nullable<Char> pST
    {
        get;
        set;
    }
    public Nullable<Char> pOBC
    {
        get;
        set;
    }
    public Nullable<Char> pPH
    {
        get;
        set;

    }
    public Nullable<Int64> pStateID
    {
        get;
        set;
    }
    public Nullable<Int64> pCourseCategoryID
    {
        get;
        set;
    }
    public Nullable<Int64> pGender
    {
        get;
        set;
    }
    public Nullable<Int64> pYearFrom
    {
        get;
        set;
    }
    public Nullable<Int64> pYearTo
    {
        get;
        set;
    }
    public Nullable<Int64> pmonthFrom
    {
        get;
        set;
    }
    public Nullable<Int64> pmonthTo
    {
        get;
        set;
    }

}