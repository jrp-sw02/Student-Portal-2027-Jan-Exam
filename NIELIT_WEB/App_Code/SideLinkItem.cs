using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SideLinkItem
/// </summary>
public class SideLinkItem
{
    public enum SideLinkType
    { 
        ButtonWithImageLink =1,
        Hyperlink =2,
        DownloadLink=3
    }
    #region Constructors
    public SideLinkItem(String text, String href)
    {
        this.Text = text;
        this.Href = href;
    }
    public SideLinkItem(String text, String href, String imageSrc)
    {
        this.Text = text;
        this.Href = href;
        this.ImageSrc = imageSrc;
    }
    public SideLinkItem(String text, String href, String imageSrc, String target)
    {
        this.Text = text;
        this.Href = href;
        this.ImageSrc = imageSrc;
        this.Target = target;
    }
    #endregion
    #region Private Members
    private string _text;
    private string _href;
    private string _target = "_self";
    private string _imageSrc = "_self";
    #endregion
    #region Public Properties
    /// <summary>
    /// gets or sets the text of  side link item  object.
    /// </summary>
    public string Text
    {
        set { _text = value; }
        get { return _text; }
    }
    /// <summary>
    /// gets or sets the href of  side link item  object.
    /// </summary>
    public string Href
    {
        set { _href = value; }
        get { return _href; }
    }
    /// <summary>
    /// gets or sets the target of  side link item  object.
    /// </summary>
    public string Target
    {
        set { _target = value; }
        get { return _target; }
    }
     /// <summary>
    /// gets or sets the image src of side link item object.
    /// </summary>
    public string ImageSrc
    {
        set { _imageSrc = value; }
        get { return _imageSrc; }
    }
    #endregion
}
