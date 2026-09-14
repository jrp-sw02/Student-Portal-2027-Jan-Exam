using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// this class provides navigation facility that reveals the user's location in a website.
/// </summary>
public class BreadCrumbItem
{
    #region Constructors
    public BreadCrumbItem()
    {
    }
    public  BreadCrumbItem(String text, String href, String target)
    {
        this.Text = text;
        this.Href = href;
        this.Target = target;
    }
    public BreadCrumbItem(String text, String href)
    {
        this.Text = text;
        this.Href = href;
        this.Target = "_self";
    }
    #endregion

    #region Private Members
    private string _text;
    private string _href;
    private string _target = "_self";
    #endregion
    #region Public Properties
    /// <summary>
    /// gets or sets the text of BreadCrumbItem object.
    /// </summary>
    public string Text
    {
        set { _text = value; }
        get { return _text; }
    }
    /// <summary>
    /// gets or sets the href of BreadCrumbItem object.
    /// </summary>
    public string Href
    {
        set { _href = value; }
        get { return _href; }
    }
    /// <summary>
    /// gets or sets the target of BreadCrumbItem object.
    /// </summary>
    public string Target
    {
        set { _target = value; }
        get { return _target; }
    }
    #endregion
}