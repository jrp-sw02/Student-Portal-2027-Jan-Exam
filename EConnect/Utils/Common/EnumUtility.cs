using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI.WebControls;

namespace EConnect.Utils.Common
{
    public static class EnumUtility
    {
        public static String GetDescription(System.Enum EnumConstant)
        {
            try
            {   
                FieldInfo fi = EnumConstant.GetType().GetField(EnumConstant.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes( typeof(DescriptionAttribute), false);
                if(attributes.Length >0)
                    return attributes[0].Description.ToString();
                else
                   return EnumConstant.ToString();
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
        public static void BindListObject(ref DropDownList pDropDownList, System.Type pEnumType, ListItem pFirstListItem)
        {
            try
            {
                pDropDownList.Items.Clear();
                Dictionary<String, Int32> ConstantTypeList = new Dictionary<string,int>();
                foreach (System.Enum enumInstance in System.Enum.GetValues(pEnumType))
                {
                    ConstantTypeList.Add(EnumUtility.GetDescription(enumInstance), Convert.ToInt32(enumInstance) );
                }
                pDropDownList.DataSource = ConstantTypeList;
                pDropDownList.DataValueField = "Value";
                pDropDownList.DataTextField = "Key";
                pDropDownList.DataBind();
                if (pFirstListItem != null)
                    pDropDownList.Items.Insert(0, pFirstListItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
