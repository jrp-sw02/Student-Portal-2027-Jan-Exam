using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UserControl_PagingBar : System.Web.UI.UserControl
{
    // Delegate declaration
    //public delegate void OnPageSizeChange(Int16 NewPageSize);
    public delegate void OnPageIndexChange(Int32 NewPageIndex);
    // Event declaration
    //public event OnPageSizeChange PageSizeChanged;
    public event OnPageIndexChange PageIndexChanged;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public Int16 CurrentPageSize
    {
        get
        {
            return Convert.ToInt16(ddlRecordNo.SelectedValue);
        }
        set
        {
            ddlRecordNo.SelectedValue = value.ToString();
            ddlRecordNo_SelectedIndexChanged(ddlRecordNo, EventArgs.Empty);
        }
    }
    public Int32 PageCount
    {
        get
        {
            return Convert.ToInt32(txtTotPage.Text);
        }
        set
        {
            txtTotPage.Text = value.ToString();
            SetPagingButtons();
        }
    }

    public Int32 CurrentPageIndex
    {
        get
        {
            return Convert.ToInt32(txtPageNumber.Text) - 1;
        }
        set
        {
            txtPageNumber.Text = (value + 1).ToString();
            txtPageNumber_TextChanged(txtPageNumber, EventArgs.Empty);
        }
    }
    public Int32 TotalRecords
    {
        get
        {
            return Convert.ToInt32(lblTotRecords.Text);
        }
    }
    protected void ddlRecordNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "-a", "autoResize(window.parent.document.getElementById('ifHome'));", true);
            btnNavigate_Click(btnFirst, EventArgs.Empty);
            //if (PageSizeChanged != null)
            //    PageSizeChanged(Convert.ToInt16(ddlRecordNo.SelectedValue));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void txtPageNumber_TextChanged(object sender, EventArgs e)
    {
        try
        {
			//Uncommented below line for trgcalendar 24 mar 2021
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "autoResize(window.parent.document.getElementById('ifHome'));", true);
            Int32 newIndex = Convert.ToInt32(txtPageNumber.Text) - 1;
            SetPagingButtons();
            if (PageIndexChanged != null)
                PageIndexChanged(newIndex);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnNavigate_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "-a", "autoResize(window.parent.document.getElementById('ifHome'));", true);
            Button btn = (Button)sender;
            if (btn.CommandArgument == "F")
                this.CurrentPageIndex = 0;
            else if (btn.CommandArgument == "P")
                this.CurrentPageIndex -= 1;
            else if (btn.CommandArgument == "N")
                this.CurrentPageIndex += 1;
            else if (btn.CommandArgument == "L")
                this.CurrentPageIndex = this.PageCount - 1;
            SetPagingButtons();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void SetPagingButtons()
    {
        try
        {
            txtPageNumber.Enabled = true;
            ddlRecordNo.Enabled = true;
            if (this.CurrentPageIndex == 0)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                if (this.PageCount - 1 <= this.CurrentPageIndex)
                {
                    btnNext.Enabled = false;
                    btnLast.Enabled = false;
                    if (this.PageCount == 0)
                    {
                        txtPageNumber.Enabled = false;
                        ddlRecordNo.Enabled = false;
                    }
                }
                else
                {
                    btnNext.Enabled = true;
                    btnLast.Enabled = true;
                }
            }
            else
            {
                if (this.CurrentPageIndex <= this.PageCount - 1)
                {
                    btnFirst.Enabled = true;
                    btnPrev.Enabled = true;
                    if (this.CurrentPageIndex == this.PageCount - 1)
                    {
                        btnNext.Enabled = false;
                        btnLast.Enabled = false;
                    }
                    else
                    {
                        btnNext.Enabled = true;
                        btnLast.Enabled = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void Bind(IEnumerable<Object> lstDataSource, ref GridView sourceGridView)
    {
        try
        {
            int totalReocords = lstDataSource.Count();
            if (this.CurrentPageSize > 0)
                sourceGridView.PageSize = this.CurrentPageSize;
            else
                sourceGridView.PageSize = totalReocords;
            int rowNumberFrom = this.CurrentPageSize * this.CurrentPageIndex;
            int rowCount = (this.CurrentPageSize == 0 ? totalReocords : this.CurrentPageSize);
            if ((rowCount + rowNumberFrom) > totalReocords)
                rowCount = totalReocords - rowNumberFrom;
            int indexTo = totalReocords;
            List<Object> lst = lstDataSource.Skip(rowNumberFrom).Take(rowCount).ToList();

            //List<Object> dt = lst.GetRange(rowNumberFrom, rowCount);
            Double pg = (double)(Convert.ToDecimal(indexTo) / Convert.ToDecimal(this.CurrentPageSize == 0 ? indexTo : this.CurrentPageSize));
            this.PageCount = (int)Math.Ceiling(pg);
            sourceGridView.DataSource = lst;
            sourceGridView.DataBind();
            lblTotRecords.Text = totalReocords.ToString();
            SetPagingButtons();

        }
        catch (Exception ex)
        {
            this.Visible = false;
            throw ex;
        }
    }
    public void Bind(DataTable dataSource, ref GridView sourceGridView)
    {
        try
        {
            int totalReocords = dataSource.Rows.Count;
            if (this.CurrentPageSize > 0)
                sourceGridView.PageSize = this.CurrentPageSize;
            else
                sourceGridView.PageSize = totalReocords;
            int rowNumberFrom = this.CurrentPageSize * this.CurrentPageIndex;
            int rowNumberTo = rowNumberFrom + (this.CurrentPageSize == 0 ? totalReocords : this.CurrentPageSize);
            if (rowNumberTo > totalReocords)
                rowNumberTo = totalReocords;
            int indexFrom = 0;
            int indexTo = dataSource.Rows.Count;

            DataTable dt = dataSource.Clone();
            dt.Rows.Clear();
            for (indexFrom = rowNumberFrom; indexFrom < rowNumberTo; indexFrom++)
            {
                dt.ImportRow(dataSource.Rows[indexFrom]);
            }
            Double pg = (double)(Convert.ToDecimal(indexTo) / Convert.ToDecimal(this.CurrentPageSize == 0 ? indexTo : this.CurrentPageSize));
            this.PageCount = (int)Math.Ceiling(pg);
            sourceGridView.DataSource = dt.DefaultView;
            sourceGridView.DataBind();
            lblTotRecords.Text = totalReocords.ToString();
            SetPagingButtons();
        }
        catch (Exception ex)
        {
            this.Visible = false;
            throw ex;
        }
    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Int32 newIndex = Convert.ToInt32(txtPageNumber.Text) - 1;
            if (PageIndexChanged != null)
                PageIndexChanged(newIndex);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}