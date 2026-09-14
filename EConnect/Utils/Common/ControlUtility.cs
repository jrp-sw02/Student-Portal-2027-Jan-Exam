using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace EConnect.Utils.Common
{
    public static  class ControlUtility
    {
        public static void BindListObject(DropDownList pDropDownList, DataTable pDataSource, ListItem pFirstListItem)
        {
            try
            {
                pDropDownList.DataSource = pDataSource.DefaultView;
                if (pDataSource.Columns.Count > 1)
                {
                    pDropDownList.DataValueField = pDataSource.Columns[0].ToString();// "Ref_Id";
                    pDropDownList.DataTextField = pDataSource.Columns[1].ToString(); //"Description";
                }
                else
                {
                    pDropDownList.DataValueField = pDataSource.Columns[0].ToString();// "Ref_Id";
                    pDropDownList.DataTextField = pDataSource.Columns[0].ToString(); //"Description";
                }
                pDropDownList.DataBind();
                if (pFirstListItem != null)
                    pDropDownList.Items.Insert(0, pFirstListItem);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
        public static void BindListObject(DropDownList pDropDownList, IEnumerable<Object> pDataSource, ListItem pFirstListItem)
        {
            try
            {
                pDropDownList.DataTextField = "TextField";
                pDropDownList.DataValueField = "ValueField";
                pDropDownList.DataSource = pDataSource.ToList();
                pDropDownList.DataBind();
                if (pFirstListItem != null)
                    pDropDownList.Items.Insert(0, pFirstListItem);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
        public static void ResetListObject(DropDownList pDropDownList, ListItem pFirstListItem)
        {
            try
            {
                pDropDownList.Items.Clear();
                if (pFirstListItem != null)
                    pDropDownList.Items.Insert(0, pFirstListItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void BindListObject(CheckBoxList pCheckBoxList, DataTable pDataSource)
        {
            try
            {
                pCheckBoxList.DataSource = pDataSource.DefaultView;
                if (pDataSource.Columns.Count > 1)
                {
                    pCheckBoxList.DataValueField = pDataSource.Columns[0].ToString();// "Ref_Id";
                    pCheckBoxList.DataTextField = pDataSource.Columns[1].ToString(); //"Description";
                }
                else
                {
                    pCheckBoxList.DataValueField = pDataSource.Columns[0].ToString();// "Ref_Id";
                    pCheckBoxList.DataTextField = pDataSource.Columns[0].ToString(); //"Description";
                }
                pCheckBoxList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void BindListObject(CheckBoxList pCheckBoxList, IEnumerable<Object> pDataSource)
        {
            try
            {
                pCheckBoxList.DataTextField = "TextField";
                pCheckBoxList.DataValueField = "ValueField";
                pCheckBoxList.DataSource = pDataSource.ToList();
                pCheckBoxList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void BindListObject(RadioButtonList pRadioButtonList, IEnumerable<Object> pDataSource)
        {
            try
            {
                pRadioButtonList.DataTextField = "TextField";
                pRadioButtonList.DataValueField = "ValueField";
                pRadioButtonList.DataSource = pDataSource.ToList();
                pRadioButtonList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void BindGridViewObject(DataTable pDataSource, GridView pGridView, Label pLabelToShowRecordNumber)
        {
            try
            {
                pGridView.DataSource = pDataSource.DefaultView;
                pGridView.DataBind();
                pGridView.Visible = true;
                if(pLabelToShowRecordNumber != null)
                    pLabelToShowRecordNumber.Text = "Total Records: " + pDataSource.DefaultView.Count.ToString();
                if (pDataSource.DefaultView.Count < pGridView.PageSize)
                {
                    if (pDataSource.DefaultView.Count <= 0)
                    {
                        pGridView.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void BindGridViewObject(DataTable pDataSource, DataGrid pGridView, Button pFirstButton, Button pPrevButton, Button pNextbutton, Button pLastbutton, Label pLabelToShowRecordNumber)
        {
            try
            {
                pGridView.DataSource = pDataSource.DefaultView;
                pGridView.DataBind();
                if(pLabelToShowRecordNumber != null)
                    pLabelToShowRecordNumber.Text = "Total Records: " + pDataSource.DefaultView.Count.ToString() + "&nbsp;&nbsp;";
                if (pDataSource.DefaultView.Count <= pGridView.PageSize)
                {
                    pFirstButton.Visible = false;
                    pPrevButton.Visible = false;
                    pNextbutton.Visible = false;
                    pLastbutton.Visible = false;
                    pGridView.Visible = true;
                    if (pDataSource.DefaultView.Count <= 0)
                    {
                        pGridView.Visible = false;
                    }
                }
                else
                {
                    pGridView.Visible = true;
                    pFirstButton.Visible = true;
                    pPrevButton.Visible = true;
                    pNextbutton.Visible = true;
                    pLastbutton.Visible = true;
                    if (pGridView.CurrentPageIndex == 0)
                    {
                        pFirstButton.Enabled = false;
                        pPrevButton.Enabled = false;
                        pNextbutton.Enabled = true;
                        pLastbutton.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void NavigateGridView(Object pSender, EventArgs pEventArgs, GridView pGridView, Button pFirstButton, Button pPrevButton, Button pNextbutton, Button pLastbutton)
        {
            Button btn = (Button)pSender;
            switch(btn.CommandArgument)
            {
                case "Next":
                {
                    if(pGridView.PageIndex < (pGridView.PageCount - 1))
                        pGridView.PageIndex += 1;
                    if (pGridView.PageIndex == (pGridView.PageCount - 1))
                    {
                        pNextbutton.Enabled = false;
					    pLastbutton.Enabled = false;
                    }
				    pFirstButton.Enabled = true;
				    pPrevButton.Enabled = true;
                    break;
                }
                case "Prev":
                {
                    if (pGridView.PageIndex > 0)
                        pGridView.PageIndex -= 1;
                    if (pGridView.PageIndex == 0)
                    {
                        pFirstButton.Enabled = false;
					    pPrevButton.Enabled = false;
                    }
				    pNextbutton.Enabled = true;
				    pLastbutton.Enabled = true;
                    break;
                }
                case "Last":
                {
                    pGridView.PageIndex = (pGridView.PageCount - 1);
				    pNextbutton.Enabled = false;
				    pLastbutton.Enabled = false;
				    pFirstButton.Enabled = true;
				    pPrevButton.Enabled = true;
                    break;
                }
                default:
                {
                    pGridView.PageIndex = Convert.ToInt32(btn.CommandArgument);
				    pFirstButton.Enabled = false;
				    pPrevButton.Enabled = false;
				    pNextbutton.Enabled = true;
				    pLastbutton.Enabled = true;
                    break;
                }
            }
        }
    }
}
